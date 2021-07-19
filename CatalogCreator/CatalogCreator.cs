using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace CatalogCreator
{
	public class CatalogCreator
	{
		private string _path = @"C:\test";
		private string _rootName = "Камала- Красноярская";
		private bool _reverseable = true;
		private string[] _RepairSchemeName = new string[] {"Ремонт Камала-1- Красноярская",
			"Ремонт Камала-1- Тайшет", "Ремонт Камала-1- Ангара",
			"Ремонт Енисей- Красноярская", "Ремонт Красноярская ГЭС- Енисей"} ;
		private List<(string, string[])> _factors = new List<(string, string[])> {
		("Количество генераторов Красноярская ГРЭС-2", new string[] {"2","4"}),
		("Количество генераторов Красноярская ГЭС-220 кВ", new string[] {"2","4"})};

		//TODO: Добавить свойства
		/*
		CatalogCreator(string path, string rootName)
		{
			_path = path;
			_rootName = rootName;
		}
		*/

		public void Create()
		{
			string pathRoot = Path.Combine(_path, _rootName);
			Directory.CreateDirectory(pathRoot);

			CreateReversable(pathRoot, new string[] { "Переток на запад", "Переток на восток" });
		}

		private void CreateReversable(string pathRoot, string[] direction)
		{
			if(_reverseable == true)
			{
				foreach (string direct in direction)
				{
					string pathReversable = Path.Combine(pathRoot, direct);
					Directory.CreateDirectory(pathReversable);
					CreateScheme(pathReversable);
				}
			}
			else
			{
				CreateScheme(pathRoot);
			}
		}


		private void CreateScheme(string pathReversable)
		{
			foreach(string scheme in _RepairSchemeName)
			{
				string pathScheme = Path.Combine(pathReversable, scheme);
				Directory.CreateDirectory(pathScheme);

				CreateFactors(pathScheme);
			}
		}

		private void CreateFactors(string pathScheme)
		{
			foreach((string, string[]) factor in _factors)
			{
				string pathFactor = Path.Combine(pathScheme, factor.Item1);
				Directory.CreateDirectory(pathFactor);
				foreach(string factorValue in factor.Item2)
				{
					string pathValueFactor = Path.Combine(pathFactor, factorValue);
					Directory.CreateDirectory(pathValueFactor);
				}
			}
		}
	}
}
