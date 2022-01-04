using System.Collections.Generic;
using DataTypes;

namespace OutputFileStructure
{
	/// <summary>
	/// Класс необходимый для сравнения УВ и НБ из БД с УВ из шаблона
	/// </summary>
	public class CompareControlActions
	{
		List<Imbalance> _imbalances;
		List<AOPO> _AOPOlist;
		List<AOCN> _AOCNlist;
		List<ControlActionRow> _LAPNYlist;

		/// <summary>
		/// Конструктор класса с 5 параметрами
		/// </summary>
		/// <param name="imbalancesDataSource"> НБ из базы данных</param>
		/// <param name="controlActions">УВ/НБ из файла шаблона</param>
		/// <param name="AOPOdataSource">АОПО из базы данных</param>
		/// <param name="AOCNdataSource">АОСН из базы данных</param>
		/// <param name="workWithDataSourseInfo">Данные из БД имеют приоритет над данными из шаблона ?</param>
		public CompareControlActions(List<ImbalanceDataSource> imbalancesDataSource, List<ControlActionRow> controlActions,
			List<AOPO> AOPOdataSource, List<AOCN> AOCNdataSource, bool workWithDataSourseInfo)
		{
			
			CompareImbalances(imbalancesDataSource, controlActions, workWithDataSourseInfo);
			CompareAOPO(AOPOdataSource, controlActions);
			CompareAOCN(AOCNdataSource, controlActions);
			GenerateLAPNYlist(controlActions);
		}

		/// <summary>
		/// Корректный список небалансов
		/// </summary>
		public List<Imbalance> Imbalances => _imbalances;
		
		/// <summary>
		/// Корректный список АОПО
		/// </summary>
		public List<AOPO> AOPOlist => _AOPOlist;

		/// <summary>
		/// Корректный список АОСН
		/// </summary>
		public List<AOCN> AOCNlist => _AOCNlist;

		/// <summary>
		/// Корректный список ЛАПНУ
		/// </summary>
		public List<ControlActionRow> LAPNYlist => _LAPNYlist;

		private void GenerateLAPNYlist(List<ControlActionRow> controlActions)
		{
			_LAPNYlist = new List<ControlActionRow>();
			foreach(ControlActionRow controlAction in controlActions)
			{
				if(controlAction.ParamSign.ToLower().Trim().Contains("лапну"))
				{
					_LAPNYlist.Add(controlAction);
				}
			}
		}

		private void CompareImbalances(List<ImbalanceDataSource> imbalancesDataSource, List<ControlActionRow> controlActions,
			bool WorkWithDataSourseInfo)
		{
			_imbalances = new List<Imbalance>();
			foreach (ImbalanceDataSource imbalanceDataSource in imbalancesDataSource)
			{
				var imbalance = new Imbalance();
				imbalance.LineName = imbalanceDataSource.LineName;

				if (WorkWithDataSourseInfo)
				{
					imbalance.ImbalanceValue = imbalanceDataSource.Imbalance;
				}
				else
				{
					foreach (ControlActionRow controlAction in controlActions)
					{
						if (imbalanceDataSource.ImbalanceName == controlAction.ParamSign)
						{
							imbalance.ImbalanceValue = controlAction;
						}
					}
				}
				foreach (ControlActionRow controlAction in controlActions)
				{
					if (imbalanceDataSource.ARPMName == controlAction.ParamSign)
					{
						imbalance.ARPM = controlAction;
					}
				}
				if (imbalance.ImbalanceValue != null)
				{
					_imbalances.Add(imbalance);
				}
			}
		}
		private void CompareAOPO(List<AOPO> AOPODataSource, List<ControlActionRow> controlActions)
		{
			foreach(AOPO aopo in AOPODataSource)
			{
				foreach (ControlActionRow controlAction in controlActions)
				{
					if (aopo.AutomaticName == controlAction.ParamSign)
					{
						aopo.Automatic = controlAction;
					}
				}
			}
			_AOPOlist = AOPODataSource;
		}
		private void CompareAOCN(List<AOCN> AOCNDataSource, List<ControlActionRow> controlActions)
		{
			foreach (AOCN aocn in AOCNDataSource)
			{
				foreach (ControlActionRow controlAction in controlActions)
				{
					if (aocn.AutomaticName == controlAction.ParamSign)
					{
						aocn.Automatic = controlAction;
					}
				}
			}
			_AOCNlist = AOCNDataSource;
		}
	}
}
