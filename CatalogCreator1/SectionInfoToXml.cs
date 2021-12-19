using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml.Serialization;
using DataTypes;


namespace WorkWithCatalog
{

	public class SectionInfoToXml
	{
		private string _path;
		private readonly XmlSerializer _xmlSerializer =
			new XmlSerializer(typeof(SectionFromDataSource));

		public SectionInfoToXml(string path, SectionFromDataSource sectionFromDataSource)
		{
			_path = path;
			WriteFile(sectionFromDataSource);
		}
		public SectionInfoToXml(string path)
		{
			_path = path;
		}
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
