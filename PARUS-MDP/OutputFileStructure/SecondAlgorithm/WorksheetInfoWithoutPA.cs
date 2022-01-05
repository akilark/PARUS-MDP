using System;
using System.Collections.Generic;
using OfficeOpenXml;
using DataTypes;

namespace OutputFileStructure
{
	/// <summary>
	/// Класс необходимый для выполнения алгоритма "МДП без ПА"
	/// </summary>
	public class WorksheetInfoWithoutPA : WorksheetInfoBase
	{
		private MaximumAllowPowerFlow _maximumAllowPowerFlow;
		private List<ImbalanceAndAutomatics> _maximumAllowPowerFlowNonBalance;
		private AllowPowerOverflows _allowPowerOverflow;

		/// <summary>
		/// Конструктор класса с 5 параметрами
		/// </summary>
		/// <param name="repairScheme">Название ремонтной схемы</param>
		/// <param name="noRegularOscilation">Значение нерегулярных колебаний</param>
		/// <param name="imbalances">Корректный список небалансов</param>
		/// <param name="excelWorksheetPARUS">Эксель файл паруса</param>
		/// <param name="disconnectingLineForEachEmergency">Учет каждого аварийного небаланса
		/// выполнялся отключением соответсвующей ветви?</param>
		public WorksheetInfoWithoutPA(Scheme repairScheme, int noRegularOscilation, List<Imbalance> imbalances,	
			ExcelWorksheet excelWorksheetPARUS, bool disconnectingLineForEachEmergency, string path)
		{
			_errorList = new List<string>();
			Inizialize();
			int startRow;
			if (repairScheme.SchemeName == "Нормальная схема")
			{
				startRow = 9;
			}
			else
			{
				startRow = FindScheme(repairScheme.SchemeName, excelWorksheetPARUS) + 1;
			}
			
			if(startRow == 0)
			{
				startRow = 9;
			}
			
			MainMethod(startRow, imbalances, noRegularOscilation, excelWorksheetPARUS, 
				disconnectingLineForEachEmergency, repairScheme.Disturbance, path);
		}


		/// <summary>
		/// МДП и АДП
		/// </summary>
		public MaximumAllowPowerFlow MaximumAllowPowerFlow => _maximumAllowPowerFlow;

		/// <summary>
		/// Лист небалансов подлежащих учету в приложении № 6 ПУР
		/// </summary>
		public List<ImbalanceAndAutomatics> MaximumAllowPowerFlowNonBalance => _maximumAllowPowerFlowNonBalance;

		/// <summary>
		/// Допустимые перетоки активной мощности
		/// </summary>
		public AllowPowerOverflows AllowPowerOverflow => _allowPowerOverflow;

		private void Inizialize()
		{
			ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
		}

		private void MainMethod(int startRow, List<Imbalance> imbalances, 
			int noRegularOscilation, ExcelWorksheet excelWorksheetPARUS, bool disconnectingLineForEachEmergency, List<(string, bool)> disturbanceDataSource, string path)
		{
			_allowPowerOverflow = NormalSchemeResults(startRow, excelWorksheetPARUS);
			int headRow = startRow + 3;
			if (excelWorksheetPARUS.Cells[headRow, 1].Value != null)
			{
				var bodyRowsAfterFault = ConsideredDisturbances(FindBodyRowDisturbance(headRow, excelWorksheetPARUS), 
					disturbanceDataSource);
				FindAbsentDisturbance(bodyRowsAfterFault, disturbanceDataSource, path);
				List<(string, List<int>)> disturbances = new List<(string, List<int>)>();
				List<(string, List<int>)> disturbancesWithControlAction = new List<(string, List<int>)>();
				for (int i = 0; i < bodyRowsAfterFault.Count; i++)
				{
					if (IsDisturbanceConsiderImbalance(bodyRowsAfterFault[i].Item1,imbalances))
					{
						disturbancesWithControlAction.Add(bodyRowsAfterFault[i]);
					}
					else
					{
						disturbances.Add(bodyRowsAfterFault[i]);
					}
				}
				MaximumAllowPowerFlowDefine(headRow, disturbances, excelWorksheetPARUS);
				MaximumAllowPowerFlowControlActionDefine(headRow, noRegularOscilation, imbalances,
					disturbancesWithControlAction, excelWorksheetPARUS, disconnectingLineForEachEmergency);
			}
		}

		private void MaximumAllowPowerFlowDefine(int headRow, List<(string, List<int>)> disturbances, 
			ExcelWorksheet excelWorksheetPARUS)
		{
			_maximumAllowPowerFlow = new MaximumAllowPowerFlow();
			_maximumAllowPowerFlow.EmergencyAllowPowerFlowValue = _allowPowerOverflow.EmergencyAllowPowerOverflow;
			_maximumAllowPowerFlow.EmergencyAllowPowerCriterion = "8% P исходная схема";
						
			foreach ((string, List<int>) disturbance in disturbances)
			{
				
				var emergency = MaximumAllowPowerFlowDefinition(headRow, disturbance, excelWorksheetPARUS);

				List<int> criteria = new List<int> { emergency.CurrentLoadLinesValue,
						emergency.StaticStabilityPostEmergency, emergency.StabilityVoltageValue, _allowPowerOverflow.StaticStabilityNormal };
				for(int i = 0; i < criteria.Count; i ++)
				{
					if(criteria[i] > 0)
					{
						if (_maximumAllowPowerFlow.MaximumAllowPowerFlowValue == 0 || 
							_maximumAllowPowerFlow.MaximumAllowPowerFlowValue > criteria[i])
						{
							_maximumAllowPowerFlow.MaximumAllowPowerFlowValue = criteria[i];
							_maximumAllowPowerFlow.MaximumAllowPowerCriterion = DefiningCriteria(i, emergency.CurrentLoadLinesCriterion, disturbance.Item1);
						}
					}
				}
			}		
		}

