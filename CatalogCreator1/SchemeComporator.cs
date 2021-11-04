using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace WorkWithCatalog
{
	public class SchemeComporator
	{
		private bool _compareResults;
		List<(string, (string, bool)[])> _necessarySchemes;
		public SchemeComporator(List<(string, (string, bool)[])> necessarySchemes, List<(string, (string, bool)[])> existingSchemes)
		{
			_compareResults = Compare(necessarySchemes, existingSchemes);
			if(!_compareResults)
			{
				_necessarySchemes = necessarySchemes;
			}
			else
			{
				_necessarySchemes = new List<(string, (string, bool)[])>();
			}

		}

		private bool Compare(List<(string, (string, bool)[])> necessarySchemes, List<(string, (string, bool)[])> existingSchemes)
		{
			bool Disturbanceflag = true;
			bool SchemeFlag;
			foreach ((string, (string, bool)[]) necessaryScheme in necessarySchemes)
			{
				SchemeFlag = false;
				foreach ((string, (string, bool)[]) existingScheme in existingSchemes)
				{
					if (necessaryScheme.Item1.Trim().ToLower() == existingScheme.Item1.Trim().ToLower())
					{
						SchemeFlag = true;
						if (necessaryScheme.Item2.Length != existingScheme.Item2.Length)
						{
							return false;
						}
						else
						{
							for (int i = 0; i < necessaryScheme.Item2.Length; i++)
							{
								if (necessaryScheme.Item2[i].Item1.Trim().ToLower() != existingScheme.Item2[i].Item1.Trim().ToLower())
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

		public List<(string, (string, bool)[])> SchemesForXML => _necessarySchemes;
	}
}
