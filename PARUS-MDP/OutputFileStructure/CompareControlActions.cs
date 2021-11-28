using System;
using System.Collections.Generic;
using System.Text;
using OutputFileStructure.DataTypes;

namespace OutputFileStructure
{
	public class CompareControlActions
	{
		List<Imbalance> _imbalances;
		List<AOPO> _AOPOlist;
		List<AOCN> _AOCNlist;

		public CompareControlActions(List<ImbalanceDataSource> imbalancesDataSource, List<ControlActionRow> controlActions,
			List<AOPO> AOPO, List<AOCN> AOCN, bool workWithDataSourseInfo)
		{
			CompareImbalances(imbalancesDataSource, controlActions, workWithDataSourseInfo);
			CompareAOPO(AOPO, controlActions);
			CompareAOCN(AOCN, controlActions);
		}

		public List<Imbalance> Imbalances => _imbalances;
		public List<AOPO> AOPOlist => _AOPOlist;
		public List<AOCN> AOCNlist => _AOCNlist;

		private void CompareImbalances(List<ImbalanceDataSource> imbalancesDataSource, List<ControlActionRow> controlActions,
			bool WorkWithDataSourseInfo)
		{
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
