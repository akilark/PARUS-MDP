using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using DataTypes;


namespace WorkWithDataSource
{
	/// <summary>
	/// Класс для вытягивания всей информации для конкретного сечения из БД
	/// </summary>
	public class PullData
	{
		private string _sectionName;
		private List<string> _sections = new List<string>();
		private List<FactorsWithDirection> _factors = new List<FactorsWithDirection>();
		private List<Scheme> _schemes = new List<Scheme>();
		private DataBaseAutentification _sqlConnectionString;
		private List<ImbalanceDataSource> _imbalancesDataSource;
		private List<AOPO> _AOPOdataSource;
		private List<AOCN> _AOCNdataSource;
		private bool _connectedFlag;

		/// <summary>
		/// Конструктор класса c 1 параметром
		/// </summary>
		public PullData(DataBaseAutentification dataBaseAutentification)
		{
			_connectedFlag = true;
			_sqlConnectionString = dataBaseAutentification;
			try
			{
				PullSections();
			}
			catch
			{
				_connectedFlag = false;
			}
		}

		/// <summary>
		/// Конструктор класса с 2 параметрами
		/// </summary>
		/// <param name="sectionName">название сечения</param>
		public PullData(string sectionName, DataBaseAutentification dataBaseAutentification)
		{
			_connectedFlag = true;
			_sectionName = sectionName;
			_sqlConnectionString = dataBaseAutentification;
			_imbalancesDataSource = new List<ImbalanceDataSource>();
			_AOCNdataSource = new List<AOCN>();
			_AOPOdataSource = new List<AOPO>();
			try
			{
				PullSections();
				PullFactors();
				PullSchemes();
				PullImbalance();
				PullARPM();
				PullAOPO();
				PullAOCN();
			}
			catch
			{
				_connectedFlag = false;
			}

		}

		/// <summary>
		/// Свойство возвращающее список контролируемых сечений 
		/// </summary>
		public List<string> Sections => _sections;

		/// <summary>
		/// Свойство возвращающее список факторов для сечения
		/// </summary>
		public List<FactorsWithDirection> Factors => _factors;

		public bool IsConnected => _connectedFlag;

		/// <summary>
		/// Свойство возврщающее список схем для сечения
		/// </summary>
		public List<Scheme> Schemes => _schemes;

		/// <summary>
		/// Свойство возвращающее список небалансов
		/// </summary>
		public List<ImbalanceDataSource> Imbalances => _imbalancesDataSource;

		/// <summary>
		/// Свойство возвращающее список АОПО
		/// </summary>
		public List<AOPO> AOPOlist => _AOPOdataSource;

		/// <summary>
		/// Свойство возвращающее список АОСН
		/// </summary>
		public List<AOCN> AOCNlist => _AOCNdataSource;

		/// <summary>
		/// Метод для заполнения списка факторов из БД
		/// </summary>
		public void PullFactors()
		{
			_factors = new List<FactorsWithDirection>();
			string sqlExpression = @$"SELECT  Factors.Direction, Factors.Factor, Factors.FactorValue
				FROM [dbo].[Sections], [dbo].[Factors]
				WHERE Sections.Section_ID = Factors.Section_ID and Sections.Section = '{_sectionName}'";
			ConnectWithDataBase(sqlExpression, DataType.Factor);
		}

		private void PullImbalance()
		{
			string sqlExpression = @$"SELECT ImbalanceLine.NameLine, Imbalance.Name, Imbalance.Direction, 
				Imbalance.MaxValue, Imbalance.Coefficient, Imbalance.Identificator 
				FROM [dbo].[Imbalance], [dbo].[ImbalanceLine]
				WHERE Imbalance.Imbalance_ID = ImbalanceLine.Imbalance_ID";
			ConnectWithDataBase(sqlExpression, DataType.Imbalance);
		}

		private void PullARPM()
		{
			string sqlExpression = @$"SELECT  [ImbalanceLine].[NameLine],[ARPM].[Name]
				FROM [dbo].[ARPM], [dbo].[Imbalance], [dbo].[ImbalanceLine]
				Where Imbalance.Imbalance_ID = ARPM.Imbalance_ID and Imbalance.Imbalance_ID = ImbalanceLine.Imbalance_ID";
			ConnectWithDataBase(sqlExpression, DataType.ARPM);
		}

