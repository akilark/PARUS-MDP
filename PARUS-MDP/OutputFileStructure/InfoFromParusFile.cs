﻿using System;
using System.Collections.Generic;
using System.IO;
using OfficeOpenXml;
using DataTypes;

namespace OutputFileStructure
{
	/// <summary>
	/// Класс для добавления в приложение №6 ПУР файла информации, 
	/// полученной из ПК ПАРУС, для конкретной группы ячеек.
	/// </summary>
	public class InfoFromParusFile
	{
		private int _nonRegularOscilation;
		private List<string> _errorList;

		/// <summary>
		/// Конструктор с 7 параметрами
		/// </summary>
		/// <param name="cellsGroupOneTemperature">группа ячеек</param>
		/// <param name="imbalances">список небалансов</param>
		/// <param name="AOPOlist">список АОПО</param>
		/// <param name="AOCNlist">список АОСН</param>
		/// <param name="LAPNYlist">список ЛАПНУ</param>
		/// <param name="disconnectingLineForEachEmergency">Учет каждого аварийного НБ
		/// выполнялся отключением соответствующей ветви?</param>
		/// <param name="excelPackageOutputFile">Файл экселя, куда записывается информация</param>
		public InfoFromParusFile(CellsGroup cellsGroupOneTemperature, List<Imbalance> imbalances, 
			List<AOPO> AOPOlist, List<AOCN> AOCNlist, List<ControlActionRow> LAPNYlist, bool disconnectingLineForEachEmergency,
			ref ExcelPackage excelPackageOutputFile)
		{
			_errorList = new List<string>();
			MainMethod(cellsGroupOneTemperature, imbalances, AOPOlist, AOCNlist, LAPNYlist, disconnectingLineForEachEmergency, ref excelPackageOutputFile);	
		}

		/// <summary>
		/// Значение нерегулярных колебаний
		/// </summary>
		public int NonRegularOscilation => _nonRegularOscilation;

		public List<string> ErrorList => _errorList;

		private void MainMethod(CellsGroup cellsGroupOneTemperature, List<Imbalance> imbalances,
			List<AOPO> AOPOlist, List<AOCN> AOCNlist, List<ControlActionRow> LAPNYlist, bool disconnectingLineForEachEmergency,
			ref ExcelPackage excelPackageOutputFile)
		{
			ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
			bool errorFlag = true;
			foreach (string path in cellsGroupOneTemperature.Folders)
			{
				FileInfo fileInfo = new FileInfo(path);
				var excelPackage = new ExcelPackage(fileInfo);
				FindNonRegularOscilation(excelPackage);
				if (cellsGroupOneTemperature.TemperatureDependence)
				{
					for (int i = 0; i < excelPackage.Workbook.Worksheets.Count; i++)
					{
						if (excelPackage.Workbook.Worksheets[i].Name == $"T= {cellsGroupOneTemperature.Temperature}")
						{
							FillExcelFile(cellsGroupOneTemperature, imbalances, AOPOlist, AOCNlist, LAPNYlist, disconnectingLineForEachEmergency,
								ref excelPackageOutputFile, excelPackage.Workbook.Worksheets[i]);
							errorFlag = false;
							break;
						}
					}
				}
				else
				{
					FillExcelFile(cellsGroupOneTemperature, imbalances, AOPOlist, AOCNlist, LAPNYlist, disconnectingLineForEachEmergency,
									ref excelPackageOutputFile, excelPackage.Workbook.Worksheets[0]);
					errorFlag = false;
				}
			}
			
			if (errorFlag)
			{
				if (cellsGroupOneTemperature.Folders.Length > 1)
				{
					foreach (string path in cellsGroupOneTemperature.Folders)
					{
						string error = $@"В выходном файле ПК ПАРУС в папке {path} нет информации о Т= {cellsGroupOneTemperature.Temperature}";
						_errorList.Add(error);
					}
				}
			}
		}

