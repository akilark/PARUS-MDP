using System;
using System.Collections.Generic;
using System.Text;
using OfficeOpenXml;

namespace OutputFileStructure
{
	public class SampleControlActions
	{

		private List<(string, List<ControlAction>)> _NBinSample;
		private List<(string, List<ControlAction>)> _AOPOinSample;
		public SampleControlActions(ExcelPackage excelPackage)
		{
			_NBinSample = new List<(string, List<ControlAction>)>();
			_AOPOinSample = new List<(string, List<ControlAction>)>();
			CountControlActions(excelPackage);
		}

		/// <summary>
		/// Получение списка управляющих воздействий в следующем формате 
		/// List<(Направление перетока,List<(строка,столбец с направлением перетока)>)>
		/// </summary>
		public List<(string, List<ControlAction>)> ImbalanceInSample => _NBinSample;

		public List<(string, List<ControlAction>)> AOPOinSample => _AOPOinSample;

		private void CountControlActions(ExcelPackage excelPackage)
		{
			var worksheet = excelPackage.Workbook.Worksheets[1];
			(int, int) firstCell = FindCellWithNeededText(excelPackage, "идентификатор");
			(int, int) cellWithDirection = FindCellWithNeededText(excelPackage, "направление перетока");
			bool endTable = false;  
			int index = 0;
			while(!endTable)
			{
				index = index + 1;
				if(worksheet.Cells[cellWithDirection.Item1 + index,cellWithDirection.Item2].Value != null)
				{
					if(worksheet.Cells[firstCell.Item1 + index, firstCell.Item2 + 4].Value.ToString().ToLower().Contains("нб"))
					{
						InfoFromSample((firstCell.Item1 + index, firstCell.Item2),_NBinSample,worksheet);
					}
					if(worksheet.Cells[firstCell.Item1 + index, firstCell.Item2 + 4].Value.ToString().ToLower().Contains("лапну"))
					{
						InfoFromSample((firstCell.Item1 + index, firstCell.Item2),_AOPOinSample, worksheet);
					}
				}
				else
				{
					if (excelPackage.Workbook.Worksheets[1].Cells[cellWithDirection.Item1 + index, firstCell.Item2].Value == null)
					{
						endTable = true;
					}
				}
				
			}

		}

		private void InfoFromSample((int, int) firstCell, List<(string, List<ControlAction>)> infoInSample,  ExcelWorksheet worksheet)
		{
			
			ControlAction controlAction = new ControlAction();
			string direction = worksheet.Cells[firstCell.Item1, firstCell.Item2 + 5].
				Value.ToString().Trim();
			controlAction.ParamID = worksheet.Cells[firstCell.Item1, firstCell.Item2].
				Value.ToString().Trim();
			controlAction.CoefficientEfficiency = float.Parse(
				worksheet.Cells[firstCell.Item1, firstCell.Item2 + 8].Value.ToString());
			controlAction.ActivePowerControlActionMax = int.Parse(
				worksheet.Cells[firstCell.Item1, firstCell.Item2 + 7].Value.ToString());

			if (infoInSample.Count == 0)
			{
				infoInSample.Add((direction, new List<ControlAction>()));
			}
			bool uniqueFlag = true;
			for (int i = 0; i < infoInSample.Count; i++)
			{
				if (direction == infoInSample[i].Item1)
				{
					uniqueFlag = false;
					controlAction.IDCell = (firstCell.Item1, firstCell.Item2);
					infoInSample[i].Item2.Add(controlAction);
				}
			}
			if (uniqueFlag)
			{
				controlAction.IDCell = (firstCell.Item1, firstCell.Item2);
				infoInSample.Add((direction, new List<ControlAction>()));
				infoInSample[infoInSample.Count - 1].Item2.Add(controlAction);
			}
		}


		private (int,int) FindCellWithNeededText(ExcelPackage excelPackage, string text)
		{
			text = text.Trim().ToLower();
			for (int rowIndex = 1; rowIndex < 10; rowIndex++)
			{
				for (int columnIndex = 1; columnIndex < 50; columnIndex++)
				{
					if(excelPackage.Workbook.Worksheets[1].Cells[rowIndex,columnIndex].Value != null)
					{
						if (excelPackage.Workbook.Worksheets[1].Cells[rowIndex, columnIndex].Value.ToString().ToLower().Contains(text))
						{
							return (rowIndex, columnIndex);
						}
					}
				}
			}

			throw new Exception("Вкладка УВ НБ шаблона не заполнена");
		}

		public int AmountControlActions(string direction)
		{
			foreach((string, List<ControlAction>) directionWithCells in ImbalanceInSample)
			{
				if(direction.Trim().ToLower() == directionWithCells.Item1.Trim().ToLower())
				{
					return directionWithCells.Item2.Count;
				}
			}
			return 0;
		}

	}
}
