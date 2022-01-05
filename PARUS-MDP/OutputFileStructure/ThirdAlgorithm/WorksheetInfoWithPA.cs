using System;
using System.Collections.Generic;
using OfficeOpenXml;
using DataTypes;

namespace OutputFileStructure
{
	/// <summary>
	/// Класс необходимый для выполнения алгоритма "МДП с ПА"
	/// </summary>
	public class WorksheetInfoWithPA : WorksheetInfoBase
	{
		private AllowPowerFlowPA _maximumAllowPowerFlowPA;
		private List<ImbalanceAndAutomatics> _maximumAllowPowerFlowNonBalancePA;

		/// <summary>
		/// Конструктор с 11 параметрами
		/// </summary>
		/// <param name="repairScheme">Схема сети.</param>
		/// <param name="noRegularOscilation">Значение нерегулярных колебаний</param>
		/// <param name="allowPowerOverflow">Допустимый переток, полученный по результатам 
		/// работы алгоритма "МДП без ПА"</param>
		/// <param name="imbalances">Корректный список небалансов</param>
		/// <param name="excelWorksheetPARUS">Эксель файл паруса</param>
		/// <param name="firstAlghorithmResult">Лист небалансов подлежащих учету в приложении 
		/// № 6 ПУР полученный в результате выполнения алгоритма "МДП без ПА и АДП"</param>
		/// <param name="AOPOlist">Корректный список АОПО</param>
		/// <param name="AOCNlist">Корректный список АОСН</param>
		/// <param name="LAPNYlist">Корректный список ЛАПНУ</param>
		/// <param name="disconnectingLineForEachEmergency">Учет каждого аварийного небаланса
		/// выполнялся отключением соответсвующей ветви?</param>
		public WorksheetInfoWithPA(Scheme repairScheme,int noRegularOscilation, AllowPowerOverflows allowPowerOverflow, 
			List<Imbalance> imbalances, ExcelWorksheet excelWorksheetPARUS, List<ImbalanceAndAutomatics> firstAlghorithmResult, List<AOPO> AOPOlist,
			List<AOCN> AOCNlist, List<ControlActionRow> LAPNYlist, bool disconnectingLineForEachEmergency, string path)
		{
			ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
			_maximumAllowPowerFlowPA = new AllowPowerFlowPA();
			_errorList = new List<string>();
			_maximumAllowPowerFlowNonBalancePA = new List<ImbalanceAndAutomatics>();
			_maximumAllowPowerFlowPA.ValueWithPA = allowPowerOverflow.StaticStabilityNormal;
			_maximumAllowPowerFlowPA.CriteriumValueWithPA = "20% P исходная схема";
			
			int startRow;
			if (repairScheme.SchemeName == "Нормальная схема")
			{
				startRow = 9;
			}
			else
			{
				startRow = FindScheme(repairScheme.SchemeName, excelWorksheetPARUS) + 1;
			}

			if (startRow == 0)
			{
				startRow = 9;
			}

			MainMethod(startRow, imbalances, noRegularOscilation, allowPowerOverflow, excelWorksheetPARUS,
				firstAlghorithmResult, AOPOlist, AOCNlist, LAPNYlist, repairScheme.Disturbance, disconnectingLineForEachEmergency, path);

			
		}

		/// <summary>
		/// Допустимый переток с ПА
		/// </summary>
		public AllowPowerFlowPA AllowPowerFlowPA => _maximumAllowPowerFlowPA;

		/// <summary>
		/// Лист небалансов подлежащих учету в приложении № 6 ПУР
		/// </summary>
		public List<ImbalanceAndAutomatics> Imbalances => _maximumAllowPowerFlowNonBalancePA;

