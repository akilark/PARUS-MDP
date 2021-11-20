using System;
using System.Collections.Generic;
using System.Text;
using OfficeOpenXml;

namespace OutputFileStructure
{
	public class WorksheetInfoWithtPA
	{
		private AllowPowerFlowPA _maximumAllowPowerFlowPA;
		private List<ImbalanceAndAutomatics> _maximumAllowPowerFlowNonBalancePA;

		public WorksheetInfoWithtPA(string repairScheme, int noRegularOscilation, AllowPowerOverflows allowPowerOverflow, List<ControlAction> NBinSample, List<ControlAction> LAPNYinSample,
			ExcelWorksheet excelWorksheetPARUS)
		{
			ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
			_maximumAllowPowerFlowPA = new AllowPowerFlowPA();
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

			MainMethod(startRow, NBinSample, LAPNYinSample, noRegularOscilation, excelWorksheetPARUS);
		}

		public AllowPowerFlowPA AllowPowerFlowPA => _maximumAllowPowerFlowPA;
		public List<ImbalanceAndAutomatics> imbalances => _maximumAllowPowerFlowNonBalancePA;

		private void MainMethod(int startRow, List<ControlAction> NBinSample, List<ControlAction> LAPNYinSample,
			int noRegularOscilation, ExcelWorksheet excelWorksheetPARUS)
		{
			int headRow = startRow + 3;
			if (excelWorksheetPARUS.Cells[headRow, 1].Value != null)
			{
				var bodyRowsAfterFault = FindBodyRowDisconnectionLineFact(headRow, excelWorksheetPARUS);
				List<(string, List<int>)> disconnectionLineFacts = new List<(string, List<int>)>();
				List<(string, List<int>)> disconnectionLineFactsWithControlAction = new List<(string, List<int>)>();
				for (int i = 0; i < bodyRowsAfterFault.Count; i++)
				{
					if (IsInSample(bodyRowsAfterFault[i].Item1, NBinSample))
					{
						disconnectionLineFactsWithControlAction.Add(bodyRowsAfterFault[i]);
					}
					else
					{
						disconnectionLineFacts.Add(bodyRowsAfterFault[i]);
					}
				}
				MaximumAllowPowerFlowDefineWithPA(headRow, disconnectionLineFacts, LAPNYinSample, excelWorksheetPARUS);


			}
		}

