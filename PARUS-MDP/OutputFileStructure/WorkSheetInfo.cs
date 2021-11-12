using System;
using System.Collections.Generic;
using System.Text;
using OfficeOpenXml;

namespace OutputFileStructure
{
	public class WorkSheetInfo
	{
		private MaximumAllowPowerFlow _maximumAllowPowerFlow;
		private List<Imbalance> _maximumAllowPowerFlowNonBalance;
		private AllowPowerOverflows _allowPowerOverflow;

		public WorkSheetInfo(string repairScheme, int noRegularOscilation, List<ControlAction> controlActionInSample, ExcelWorksheet excelWorksheetPARUS)
		{
			Inizialize();
			int startRow = FindScheme(repairScheme, excelWorksheetPARUS) + 1;
			MainMethod(startRow, controlActionInSample, noRegularOscilation, excelWorksheetPARUS);
		}
		public WorkSheetInfo(int noRegularOscilation, List<ControlAction> controlActionInSample, ExcelWorksheet excelWorksheetPARUS)
		{
			Inizialize();
			int startRow = 9;
			MainMethod(startRow, controlActionInSample, noRegularOscilation, excelWorksheetPARUS);
		}

		public MaximumAllowPowerFlow MaximumAllowPowerFlow => _maximumAllowPowerFlow;


		public List<Imbalance> MaximumAllowPowerFlowNonBalance => _maximumAllowPowerFlowNonBalance;


		private void Inizialize()
		{
			ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
		}

		private void MainMethod(int startRow, List<ControlAction> controlActionInSample,
			int noRegularOscilation, ExcelWorksheet excelWorksheetPARUS)
		{
			_allowPowerOverflow = NormalSchemeResults(startRow, excelWorksheetPARUS);
			int headRow = startRow + 3;
			if (excelWorksheetPARUS.Cells[headRow, 1].Value != null)
			{
				var bodyRowsAfterFault = FindBodyRowDisconnectionLineFact(headRow, excelWorksheetPARUS);
				List<(string, List<int>)> disconnectionLineFacts = new List<(string, List<int>)>();
				List<(string, List<int>)> disconnectionLineFactsWithControlAction = new List<(string, List<int>)>();
				for (int i = 0; i < bodyRowsAfterFault.Count; i++)
				{
					if (IsInSample(bodyRowsAfterFault[i].Item1, controlActionInSample))
					{
						disconnectionLineFactsWithControlAction.Add(bodyRowsAfterFault[i]);
					}
					else
					{
						disconnectionLineFacts.Add(bodyRowsAfterFault[i]);
					}
				}
				MaximumAllowPowerFlowDefine(headRow, disconnectionLineFacts, excelWorksheetPARUS);
				MaximumAllowPowerFlowControlActionDefine(headRow, noRegularOscilation,
					controlActionInSample, disconnectionLineFactsWithControlAction, excelWorksheetPARUS);
			}
		}

		private void MaximumAllowPowerFlowDefine(int headRow, List<(string, List<int>)> disconnectionLineFacts, 
			ExcelWorksheet excelWorksheetPARUS)
		{
			_maximumAllowPowerFlow = new MaximumAllowPowerFlow();
			_maximumAllowPowerFlow.EmergencyAllowPowerFlowValue = _allowPowerOverflow.EmergencyAllowPowerOverflow;
			_maximumAllowPowerFlow.EmergencyAllowPowerCriterion = "8% P исходная схема";
						
			foreach ((string, List<int>) disconnectionLineFact in disconnectionLineFacts)
			{
				//заменить
				var emergency = MaximumAllowPowerFlowDefinition(headRow, disconnectionLineFact, excelWorksheetPARUS);

				List<int> criteria = new List<int> { emergency.CurrentLoadLinesValue,
						emergency.StaticStabilityPostEmergency, emergency.StabilityVoltageValue, _allowPowerOverflow.StaticStabilityNormal };
				for(int i = 0; i < criteria.Count; i ++)
				{
					if(criteria[i] > 0)
					{
						if (_maximumAllowPowerFlow.MaximumAllowPowerFlowValue == 0 || 
							_maximumAllowPowerFlow.MaximumAllowPowerFlowValue > criteria[i])
						{
							_maximumAllowPowerFlow.MaximumAllowPowerFlowValue = criteria[i];
							_maximumAllowPowerFlow.MaximumAllowPowerCriterion = DefiningCriteria(i, emergency.CurrentLoadLinesCriterion, disconnectionLineFact.Item1);
						}
					}
				}
			}		
		}

