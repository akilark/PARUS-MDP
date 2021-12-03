using System;
using System.Collections.Generic;
using System.IO;
using OfficeOpenXml;

namespace ExcelForParus
{
	/// <summary>
	/// Класс для генерации Excel файла для ПК ПАРУС
	/// </summary>
	public class CreateExcelForParus
	{
		private List<string> _errorList;
		private string _path;
		private List<(string, string, string)> _filesLinkList = new List<(string, string, string)>();
		private string _fileSch;
		private bool _outputDirectory = false;

		/// <summary>
		/// Конструктор класса с 1 параметром
		/// </summary>
		/// <param name="path">Путь к директории</param>
		public CreateExcelForParus(string path)
		{
			_path = path;
			_errorList = new List<string>();
			ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
			Generate();
		}

		public List<string> ErrorList => _errorList;

		/// <summary>
		/// Метод инициирующий создание excel файла для ПК ПАРУС
		/// </summary>
		private void Generate()
		{
			FilesLinkListFill(_path);
			try
			{
				_fileSch = FindFile(_path, "*.sch");
			}
			catch (ArgumentException exception)
			{
				_errorList.Add(exception.Message);
			}
			if(_errorList.Count < 1)
			{
				TransportedInfoInExcel();
			}
		}

		/// <summary>
		/// Метод необходимый для заполнения листа 
		/// информацией о путях расположения файлов
		/// </summary>
		/// <param name="path">Путь к директории</param>
		private void FilesLinkListFill(string path)
		{
			var ArrayDirectory = Directory.GetDirectories(path);
			if (ArrayDirectory.Length == 0)
			{
				string fileRg ="";
				string fileUt= "";
				try
				{
					fileRg = FindFile(path, "*.rg2");
				}
				catch (ArgumentException exception)
				{
					_errorList.Add(exception.Message);
				}

				try
				{
					fileUt = FindFile(path, "*.ut2");
				}
				catch (ArgumentException exception)
				{
					_errorList.Add(exception.Message);
				}

				if (fileRg != "" && fileUt != "")
				{
					_filesLinkList.Add((fileRg, fileUt, path));
				}
				
			}
			else
			{
				foreach (string nextPath in ArrayDirectory)
				{
					FilesLinkListFill(nextPath);
				}
			}
		}

		/// <summary>
		/// Поиск в директории файла имеющего соотвествующее расширение
		/// </summary>
		/// <param name="path">Путь к директории</param>
		/// <param name="pattern">расширение файла</param>
		/// <returns>Путь к файлу</returns>
		private string FindFile(string path, string pattern)
		{
			if (!pattern.Contains(".")) throw new Exception("Необходимо в качестве " +
				"параметра 'pattern' передавать расширение файла");
			var files = Directory.GetFiles(path, pattern);
			if (files.Length == 1)
			{
				return files[0];
			}
			else
			{
				throw new ArgumentException(@$"Необходим 1 файл с расширением '{pattern}' в директории: {path}");
			}
		}

		/// <summary>
		/// Метод необходимый для получения имени сечения из пути
		/// </summary>
		/// <returns>Имя сечения</returns>
		private string SectionName()
		{
			string path = _path;
			while (path.Contains(@"\"))
			{
				path = path.Substring(path.IndexOf(@"\") + 1);
			}
			return path;
		}

		/// <summary>
		/// Метод необходимый для переноса данных о путях к файлам в Excel файл
		/// </summary>
		private void TransportedInfoInExcel()
		{
			using (ExcelPackage excelPackage = new ExcelPackage())
			{
				excelPackage.Workbook.Properties.Author = "PARUS-MDP";
				excelPackage.Workbook.Properties.Title = @$"Для Паруса '{SectionName()}'";
				excelPackage.Workbook.Properties.Created = DateTime.Now;
				ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets.Add("Лист 1");
				for (int listIndex = 0; listIndex < _filesLinkList.Count; listIndex++)
				{
					var excelIndex = listIndex + 1;
					worksheet.Cells[excelIndex, 1].Value = _filesLinkList[listIndex].Item1;
					worksheet.Cells[excelIndex, 2].Value = _fileSch;
					worksheet.Cells[excelIndex, 3].Value = _filesLinkList[listIndex].Item2;
					if (_outputDirectory)
					{
						worksheet.Cells[excelIndex, 5].Value = _filesLinkList[listIndex].Item3;
					}
					worksheet.Cells.AutoFitColumns();

				}
				FileInfo file = new FileInfo(_path + @$"\Для Паруса {SectionName()}.xlsx");
				excelPackage.SaveAs(file);
			}
		}
	}
}