		private void FillExcelFile(CellsGroup cellsGroupOneTemperature, List<Imbalance> imbalances,
			List<AOPO> AOPOlist, List<AOCN> AOCNlist, List<ControlActionRow> LAPNYlist, bool disconnectingLineForEachEmergency,
			ref ExcelPackage excelPackageOutputFile, ExcelWorksheet excelWorksheetPARUS)
		{
			WorksheetInfoWithoutPA workSheetInfoTMP = new WorksheetInfoWithoutPA(cellsGroupOneTemperature.SchemeName, NonRegularOscilation,
							imbalances, excelWorksheetPARUS, disconnectingLineForEachEmergency);
			WorksheetInfoWithPA worksheetInfoWithPA = new WorksheetInfoWithPA(cellsGroupOneTemperature.SchemeName,
				NonRegularOscilation, workSheetInfoTMP.AllowPowerOverflow, imbalances, excelWorksheetPARUS,
				workSheetInfoTMP.MaximumAllowPowerFlowNonBalance, AOPOlist, AOCNlist, LAPNYlist, cellsGroupOneTemperature.Disturbance, disconnectingLineForEachEmergency);

			InsertText("МИН",
				(cellsGroupOneTemperature.StartID.Item1, cellsGroupOneTemperature.StartID.Item2), ref excelPackageOutputFile);
			InsertText("МИН",
				(cellsGroupOneTemperature.StartID.Item1, cellsGroupOneTemperature.StartID.Item2 + 1), ref excelPackageOutputFile);
			InsertText("МИН",
				(cellsGroupOneTemperature.StartID.Item1, cellsGroupOneTemperature.StartID.Item2 + 3), ref excelPackageOutputFile);
			InsertText("МИН",
				(cellsGroupOneTemperature.StartID.Item1, cellsGroupOneTemperature.StartID.Item2 + 4), ref excelPackageOutputFile);

			int nextRow = FindNextRowWithoutText(cellsGroupOneTemperature.StartID, cellsGroupOneTemperature.SizeCellsArea, excelPackageOutputFile);

			InsertText(workSheetInfoTMP.MaximumAllowPowerFlow.MaximumAllowPowerFlowValue.ToString(),
				(nextRow, cellsGroupOneTemperature.StartID.Item2), ref excelPackageOutputFile);
			InsertText(workSheetInfoTMP.MaximumAllowPowerFlow.MaximumAllowPowerCriterion,
				(nextRow, cellsGroupOneTemperature.StartID.Item2 + 3), ref excelPackageOutputFile);

			InsertText(workSheetInfoTMP.MaximumAllowPowerFlow.EmergencyAllowPowerFlowValue.ToString(),
				(cellsGroupOneTemperature.StartID.Item1, cellsGroupOneTemperature.StartID.Item2 + 2), ref excelPackageOutputFile);
			excelPackageOutputFile.Workbook.Worksheets[0].Cells[cellsGroupOneTemperature.StartID.Item1, cellsGroupOneTemperature.StartID.Item2 + 2,
				cellsGroupOneTemperature.StartID.Item1 + cellsGroupOneTemperature.SizeCellsArea, cellsGroupOneTemperature.StartID.Item2 + 2].Merge = true;
			InsertText(workSheetInfoTMP.MaximumAllowPowerFlow.EmergencyAllowPowerCriterion,
				(cellsGroupOneTemperature.StartID.Item1, cellsGroupOneTemperature.StartID.Item2 + 5), ref excelPackageOutputFile);
			excelPackageOutputFile.Workbook.Worksheets[0].Cells[cellsGroupOneTemperature.StartID.Item1, cellsGroupOneTemperature.StartID.Item2 + 5,
				cellsGroupOneTemperature.StartID.Item1 + cellsGroupOneTemperature.SizeCellsArea, cellsGroupOneTemperature.StartID.Item2 + 5].Merge = true;

			foreach (ImbalanceAndAutomatics imbalance in workSheetInfoTMP.MaximumAllowPowerFlowNonBalance)
			{
				nextRow = FindNextRowWithoutText(cellsGroupOneTemperature.StartID, cellsGroupOneTemperature.SizeCellsArea, excelPackageOutputFile);
				InsertText(imbalance.Equation, (nextRow, cellsGroupOneTemperature.StartID.Item2), ref excelPackageOutputFile);
				InsertText(imbalance.ImbalanceCriterion, (nextRow, cellsGroupOneTemperature.StartID.Item2 + 3), ref excelPackageOutputFile);
			}

			NeedForControl(workSheetInfoTMP, cellsGroupOneTemperature.StartID, cellsGroupOneTemperature.SizeCellsArea, ref excelPackageOutputFile);

			List<float> MDPwithPAlist = ValuesWithPA(worksheetInfoWithPA, cellsGroupOneTemperature, ref excelPackageOutputFile);
			NeedForControlWithPA(worksheetInfoWithPA, MDPwithPAlist, workSheetInfoTMP.AllowPowerOverflow,
				cellsGroupOneTemperature.StartID, cellsGroupOneTemperature.SizeCellsArea, ref excelPackageOutputFile);
			for (int j = 0; j < 9; j++)
			{
				excelPackageOutputFile.Workbook.Worksheets[0].Cells[cellsGroupOneTemperature.StartID.Item1, cellsGroupOneTemperature.StartID.Item2 + j,
					cellsGroupOneTemperature.StartID.Item1 + cellsGroupOneTemperature.SizeCellsArea, cellsGroupOneTemperature.StartID.Item2 + j].
					Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Medium);
			}
		}