		private void MaximumAllowPowerFlowControlActionDefine(int headRow, int noRegularOscilation, 
			List<ControlAction> controlActionsInSample, List<(string, List<int>)> disconnectionLineFactsWithControlAction,
			ExcelWorksheet excelWorksheetPARUS)
		{
			_maximumAllowPowerFlowNonBalance = new List<Imbalance>();

			foreach ((string, List<int>) bodyRow in disconnectionLineFactsWithControlAction)
			{
				Imbalance imbalance = new Imbalance();

				if (disconnectionLineFactsWithControlAction.Count == controlActionsInSample.Count)
				{
					var emergency = MaximumAllowPowerFlowDefinition(headRow, bodyRow, excelWorksheetPARUS);
					List<int> criteria = new List<int> { emergency.CurrentLoadLinesValue, 
						emergency.StaticStabilityPostEmergency, emergency.StabilityVoltageValue };
					for (int i = 0; i < criteria.Count; i++)
					{
						
						if (criteria[i] > 0)
						{
							if (imbalance.ImbalanceValue == 0 || imbalance.ImbalanceValue > criteria[i])
							{
								imbalance.ImbalanceValue = criteria[i];
								imbalance.ImbalanceCriterion = DefiningCriteria(i, emergency.CurrentLoadLinesCriterion, bodyRow.Item1);
							}
						}
					}
				}
				else
				{
					imbalance.ImbalanceValue = _allowPowerOverflow.EmergencyAllowPowerOverflow - noRegularOscilation;
					imbalance.ImbalanceCriterion = $"8%P ПАР '{bodyRow.Item1}'";
				}

				var controlAction = FindRightControlAction(controlActionsInSample, bodyRow.Item1);
				imbalance.ImbalanceCoefficient = controlAction.CoefficientEfficiency;
				imbalance.MaximumImbalance = controlAction.ActivePowerControlActionMax;
				var compare = CompareAllowPowerFlowWithImbalanceEquation(controlAction.CoefficientEfficiency,
					controlAction.ActivePowerControlActionMax, imbalance.ImbalanceValue, _maximumAllowPowerFlow.MaximumAllowPowerFlowValue);
				imbalance.EquationValue = compare.Item2;
				if (compare.Item1)
				{
					imbalance.Equation = imbalance.ImbalanceValue.ToString() + "-" + 
						controlAction.CoefficientEfficiency.ToString() + "*" + bodyRow.Item1;
					
					_maximumAllowPowerFlowNonBalance.Add(imbalance);
				}
			}
		}

		private (bool, float) CompareAllowPowerFlowWithImbalanceEquation(float coefficientEfficiency,
			int activePowerControlActionMax, int nonBalanceValue, int maximumAllowPowerFlow)
		{
			float equationResult = nonBalanceValue - activePowerControlActionMax * coefficientEfficiency;
			
			if(maximumAllowPowerFlow > equationResult)
			{
				return (true, equationResult);
			}
			else
			{
				return (false, equationResult);
			}
		}

		private ControlAction FindRightControlAction(List<ControlAction> controlActions, string disconnectionLineFact)
		{
			for (int i = 0; i < controlActions.Count; i++)
			{
				if(disconnectionLineFact == controlActions[i].ParamID)
				{
					return controlActions[i];
				}
			}
			throw new Exception($"УВ {disconnectionLineFact} нет в файле шаблона ");
		}

