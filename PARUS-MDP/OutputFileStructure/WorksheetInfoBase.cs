using System;
using System.Collections.Generic;
using OfficeOpenXml;
using DataTypes;

namespace OutputFileStructure
{
	/// <summary>
	/// Класс необходимый для поиска информации из рабочего листа файла ПК ПАРУС
	/// </summary>
	public class WorksheetInfoBase
	{

		private protected List<string> _errorList;

		/// <summary>
		/// Список ошибок
		/// </summary>
		public List<string> ErrorList => _errorList;

		private protected List<(string, List<int>)> ConsideredDisturbances(List<(string, List<int>)> bodyRowsAfterFault, List<(string, bool)> disturbanceDataSource)
		{
			for (int i = 0; i < bodyRowsAfterFault.Count; i++)
			{
				if (!IsDisturbanceTakeAccount(bodyRowsAfterFault[i].Item1, disturbanceDataSource))
				{
					bodyRowsAfterFault.Remove(bodyRowsAfterFault[i]);
					i--;
				}
			}
			return bodyRowsAfterFault;
		}

		private protected void FindAbsentDisturbance(List<(string, List<int>)> bodyRowsAfterFault, List<(string, bool)> disturbanceDataSource, string path)
		{
			if(disturbanceDataSource.Count > bodyRowsAfterFault.Count)
			{
				foreach((string, bool) disturbance in disturbanceDataSource)
				{
					string[] disturbanceArray= disturbance.Item1.Split(" ");
					string disturbanceTmp = "";
					for(int i = 1; i< disturbanceArray.Length; i++)
					{
						disturbanceTmp += disturbanceArray[i];
					}
					bool flag = true;
					foreach((string, List<int>) bodyRow in bodyRowsAfterFault)
					{
						if(Comparator.CompareString(disturbanceTmp,bodyRow.Item1))
						{
							flag =false;
						}
					}
					
					if(flag)
					{
						string error = $"В файле {path} нет информации о возмущении {disturbance.Item1}";
						_errorList.Add(error);
					}
				}
			}
			
		}
		private protected ControlActionRow FindRightControlAction(List<Imbalance> imbalance, string disturbance)
		{
			for (int i = 0; i < imbalance.Count; i++)
			{
				if (Comparator.CompareString(disturbance, imbalance[i].LineName))
				{
					return imbalance[i].ImbalanceValue;
				}
			}
			throw new Exception($"УВ для {disturbance} нет в файле шаблона ");
		}

		private protected string DefiningCriteria(int index, string currentCriteria, string disturbanceName)
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

		private protected AllowPowerOverflows MaximumAllowPowerFlowDefinition(int headRow,
			(string, List<int>) bodyRowAfterFault, ExcelWorksheet excelWorksheetPARUS)
		{
			var outputValue = DisconnectionLineFact(headRow, bodyRowAfterFault.Item2, excelWorksheetPARUS);
			outputValue.DisconnectionLineFact = bodyRowAfterFault.Item1;
			return outputValue;
		}

		private protected AllowPowerOverflows DisconnectionLineFact(int headRow,
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

		private protected string FindCellValue(int headRow, int bodyRow, string columnName, ExcelWorksheet excelWorksheetPARUS)
		{
			var columnIndex = FindColumn(headRow, columnName, excelWorksheetPARUS);

			string outputValue = "";

			if (excelWorksheetPARUS.Cells[bodyRow, columnIndex].Value != null)
			{
				outputValue = excelWorksheetPARUS.Cells[bodyRow, columnIndex].Value.ToString();
			}

			return outputValue;


		}

		private protected int FindColumn(int headRow, string columnName, ExcelWorksheet excelWorksheetPARUS)
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

		private protected int FindScheme(string repairScheme, ExcelWorksheet excelWorksheetPARUS)
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
			return -1;
		}

		private protected int FindRow(string textInColumn, int startRow, int column, ExcelWorksheet excelWorksheetPARUS)
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

		private protected string RoundAndMultiply(string text, double multiplier)
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

		private protected List<(string, List<int>)> FindBodyRowDisturbance(int headRow, ExcelWorksheet excelWorksheetPARUS)
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

		private protected bool IsDisturbanceConsiderImbalance(string afterFault, List<Imbalance> imbalances)
		{
			if (imbalances == null) return false;

			foreach (Imbalance imbalance in imbalances)
			{
				if (Comparator.CompareString(imbalance.LineName, afterFault))
				{
					return true;
				}
			}
			return false;
		}

		private protected bool IsDisturbanceTakeAccount(string afterFault, List<(string, bool)> disturbances)
		{
			if (disturbances == null) return false;

			foreach ((string,bool) disturbance in disturbances)
			{
				string disturbanceTmp = "";
				if (disturbance.Item1.Contains("ФОЛ"))
				{
					string[] disturbanceArray = disturbance.Item1.Split(" ");

					for (int i = 1; i < disturbanceArray.Length; i++)
					{
						disturbanceTmp += disturbanceArray[i] + " ";
					}
				}
				else
				{
					disturbanceTmp = disturbance.Item1;
				}
				if (Comparator.CompareString(disturbanceTmp, afterFault))
				{
					return true;
				}
			}
			return false;
		}
	}
}
