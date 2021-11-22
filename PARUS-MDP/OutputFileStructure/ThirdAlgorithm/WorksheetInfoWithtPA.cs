using System;
using System.Collections.Generic;
using System.Text;
using OfficeOpenXml;
using OutputFileStructure.DataTypes;

namespace OutputFileStructure
{
	public class WorksheetInfoWithtPA
	{
		private AllowPowerFlowPA _maximumAllowPowerFlowPA;
		private List<ImbalanceAndAutomatics> _maximumAllowPowerFlowNonBalancePA;

		public WorksheetInfoWithtPA(string repairScheme, int noRegularOscilation, AllowPowerOverflows allowPowerOverflow, List<ControlAction> NBinSample,
			List<ImbalanceDataSource> imbalanceDataSource, ExcelWorksheet excelWorksheetPARUS, List<ImbalanceAndAutomatics> firstAlghorithmResult)
		{
			ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
			_maximumAllowPowerFlowPA = new AllowPowerFlowPA();
			_maximumAllowPowerFlowNonBalancePA = new List<ImbalanceAndAutomatics>();
			_maximumAllowPowerFlowPA.ValueWithPA = allowPowerOverflow.StaticStabilityNormal;
			_maximumAllowPowerFlowPA.CriteriumValueWithPA = "20% P исходная схема";
			
			int startRow;
			if (repairScheme == "Нормальная схема")
			{
				startRow = 9;
			}
			else
			{
				startRow = FindScheme(repairScheme, excelWorksheetPARUS) + 1;
			}

			MainMethod(startRow, NBinSample, imbalanceDataSource,noRegularOscilation, allowPowerOverflow, excelWorksheetPARUS);
		}

		public AllowPowerFlowPA AllowPowerFlowPA => _maximumAllowPowerFlowPA;
		public List<ImbalanceAndAutomatics> imbalances => _maximumAllowPowerFlowNonBalancePA;

		private void MainMethod(int startRow, List<ControlAction> NBinSample,
			List<ImbalanceDataSource> imbalanceDataSource,	int noRegularOscilation, AllowPowerOverflows allowPowerOverflow, ExcelWorksheet excelWorksheetPARUS)
		{
			int headRow = startRow + 3;
			if (excelWorksheetPARUS.Cells[headRow, 1].Value != null)
			{
				var bodyRowsAfterFault = FindBodyRowDisturbance(headRow, excelWorksheetPARUS);
				List<(string, List<int>)> disturbances = new List<(string, List<int>)>();
				List<(string, List<int>)> disturbancesWithControlAction = new List<(string, List<int>)>();
				for (int i = 0; i < bodyRowsAfterFault.Count; i++)
				{
					if (IsDisturbanceConsiderImbalance(bodyRowsAfterFault[i].Item1, NBinSample, imbalanceDataSource))
					{
						disturbancesWithControlAction.Add(bodyRowsAfterFault[i]);
					}
					else
					{
						disturbances.Add(bodyRowsAfterFault[i]);
					}
				}
				MaximumAllowPowerFlowDefineWithPA(headRow, disturbances, imbalanceDataSource, excelWorksheetPARUS);


			}
		}

