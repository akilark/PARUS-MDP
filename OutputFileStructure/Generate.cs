using System;
using System.Collections.Generic;
using System.Text;
using CatalogCreator;

namespace OutputFileStructure
{
	public class Generate
	{
		private string _pathValue;
		private string _pathOut;
		private CatalogReader _catalog;
		public Generate(string pathWithValue, string pathOut) 
		{
			_pathValue = pathWithValue;
			_pathOut = pathOut;
			_catalog = new CatalogReader(_pathValue);
			Console.WriteLine("_catalog.Reversable");
		}


	}
}
