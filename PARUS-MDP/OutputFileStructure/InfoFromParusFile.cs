using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using OfficeOpenXml;
using OutputFileStructure.DataTypes;

namespace OutputFileStructure
{
	public class InfoFromParusFile
	{
		private int _nonRegularOscilation;

		public InfoFromParusFile(List<CellsGroup> cellsGroupManyTemperature, List<ControlActionRow> NBinSample,
			List<ImbalanceDataSource> imbalanceDataSources, ref ExcelPackage excelPackageOutputFile)
		{
			var worksheet = excelPackageOutputFile.Workbook.Worksheets[0];
			ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
			foreach(CellsGroup cellsGroupOneTemperature in cellsGroupManyTemperature)
			{
				foreach (string path in cellsGroupOneTemperature.Folders)
				{
					bool flag = false;
					FileInfo fileInfo = new FileInfo(path);
					var excelPackage = new ExcelPackage(fileInfo);
					FindNonRegularOscilation(excelPackage);
					for (int i = 0; i < excelPackage.Workbook.Worksheets.Count; i++)
					{
						if(excelPackage.Workbook.Worksheets[i].Name == $"T= {cellsGroupOneTemperature.Temperature}")
						{
							flag = true;
							WorksheetInfoWithoutPA workSheetInfoTMP = new WorksheetInfoWithoutPA(cellsGroupOneTemperature.SchemeName, NonRegularOscilation,
							NBinSample, imbalanceDataSources, excelPackage.Workbook.Worksheets[i]);
							WorksheetInfoWithtPA worksheetInfoWithtPA = new WorksheetInfoWithtPA(cellsGroupOneTemperature.SchemeName, NonRegularOscilation,
								workSheetInfoTMP.AllowPowerOverflow, NBinSample, imbalanceDataSources, excelPackage.Workbook.Worksheets[i], workSheetInfoTMP.MaximumAllowPowerFlowNonBalance);


							int nextRow = FindNextRowWithoutText(cellsGroupOneTemperature.StartID, cellsGroupOneTemperature.SizeCellsArea, excelPackageOutputFile);

							InsertText(workSheetInfoTMP.MaximumAllowPowerFlow.MaximumAllowPowerFlowValue.ToString(),
								(nextRow, cellsGroupOneTemperature.StartID.Item2), ref excelPackageOutputFile);
							InsertText(workSheetInfoTMP.MaximumAllowPowerFlow.MaximumAllowPowerCriterion,
								(nextRow, cellsGroupOneTemperature.StartID.Item2 + 3), ref excelPackageOutputFile);
							
							InsertText(workSheetInfoTMP.MaximumAllowPowerFlow.EmergencyAllowPowerFlowValue.ToString(),
								(cellsGroupOneTemperature.StartID.Item1, cellsGroupOneTemperature.StartID.Item2 + 2), ref excelPackageOutputFile);
							MergeColumn((cellsGroupOneTemperature.StartID.Item1, cellsGroupOneTemperature.StartID.Item1 + cellsGroupOneTemperature.SizeCellsArea + 1),
								cellsGroupOneTemperature.StartID.Item2 + 2, ref excelPackageOutputFile);
							InsertText(workSheetInfoTMP.MaximumAllowPowerFlow.EmergencyAllowPowerCriterion,
								(cellsGroupOneTemperature.StartID.Item1, cellsGroupOneTemperature.StartID.Item2 + 5), ref excelPackageOutputFile);
							MergeColumn((cellsGroupOneTemperature.StartID.Item1, cellsGroupOneTemperature.StartID.Item1 + cellsGroupOneTemperature.SizeCellsArea + 1),
								cellsGroupOneTemperature.StartID.Item2 + 5, ref excelPackageOutputFile);
							
							foreach(ImbalanceAndAutomatics imbalance in workSheetInfoTMP.MaximumAllowPowerFlowNonBalance)
							{
								nextRow = FindNextRowWithoutText(cellsGroupOneTemperature.StartID, cellsGroupOneTemperature.SizeCellsArea, excelPackageOutputFile);
								InsertText(imbalance.Equation, (nextRow, cellsGroupOneTemperature.StartID.Item2), ref excelPackageOutputFile);
								InsertText(imbalance.ImbalanceCriterion, (nextRow, cellsGroupOneTemperature.StartID.Item2 + 3), ref excelPackageOutputFile);
							}

							NeedForControl(workSheetInfoTMP, cellsGroupOneTemperature.StartID, cellsGroupOneTemperature.SizeCellsArea, ref excelPackageOutputFile);
							break;
						}
					}
					if(flag)
					{
						break ;
					}
				}
			}
			
		}

