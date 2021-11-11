using System;
using System.Collections.Generic;
using System.Text;
using OfficeOpenXml;

namespace OutputFileStructure
{
	public class WorkSheetInfo
	{
		private string[] _maximumAllowPowerFlow;
		private List<string[]> _maximumAllowPowerFlowNonBalance;
		private List<float> _equationNonBalance;

		public WorkSheetInfo(int workSheetNumber, string repairScheme, string[] nonBalances, ExcelPackage excelPackagePARUS)
		{
			Inizialize();
			int startRow = FindScheme(repairScheme, workSheetNumber, excelPackagePARUS) + 1;
			MaximumAllowPowerFlowDefine(startRow, workSheetNumber, nonBalances, excelPackagePARUS);
		}
		public WorkSheetInfo(int workSheetNumber, string[] nonBalances, ExcelPackage excelPackagePARUS)
		{
			Inizialize();
			int startRow = 9;
			MaximumAllowPowerFlowDefine(startRow, workSheetNumber,nonBalances, excelPackagePARUS);
		}
		/// <summary>
		/// 0- Значение МДП без ПА;
		/// 1- Критерий определения МДП без ПА;
		/// 2- Значение АДП;
		/// 3- Критерий определения АДП без ПА;
		/// </summary>
		public string[] MaximumAllowPowerFlow => _maximumAllowPowerFlow;

		/// <summary>
		/// 
		/// </summary>
		public List<string[]> MaximumAllowPowerFlowNonBalance => _maximumAllowPowerFlowNonBalance;

		public List<float> EquationNonBalance => _equationNonBalance;

		private void Inizialize()
		{
			ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
		}

		private void MaximumAllowPowerFlowDefine(int startRow, int workSheetNumber, string[] nonBalances, ExcelPackage excelPackagePARUS)
		{
			_maximumAllowPowerFlow = new string[4];
			var normal = NormalSchemeResults(startRow, workSheetNumber, excelPackagePARUS);
			_maximumAllowPowerFlow[2] = normal[6];
			_maximumAllowPowerFlow[3] = "8% P исходная схема";

			int headRow = startRow + 3;
			if (excelPackagePARUS.Workbook.Worksheets[workSheetNumber].Cells[headRow, 1].Value != null)
			{
				var bodyRowsAfterFault = FindBodyRowAfterFault(headRow, workSheetNumber, excelPackagePARUS);
				List<(string, List<int>)> maximumAllowFlow = new List<(string, List<int>)>();
				for (int i = 0; i < bodyRowsAfterFault.Count; i++)
				{
					if (!IsInSample(bodyRowsAfterFault[i].Item1, nonBalances))
					{
						maximumAllowFlow.Add(bodyRowsAfterFault[i]);
					}
				}
				foreach ((string, List<int>) bodyRow in maximumAllowFlow)
				{
					var emergency = MaximumAllowPowerFlowDefinition(headRow, workSheetNumber,bodyRow, nonBalances, excelPackagePARUS);

					List<string> criteria = new List<string> { emergency[0], emergency[2], emergency[3], normal[2] };
					for(int i = 0; i < criteria.Count; i ++)
					{
						if (int.TryParse(criteria[i], out int criterion))
						{
							if(criterion > 0)
							{
								if (_maximumAllowPowerFlow[0] == null)
								{
									_maximumAllowPowerFlow[0] = criterion.ToString();
									_maximumAllowPowerFlow[1] = DefiningCriteria(i, emergency[1], bodyRow.Item1);
								}
								else
								{
									if (int.Parse(_maximumAllowPowerFlow[0]) > criterion)
									{
										_maximumAllowPowerFlow[0] = criterion.ToString();
										_maximumAllowPowerFlow[1] = DefiningCriteria(i, emergency[1], bodyRow.Item1);
									}
								}
							}
						}
					}
				}
			}				
		}

