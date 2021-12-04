using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml.Serialization;
using DataTypes;

namespace WorkWithCatalog
{
	//не нужный класс
	public class WorkWithFiles
	{
		private string _path;
		private readonly XmlSerializer _xmlSerializer =
			new XmlSerializer(typeof(List<Scheme>));

		public WorkWithFiles(string path, List<Scheme> schemes)
		{
			_path = path;
			WriteFile(schemes);
		}
		public WorkWithFiles(string path)
		{
			_path = path;
		}
		public List<(string, (string, bool)[])> ReadFileInfo()
		{
			using (FileStream fs = new FileStream(_path, FileMode.OpenOrCreate))
			{
				return (List <(string, (string, bool)[])>)_xmlSerializer.Deserialize(fs);
			}
		}

		private void WriteFile(List<Scheme> schemes)
		{
			using (FileStream fs = new FileStream(_path, FileMode.Create))
			{
				_xmlSerializer.Serialize(fs, schemes);
			}
		}
	}
}