		public int NonRegularOscilation => _nonRegularOscilation;

		private void ValuesWithPA(WorksheetInfoWithtPA worksheetInfoWithtPA, CellsGroup cellsGroupOneTemperature, ref ExcelPackage excelPackageOutputFile)
		{
			int nextRow = FindNextRowWithoutText((cellsGroupOneTemperature.StartID.Item1, cellsGroupOneTemperature.StartID.Item2 + 1), 
				cellsGroupOneTemperature.SizeCellsArea, excelPackageOutputFile);
			
			string MDPwithPA;
			string MDPwithPAcriterium;

			if (worksheetInfoWithtPA.AllowPowerFlowPA.LocalAutomaticValueWithoutPA > 0)
			{
				MDPwithPA = worksheetInfoWithtPA.AllowPowerFlowPA.LocalAutomaticValueWithoutPA.ToString();
				MDPwithPAcriterium = worksheetInfoWithtPA.AllowPowerFlowPA.CriteriumLocalAutomaticValueWithoutPA;
				if(worksheetInfoWithtPA.AllowPowerFlowPA.ControlActionsLAPNY.Count > 0)
				{
					foreach(ControlActionRow LAPNY in worksheetInfoWithtPA.AllowPowerFlowPA.ControlActionsLAPNY)
					{
						MDPwithPAcriterium = MDPwithPAcriterium + " + " + LAPNY.CoefficientEfficiency.ToString() + LAPNY.ParamID; 
					}
				}
				InsertText(MDPwithPA,
				(nextRow, cellsGroupOneTemperature.StartID.Item2 + 1), ref excelPackageOutputFile);
				InsertText(MDPwithPAcriterium,
					(nextRow, cellsGroupOneTemperature.StartID.Item2 + 4), ref excelPackageOutputFile);
				
				nextRow = FindNextRowWithoutText((cellsGroupOneTemperature.StartID.Item1, cellsGroupOneTemperature.StartID.Item2 + 1),
				cellsGroupOneTemperature.SizeCellsArea, excelPackageOutputFile);
			}

			if(worksheetInfoWithtPA.AllowPowerFlowPA.EqupmentOverloadingWithoutPA > 0)
			{
				MDPwithPA = worksheetInfoWithtPA.AllowPowerFlowPA.EqupmentOverloadingWithoutPA.ToString() + " + " + 
					worksheetInfoWithtPA.AllowPowerFlowPA.ControlActionAOPO.CoefficientEfficiency.ToString() + 
					worksheetInfoWithtPA.AllowPowerFlowPA.ControlActionAOPO.ParamID;
				MDPwithPAcriterium = worksheetInfoWithtPA.AllowPowerFlowPA.CriteriumEqupmentOverloadingWithoutPA;
				InsertText(MDPwithPA,
				(nextRow, cellsGroupOneTemperature.StartID.Item2 + 1), ref excelPackageOutputFile);
				InsertText(MDPwithPAcriterium,
					(nextRow, cellsGroupOneTemperature.StartID.Item2 + 4), ref excelPackageOutputFile);
				nextRow = FindNextRowWithoutText((cellsGroupOneTemperature.StartID.Item1, cellsGroupOneTemperature.StartID.Item2 + 1),
				cellsGroupOneTemperature.SizeCellsArea, excelPackageOutputFile);
			}
			else
			{
				InsertText("-",
				(nextRow, cellsGroupOneTemperature.StartID.Item2 + 1), ref excelPackageOutputFile);
				nextRow = FindNextRowWithoutText((cellsGroupOneTemperature.StartID.Item1, cellsGroupOneTemperature.StartID.Item2 + 1),
				cellsGroupOneTemperature.SizeCellsArea, excelPackageOutputFile);
			}
			
			if(worksheetInfoWithtPA.AllowPowerFlowPA.VoltageLimitingWithoutPA > 0)
			{
				MDPwithPA = worksheetInfoWithtPA.AllowPowerFlowPA.VoltageLimitingWithoutPA.ToString() + " + " +
					worksheetInfoWithtPA.AllowPowerFlowPA.ControlActionAOCN.CoefficientEfficiency.ToString() +
					worksheetInfoWithtPA.AllowPowerFlowPA.ControlActionAOCN.ParamID;
				MDPwithPAcriterium = worksheetInfoWithtPA.AllowPowerFlowPA.CriteriumVoltageLimitingWithPA;
				InsertText(MDPwithPA,
				(nextRow, cellsGroupOneTemperature.StartID.Item2 + 1), ref excelPackageOutputFile);
				InsertText(MDPwithPAcriterium,
					(nextRow, cellsGroupOneTemperature.StartID.Item2 + 4), ref excelPackageOutputFile);
				nextRow = FindNextRowWithoutText((cellsGroupOneTemperature.StartID.Item1, cellsGroupOneTemperature.StartID.Item2 + 1),
				cellsGroupOneTemperature.SizeCellsArea, excelPackageOutputFile);
			}
			else
			{
				InsertText("-",
				(nextRow, cellsGroupOneTemperature.StartID.Item2 + 1), ref excelPackageOutputFile);
				nextRow = FindNextRowWithoutText((cellsGroupOneTemperature.StartID.Item1, cellsGroupOneTemperature.StartID.Item2 + 1),
				cellsGroupOneTemperature.SizeCellsArea, excelPackageOutputFile);
			}

			if(worksheetInfoWithtPA.AllowPowerFlowPA.ValueWithPA > 0)
			{
				MDPwithPA = worksheetInfoWithtPA.AllowPowerFlowPA.ValueWithPA.ToString();
				MDPwithPAcriterium = worksheetInfoWithtPA.AllowPowerFlowPA.CriteriumValueWithPA;
				InsertText(MDPwithPA,
				(nextRow, cellsGroupOneTemperature.StartID.Item2 + 1), ref excelPackageOutputFile);
				InsertText(MDPwithPAcriterium,
					(nextRow, cellsGroupOneTemperature.StartID.Item2 + 4), ref excelPackageOutputFile);
			}

			foreach (ImbalanceAndAutomatics imbalance in worksheetInfoWithtPA.imbalances)
			{
				//учет НБ сделать другим
				nextRow = FindNextRowWithoutText(cellsGroupOneTemperature.StartID, cellsGroupOneTemperature.SizeCellsArea, excelPackageOutputFile);
				string equation = imbalance.Equation; // добавить КувАРПМ
				InsertText(imbalance.Equation, (nextRow, cellsGroupOneTemperature.StartID.Item2), ref excelPackageOutputFile);
				InsertText(imbalance.ImbalanceCriterion, (nextRow, cellsGroupOneTemperature.StartID.Item2 + 3), ref excelPackageOutputFile);
			}
		}

