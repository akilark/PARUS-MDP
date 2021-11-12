using System;
using System.Collections.Generic;
using System.Text;
using OfficeOpenXml;

namespace OutputFileStructure
{
	public class SampleControlActions
	{

		private List<(string, List<ControlAction>)> _controlActionsWithDirections;
		public SampleControlActions(ExcelPackage excelPackage)
		{
			_controlActionsWithDirections = new List<(string, List<ControlAction>)>();
			CountControlActions(excelPackage);
		}

		/// <summary>
		/// Получение списка управляющих воздействий в следующем формате 
		/// List<(Направление перетока,List<(строка,столбец с направлением перетока)>)>
		/// </summary>
		public List<(string, List<ControlAction>)> ControlActionsWithDirection => _controlActionsWithDirections;


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
					ControlAction controlAction = new ControlAction();
					string direction = worksheet.Cells[cellWithDirection.Item1 + index, cellWithDirection.Item2].
						Value.ToString().Trim();
					controlAction.ParamID = worksheet.Cells[cellWithDirection.Item1 + index, firstCell.Item2].
						Value.ToString().Trim();
					controlAction.CoefficientEfficiency = float.Parse(
						worksheet.Cells[cellWithDirection.Item1 + index, cellWithDirection.Item2 + 3].Value.ToString());
					controlAction.ActivePowerControlActionMax = int.Parse(
						worksheet.Cells[cellWithDirection.Item1 + index, cellWithDirection.Item2 + 2].Value.ToString());
					

					if (_controlActionsWithDirections.Count == 0)
					{
						_controlActionsWithDirections.Add((direction,new List<ControlAction>()));
						
					}
					bool uniqueFlag = true;
					for(int i = 0; i < _controlActionsWithDirections.Count; i++)
					{
						if(direction == _controlActionsWithDirections[i].Item1)
						{
							uniqueFlag = false;
							controlAction.IDCell = (cellWithDirection.Item1 + index, firstCell.Item2);
							_controlActionsWithDirections[i].Item2.Add(controlAction);
						}
					}
					if (uniqueFlag)
					{
						controlAction.IDCell = (cellWithDirection.Item1 + index, firstCell.Item2);
						_controlActionsWithDirections.Add((direction, new List<ControlAction>()));
						_controlActionsWithDirections[_controlActionsWithDirections.Count-1].Item2.Add(controlAction);
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
			foreach((string, List<ControlAction>) directionWithCells in ControlActionsWithDirection)
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