		private void MaximumAllowPowerFlowNonBalanceDefine(int startRow, int workSheetNumber, 
			string[] nonBalances, int noRegularOscilation, ExcelPackage excelPackagePARUS, List<(string, int, int)> nonBalanceInSample, ExcelPackage excelPackageSample)
		{
			_maximumAllowPowerFlowNonBalance = new List<string[]>();
			int headRow = startRow + 3;
			if (excelPackagePARUS.Workbook.Worksheets[workSheetNumber].Cells[headRow, 1].Value != null)
			{
				var bodyRowsAfterFault = FindBodyRowAfterFault(headRow, workSheetNumber, excelPackagePARUS);
				List<(string, List<int>)> maximumAllowFlowNonBalance = new List<(string, List<int>)>();
				for (int i = 0; i < bodyRowsAfterFault.Count; i++)
				{
					if (IsInSample(bodyRowsAfterFault[i].Item1, nonBalances))
					{
						maximumAllowFlowNonBalance.Add(bodyRowsAfterFault[i]);
					}
				}

				foreach ((string, List<int>) bodyRow in maximumAllowFlowNonBalance)
				{
					int nonBalanceValue=0;
					string nonBalanceCriterion ="";
					if (maximumAllowFlowNonBalance.Count == nonBalances.Length)
					{
						var emergency = MaximumAllowPowerFlowDefinition(headRow, workSheetNumber, bodyRow, nonBalances, excelPackagePARUS);
						List<string> criteria = new List<string> { emergency[0], emergency[2], emergency[3] };
						for (int i = 0; i < criteria.Count; i++)
						{
							if (int.TryParse(criteria[i], out int criterion))
							{
								if (criterion > 0)
								{
									if (nonBalanceValue == 0)
									{
										nonBalanceValue = criterion;
										nonBalanceCriterion = DefiningCriteria(i, emergency[1], bodyRow.Item1);
									}
									else
									{
										if (nonBalanceValue > criterion)
										{
											nonBalanceValue = criterion;
											nonBalanceCriterion = DefiningCriteria(i, emergency[1], bodyRow.Item1);
										}
									}
								}
							}
						}
					}
					else
					{
						var normal = NormalSchemeResults(startRow, workSheetNumber, excelPackagePARUS);

						nonBalanceValue = int.Parse(normal[6]) - noRegularOscilation;
						nonBalanceCriterion = $"8%P ПАР '{bodyRow.Item1}'";
					}

					var efficiencyCoefficient = DefiningEfficiencyCoefficient(nonBalanceInSample, bodyRow.Item1, excelPackageSample);
					if (CompareAllowPowerFlowWithNonBalanceEquation(
						efficiencyCoefficient,
						nonBalanceValue,
						int.Parse(MaximumAllowPowerFlow[0])))
					{
						string[] outputArray = new string[]
						{ nonBalanceValue.ToString() + "-" + efficiencyCoefficient.Item1.ToString() + "*" + bodyRow.Item1,
								nonBalanceCriterion };
						_maximumAllowPowerFlowNonBalance.Add(outputArray);
					}

				}
				
			}
		}

		private bool CompareAllowPowerFlowWithNonBalanceEquation((float, int) efficiencyCoefficient, int nonBalanceValue, int maximumAllowPowerFlow)
		{
			float equationResult = nonBalanceValue - efficiencyCoefficient.Item2 * efficiencyCoefficient.Item1;
			_equationNonBalance.Add(equationResult);
			if(maximumAllowPowerFlow > equationResult)
			{
				return true;
			}
			else
			{
				return false;
			}
		}

