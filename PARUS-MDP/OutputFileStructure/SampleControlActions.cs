using System;
using System.Collections.Generic;
using System.Text;
using OfficeOpenXml;
using DataTypes;

namespace OutputFileStructure
{
	public class SampleControlActions
	{

		private List<ControlActionRow> _controlActionRows;
		public SampleControlActions(ExcelPackage excelPackage)
		{
			_controlActionRows = new List<ControlActionRow>();
			CountControlActions(excelPackage);
		}

		/// <summary>
		/// Получение списка управляющих воздействий в следующем формате 
		/// List<(Направление перетока,List<(строка,столбец с направлением перетока)>)>
		/// </summary>
		public List<ControlActionRow> ControlActionRows => _controlActionRows;

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
					InfoFromSample((firstCell.Item1 + index, firstCell.Item2),worksheet);
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

		private void InfoFromSample((int, int) firstCell,  ExcelWorksheet worksheet)
		{
			
			ControlActionRow controlAction = new ControlActionRow();
			controlAction.ParamID = worksheet.Cells[firstCell.Item1, firstCell.Item2].
				Value.ToString().Trim();
			controlAction.CoefficientEfficiency = float.Parse(
				worksheet.Cells[firstCell.Item1, firstCell.Item2 + 8].Value.ToString());
			controlAction.MaxValue = int.Parse(
				worksheet.Cells[firstCell.Item1, firstCell.Item2 + 7].Value.ToString());
			controlAction.ParamSign = worksheet.Cells[firstCell.Item1, firstCell.Item2 + 4].
				Value.ToString().Trim();
			controlAction.Direction = worksheet.Cells[firstCell.Item1, firstCell.Item2 + 5].
				Value.ToString().Trim();
			_controlActionRows.Add(controlAction);
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
			int outputValue = 0;
			foreach(ControlActionRow row in ControlActionRows)
			{
				if(direction.Trim().ToLower() == row.Direction.Trim().ToLower())
				{
					outputValue += 1;
				}
			}
			return outputValue;
		}
		public List<ControlActionRow> ControlActionsForNeedDirection(string direction)
		{
			var outputValue = new List<ControlActionRow>();
			foreach (ControlActionRow row in ControlActionRows)
			{
				if (direction.Trim().ToLower() == row.Direction.Trim().ToLower())
				{
					outputValue.Add(row);
				}
			}
			return outputValue;
		}

	}
}