		private void MaximumAllowPowerFlowDefineWithPA(int headRow, List<(string, List<int>)> disturbances,
			List<ImbalanceDataSource> imbalanceDataSource, ExcelWorksheet excelWorksheetPARUS)
		{
			foreach ((string, List<int>) disturbance in disturbances)
			{
				
				var emergency = MaximumAllowPowerFlowDefinition(headRow, disturbance, excelWorksheetPARUS);

				for (int valueNumber = 0; valueNumber < disturbance.Item2.Count; valueNumber++)
				{
					if (FindCellValue(headRow, disturbance.Item2[valueNumber], "Примечание", excelWorksheetPARUS) == "АОПО")
					{
						continue;
					}
					string textValueCurrent = FindCellValue(headRow, disturbance.Item2[valueNumber],
								"Перегружаемый элемент", excelWorksheetPARUS);
					var controlActionAOPO = ControlActionsForAutomatic(textValueCurrent, imbalanceDataSource, "АОПО");
					if (controlActionAOPO.Count > 0)
					{
						if (FindCellValue(headRow, disturbance.Item2[valueNumber], "Примечание", excelWorksheetPARUS) != "Токовых перегрузов нет")
						{
							int currentValueTmp = int.Parse(RoundAndMultiply(
								FindCellValue(headRow, disturbance.Item2[valueNumber], "Рсеч-Рно, МВт", excelWorksheetPARUS), 1));
							if (_maximumAllowPowerFlowPA.EqupmentOverloadingWithoutPA == 0 ||
								(_maximumAllowPowerFlowPA.EqupmentOverloadingWithoutPA > currentValueTmp && currentValueTmp > 0))
							{
								_maximumAllowPowerFlowPA.EqupmentOverloadingWithoutPA = currentValueTmp;
								_maximumAllowPowerFlowPA.CriteriumEqupmentOverloadingWithoutPA = FindCellValue(headRow, disturbance.Item2[valueNumber],
																"Перегружаемый элемент", excelWorksheetPARUS);
								_maximumAllowPowerFlowPA.DisconnectionLineFactEqupmentOverloading = disturbance.Item1;
								_maximumAllowPowerFlowPA.CriteriumEqupmentOverloadingWithtPA = $"АДТН '{_maximumAllowPowerFlowPA.CriteriumEqupmentOverloadingWithoutPA}'" +
									$" ПАР '{_maximumAllowPowerFlowPA.DisconnectionLineFactEqupmentOverloading}' с учетом объема УВ";
								_maximumAllowPowerFlowPA.ControlActionAOPO = controlActionAOPO[0];
							}
						}
					}
					string textValueVoltage = FindCellValue(headRow, disturbance.Item2[valueNumber],
							"Узел", excelWorksheetPARUS);
					var controlActionAOCN = ControlActionsForAutomatic(textValueVoltage, imbalanceDataSource, "АОСН");
					if (controlActionAOCN.Count > 0)
						{
							if (FindCellValue(headRow, disturbance.Item2[valueNumber], "Рда(Uкр/0.9)-Рно", excelWorksheetPARUS) != "Критерий по U не достижим")
							{
								int voltageValueTmp = int.Parse(RoundAndMultiply(
								FindCellValue(headRow, disturbance.Item2[valueNumber], "Рда(Uкр/0.9)-Рно", excelWorksheetPARUS), 1));
								if (_maximumAllowPowerFlowPA.VoltageLimitingWithoutPA == 0 ||
									(_maximumAllowPowerFlowPA.VoltageLimitingWithoutPA > voltageValueTmp && voltageValueTmp > 0))
								{
									_maximumAllowPowerFlowPA.VoltageLimitingWithoutPA = voltageValueTmp;
									_maximumAllowPowerFlowPA.DisconnectionLineFactVoltageLimiting = disturbance.Item1;
									_maximumAllowPowerFlowPA.CriteriumVoltageLimitingWithtPA = $"10% U ПАР '{_maximumAllowPowerFlowPA.DisconnectionLineFactVoltageLimiting}'" +
										$" с учетом объема УВ";
									_maximumAllowPowerFlowPA.ControlActionAOCN = controlActionAOCN[0];
								}
							}
					}

					
					var controlActionLAPNY = ControlActionsForAutomatic(disturbance.Item1, imbalanceDataSource, "ЛАПНУ");

					if (controlActionLAPNY.Count > 0)
					{
						List<int> criteria = new List<int> { emergency.CurrentLoadLinesValue,
						emergency.StaticStabilityPostEmergency, emergency.StabilityVoltageValue };
						for (int i = 0; i < criteria.Count; i++)
						{
							if (criteria[i] > 0)
							{
								if (_maximumAllowPowerFlowPA.LocalAutomaticValueWitoutPA == 0 ||
									_maximumAllowPowerFlowPA.LocalAutomaticValueWitoutPA > criteria[i])
								{
									_maximumAllowPowerFlowPA.LocalAutomaticValueWitoutPA = criteria[i];
									_maximumAllowPowerFlowPA.CriteriumLocalAutomaticValueWithoutPA =
										DefiningCriteria(i, emergency.CurrentLoadLinesCriterion, disturbance.Item1) + " с учетом объема УВ";
									_maximumAllowPowerFlowPA.ControlActionsLAPNY = controlActionLAPNY;
								}
							}
						}
					}
					else
					{
						List<int> criteria = new List<int> { emergency.CurrentLoadLinesValue,
						emergency.StaticStabilityPostEmergency, emergency.StabilityVoltageValue, emergency.StaticStabilityNormal };
						for (int i = 0; i < criteria.Count; i++)
						{
							if (criteria[i] > 0)
							{
								if (_maximumAllowPowerFlowPA.ValueWithPA == 0 ||
									_maximumAllowPowerFlowPA.ValueWithPA > criteria[i])
								{
									_maximumAllowPowerFlowPA.ValueWithPA = criteria[i];
									_maximumAllowPowerFlowPA.CriteriumValueWithPA = DefiningCriteria(i, emergency.CurrentLoadLinesCriterion, disturbance.Item1);
								}
							}
						}
					}
				}


			}
		}