		private (float, int) DefiningEfficiencyCoefficient(List<(string, int, int)> nonBalanceInSample, string disconnectionLineFact, 
			ExcelPackage excelPackageSample)
		{
			var worksheet = excelPackageSample.Workbook.Worksheets[1];
			float coefficientNonBalance = 0;
			int activePoverNonBalanceMax = 0;
			for (int i = 0; i < nonBalanceInSample.Count; i++)
			{
				if(nonBalanceInSample[i].Item1 == disconnectionLineFact)
				{
					coefficientNonBalance = 
						float.Parse(worksheet.Cells[nonBalanceInSample[i].Item2, nonBalanceInSample[i].Item3 + 3].Value.ToString());
					activePoverNonBalanceMax = 
						int.Parse(worksheet.Cells[nonBalanceInSample[i].Item2, nonBalanceInSample[i].Item3 + 2].Value.ToString());
				}
			}
			return (coefficientNonBalance, activePoverNonBalanceMax);
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


		private string[] NormalSchemeResults(int startRow, int workSheetNumber, ExcelPackage excelPackagePARUS)
		{

			string[] columnsNameBeforeFault =
				new string[] { "Рсеч-Рно, МВт", "Перегружаемый элемент", "Рпред*0,8-Pно", "Р(Uкр/0.85)-Рно", "Узел", "Рпред" };
			string[] assignedValuesBeforeFault = new string[columnsNameBeforeFault.Length + 1];

			for (int columnBefore = 0; columnBefore < columnsNameBeforeFault.Length; columnBefore++)
			{
				try
				{
					assignedValuesBeforeFault[columnBefore] = RoundAndMultiply(
					FindCellValue(startRow, startRow + 1, columnsNameBeforeFault[columnBefore], workSheetNumber, excelPackagePARUS),
					1);
				}
				catch
				{

				}
			}
			assignedValuesBeforeFault[assignedValuesBeforeFault.Length - 1] =
				RoundAndMultiply(assignedValuesBeforeFault[assignedValuesBeforeFault.Length - 2], 0.92);

			return assignedValuesBeforeFault;
		}

		private string[] MaximumAllowPowerFlowDefinition(int headRow, int workSheetNumber, 
			(string, List<int>) bodyRowAfterFault, string[] paramIdentificator, ExcelPackage excelPackagePARUS)
		{
			var outputArray = DisconnectionLineFact(headRow, workSheetNumber, bodyRowAfterFault.Item2, excelPackagePARUS);
			Array.Resize(ref outputArray, outputArray.Length + 1);
			outputArray[outputArray.Length - 1] = bodyRowAfterFault.Item1;
			return outputArray;
		}

		private string[] DisconnectionLineFact(int headRow, int workSheetNumber,
			List<int> bodyRowAfterFault, ExcelPackage excelPackagePARUS)
		{
			string[] columnsNameAfterFault =
				new string[] { "Рсеч-Рно, МВт", "Перегружаемый элемент", "Рдоав8%-Pно", "Рда(Uкр/0.9)-Рно", "Примечание" };

			string[] valuesAfterFault = new string[columnsNameAfterFault.Length];

			for (int valueNumber = 0; valueNumber < bodyRowAfterFault.Count; valueNumber++)
			{
				string[] valuesAfterFaultTmp = new string[columnsNameAfterFault.Length];
				for (int columnAfter = 0; columnAfter < columnsNameAfterFault.Length; columnAfter++)
				{
					try
					{
						valuesAfterFaultTmp[columnAfter] = FindCellValue(headRow, bodyRowAfterFault[valueNumber],
							columnsNameAfterFault[columnAfter], workSheetNumber, excelPackagePARUS);
					}
					catch
					{

					}
					
				}
				
				for (int i = 0; i < columnsNameAfterFault.Length; i++)
				{
					if (i == 0 && (valuesAfterFaultTmp[4] == "АОПО" ||
						valuesAfterFaultTmp[1] == "Токовых перегрузов нет"))
					{
						continue;
					}
					if (valuesAfterFault[i] == null)
					{
						valuesAfterFault[i] = RoundAndMultiply(valuesAfterFaultTmp[i], 1);
					}

					int valueTmp;
					if (!int.TryParse(RoundAndMultiply(valuesAfterFaultTmp[i], 1), out valueTmp))
					{
						continue;
					}
					if (int.Parse(valuesAfterFault[i]) > valueTmp)
					{
						valuesAfterFault[i] = valuesAfterFaultTmp[i];
						if (i == 0)
						{
							valuesAfterFault[1] = valuesAfterFaultTmp[1];
						}
					}
				}
			}
			Array.Resize(ref valuesAfterFault, valuesAfterFault.Length - 1);
			return valuesAfterFault;
		}

		private string FindCellValue(int headRow, int bodyRow, string columnName, int workSheetNumber, ExcelPackage excelPackagePARUS)
		{
			var workSheet = excelPackagePARUS.Workbook.Worksheets[workSheetNumber];
			var columnIndex = FindColumn(headRow, columnName, workSheetNumber, excelPackagePARUS);
	

			if (workSheet.Cells[bodyRow, columnIndex].Value == null)
			{
				throw new Exception($"Данные в ячейке {workSheet.Cells[bodyRow, columnIndex]} отсутствуют");
			}

			return workSheet.Cells[bodyRow, columnIndex].Value.ToString();
				
					
		}

		private int FindColumn(int headRow, string columnName, int workSheetNumber, ExcelPackage excelPackagePARUS)
		{
			var workSheet = excelPackagePARUS.Workbook.Worksheets[workSheetNumber];
			for (int i = 2; i < 50; i++)
			{
				if (workSheet.Cells[headRow, i].Value != null)
				{
					if (workSheet.Cells[headRow, i].Value.ToString() == columnName)
					{
						return i;
					}
				}
			}
			throw new Exception($"В строке {headRow} нет ячейки с текстом {columnName}");
		}

		private int FindScheme(string repairScheme, int workSheetNumber, ExcelPackage excelPackagePARUS)
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
			int rowNumber = FindRow("Схема Ремонта", 9, 1, workSheetNumber, excelPackagePARUS);
			while(rowNumber != 0)
			{
				object CellValue = excelPackagePARUS.Workbook.Worksheets[workSheetNumber].Cells[rowNumber, 1].Value;
				if (CellValue.ToString().ToLower().Contains(schemeName))
				{
					return rowNumber;
				}
				rowNumber = FindRow("Схема Ремонта", rowNumber, 1, workSheetNumber, excelPackagePARUS);
			}
			throw new Exception($"Схемы {schemeName} на листе {excelPackagePARUS.Workbook.Worksheets[workSheetNumber].Name} нет");
		}

