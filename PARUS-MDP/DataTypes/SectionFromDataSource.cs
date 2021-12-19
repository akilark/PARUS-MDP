using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTypes
{
	[Serializable]
	public class SectionFromDataSource
	{
		private string _sectionName;
		private List<FactorsWithDirection> _factors = new List<FactorsWithDirection>();
		private List<Scheme> _schemes = new List<Scheme>();
		private List<ImbalanceDataSource> _imbalancesDataSource;
		private List<AOPO> _AOPOdataSource;
		private List<AOCN> _AOCNdataSource;
		public SectionFromDataSource() { }

		public string SectionName
		{
			get
			{
				return _sectionName;
			}
			set
			{
				_sectionName = value;
			}
		}

		

		/// <summary>
		/// Свойство возвращающее список факторов в формате (string, (string, string[])[])
		/// </summary>
		public List<FactorsWithDirection> Factors
		{
			get
			{
				return _factors;
			}
			set
			{
				_factors = value;
			}
		}

		/// <summary>
		/// Свойство возврщающее список схем в формате (string, (string, bool)[])
		/// </summary>
		public List<Scheme> Schemes
		{
			get
			{
				return _schemes;
			}
			set
			{
				_schemes = value;
			}
		}

		public List<ImbalanceDataSource> Imbalances
		{
			get
			{
				return _imbalancesDataSource;
			}
			set
			{
				_imbalancesDataSource = value;
			}
		}
		public List<AOPO> AOPOlist
		{
			get
			{
				return _AOPOdataSource;
			}
			set
			{
				_AOPOdataSource = value;
			}
		}
		public List<AOCN> AOCNlist
		{
			get
			{
				return _AOCNdataSource;
			}
			set
			{
				_AOCNdataSource = value;
			}
		}
	}
}