		private List<float> ValuesWithPA(WorksheetInfoWithPA worksheetInfoWithPA, CellsGroup cellsGroupOneTemperature, ref ExcelPackage excelPackageOutputFile)
		{
			List<float> MDPwithPAlist = new List<float>();
			int nextRow = FindNextRowWithoutText((cellsGroupOneTemperature.StartID.Item1, cellsGroupOneTemperature.StartID.Item2 + 1), 
				cellsGroupOneTemperature.SizeCellsArea, excelPackageOutputFile);
			
			string MDPwithPA;
			string MDPwithPAcriterium;

			if (worksheetInfoWithPA.AllowPowerFlowPA.LocalAutomaticValueWithoutPA > 0)
			{
				float MDPtmp = worksheetInfoWithPA.AllowPowerFlowPA.LocalAutomaticValueWithoutPA;
				MDPwithPA = worksheetInfoWithPA.AllowPowerFlowPA.LocalAutomaticValueWithoutPA.ToString();
				MDPwithPAcriterium = worksheetInfoWithPA.AllowPowerFlowPA.CriteriumLocalAutomaticValueWithoutPA;
				if(worksheetInfoWithPA.AllowPowerFlowPA.ControlActionsLAPNY.Count > 0)
				{
					foreach(ControlActionRow LAPNY in worksheetInfoWithPA.AllowPowerFlowPA.ControlActionsLAPNY)
					{
						MDPtmp += LAPNY.CoefficientEfficiency * LAPNY.MaxValue;
						MDPwithPA = MDPwithPA + " + " + LAPNY.CoefficientEfficiency.ToString() + "*" + LAPNY.ParamID; 
					}
				}
				InsertText(MDPwithPA,
				(nextRow, cellsGroupOneTemperature.StartID.Item2 + 1), ref excelPackageOutputFile);
				InsertText(MDPwithPAcriterium,
					(nextRow, cellsGroupOneTemperature.StartID.Item2 + 4), ref excelPackageOutputFile);

				MDPwithPAlist.Add(MDPtmp);

				nextRow = FindNextRowWithoutText((cellsGroupOneTemperature.StartID.Item1, cellsGroupOneTemperature.StartID.Item2 + 1),
				cellsGroupOneTemperature.SizeCellsArea, excelPackageOutputFile);
			}

			if(worksheetInfoWithPA.AllowPowerFlowPA.EqupmentOverloadingWithoutPA > 0)
			{
				float MDPtmp = worksheetInfoWithPA.AllowPowerFlowPA.EqupmentOverloadingWithoutPA + 
					worksheetInfoWithPA.AllowPowerFlowPA.ControlActionAOPO.CoefficientEfficiency * 
					worksheetInfoWithPA.AllowPowerFlowPA.ControlActionAOPO.MaxValue;
				MDPwithPAlist.Add(MDPtmp);

				MDPwithPA = worksheetInfoWithPA.AllowPowerFlowPA.EqupmentOverloadingWithoutPA.ToString() + " + " + 
					worksheetInfoWithPA.AllowPowerFlowPA.ControlActionAOPO.CoefficientEfficiency.ToString() + 
					worksheetInfoWithPA.AllowPowerFlowPA.ControlActionAOPO.ParamID;
				MDPwithPAcriterium = worksheetInfoWithPA.AllowPowerFlowPA.CriteriumEqupmentOverloadingWithoutPA;
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
				InsertText("-",
				(nextRow, cellsGroupOneTemperature.StartID.Item2 + 4), ref excelPackageOutputFile);
				nextRow = FindNextRowWithoutText((cellsGroupOneTemperature.StartID.Item1, cellsGroupOneTemperature.StartID.Item2 + 1),
				cellsGroupOneTemperature.SizeCellsArea, excelPackageOutputFile);
			}
			
			if(worksheetInfoWithPA.AllowPowerFlowPA.VoltageLimitingWithoutPA > 0)
			{
				float MDPtmp = worksheetInfoWithPA.AllowPowerFlowPA.VoltageLimitingWithoutPA +
					worksheetInfoWithPA.AllowPowerFlowPA.ControlActionAOCN.CoefficientEfficiency *
					worksheetInfoWithPA.AllowPowerFlowPA.ControlActionAOCN.MaxValue;
				MDPwithPAlist.Add(MDPtmp);
				MDPwithPA = worksheetInfoWithPA.AllowPowerFlowPA.VoltageLimitingWithoutPA.ToString() + " + " +
					worksheetInfoWithPA.AllowPowerFlowPA.ControlActionAOCN.CoefficientEfficiency.ToString() +
					worksheetInfoWithPA.AllowPowerFlowPA.ControlActionAOCN.ParamID;
				MDPwithPAcriterium = worksheetInfoWithPA.AllowPowerFlowPA.CriteriumVoltageLimitingWithPA;
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
				InsertText("-",
				(nextRow, cellsGroupOneTemperature.StartID.Item2 + 4), ref excelPackageOutputFile);
				nextRow = FindNextRowWithoutText((cellsGroupOneTemperature.StartID.Item1, cellsGroupOneTemperature.StartID.Item2 + 1),
				cellsGroupOneTemperature.SizeCellsArea, excelPackageOutputFile);
			}

			if(worksheetInfoWithPA.AllowPowerFlowPA.ValueWithPA > 0)
			{
				float MDPtmp = worksheetInfoWithPA.AllowPowerFlowPA.ValueWithPA;
				MDPwithPAlist.Add(MDPtmp);

				MDPwithPA = worksheetInfoWithPA.AllowPowerFlowPA.ValueWithPA.ToString();
				MDPwithPAcriterium = worksheetInfoWithPA.AllowPowerFlowPA.CriteriumValueWithPA;
				InsertText(MDPwithPA,
				(nextRow, cellsGroupOneTemperature.StartID.Item2 + 1), ref excelPackageOutputFile);
				InsertText(MDPwithPAcriterium,
					(nextRow, cellsGroupOneTemperature.StartID.Item2 + 4), ref excelPackageOutputFile);
			}

			foreach (ImbalanceAndAutomatics imbalance in worksheetInfoWithPA.Imbalances)
			{
				nextRow = FindNextRowWithoutText((cellsGroupOneTemperature.StartID.Item1, cellsGroupOneTemperature.StartID.Item2 + 1),
				cellsGroupOneTemperature.SizeCellsArea, excelPackageOutputFile);

				if(imbalance.ImbalanceCriterion.Contains("АДТН "))
				{
					InsertText(imbalance.Equation + " Проверить необходимость учета УВ от АОПО", (nextRow, cellsGroupOneTemperature.StartID.Item2 + 1), ref excelPackageOutputFile);
				}
				else if(imbalance.ImbalanceCriterion.Contains("10%U ПАР "))
				{
					InsertText(imbalance.Equation + " Проверить необходимость учета УВ от АОСН", (nextRow, cellsGroupOneTemperature.StartID.Item2 + 1), ref excelPackageOutputFile);
				}
				else
				{
					InsertText(imbalance.Equation, (nextRow, cellsGroupOneTemperature.StartID.Item2 + 1), ref excelPackageOutputFile);
				}

				if (imbalance.ImbalanceCriterion.Contains(" с учетом объема УВ"))
				{
					excelPackageOutputFile.Workbook.Worksheets[0].Cells[nextRow, cellsGroupOneTemperature.StartID.Item2 + 1].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
					excelPackageOutputFile.Workbook.Worksheets[0].Cells[nextRow, cellsGroupOneTemperature.StartID.Item2 + 1].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Red);
				}
				InsertText(imbalance.ImbalanceCriterion, (nextRow, cellsGroupOneTemperature.StartID.Item2 + 4), ref excelPackageOutputFile);
			}

