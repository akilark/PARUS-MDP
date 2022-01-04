using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml.Serialization;
using DataTypes;


namespace WorkWithCatalog
{
	/// <summary>
	/// Класс необходимый для записи данных из БД в файл XML структуры и чтения файлов
	/// </summary>
	public class SectionInfoToXml
	{
		private string _path;
		private readonly XmlSerializer _xmlSerializer =
			new XmlSerializer(typeof(SectionFromDataSource));

		/// <summary>
		/// Конструктор класса с 2 параметрами для записи файла
		/// </summary>
		/// <param name="path">Путь в файловой системе для записи</param>
		/// <param name="sectionFromDataSource">Информацию о сечении</param>
		public SectionInfoToXml(string path, SectionFromDataSource sectionFromDataSource)
		{
			_path = path;
			WriteFile(sectionFromDataSource);
		}

		/// <summary>
		/// Конструктор с 1 параметром для чтения файла
		/// </summary>
		/// <param name="path">Путь в файловой системе к файлу, который необходимо прочесть</param>
		public SectionInfoToXml(string path)
		{
			_path = path;
		}

		/// <summary>
		/// Метод для чтения файла
		/// </summary>
		/// <returns>Информацию о сечении</returns>
		public SectionFromDataSource ReadFileInfo()
		{
			using (FileStream fs = new FileStream(_path, FileMode.OpenOrCreate))
			{
				return (SectionFromDataSource)_xmlSerializer.Deserialize(fs);
			}
		}

		private void WriteFile(SectionFromDataSource sectionFromDataSource)
		{
			using (FileStream fs = new FileStream(_path + @"\configurationFile.kek", FileMode.Create))
			{
				_xmlSerializer.Serialize(fs, sectionFromDataSource);
			}
		}
	}
}