		private void MaximumAllowPowerFlowDefineWithPA(int headRow, List<(string, List<int>)> disconnectionLineFacts,
			List<ControlAction> LAPNYinSample, ExcelWorksheet excelWorksheetPARUS)
		{
			AllowPowerOverflows allowPowerOverflowsLAPNY = new AllowPowerOverflows();
			AllowPowerOverflows allowPowerOverflowsMDPwithPA = new AllowPowerOverflows();
			
			foreach ((string, List<int>) disconnectionLineFact in disconnectionLineFacts)
			{
				var emergency = MaximumAllowPowerFlowDefinition(headRow, disconnectionLineFact, excelWorksheetPARUS);

				for (int valueNumber = 0; valueNumber < disconnectionLineFact.Item2.Count; valueNumber++)
				{
					string equpmentOverloadingOrVoltageLimiting = FindCellValue(headRow, disconnectionLineFact.Item2[valueNumber],
								"АОПО/АОСН №", excelWorksheetPARUS);
					string textValue = DefineTextValue(equpmentOverloadingOrVoltageLimiting);
					if (textValue == FindCellValue(headRow, disconnectionLineFact.Item2[valueNumber],
								"Перегружаемый элемент", excelWorksheetPARUS))
					{
						if (equpmentOverloadingOrVoltageLimiting.Contains("АОПО"))
						{
							if (FindCellValue(headRow, disconnectionLineFact.Item2[valueNumber], "Примечание", excelWorksheetPARUS) != "АОПО" &&
								FindCellValue(headRow, disconnectionLineFact.Item2[valueNumber], "Примечание", excelWorksheetPARUS) != "Токовых перегрузов нет")
							{
								int currentValueTmp = int.Parse(RoundAndMultiply(
									FindCellValue(headRow, disconnectionLineFact.Item2[valueNumber], "Рсеч-Рно, МВт", excelWorksheetPARUS), 1));
								if (_maximumAllowPowerFlowPA.EqupmentOverloadingWithoutPA == 0 ||
									(_maximumAllowPowerFlowPA.EqupmentOverloadingWithoutPA > currentValueTmp && currentValueTmp > 0))
								{
									_maximumAllowPowerFlowPA.EqupmentOverloadingWithoutPA = currentValueTmp;
									_maximumAllowPowerFlowPA.CriteriumEqupmentOverloadingWithoutPA = FindCellValue(headRow, disconnectionLineFact.Item2[valueNumber],
																	"Перегружаемый элемент", excelWorksheetPARUS);
									_maximumAllowPowerFlowPA.DisconnectionLineFactEqupmentOverloading = disconnectionLineFact.Item1;
									_maximumAllowPowerFlowPA.CriteriumEqupmentOverloadingWithtPA = $"АДТН '{_maximumAllowPowerFlowPA.CriteriumEqupmentOverloadingWithoutPA}'" +
										$" ПАР '{_maximumAllowPowerFlowPA.DisconnectionLineFactEqupmentOverloading}' с учетом объема УВ";
								}

							}


						}
						else if (equpmentOverloadingOrVoltageLimiting.Contains("АОСН"))
						{
							if (FindCellValue(headRow, disconnectionLineFact.Item2[valueNumber], "Примечание", excelWorksheetPARUS) != "АОПО" &&
								FindCellValue(headRow, disconnectionLineFact.Item2[valueNumber], "Рда(Uкр/0.9)-Рно", excelWorksheetPARUS) != "Критерий по U не достижим")
							{
								int voltageValueTmp = int.Parse(RoundAndMultiply(
								FindCellValue(headRow, disconnectionLineFact.Item2[valueNumber], "Рда(Uкр/0.9)-Рно", excelWorksheetPARUS), 1));
								if (_maximumAllowPowerFlowPA.VoltageLimitingWithoutPA == 0 ||
									(_maximumAllowPowerFlowPA.VoltageLimitingWithoutPA > voltageValueTmp && voltageValueTmp > 0))
								{
									_maximumAllowPowerFlowPA.VoltageLimitingWithoutPA = voltageValueTmp;
									_maximumAllowPowerFlowPA.DisconnectionLineFactVoltageLimiting = disconnectionLineFact.Item1;
									_maximumAllowPowerFlowPA.CriteriumVoltageLimitingWithtPA = $"10% U ПАР '{_maximumAllowPowerFlowPA.DisconnectionLineFactVoltageLimiting}'" +
										$" с учетом объема УВ";
								}
							}
						}
					}

					bool flagLAPNY = false;
					foreach (ControlAction controlAction in LAPNYinSample)
					{
						if (controlAction.ParamID == textValue)
						{
							flagLAPNY = true;
							break;
						}
					}

					if (flagLAPNY)
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
										DefiningCriteria(i, emergency.CurrentLoadLinesCriterion, disconnectionLineFact.Item1) + " с учетом объема УВ";
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
									_maximumAllowPowerFlowPA.CriteriumValueWithPA = DefiningCriteria(i, emergency.CurrentLoadLinesCriterion, disconnectionLineFact.Item1);
								}
							}
						}
					}
				}


			}
		}
		private string DefineTextValue(string equpmentOverloadingOrVoltageLimiting)
		{
			int start = equpmentOverloadingOrVoltageLimiting.IndexOf("АО");
			if (start <= 0)
			{
				return "";
			}
			return equpmentOverloadingOrVoltageLimiting.Substring(start + 4).Trim();
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
		private bool IsInSample(string afterFault, List<ControlAction> controlActionInSample)
		{
			foreach (ControlAction controlAction in controlActionInSample)
			{
				if (controlAction.ParamID == afterFault)
				{
					return true;
				}
			}
			return false;
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