			return MDPwithPAlist;
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
					
			excelPackageOutputFile.Workbook.Worksheets[0].Cells[startID.Item1, startID.Item2 + 6,
				startID.Item1 + sizeCellsArea, startID.Item2 + 6].Merge = true;

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
			excelPackageOutputFile.Workbook.Worksheets[0].Cells[startID.Item1, startID.Item2 + 8,
				startID.Item1 + sizeCellsArea, startID.Item2 + 8].Merge = true;
		}

		private void NeedForControlWithPA(WorksheetInfoWithPA worksheetInfoWithPA, List<float> MDPwithPAlist, AllowPowerOverflows allowPowerOverflow, (int, int) startID, int sizeCellsArea, ref ExcelPackage excelPackageOutputFile)
		{
			
			if (CompareWithImbalancePA(worksheetInfoWithPA.Imbalances,
							MDPwithPAlist, allowPowerOverflow.CurrentLoadLinesValue))
			{
				InsertText( allowPowerOverflow.CurrentLoadLinesValue.ToString() + "*",
					(startID.Item1 + sizeCellsArea, startID.Item2 + 1), ref excelPackageOutputFile);
				InsertText($"ДДТН {allowPowerOverflow.CurrentLoadLinesCriterion}",
					(startID.Item1 + sizeCellsArea, startID.Item2 + 4), ref excelPackageOutputFile);
				InsertText($"Дополнительно осуществляется контроль токовой нагрузки '{allowPowerOverflow.CurrentLoadLinesCriterion}'",
					(startID.Item1, startID.Item2 + 7), ref excelPackageOutputFile);
			}
			else if (CompareWithImbalancePA(worksheetInfoWithPA.Imbalances,
							MDPwithPAlist, allowPowerOverflow.StabilityVoltageValue))
			{
				InsertText(allowPowerOverflow.StabilityVoltageValue.ToString() + "*",
					(startID.Item1 + sizeCellsArea, startID.Item2 + 1), ref excelPackageOutputFile);
				InsertText("15% U исходная схема",
					(startID.Item1 + sizeCellsArea, startID.Item2 + 4), ref excelPackageOutputFile);
				InsertText($"Дополнительно осуществляется контроль напряжения на '{allowPowerOverflow.StabilityVoltageCriterion}'",
					(startID.Item1, startID.Item2 + 7), ref excelPackageOutputFile);
			}
			else
			{
				InsertText("-",
					(startID.Item1 + sizeCellsArea, startID.Item2 + 1), ref excelPackageOutputFile);
				InsertText("-",
					(startID.Item1 + sizeCellsArea, startID.Item2 + 4), ref excelPackageOutputFile);
				InsertText("-",
					(startID.Item1, startID.Item2 + 7), ref excelPackageOutputFile);
			}
			excelPackageOutputFile.Workbook.Worksheets[0].Cells[startID.Item1, startID.Item2 + 7,
				startID.Item1 + sizeCellsArea, startID.Item2 + 7].Merge = true;

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

		private bool CompareWithImbalancePA(List<ImbalanceAndAutomatics> imbalances, List<float> MDPwithPAlist, int compareValue)
		{
			if (compareValue == 0)
			{
				return false;
			}
			for (int i = 0; i < imbalances.Count; i++)
			{
				if (compareValue > imbalances[i].EquationValue)
				{
					return false;
				}
			}
			for(int i = 0; i< MDPwithPAlist.Count; i ++)
			{
				if (compareValue > MDPwithPAlist[i])
				{
					return false;
				}
			}
			return true;
		}

		private int FindNextRowWithoutText((int, int) StartID, int SizeCellsArea, ExcelPackage excelPackageOutputFile)
		{
			for(int i = 0; i <= SizeCellsArea; i ++ )
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
