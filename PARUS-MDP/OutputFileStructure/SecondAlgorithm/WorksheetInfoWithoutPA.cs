using System;
using System.Collections.Generic;
using System.Text;
using OfficeOpenXml;
using OutputFileStructure.DataTypes;

namespace OutputFileStructure
{
	public class WorksheetInfoWithoutPA
	{
		private MaximumAllowPowerFlow _maximumAllowPowerFlow;
		private List<ImbalanceAndAutomatics> _maximumAllowPowerFlowNonBalance;
		private AllowPowerOverflows _allowPowerOverflow;

		public WorksheetInfoWithoutPA(string repairScheme, int noRegularOscilation, List<ControlAction> NBinSample,
			List<ImbalanceDataSource> imbalanceDataSources,	ExcelWorksheet excelWorksheetPARUS)
		{
			Inizialize();
			int startRow;
			if (repairScheme == "Нормальная схема")
			{
				startRow = 9;
			}
			else
			{
				startRow = FindScheme(repairScheme, excelWorksheetPARUS) + 1;
			}
			
			MainMethod(startRow, NBinSample, imbalanceDataSources, noRegularOscilation, excelWorksheetPARUS);
		}

		public MaximumAllowPowerFlow MaximumAllowPowerFlow => _maximumAllowPowerFlow;


		public List<ImbalanceAndAutomatics> MaximumAllowPowerFlowNonBalance => _maximumAllowPowerFlowNonBalance;

		public AllowPowerOverflows AllowPowerOverflow => _allowPowerOverflow;

		private void Inizialize()
		{
			ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
		}

		private void MainMethod(int startRow, List<ControlAction> NBinSample, 
			List<ImbalanceDataSource> imbalanceDataSources,	int noRegularOscilation, ExcelWorksheet excelWorksheetPARUS)
		{
			_allowPowerOverflow = NormalSchemeResults(startRow, excelWorksheetPARUS);
			int headRow = startRow + 3;
			if (excelWorksheetPARUS.Cells[headRow, 1].Value != null)
			{
				var bodyRowsAfterFault = FindBodyRowDisturbance(headRow, excelWorksheetPARUS);
				List<(string, List<int>)> disturbances = new List<(string, List<int>)>();
				List<(string, List<int>)> disturbancesWithControlAction = new List<(string, List<int>)>();
				for (int i = 0; i < bodyRowsAfterFault.Count; i++)
				{
					if (IsDisturbanceConsiderImbalance(bodyRowsAfterFault[i].Item1, NBinSample, imbalanceDataSources))
					{
						disturbancesWithControlAction.Add(bodyRowsAfterFault[i]);
					}
					else
					{
						disturbances.Add(bodyRowsAfterFault[i]);
					}
				}
				MaximumAllowPowerFlowDefine(headRow, disturbances, excelWorksheetPARUS);
				MaximumAllowPowerFlowControlActionDefine(headRow, noRegularOscilation,
					NBinSample, disturbancesWithControlAction, imbalanceDataSources, excelWorksheetPARUS);
			}
		}