		private string DefiningCriteria(int index, string currentCriteria, string disconectionLineFactName)
		{
			switch (index)
			{
				case 0:
				{
					return $"АДТН '{currentCriteria}' ПАР '{disconectionLineFactName}'";
				}
				case 1:
				{
					return $"8%P ПАР '{disconectionLineFactName}'";
				}
				case 2:
				{
					return $"10%U ПАР '{disconectionLineFactName}'";
				}
				case 3:
				{
					return "20% исходная схема";
				}
				default:
				{
					throw new Exception("Сравнение критериев не удалось");
				}
			}
		}


		private AllowPowerOverflows NormalSchemeResults(int startRow, ExcelWorksheet excelWorksheetPARUS)
		{

			string[] columnsNameBeforeFault =
				new string[] { "Рсеч-Рно, МВт", "Перегружаемый элемент", "Рпред*0,8-Pно", "Р(Uкр/0.85)-Рно", "Узел", "Рпред" };
			string[] assignedValuesBeforeFault = new string[columnsNameBeforeFault.Length + 1];

			assignedValuesBeforeFault[assignedValuesBeforeFault.Length - 1] =
				RoundAndMultiply(assignedValuesBeforeFault[assignedValuesBeforeFault.Length - 2], 0.92);
			AllowPowerOverflows allowPowerOverflows = new AllowPowerOverflows();
			//переделать во что-то красивое
			try
			{
				allowPowerOverflows.CurrentLoadLinesValue = int.Parse(FindCellValue(startRow, startRow + 1, columnsNameBeforeFault[0], excelWorksheetPARUS));
			}
			catch { }
			try
			{
				allowPowerOverflows.CurrentLoadLinesCriterion = FindCellValue(startRow, startRow + 1, columnsNameBeforeFault[1], excelWorksheetPARUS);
			}
			catch { }
			try
			{
				allowPowerOverflows.StaticStabilityNormal = int.Parse(FindCellValue(startRow, startRow + 1, columnsNameBeforeFault[2], excelWorksheetPARUS));
			}
			catch { }
			try
			{
				allowPowerOverflows.StabilityVoltageValue = int.Parse(FindCellValue(startRow, startRow + 1, columnsNameBeforeFault[3], excelWorksheetPARUS));
			}
			catch { }
			try
			{
				allowPowerOverflows.StabilityVoltageCriterion = FindCellValue(startRow, startRow + 1, columnsNameBeforeFault[4], excelWorksheetPARUS);
			}
			catch { }
			try
			{
				allowPowerOverflows.CriticalValue = int.Parse(FindCellValue(startRow, startRow + 1, columnsNameBeforeFault[5], excelWorksheetPARUS));
			}
			catch { }
			
			allowPowerOverflows.EmergencyAllowPowerOverflow = int.Parse((allowPowerOverflows.CriticalValue * 0.92).ToString());

			return allowPowerOverflows;
		}

		private AllowPowerOverflows MaximumAllowPowerFlowDefinition(int headRow,
			(string, List<int>) bodyRowAfterFault, ExcelWorksheet excelWorksheetPARUS)
		{
			var outputArray = DisconnectionLineFact(headRow, bodyRowAfterFault.Item2, excelWorksheetPARUS);
			outputArray.DisconnectionLineFact = bodyRowAfterFault.Item1;
			return outputArray;
		}

