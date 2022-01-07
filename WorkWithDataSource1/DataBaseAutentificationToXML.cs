using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml.Serialization;
using DataTypes;


namespace WorkWithDataSource
{
	public class DataBaseAutentificationToXML
	{
		private string _path;
		private readonly XmlSerializer _xmlSerializer =
			new XmlSerializer(typeof(DataBaseAutentification));

		/// <summary>
		/// Конструктор классас 1 параметром для записи файла
		/// </summary>
		public DataBaseAutentificationToXML(DataBaseAutentification dataBaseAutentification)
		{
			string mydocument = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
			_path = mydocument + @"\PARUS_AFTP_File.db";
			WriteFile(dataBaseAutentification);
		}

		/// <summary>
		/// Конструктор без параметров для чтения файла
		/// </summary>
		public DataBaseAutentificationToXML()
		{
			string mydocument = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
			_path = mydocument + @"\PARUS_AFTP_File.db";

		}

		/// <summary>
		/// Метод для чтения файла
		/// </summary>
		public DataBaseAutentification ReadFileInfo()
		{
			if (File.Exists(_path))
			{
				using (FileStream fs = new FileStream(_path, FileMode.OpenOrCreate))
				{
					return (DataBaseAutentification)_xmlSerializer.Deserialize(fs);
				}
			}
			else
			{
				return new DataBaseAutentification();
			}
		}

		private void WriteFile(DataBaseAutentification dataBaseAutentification)
		{
			using (FileStream fs = new FileStream(_path, FileMode.Create))
			{
				_xmlSerializer.Serialize(fs, dataBaseAutentification);
			}
		}
	}
}
