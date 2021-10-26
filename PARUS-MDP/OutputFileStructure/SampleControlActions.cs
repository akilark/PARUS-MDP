using System;
using System.Collections.Generic;
using System.Text;
using OfficeOpenXml;

namespace OutputFileStructure
{
	public class SampleControlActions
	{

		private List<(string, List<(int,int)>)> _controlActionsWithDirections;
		public SampleControlActions(ExcelPackage excelPackage)
		{
			_controlActionsWithDirections = new List<(string, List<(int, int)>)>();
			CountControlActions(excelPackage);
		}

		/// <summary>
		/// Получение списка управляющих воздействий в следующем формате 
		/// List<(Направление перетока,List<(строка,столбец с направлением перетока)>)>
		/// </summary>
		public List<(string, List<(int, int)>)> ControlActionsWithDirection => _controlActionsWithDirections;

		private void CountControlActions(ExcelPackage excelPackage)
		{
			(int, int) firstCell = FindCellWithNeededText(excelPackage, "идентификатор");
			(int, int) cellWithDirection = FindCellWithNeededText(excelPackage, "направление перетока");
			bool endTable = false;  
			int index = 0;
			while(!endTable)
			{
				index = index + 1;
				if(excelPackage.Workbook.Worksheets[1].Cells[cellWithDirection.Item1 + index,cellWithDirection.Item2].Value != null)
				{
					string direction = excelPackage.Workbook.Worksheets[1].Cells[cellWithDirection.Item1 + index, cellWithDirection.Item2].
						Value.ToString().Trim();
					if (_controlActionsWithDirections.Count == 0)
					{
						_controlActionsWithDirections.Add((direction,new List<(int,int)>()));
						
					}
					bool uniqueFlag = true;
					for(int i = 0; i < _controlActionsWithDirections.Count; i++)
					{
						if(direction == _controlActionsWithDirections[i].Item1)
						{
							uniqueFlag = false;
							_controlActionsWithDirections[i].Item2.Add((cellWithDirection.Item1 + index, cellWithDirection.Item2));
						}
					}
					if (uniqueFlag)
					{
						_controlActionsWithDirections.Add((direction, new List<(int,int)>()));
						_controlActionsWithDirections[_controlActionsWithDirections.Count-1].Item2.Add((cellWithDirection.Item1 + index, cellWithDirection.Item2));
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
		public (int,int) FindCellWithNeededText(ExcelPackage excelPackage, string text)
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
			foreach((string, List<(int, int)>) directionWithCells in ControlActionsWithDirection)
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