		private AllowPowerOverflows DisconnectionLineFact(int headRow,
			List<int> bodyRowAfterFault, ExcelWorksheet excelWorksheetPARUS)
		{
			string[] columnsNameAfterFault =
				new string[] { "Рсеч-Рно, МВт", "Перегружаемый элемент", "Рдоав8%-Pно", "Рда(Uкр/0.9)-Рно", "Примечание" };

			AllowPowerOverflows valuesAfterFault = new AllowPowerOverflows();

			for (int valueNumber = 0; valueNumber < bodyRowAfterFault.Count; valueNumber++)
			{
				AllowPowerOverflows valuesAfterFaultTmp = new AllowPowerOverflows();
				try
				{
					valuesAfterFaultTmp.StaticStabilityPostEmergency = int.Parse(FindCellValue(headRow, bodyRowAfterFault[valueNumber],
							columnsNameAfterFault[2], excelWorksheetPARUS));
					if (valuesAfterFault.StaticStabilityPostEmergency > valuesAfterFaultTmp.StaticStabilityPostEmergency ||
						valuesAfterFault.StaticStabilityPostEmergency == 0)
					{
						valuesAfterFault.StaticStabilityPostEmergency = valuesAfterFaultTmp.StaticStabilityPostEmergency;
					}
				}
				catch { }
				try
				{
					valuesAfterFaultTmp.StabilityVoltageValue = int.Parse(FindCellValue(headRow, bodyRowAfterFault[valueNumber],
							columnsNameAfterFault[3], excelWorksheetPARUS));
					if (valuesAfterFault.StabilityVoltageValue > valuesAfterFaultTmp.StabilityVoltageValue ||
						valuesAfterFault.StabilityVoltageValue == 0)
					{
						valuesAfterFault.StabilityVoltageValue = valuesAfterFaultTmp.StabilityVoltageValue;
					}
				}
				catch { }
				try
				{
					valuesAfterFaultTmp.Note = FindCellValue(headRow, bodyRowAfterFault[valueNumber],
							columnsNameAfterFault[4], excelWorksheetPARUS);
				}
				catch { }
				try
				{
					valuesAfterFaultTmp.CurrentLoadLinesCriterion = FindCellValue(headRow, bodyRowAfterFault[valueNumber],
													columnsNameAfterFault[1], excelWorksheetPARUS);
				}
				catch { }

				if (valuesAfterFaultTmp.Note != "АОПО" &&
					valuesAfterFaultTmp.CurrentLoadLinesCriterion != "Токовых перегрузов нет")
				{
					try
					{
						valuesAfterFaultTmp.CurrentLoadLinesValue = int.Parse(FindCellValue(headRow, bodyRowAfterFault[valueNumber],
								columnsNameAfterFault[0], excelWorksheetPARUS));
						if (valuesAfterFault.CurrentLoadLinesValue > valuesAfterFaultTmp.CurrentLoadLinesValue ||
							valuesAfterFault.CurrentLoadLinesValue == 0)
						{
							valuesAfterFault.CurrentLoadLinesValue = valuesAfterFaultTmp.CurrentLoadLinesValue;
							valuesAfterFault.CurrentLoadLinesCriterion = valuesAfterFaultTmp.CurrentLoadLinesCriterion;
						}
					}
					catch { }
				}
			}

			return valuesAfterFault;
		}

		private string FindCellValue(int headRow, int bodyRow, string columnName, ExcelWorksheet excelWorksheetPARUS)
		{
			var columnIndex = FindColumn(headRow, columnName, excelWorksheetPARUS);
	

			if (excelWorksheetPARUS.Cells[bodyRow, columnIndex].Value == null)
			{
				throw new Exception($"Данные в ячейке {excelWorksheetPARUS.Cells[bodyRow, columnIndex]} отсутствуют");
			}

			return excelWorksheetPARUS.Cells[bodyRow, columnIndex].Value.ToString();
				
					
		}

		private int FindColumn(int headRow, string columnName, ExcelWorksheet excelWorksheetPARUS)
		{
			for (int i = 2; i < 50; i++)
			{
				if (excelWorksheetPARUS.Cells[headRow, i].Value != null)
				{
					if (excelWorksheetPARUS.Cells[headRow, i].Value.ToString() == columnName)
					{
						return i;
					}
				}
			}
			throw new Exception($"В строке {headRow} нет ячейки с текстом {columnName}");
		}