		private void FindNonRegularOscilation(ExcelPackage excelPackage)
		{
			string cellInfo = excelPackage.Workbook.Worksheets[0].Cells[2, 1].Value.ToString();
			string[] cellInfoSplit = cellInfo.Split(" ");
			_nonRegularOscilation =  Int32.Parse(cellInfoSplit[2]);
		}

		private void InsertText( string value, (int,int) rowAndColumn, ref ExcelPackage excelPackageOutputFile)
		{
			excelPackageOutputFile.Workbook.Worksheets[0].Cells[rowAndColumn.Item1, rowAndColumn.Item2].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
			excelPackageOutputFile.Workbook.Worksheets[0].Cells[rowAndColumn.Item1, rowAndColumn.Item2].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
			excelPackageOutputFile.Workbook.Worksheets[0].Cells[rowAndColumn.Item1, rowAndColumn.Item2].Style.WrapText = true;
			excelPackageOutputFile.Workbook.Worksheets[0].Cells[rowAndColumn.Item1, rowAndColumn.Item2].Value = value;
			excelPackageOutputFile.Workbook.Worksheets[0].Cells[rowAndColumn.Item1, rowAndColumn.Item2].Style.Font.Name = "Times New Roman";
			excelPackageOutputFile.Workbook.Worksheets[0].Cells[rowAndColumn.Item1, rowAndColumn.Item2].Style.Font.Size = 8;
			
		}

		private void MergeColumn((int, int) columnMerge, int column, ref ExcelPackage excelPackageOutputFile)
		{
			excelPackageOutputFile.Workbook.Worksheets[0].Cells[columnMerge.Item1, column, columnMerge.Item2 - 1, column].Merge = true;
			excelPackageOutputFile.Workbook.Worksheets[0].Cells[columnMerge.Item1, column, columnMerge.Item2 - 1, column].
					Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Medium);
		}