		private int FindRow(string textInColumn, int startRow, int column, int workSheetNumber, ExcelPackage excelPackagePARUS)
		{
			for (int i = startRow + 1; i < startRow + 1000; i++)
			{
				object CellValue = excelPackagePARUS.Workbook.Worksheets[workSheetNumber].Cells[i, column].Value;
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

		private List<(string, List<int>)> FindBodyRowAfterFault(int headRow, int workSheetNumber, ExcelPackage excelPackagePARUS)
		{
			var outputList = new List<(string, List<int>)>();

			if (excelPackagePARUS.Workbook.Worksheets[workSheetNumber].Cells[headRow, 1].Value.ToString() != "Послеаварийный режим")
			{
				return outputList;
			}
			object cellValue = excelPackagePARUS.Workbook.Worksheets[workSheetNumber].Cells[headRow + 1, 1].Value;
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
				cellValue = excelPackagePARUS.Workbook.Worksheets[workSheetNumber].Cells[headRow + index, 1].Value;
				if(cellValue == null)
				{
					int overlodedElementColumn = FindColumn(headRow, "Перегружаемый элемент", workSheetNumber, excelPackagePARUS);
					object overlodedElementValue =
						excelPackagePARUS.Workbook.Worksheets[workSheetNumber].Cells[headRow + index, overlodedElementColumn].Value;
					while (overlodedElementValue != null && cellValue == null)
					{
						outputList[outputList.Count - 1].Item2.Add(headRow + index);
						index += 1;
						overlodedElementValue =
							excelPackagePARUS.Workbook.Worksheets[workSheetNumber].Cells[headRow + index, overlodedElementColumn].Value;
						cellValue = excelPackagePARUS.Workbook.Worksheets[workSheetNumber].Cells[headRow + index, 1].Value;

					}
				}
			}
			return outputList;
		}

		private bool IsInSample(string afterFault, string[] paramIdentificatorSample)
		{
			foreach(string paramIdentificator in paramIdentificatorSample)
			{
				if(afterFault == paramIdentificator)
				{
					return true;
				}
			}
			return false;
		}
	}
}