		private void PullAOPO()
		{
			string sqlExpression = @$"SELECT [NetworkElementName] ,[NameAOPO]
				FROM [dbo].[AOPO]";
			ConnectWithDataBase(sqlExpression, DataType.AOPO);
		}

		private void PullAOCN()
		{
			string sqlExpression = @$"SELECT [ControlPointName],[NameAOCN]
				FROM [dbo].[AOCN]";
			ConnectWithDataBase(sqlExpression, DataType.AOCN);
		}

		/// <summary>
		/// Метод для заполнения списка контролируемых сечений из БД
		/// </summary>
		private void PullSections()
		{
			string sqlExpression = @$"SELECT Sections.Section FROM [dbo].[Sections]";
			ConnectWithDataBase(sqlExpression, DataType.Section);
		}

		/// <summary>
		/// Метод для заполнения списка схем из БД
		/// </summary>
		public void PullSchemes()
		{
			_schemes = new List<Scheme>();
			string sqlExpression = @$"SELECT Schemes.Scheme, Schemes.Disturbance, Schemes.Automation
				FROM [dbo].[Schemes],[dbo].[Sections]
				WHERE Sections.Section_ID = Schemes.Section_ID and Sections.Section = '{_sectionName}'";
			ConnectWithDataBase(sqlExpression, DataType.Scheme);
		}

		/// <summary>
		/// Метод для соединения с БД
		/// </summary>
		/// <param name="sqlExpression">Строка подключения к БД</param>
		/// <param name="dataType">Что необходимо извлечь</param>
		private void ConnectWithDataBase(string sqlExpression, DataType dataType)
		{
			using (SqlConnection connection = new SqlConnection(
				_sqlConnectionString.StringForConnect))
			{
				using (SqlCommand command = new SqlCommand(sqlExpression, connection))
				{
					connection.Open();
					using (SqlDataReader reader = command.ExecuteReader())
					{
						switch (dataType)
						{
							case DataType.Section:
								{
									SectionConvertToList(reader);
									break;
								}
							case DataType.Factor:
								{
									FactorsConvertToList(reader);
									break;
								}
							case DataType.Scheme:
								{
									SchemeConvertToList(reader);
									break;
								}
							case DataType.Imbalance:
								{
									ImbalanceAllConvertToList(reader);
									break;
								}
							case DataType.ARPM:
								{
									ARPMaddToList(reader);
									break;
								}
							case DataType.AOCN:
								{
									AOCNConvertToList(reader);
									break;
								}
							case DataType.AOPO:
								{
									AOPOConvertToList(reader);
									break;
								}
						}
					}
				}
			}
		}

		private void ImbalanceAllConvertToList(SqlDataReader reader)
		{
			while (reader.Read())
			{
				ImbalanceDataSource imbalanceDataSource = new ImbalanceDataSource();
				imbalanceDataSource.LineName = reader.GetString(0);
				imbalanceDataSource.ImbalanceName = reader.GetString(1);
				imbalanceDataSource.Imbalance = new ControlActionRow();
				imbalanceDataSource.Imbalance.Direction = reader.GetString(2);
				imbalanceDataSource.Imbalance.MaxValue = int.Parse(reader.GetDecimal(3).ToString());
				imbalanceDataSource.Imbalance.CoefficientEfficiency = reader.GetFloat(4);
				imbalanceDataSource.Imbalance.ParamID = reader.GetString(5);
				imbalanceDataSource.Imbalance.ParamSign = reader.GetString(1);
				
				_imbalancesDataSource.Add(imbalanceDataSource);
			}
		}

		private void ARPMaddToList(SqlDataReader reader)
		{
			while (reader.Read())
			{
				foreach(ImbalanceDataSource imbalanceDataSource in _imbalancesDataSource)
				{
					if(imbalanceDataSource.LineName == reader.GetString(0))
					{
						imbalanceDataSource.ARPMName = reader.GetString(1);
					}
				}
			}
		}