		private void MainMethod(int startRow, List<Imbalance> imbalances, int noRegularOscilation, 
			AllowPowerOverflows allowPowerOverflow, ExcelWorksheet excelWorksheetPARUS, List<ImbalanceAndAutomatics> firstAlghorithmResult,
			List<AOPO> AOPOlist, List<AOCN> AOCNlist, List<ControlActionRow> LAPNYlist, List<(string, bool)> disturbanceDataSource, bool disconnectingLineForEachEmergency, string path)
		{
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
					if (IsDisturbanceConsiderImbalance(bodyRowsAfterFault[i].Item1, imbalances))
					{
						disturbancesWithControlAction.Add(bodyRowsAfterFault[i]);
					}
					else
					{
						disturbances.Add(bodyRowsAfterFault[i]);
					}
				}
				MaximumAllowPowerFlowDefineWithPA(headRow, disturbances, allowPowerOverflow, AOPOlist, AOCNlist, LAPNYlist,excelWorksheetPARUS, disturbanceDataSource);
				MaximumAllowPowerFlowControlActionDefineWithPA(headRow,noRegularOscilation, disturbancesWithControlAction, 
					imbalances,	allowPowerOverflow, firstAlghorithmResult, excelWorksheetPARUS, disconnectingLineForEachEmergency);

			}
		}

		private void MaximumAllowPowerFlowDefineWithPA(int headRow, List<(string, List<int>)> disturbances, 
			AllowPowerOverflows allowPowerOverflow,	List<AOPO> AOPOlist, List<AOCN> AOCNlist, List<ControlActionRow> LAPNYlist, ExcelWorksheet excelWorksheetPARUS, List<(string, bool)> disturbanceDataSource)
		{
			foreach ((string, List<int>) disturbance in disturbances)
			{
				
				var emergency = MaximumAllowPowerFlowDefinition(headRow, disturbance, excelWorksheetPARUS);

				for (int valueNumber = 0; valueNumber < disturbance.Item2.Count; valueNumber++)
				{
					try
					{
						if (FindCellValue(headRow, disturbance.Item2[valueNumber], "Примечание", excelWorksheetPARUS) == "АОПО")
						{
							continue;
						}
					}
					catch
					{

					}

					try
					{
						string textValueCurrent = FindCellValue(headRow, disturbance.Item2[valueNumber],
														"Перегружаемый элемент", excelWorksheetPARUS);
						AOPO aopoTmp = new AOPO(); 
						foreach(AOPO aopo in AOPOlist)
						{
							if(aopo.LineName == textValueCurrent)
							{
								aopoTmp = aopo;
							}
						}
						if (aopoTmp.LineName != null)
						{
							int currentValueTmp = int.Parse(RoundAndMultiply(
								FindCellValue(headRow, disturbance.Item2[valueNumber], "Рсеч-Рно, МВт", excelWorksheetPARUS), 1));
							if (_maximumAllowPowerFlowPA.EqupmentOverloadingWithoutPA == 0 ||
								(_maximumAllowPowerFlowPA.EqupmentOverloadingWithoutPA > currentValueTmp && currentValueTmp > 0))
							{
								_maximumAllowPowerFlowPA.EqupmentOverloadingWithoutPA = currentValueTmp;
								_maximumAllowPowerFlowPA.CriteriumEqupmentOverloadingWithoutPA = FindCellValue(headRow, disturbance.Item2[valueNumber],
																"Перегружаемый элемент", excelWorksheetPARUS);
								_maximumAllowPowerFlowPA.DisconnectionLineFactEqupmentOverloading = disturbance.Item1;
								_maximumAllowPowerFlowPA.CriteriumEqupmentOverloadingWithPA = $"АДТН '{_maximumAllowPowerFlowPA.CriteriumEqupmentOverloadingWithoutPA}'" +
									$" ПАР '{_maximumAllowPowerFlowPA.DisconnectionLineFactEqupmentOverloading}' с учетом объема УВ";
								_maximumAllowPowerFlowPA.ControlActionAOPO = aopoTmp.Automatic;
							}
						}
					}
					catch
					{

					}

					try
					{
						string textValueVoltage = FindCellValue(headRow, disturbance.Item2[valueNumber],
													"Узел", excelWorksheetPARUS);
						AOCN aocnTmp = new AOCN();
						foreach (AOCN aocn in AOCNlist)
						{
							if (aocn.NodeName == textValueVoltage)
							{
								aocnTmp = aocn;
							}
						}
						if (aocnTmp != null)
						{
							if (FindCellValue(headRow, disturbance.Item2[valueNumber], "Рда(Uкр/0.9)-Рно", excelWorksheetPARUS) != "Критерий по U не достижим")
							{
								int voltageValueTmp = int.Parse(RoundAndMultiply(
								FindCellValue(headRow, disturbance.Item2[valueNumber], "Рда(Uкр/0.9)-Рно", excelWorksheetPARUS), 1));
								if (_maximumAllowPowerFlowPA.VoltageLimitingWithoutPA == 0 ||
									(_maximumAllowPowerFlowPA.VoltageLimitingWithoutPA > voltageValueTmp && voltageValueTmp > 0))
								{
									_maximumAllowPowerFlowPA.VoltageLimitingWithoutPA = voltageValueTmp;
									_maximumAllowPowerFlowPA.DisconnectionLineFactVoltageLimiting = disturbance.Item1;
									_maximumAllowPowerFlowPA.CriteriumVoltageLimitingWithPA = $"10% U ПАР '{_maximumAllowPowerFlowPA.DisconnectionLineFactVoltageLimiting}'" +
										$" с учетом объема УВ";
									_maximumAllowPowerFlowPA.ControlActionAOCN = aocnTmp.Automatic;
								}
							}
						}
					}
					catch
					{

					}
					
					string overloadingElement = FindCellValue(headRow, disturbance.Item2[valueNumber],
														"Перегружаемый элемент", excelWorksheetPARUS);
					bool automaticFlag = false;
					foreach (AOPO aopo in AOPOlist)
					{
						if (aopo.LineName == overloadingElement)
						{
							automaticFlag = true;
							continue;
						}
					}
					if(automaticFlag || overloadingElement == "Токовых перегрузов нет")
					{
						continue;
					}

					bool automaticForDisturbance = false;
					
					foreach ((string, bool) disturbanceDS in disturbanceDataSource)
					{
						if (disturbanceDS.Item1.ToLower().Contains(disturbance.Item1.ToLower()))
						{
							automaticForDisturbance = disturbanceDS.Item2;
							break;
						}
					}

					if (automaticForDisturbance)
					{
						
						List<int> criteria = new List<int> { emergency.CurrentLoadLinesValue,
						emergency.StaticStabilityPostEmergency, emergency.StabilityVoltageValue };
						for (int i = 0; i < criteria.Count; i++)
						{
							if (criteria[i] > 0)
							{
								if (_maximumAllowPowerFlowPA.LocalAutomaticValueWithoutPA == 0 ||
									_maximumAllowPowerFlowPA.LocalAutomaticValueWithoutPA > criteria[i])
								{
									_maximumAllowPowerFlowPA.LocalAutomaticValueWithoutPA = criteria[i];
									_maximumAllowPowerFlowPA.CriteriumLocalAutomaticValueWithoutPA =
										DefiningCriteria(i, emergency.CurrentLoadLinesCriterion, disturbance.Item1) + " с учетом объема УВ";
									_maximumAllowPowerFlowPA.ControlActionsLAPNY = LAPNYlist;
								}
							}
						}
					}
					else
					{
						List<int> criteria = new List<int> { emergency.CurrentLoadLinesValue,
						emergency.StaticStabilityPostEmergency, emergency.StabilityVoltageValue, allowPowerOverflow.StaticStabilityNormal };
						for (int i = 0; i < criteria.Count; i++)
						{
							if (criteria[i] > 0)
							{
								if (_maximumAllowPowerFlowPA.ValueWithPA == 0 ||
									_maximumAllowPowerFlowPA.ValueWithPA > criteria[i])
								{
									_maximumAllowPowerFlowPA.ValueWithPA = criteria[i];
									_maximumAllowPowerFlowPA.CriteriumValueWithPA = DefiningCriteria(i, emergency.CurrentLoadLinesCriterion, disturbance.Item1);
								}
							}
						}
					}
				}
			}
		}

		private void MaximumAllowPowerFlowControlActionDefineWithPA(int headRow, int noRegularOscilation,
			List<(string, List<int>)> disturbanceWithControlAction, List<Imbalance> imbalances,	AllowPowerOverflows allowPowerOverflow, 
			List<ImbalanceAndAutomatics> firstAlghorithmResult, ExcelWorksheet excelWorksheetPARUS, bool disconnectingLineForEachEmergency)
		{
			_maximumAllowPowerFlowNonBalancePA = new List<ImbalanceAndAutomatics>();

			foreach ((string, List<int>) bodyRow in disturbanceWithControlAction)
			{
				Imbalance imbalanceTmp = new Imbalance();
				foreach(Imbalance imbalance in imbalances)
				{
					if (Comparator.CompareString(imbalance.LineName, bodyRow.Item1))
					{
						imbalanceTmp = imbalance;
						break;
					}
				}
				if(imbalanceTmp.ImbalanceValue == null)
				{
					continue;
				}
				var controlAction = FindRightControlAction(imbalances, bodyRow.Item1);
				ImbalanceAndAutomatics imbalanceOutput = new ImbalanceAndAutomatics();
				imbalanceOutput.ImbalanceID = controlAction.ParamID;
				if (disconnectingLineForEachEmergency)
				{
					var emergency = MaximumAllowPowerFlowDefinition(headRow, bodyRow, excelWorksheetPARUS);
					List<int> criteria = new List<int> { emergency.CurrentLoadLinesValue,
						emergency.StaticStabilityPostEmergency, emergency.StabilityVoltageValue };
					for (int i = 0; i < criteria.Count; i++)
					{

						if (criteria[i] > 0)
						{
							if (imbalanceOutput.ImbalanceValue == 0 || imbalanceOutput.ImbalanceValue > criteria[i])
							{
								imbalanceOutput.ImbalanceValue = criteria[i];
								imbalanceOutput.ImbalanceCriterion = DefiningCriteria(i, emergency.CurrentLoadLinesCriterion, bodyRow.Item1);
								if (imbalanceTmp.ARPM != null)
								{
									imbalanceOutput.ImbalanceCriterion = imbalanceOutput.ImbalanceCriterion + " с учетом объема УВ";
								}
							}
						}
					}
				}
				else
				{
					imbalanceOutput.ImbalanceValue = allowPowerOverflow.EmergencyAllowPowerOverflow - noRegularOscilation;
					imbalanceOutput.ImbalanceCriterion = $"8%P ПАР '{bodyRow.Item1}'";
					if(imbalanceTmp.ARPM != null)
					{
						imbalanceOutput.ImbalanceCriterion = imbalanceOutput.ImbalanceCriterion + " с учетом объема УВ";
					}
				}

				foreach(ImbalanceAndAutomatics imbalanceAndAutomatics in firstAlghorithmResult)
				{
					if (imbalanceAndAutomatics.ImbalanceID == imbalanceOutput.ImbalanceID)
					{
						imbalanceOutput.ImbalanceCoefficient = imbalanceTmp.ImbalanceValue.CoefficientEfficiency;
						imbalanceOutput.MaximumImbalance = imbalanceTmp.ImbalanceValue.MaxValue;
						imbalanceOutput.Equation = imbalanceOutput.ImbalanceValue.ToString() + " - " +
							imbalanceOutput.ImbalanceCoefficient.ToString() + "*" + imbalanceAndAutomatics.ImbalanceID;
						imbalanceOutput.EquationValue = imbalanceOutput.ImbalanceValue - imbalanceOutput.ImbalanceCoefficient * imbalanceOutput.MaximumImbalance;
						if (imbalanceTmp.ARPM != null)
						{
							imbalanceOutput.Equation = imbalanceOutput.Equation +  " + " + 
								imbalanceTmp.ARPM.CoefficientEfficiency.ToString() + "*" + imbalanceTmp.ARPM.ParamID;
						}
						_maximumAllowPowerFlowNonBalancePA.Add(imbalanceOutput);
					}
				}
			}
		}
	}

}