		private void NeedForControl(WorksheetInfoWithoutPA workSheetInfo, (int,int) startID, int sizeCellsArea, ref ExcelPackage excelPackageOutputFile)
		{
			if (CompareWithImbalance(workSheetInfo.MaximumAllowPowerFlowNonBalance,
				workSheetInfo.AllowPowerOverflow.StabilityVoltageValue, 
				workSheetInfo.MaximumAllowPowerFlow.MaximumAllowPowerFlowValue,
				workSheetInfo.AllowPowerOverflow.CurrentLoadLinesValue))
			{
				InsertText(workSheetInfo.AllowPowerOverflow.CurrentLoadLinesValue.ToString() + "*",
					(startID.Item1 + sizeCellsArea, startID.Item2), ref excelPackageOutputFile);
				InsertText( $"ДДТН {workSheetInfo.AllowPowerOverflow.CurrentLoadLinesCriterion}",
					(startID.Item1 + sizeCellsArea, startID.Item2 + 3), ref excelPackageOutputFile);
				InsertText($"Дополнительно осуществляется контроль токовой нагрузки '{workSheetInfo.AllowPowerOverflow.CurrentLoadLinesCriterion}'",
					(startID.Item1, startID.Item2 + 6), ref excelPackageOutputFile);
			}
			else if(CompareWithImbalance(workSheetInfo.MaximumAllowPowerFlowNonBalance, 
				workSheetInfo.AllowPowerOverflow.CurrentLoadLinesValue,
				workSheetInfo.MaximumAllowPowerFlow.MaximumAllowPowerFlowValue,
				workSheetInfo.AllowPowerOverflow.StabilityVoltageValue))
			{
				InsertText(workSheetInfo.AllowPowerOverflow.StabilityVoltageValue.ToString() + "*",
					(startID.Item1 + sizeCellsArea, startID.Item2), ref excelPackageOutputFile);
				InsertText("15% U исходная схема",
					(startID.Item1 + sizeCellsArea, startID.Item2 + 3), ref excelPackageOutputFile);
				InsertText($"Дополнительно осуществляется контроль напряжения на '{workSheetInfo.AllowPowerOverflow.StabilityVoltageCriterion}'",
					(startID.Item1, startID.Item2 + 6), ref excelPackageOutputFile);
			}
			else
			{
				InsertText("-",
					(startID.Item1 + sizeCellsArea, startID.Item2), ref excelPackageOutputFile);
				InsertText("-",
					(startID.Item1 + sizeCellsArea, startID.Item2 + 3), ref excelPackageOutputFile);
				InsertText("-",
					(startID.Item1, startID.Item2 + 6), ref excelPackageOutputFile);
			}
			MergeColumn((startID.Item1, startID.Item1 + sizeCellsArea+1), startID.Item2 + 6, ref excelPackageOutputFile);

			if (workSheetInfo.AllowPowerOverflow.CurrentLoadLinesValue < workSheetInfo.AllowPowerOverflow.EmergencyAllowPowerOverflow)
			{
				InsertText($"Дополнительно осуществляется контроль токовой нагрузки '{workSheetInfo.AllowPowerOverflow.CurrentLoadLinesCriterion}'",
					(startID.Item1, startID.Item2 + 8), ref excelPackageOutputFile);
			}
			else if (workSheetInfo.AllowPowerOverflow.StabilityVoltageValue < workSheetInfo.AllowPowerOverflow.EmergencyAllowPowerOverflow) 
			{
				InsertText($"Дополнительно осуществляется контроль напряжения на '{workSheetInfo.AllowPowerOverflow.StabilityVoltageCriterion}'",
					(startID.Item1, startID.Item2 + 8), ref excelPackageOutputFile);
			}
			else
			{
				InsertText("-",
					(startID.Item1, startID.Item2 + 8), ref excelPackageOutputFile);
			}
			MergeColumn((startID.Item1, startID.Item1 + sizeCellsArea+1), startID.Item2 + 8, ref excelPackageOutputFile);
		}

		private bool CompareWithImbalance(List<ImbalanceAndAutomatics> imbalances, int criterium, int maximumAllowPowerFlow, int compareValue)
		{
			if (compareValue == 0)
			{
				return false;
			}
			for (int i = 0; i < imbalances.Count; i++)
			{
				if(compareValue > imbalances[i].EquationValue)
				{
					return false;
				}
			}
			if( compareValue > criterium)
			{
				return false;
			}
			if(compareValue > maximumAllowPowerFlow)
			{
				return false;
			}
			return true;
		}

		private int FindNextRowWithoutText((int, int) StartID, int SizeCellsArea, ExcelPackage excelPackageOutputFile)
		{
			for(int i = 0; i < SizeCellsArea; i ++ )
			{
				if(excelPackageOutputFile.Workbook.Worksheets[0].Cells[StartID.Item1 + i, StartID.Item2].Value == null)
				{
					return StartID.Item1 + i;
				}
			}
			throw new Exception("Пустые ячейки для данной температуры закончались");
		}
	}
}
