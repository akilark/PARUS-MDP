using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using DataTypes;

namespace WorkWithCatalog
{
	//не нужный класс
	public class SchemeComporator
	{
		private bool _compareResults;
		List<Scheme> _necessarySchemes;
		public SchemeComporator(List<Scheme> necessarySchemes, List<Scheme> existingSchemes)
		{
			_compareResults = Compare(necessarySchemes, existingSchemes);
			if(!_compareResults)
			{
				_necessarySchemes = necessarySchemes;
			}
			else
			{
				_necessarySchemes = new List<Scheme>();
			}

		}

		private bool Compare(List<Scheme> necessarySchemes, List<Scheme> existingSchemes)
		{
			bool Disturbanceflag = true;
			bool SchemeFlag;
			foreach (Scheme necessaryScheme in necessarySchemes)
			{
				SchemeFlag = false;
				foreach (Scheme existingScheme in existingSchemes)
				{
					if (necessaryScheme.SchemeName.Trim().ToLower() == existingScheme.SchemeName.Trim().ToLower())
					{
						SchemeFlag = true;
						if (necessaryScheme.Disturbance.Count != existingScheme.Disturbance.Count)
						{
							return false;
						}
						else
						{
							for (int i = 0; i < necessaryScheme.Disturbance.Count; i++)
							{
								if (necessaryScheme.Disturbance[i].Item1.Trim().ToLower() != existingScheme.Disturbance[i].Item1.Trim().ToLower())
								{
									return false;
								}
							}
						}
					}
				}
				if (Disturbanceflag == false || SchemeFlag == false)
				{
					return false;
				}
			}
			return true;
		}

		public bool CompareResults => _compareResults;

		public List<Scheme> SchemesForXML => _necessarySchemes;
	}
}