		private void MaximumAllowPowerFlowControlActionDefine(int headRow, int noRegularOscilation, List<Imbalance> imbalances,
			List<(string, List<int>)> disturbanceWithControlAction,	ExcelWorksheet excelWorksheetPARUS, bool disconnectingLineForEachEmergency)
		{
			_maximumAllowPowerFlowNonBalance = new List<ImbalanceAndAutomatics>();

			foreach ((string, List<int>) bodyRow in disturbanceWithControlAction)
			{
				ImbalanceAndAutomatics imbalance = new ImbalanceAndAutomatics();
				var controlAction = FindRightControlAction(imbalances, bodyRow.Item1);
				imbalance.ImbalanceID = controlAction.ParamID;
				if (disconnectingLineForEachEmergency)
				{
					var emergency = MaximumAllowPowerFlowDefinition(headRow, bodyRow, excelWorksheetPARUS);
					List<int> criteria = new List<int> { emergency.CurrentLoadLinesValue, 
						emergency.StaticStabilityPostEmergency, emergency.StabilityVoltageValue };
					for (int i = 0; i < criteria.Count; i++)
					{
						if (criteria[i] > 0)
						{
							if (imbalance.ImbalanceValue == 0 || imbalance.ImbalanceValue > criteria[i])
							{
								imbalance.ImbalanceValue = criteria[i];
								imbalance.ImbalanceCriterion = DefiningCriteria(i, emergency.CurrentLoadLinesCriterion, bodyRow.Item1);
							}
						}
					}
				}
				else
				{
					imbalance.ImbalanceValue = _allowPowerOverflow.EmergencyAllowPowerOverflow - noRegularOscilation;
					imbalance.ImbalanceCriterion = $"8%P ПАР '{bodyRow.Item1}'";
				}

				
				imbalance.ImbalanceCoefficient = controlAction.CoefficientEfficiency;
				imbalance.MaximumImbalance = controlAction.MaxValue;
				var compare = CompareAllowPowerFlowWithImbalanceEquation(controlAction.CoefficientEfficiency,
					controlAction.MaxValue, imbalance.ImbalanceValue, _maximumAllowPowerFlow.MaximumAllowPowerFlowValue);
				if (compare.Item1)
				{
					imbalance.EquationValue = compare.Item2;
					imbalance.Equation = imbalance.ImbalanceValue.ToString() + "-" + 
						controlAction.CoefficientEfficiency.ToString() + "*" + controlAction.ParamID;
					
					_maximumAllowPowerFlowNonBalance.Add(imbalance);
				}
			}
		}

		private (bool, float) CompareAllowPowerFlowWithImbalanceEquation(float coefficientEfficiency,
			int activePowerControlActionMax, int nonBalanceValue, int maximumAllowPowerFlow)
		{
			float equationResult = nonBalanceValue - activePowerControlActionMax * coefficientEfficiency;
			
			if(maximumAllowPowerFlow > equationResult)
			{
				return (true, equationResult);
			}
			else
			{
				return (false, equationResult);
			}
		}


		private AllowPowerOverflows NormalSchemeResults(int startRow, ExcelWorksheet excelWorksheetPARUS)
		{

			string[] columnsNameBeforeFault =
				new string[] { "Рсеч-Рно, МВт", "Перегружаемый элемент", "Рпред*0,8-Pно", "Р(Uкр/0.85)-Рно", "Узел", "Рпред" };
			string[] assignedValuesBeforeFault = new string[columnsNameBeforeFault.Length + 1];

			assignedValuesBeforeFault[assignedValuesBeforeFault.Length - 1] =
				RoundAndMultiply(assignedValuesBeforeFault[assignedValuesBeforeFault.Length - 2], 0.92);
			AllowPowerOverflows allowPowerOverflows = new AllowPowerOverflows();
			//переделать во что-то красивое
			try
			{
				
				allowPowerOverflows.CurrentLoadLinesValue = int.Parse(RoundAndMultiply(FindCellValue(startRow, startRow + 1, columnsNameBeforeFault[0], excelWorksheetPARUS),1));
			}
			catch { }
			try
			{
				allowPowerOverflows.CurrentLoadLinesCriterion = FindCellValue(startRow, startRow + 1, columnsNameBeforeFault[1], excelWorksheetPARUS);
			}
			catch { }
			try
			{
				allowPowerOverflows.StaticStabilityNormal = int.Parse(RoundAndMultiply(FindCellValue(startRow, startRow + 1, columnsNameBeforeFault[2], excelWorksheetPARUS),1));
			}
			catch { }
			try
			{
				allowPowerOverflows.StabilityVoltageValue = int.Parse(RoundAndMultiply(FindCellValue(startRow, startRow + 1, columnsNameBeforeFault[3], excelWorksheetPARUS),1));
			}
			catch { }
			try
			{
				allowPowerOverflows.StabilityVoltageCriterion = FindCellValue(startRow, startRow + 1, columnsNameBeforeFault[4], excelWorksheetPARUS);
			}
			catch { }
			try
			{
				allowPowerOverflows.CriticalValue = int.Parse(RoundAndMultiply(FindCellValue(startRow, startRow + 1, columnsNameBeforeFault[5], excelWorksheetPARUS),1));
			}
			catch { }
			
			allowPowerOverflows.EmergencyAllowPowerOverflow = int.Parse(RoundAndMultiply((allowPowerOverflows.CriticalValue).ToString(),0.92));

			return allowPowerOverflows;
		}
	}
}