		private void MaximumAllowPowerFlowControlActionDefineWithPA(int headRow, int noRegularOscilation,
			List<ControlAction> controlActionsInSample, List<(string, List<int>)> disturbanceWithControlAction,
			List<ImbalanceDataSource> imbalanceDataSource, AllowPowerOverflows allowPowerOverflow, List<ImbalanceAndAutomatics> firstAlghorithmResult, ExcelWorksheet excelWorksheetPARUS)
		{
			_maximumAllowPowerFlowNonBalancePA = new List<ImbalanceAndAutomatics>();

			foreach ((string, List<int>) bodyRow in disturbanceWithControlAction)
			{
				
				ImbalanceDataSource imbalanceFromDataSource = new ImbalanceDataSource();
				for(int i = 0; i < imbalanceDataSource.Count; i ++)
				{
					if(bodyRow.Item1 == imbalanceDataSource[i].LineName)
					{
						imbalanceFromDataSource = imbalanceDataSource[i];
						break;
					}
				}
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
								if (imbalanceFromDataSource.ARPM != null)
								{
									imbalance.ImbalanceCriterion = imbalance.ImbalanceCriterion + "с учетом объема УВ";
								}

							}
						}
					}
				}
				else
				{
					imbalance.ImbalanceValue = allowPowerOverflow.EmergencyAllowPowerOverflow - noRegularOscilation;
					imbalance.ImbalanceCriterion = $"8%P ПАР '{bodyRow.Item1}'";
					if(imbalanceFromDataSource.ARPM != null)
					{
						imbalance.ImbalanceCriterion = imbalance.ImbalanceCriterion + "с учетом объема УВ";
					}
				}

				foreach(ImbalanceAndAutomatics imbalanceAndAutomatics in firstAlghorithmResult)
				{
					imbalance.ImbalanceCoefficient = imbalanceFromDataSource.Imbalance.ImbalanceCoefficient;
					imbalance.MaximumImbalance = imbalanceFromDataSource.Imbalance.MaximumImbalance;
					imbalance.Equation = imbalance.ImbalanceValue.ToString() + "-" +
						imbalance.ImbalanceCoefficient.ToString() + "*" + bodyRow.Item1;
					imbalance.EquationValue = imbalance.ImbalanceValue - imbalance.ImbalanceCoefficient * imbalance.MaximumImbalance;
					if (imbalanceAndAutomatics.ImbalanceID == imbalance.ImbalanceID)
					{
						_maximumAllowPowerFlowNonBalancePA.Add(imbalance);
					}
				}
			}
		}

		private bool IsDisturbanceConsiderAutomatics(string LineName, List<ImbalanceDataSource> imbalanceDataSources,
			string containsText)
		{
			foreach (ImbalanceDataSource imbalance in imbalanceDataSources)
			{
				if (imbalance.LineName == LineName && imbalance.ImbalanceName.Contains(containsText))
				{
					return true;

				}
			}
			return false;
		}

		private List<ControlAction> ControlActionsForAutomatic(string LineName, List<ImbalanceDataSource> imbalanceDataSources,
			string containsText)
		{
			List<ControlAction> controlActionsOutput = new List<ControlAction>();
			foreach(ImbalanceDataSource imbalanceDataSource in imbalanceDataSources)
			{
				if(IsDisturbanceConsiderAutomatics(LineName, new List<ImbalanceDataSource> {imbalanceDataSource }, containsText))
				{
					controlActionsOutput.Add(imbalanceDataSource.ControlAction);
				}
			}
			return controlActionsOutput;
		}

		private string FindCellValue(int headRow, int bodyRow, string columnName, ExcelWorksheet excelWorksheetPARUS)
		{
			var columnIndex = FindColumn(headRow, columnName, excelWorksheetPARUS);

			string outputValue = "";

			if (excelWorksheetPARUS.Cells[bodyRow, columnIndex].Value != null)
			{
				outputValue = excelWorksheetPARUS.Cells[bodyRow, columnIndex].Value.ToString();
			}

			return outputValue;


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
				if (schemeName.Length > repairSchemeSplit[repairSchemeSplit.Length - 1].Length)
				{
					schemeName = repairSchemeSplit[repairSchemeSplit.Length - 1];
				}
			}
			int rowNumber = FindRow("Схема Ремонта", 9, 1, excelWorksheetPARUS);
			while (rowNumber != 0)
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
			if (double.TryParse(text, out number))
			{
				return Math.Round(Math.Round(number) * multiplier).ToString();
			}
			else
			{
				return text;
			}

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
							columnsNameAfterFault[2], excelWorksheetPARUS), 1));
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
							columnsNameAfterFault[3], excelWorksheetPARUS), 1));
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
								columnsNameAfterFault[0], excelWorksheetPARUS), 1));
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
		private bool IsDisturbanceConsiderImbalance(string afterFault, List<ControlAction> controlActionInSample,
			List<ImbalanceDataSource> imbalanceDataSources)
		{
			foreach (ImbalanceDataSource imbalance in imbalanceDataSources )
			{
				if (imbalance.LineName == afterFault && imbalance.ImbalanceName.Contains("НБ"))
				{
					foreach (ControlAction controlAction in controlActionInSample)
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
		private List<(string, List<int>)> FindBodyRowDisturbance(int headRow, ExcelWorksheet excelWorksheetPARUS)
		{
			var outputList = new List<(string, List<int>)>();

			if (excelWorksheetPARUS.Cells[headRow, 1].Value.ToString() != "Послеаварийный режим")
			{
				return outputList;
			}
			object cellValue = excelWorksheetPARUS.Cells[headRow + 1, 1].Value;
			int index = 1;
			while (cellValue != null)
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
				if (cellValue == null)
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
	}

}