		private void MaximumAllowPowerFlowDefine(int headRow, List<(string, List<int>)> disturbances, 
			ExcelWorksheet excelWorksheetPARUS)
		{
			_maximumAllowPowerFlow = new MaximumAllowPowerFlow();
			_maximumAllowPowerFlow.EmergencyAllowPowerFlowValue = _allowPowerOverflow.EmergencyAllowPowerOverflow;
			_maximumAllowPowerFlow.EmergencyAllowPowerCriterion = "8% P исходная схема";
						
			foreach ((string, List<int>) disturbance in disturbances)
			{
				
				var emergency = MaximumAllowPowerFlowDefinition(headRow, disturbance, excelWorksheetPARUS);

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
							_maximumAllowPowerFlow.MaximumAllowPowerCriterion = DefiningCriteria(i, emergency.CurrentLoadLinesCriterion, disturbance.Item1);
						}
					}
				}
			}		
		}

		private void MaximumAllowPowerFlowControlActionDefine(int headRow, int noRegularOscilation, 
			List<ControlAction> controlActionsInSample, List<(string, List<int>)> disturbanceWithControlAction,
			List<ImbalanceDataSource> imbalanceDataSource,ExcelWorksheet excelWorksheetPARUS)
		{
			_maximumAllowPowerFlowNonBalance = new List<ImbalanceAndAutomatics>();

			foreach ((string, List<int>) bodyRow in disturbanceWithControlAction)
			{
				ImbalanceAndAutomatics imbalance = new ImbalanceAndAutomatics();
				imbalance.ImbalanceID = bodyRow.Item1;
				//нужна другая проверка
				if (disturbanceWithControlAction.Count == controlActionsInSample.Count)
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

				var controlAction = FindRightControlAction(imbalanceDataSource, bodyRow.Item1);
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


		private ControlAction FindRightControlAction(List<ImbalanceDataSource> imbalanceDataSource, string disturbance)
		{
			for (int i = 0; i < imbalanceDataSource.Count; i++)
			{
				if (disturbance == imbalanceDataSource[i].LineName)
				{
					return imbalanceDataSource[i].ControlAction;
				}
			}
			throw new Exception($"УВ {disturbance} нет в файле шаблона ");
		}

		private string DefiningCriteria(int index, string currentCriteria, string disturbanceName)
		{
			switch (index)
			{
				case 0:
					{
						return $"АДТН '{currentCriteria}' ПАР '{disturbanceName}'";
					}
				case 1:
					{
						return $"8%P ПАР '{disturbanceName}'";
					}
				case 2:
					{
						return $"10%U ПАР '{disturbanceName}'";
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
				
				allowPowerOverflows.CurrentLoadLinesValue = int.Parse(RoundAndMultiply(FindCellValue(startRow, startRow + 1, columnsNameBeforeFault[0], excelWorksheetPARUS),1));
			}
			catch { }
			try
			{
				allowPowerOverflows.CurrentLoadLinesCriterion = FindCellValue(startRow, startRow + 1, columnsNameBeforeFault[1], excelWorksheetPARUS);
			}
			catch { }
			try
			{
				allowPowerOverflows.StaticStabilityNormal = int.Parse(RoundAndMultiply(FindCellValue(startRow, startRow + 1, columnsNameBeforeFault[2], excelWorksheetPARUS),1));
			}
			catch { }
			try
			{
				allowPowerOverflows.StabilityVoltageValue = int.Parse(RoundAndMultiply(FindCellValue(startRow, startRow + 1, columnsNameBeforeFault[3], excelWorksheetPARUS),1));
			}
			catch { }
			try
			{
				allowPowerOverflows.StabilityVoltageCriterion = FindCellValue(startRow, startRow + 1, columnsNameBeforeFault[4], excelWorksheetPARUS);
			}
			catch { }
			try
			{
				allowPowerOverflows.CriticalValue = int.Parse(RoundAndMultiply(FindCellValue(startRow, startRow + 1, columnsNameBeforeFault[5], excelWorksheetPARUS),1));
			}
			catch { }
			
			allowPowerOverflows.EmergencyAllowPowerOverflow = int.Parse(RoundAndMultiply((allowPowerOverflows.CriticalValue).ToString(),0.92));

			return allowPowerOverflows;
		}

		private AllowPowerOverflows MaximumAllowPowerFlowDefinition(int headRow,
			(string, List<int>) bodyRowAfterFault, ExcelWorksheet excelWorksheetPARUS)
		{
			var outputValue = DisconnectionLineFact(headRow, bodyRowAfterFault.Item2, excelWorksheetPARUS);
			outputValue.DisconnectionLineFact = bodyRowAfterFault.Item1;
			return outputValue;
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
					
					valuesAfterFaultTmp.StaticStabilityPostEmergency = int.Parse(RoundAndMultiply(FindCellValue(headRow, bodyRowAfterFault[valueNumber],
							columnsNameAfterFault[2], excelWorksheetPARUS),1));
					if (valuesAfterFault.StaticStabilityPostEmergency > valuesAfterFaultTmp.StaticStabilityPostEmergency ||
						valuesAfterFault.StaticStabilityPostEmergency == 0)
					{
						valuesAfterFault.StaticStabilityPostEmergency = valuesAfterFaultTmp.StaticStabilityPostEmergency;
					}
				}
				catch { }
				try
				{
					valuesAfterFaultTmp.StabilityVoltageValue = int.Parse(RoundAndMultiply(FindCellValue(headRow, bodyRowAfterFault[valueNumber],
							columnsNameAfterFault[3], excelWorksheetPARUS),1));
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
						valuesAfterFaultTmp.CurrentLoadLinesValue = int.Parse(RoundAndMultiply(FindCellValue(headRow, bodyRowAfterFault[valueNumber],
								columnsNameAfterFault[0], excelWorksheetPARUS),1));
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

			string outputValue ="";

			if (excelWorksheetPARUS.Cells[bodyRow, columnIndex].Value != null)
			{
				outputValue = excelWorksheetPARUS.Cells[bodyRow, columnIndex].Value.ToString();
			}

			return outputValue;
				
					
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

		private List<(string, List<int>)> FindBodyRowDisturbance(int headRow, ExcelWorksheet excelWorksheetPARUS)
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

		private bool IsDisturbanceConsiderImbalance(string afterFault, List<ControlAction> controlActionInSample, 
			List<ImbalanceDataSource> imbalanceDataSources)
		{
			foreach(ImbalanceDataSource imbalance in imbalanceDataSources)
			{
				if(imbalance.LineName == afterFault)
				{
					foreach(ControlAction controlAction in controlActionInSample)
					{
						if (imbalance.ImbalanceName == controlAction.ParamID)
						{
							return true;
						}
					}
				}
			}
			return false;
		}
	}
}