		private void AOCNConvertToList(SqlDataReader reader)
		{
			while (reader.Read())
			{
				AOCN AOCNtmp = new AOCN();
				AOCNtmp.NodeName = reader.GetString(0);
				AOCNtmp.AutomaticName = reader.GetString(1);
				_AOCNdataSource.Add(AOCNtmp);
			}
		}

		private void AOPOConvertToList(SqlDataReader reader)
		{
			while (reader.Read())
			{
				AOPO AOPOtmp = new AOPO();
				AOPOtmp.LineName = reader.GetString(0);
				AOPOtmp.AutomaticName = reader.GetString(1);
				_AOPOdataSource.Add(AOPOtmp);
			}
		}

		/// <summary>
		/// Перевод данных о сечениях из БД в тип данных используемый в приложении
		/// </summary>
		/// <param name="reader"> Объект класса SqlDataReader предоставляет 
		/// способ чтения потока строк в БД</param>
		private void SectionConvertToList(SqlDataReader reader)
		{
			while (reader.Read())
			{
				_sections.Add(reader.GetString(0));
			}
		}

		/// <summary>
		/// Перевод данных о факторах из БД в тип данных используемый в приложении
		/// </summary>
		/// <param name="reader"> Объект класса SqlDataReader предоставляет 
		/// способ чтения потока строк в БД</param>
		private void FactorsConvertToList(SqlDataReader reader)
		{
			string[] factorValues = new string[0];

			List<(string, string[])> factors = new List<(string, string[])>();
			bool firstFactorFlag = true;
			bool firstDirectionFlag = true;
			string compareDirection = "";
			string compareFactor = "";
			while (reader.Read())
			{
				if (reader.GetString(1) != compareFactor || reader.GetString(0) != compareDirection)
				{
					if (!firstFactorFlag)
					{
						factors.Add((compareFactor, factorValues));
						factorValues = new string[0];
					}
					compareFactor = reader.GetString(1);
				}

				if (reader.GetString(0) != compareDirection)
				{
					if (!firstDirectionFlag)
					{
						FactorsWithDirection factorsWithDirection = new FactorsWithDirection();
						factorsWithDirection.FactorNameAndValues = factors;
						factorsWithDirection.Direction = compareDirection;
						_factors.Add(factorsWithDirection);
						factors = new List<(string, string[])>();
					}
					compareDirection = reader.GetString(0);
				}


				if (reader.GetString(1) == compareFactor)
				{
					Array.Resize(ref factorValues, factorValues.Length + 1);
					factorValues[factorValues.Length - 1] = (reader.GetString(2));
				}
				firstFactorFlag = false;
				firstDirectionFlag = false;
			}
			factors.Add((compareFactor, factorValues));
			FactorsWithDirection factorsWithDirectionTmp = new FactorsWithDirection();
			factorsWithDirectionTmp.FactorNameAndValues = factors;
			factorsWithDirectionTmp.Direction = compareDirection;
			_factors.Add(factorsWithDirectionTmp);
		}

		/// <summary>
		/// Перевод данных о схемах из БД в тип данных используемый в приложении
		/// </summary>
		/// <param name="reader"> Объект класса SqlDataReader предоставляет 
		/// способ чтения потока строк в БД</param>
		private void SchemeConvertToList(SqlDataReader reader)
		{
			List<(string, bool)> disturbance = new List<(string, bool)>();
			bool firstSchemeFlag = true;
			string compareScheme = "";
			while (reader.Read())
			{
				if (reader.GetString(0) != compareScheme)
				{
					if (!firstSchemeFlag)
					{
						Scheme schemeTmp = new Scheme();
						schemeTmp.SchemeName = compareScheme;
						schemeTmp.Disturbance = disturbance;
						_schemes.Add(schemeTmp);
						disturbance = new List<(string, bool)>();
					}
					compareScheme = reader.GetString(0);
				}

				if (reader.GetString(0) == compareScheme)
				{
					disturbance.Add((reader.GetString(1), reader.GetBoolean(2)));
				}

				firstSchemeFlag = false;
			}
			Scheme scheme = new Scheme();
			scheme.SchemeName = compareScheme;
			scheme.Disturbance = disturbance;
			_schemes.Add(scheme);
		}
	}
}