		private int FindScheme(string repairScheme, ExcelWorksheet excelWorksheetPARUS)
		{
			string[] textSeparators = new string[] { "ремонт ", "ремонта " };
			repairScheme.Trim();
			string schemeName = repairScheme;
			foreach (string textSeparator in textSeparators)
			{
				string[] repairSchemeSplit = repairScheme.ToLower().Split(textSeparator);
				if(schemeName.Length > repairSchemeSplit[repairSchemeSplit.Length - 1].Length)
				{
					schemeName = repairSchemeSplit[repairSchemeSplit.Length - 1];
				}
			}
			int rowNumber = FindRow("Схема Ремонта", 9, 1, excelWorksheetPARUS);
			while(rowNumber != 0)
			{
				object CellValue = excelWorksheetPARUS.Cells[rowNumber, 1].Value;
				if (CellValue.ToString().ToLower().Contains(schemeName))
				{
					return rowNumber;
				}
				rowNumber = FindRow("Схема Ремонта", rowNumber, 1, excelWorksheetPARUS);
			}
			throw new Exception($"Схемы {schemeName} на листе {excelWorksheetPARUS.Name} нет");
		}

		private int FindRow(string textInColumn, int startRow, int column, ExcelWorksheet excelWorksheetPARUS)
		{
			for (int i = startRow + 1; i < startRow + 1000; i++)
			{
				object CellValue = excelWorksheetPARUS.Cells[i, column].Value;
				if (CellValue != null)
				{
					if (CellValue.ToString().Contains(textInColumn))
					{
						return i;
					}
				}
			}
			return 0;
		}

		private string RoundAndMultiply(string text, double multiplier)
		{
			double number;
			if(double.TryParse(text,out number))
			{
				return Math.Round(Math.Round(number) * multiplier).ToString();
			}
			else
			{
				return text;
			}
			
		}

		private List<(string, List<int>)> FindBodyRowDisconnectionLineFact(int headRow, ExcelWorksheet excelWorksheetPARUS)
		{
			var outputList = new List<(string, List<int>)>();

			if (excelWorksheetPARUS.Cells[headRow, 1].Value.ToString() != "Послеаварийный режим")
			{
				return outputList;
			}
			object cellValue = excelWorksheetPARUS.Cells[headRow + 1, 1].Value;
			int index = 1;
			while (cellValue !=null)
			{
				string[] cellValueSplit = cellValue.ToString().Split(" ");
				string outputString = "";
				for (int i = 1; i < cellValueSplit.Length; i++)
				{
					outputString = outputString + " " + cellValueSplit[i];
				}
				outputList.Add((outputString.Trim(), new List<int>()));
				outputList[outputList.Count - 1].Item2.Add(headRow + index);

				index += 1;
				cellValue = excelWorksheetPARUS.Cells[headRow + index, 1].Value;
				if(cellValue == null)
				{
					int overlodedElementColumn = FindColumn(headRow, "Перегружаемый элемент", excelWorksheetPARUS);
					object overlodedElementValue =
						excelWorksheetPARUS.Cells[headRow + index, overlodedElementColumn].Value;
					while (overlodedElementValue != null && cellValue == null)
					{
						outputList[outputList.Count - 1].Item2.Add(headRow + index);
						index += 1;
						overlodedElementValue =
							excelWorksheetPARUS.Cells[headRow + index, overlodedElementColumn].Value;
						cellValue = excelWorksheetPARUS.Cells[headRow + index, 1].Value;

					}
				}
			}
			return outputList;
		}

		private bool IsInSample(string afterFault, List<ControlAction> controlActionInSample)
		{
			foreach(ControlAction controlAction in controlActionInSample)
			{
				if(controlAction.ParamID == afterFault)
				{
					return true;
				}
			}
			return false;
		}
	}
}
