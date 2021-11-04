using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;

// Добавить проверку соединения
namespace WorkWithDataSource
{
	/// <summary>
	/// Класс для вытягивания всей информации для конкретного сечения из БД
	/// </summary>
	public class PullData
	{
		private string _sectionName;
		private List<string> _sections = new List<string>();
		private List<(string, (string, string[])[])> _factors = new List<(string, (string, string[])[])>();
		private List<(string, (string, bool)[])> _schemes = new List<(string, (string, bool)[])>();
		private DataBaseAutentification _sqlConnectionString;

		//TODO: удалить тестовый конструктор
		public PullData()
		{
			_sectionName = " ";
			_sqlConnectionString = new DataBaseAutentification();
		}

		/// <summary>
		/// Конструктор класса с 1 параметром
		/// </summary>
		/// <param name="sectionName">название сечения</param>
		public PullData(string sectionName)
		{
			_sectionName = sectionName;
			_sqlConnectionString = new DataBaseAutentification();
			PullSections();
			PullFactors();
			PullSchemes();
		}

		/// <summary>
		/// Свойство возвращающее список контролируемых сечений 
		/// </summary>
		public List<string> Sections => _sections;

		/// <summary>
		/// Свойство возвращающее список факторов в формате (string, (string, string[])[])
		/// </summary>
		public List<(string, (string, string[])[])> Factors => _factors;

		/// <summary>
		/// Свойство возврщающее список схем в формате (string, (string, bool)[])
		/// </summary>
		public List<(string, (string, bool)[])> Shemes => _schemes;

		/// <summary>
		/// Метод для заполнения списка факторов из БД
		/// </summary>
		private void PullFactors()
		{
			string sqlExpression = @$"SELECT  Factors.Direction, Factors.Factor, Factors.FactorValue
				FROM [dbo].[Sections], [dbo].[Factors]
				WHERE Sections.Section_ID = Factors.Section_ID and Sections.Section = '{_sectionName}'";
			ConnectWithDataBase(sqlExpression, DataType.Factor);
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
		private void PullSchemes()
		{
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
				_sqlConnectionString.GetStringForConnect()))
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
						}
					}
				}
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

			(string, string[])[] factors = new (string, string[])[0];
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
						Array.Resize(ref factors, factors.Length + 1);
						factors[factors.Length - 1] = (compareFactor, factorValues);
						factorValues = new string[0];
					}
					compareFactor = reader.GetString(1);
				}

				if (reader.GetString(0) != compareDirection)
				{
					if (!firstDirectionFlag)
					{
						_factors.Add((compareDirection, factors));
						factors = new (string, string[])[0];
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
			Array.Resize(ref factors, factors.Length + 1);
			factors[factors.Length - 1] = (compareFactor, factorValues);
			_factors.Add((compareDirection, factors));
		}

		/// <summary>
		/// Перевод данных о схемах из БД в тип данных используемый в приложении
		/// </summary>
		/// <param name="reader"> Объект класса SqlDataReader предоставляет 
		/// способ чтения потока строк в БД</param>
		private void SchemeConvertToList(SqlDataReader reader)
		{
			(string, bool)[] disturbance = new (string, bool)[0];
			bool firstSchemeFlag = true;
			string compareScheme = "";
			while (reader.Read())
			{
				if (reader.GetString(0) != compareScheme)
				{
					if (!firstSchemeFlag)
					{
						_schemes.Add((compareScheme, (disturbance)));
						disturbance = new (string, bool)[0];
					}
					compareScheme = reader.GetString(0);
				}

				if (reader.GetString(0) == compareScheme)
				{
					Array.Resize(ref disturbance, disturbance.Length + 1);
					disturbance[disturbance.Length - 1] = (reader.GetString(1), reader.GetBoolean(2));
				}

				firstSchemeFlag = false;
			}
			_schemes.Add((compareScheme, (disturbance)));
		}
	}
}
