using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;

namespace WorkWithDataSource
{
	public class PullData
	{
		private string _sectionName;
		private List<string> _sections = new List<string>();
		private List<(string, (string, string[])[])> _factors = new List<(string, (string, string[])[])>();
		private List<(string, (string, bool)[])> _scheme = new List<(string, (string, bool)[])>();
		private SqlConnectionStringBuilder _sqlConnectionBuilder;

		public PullData(string sectionName)
		{
			_sectionName = sectionName;
		}

		public List<string> Sections => _sections;

		internal string StringConnect
		{
			get
			{
				_sqlConnectionBuilder = new SqlConnectionStringBuilder();
				_sqlConnectionBuilder.DataSource = @"LAPTOP-EFSS8TJ6\SQLEXPRESS";
				_sqlConnectionBuilder.UserID = @"ParusMDP";
				_sqlConnectionBuilder.Password = "1234567890";
				_sqlConnectionBuilder.InitialCatalog = "PARUS-MDP";
				return _sqlConnectionBuilder.ConnectionString;
			}
			
		}

		public void PullFactors()
		{
			string sqlExpression = @$"SELECT  SectionWithFactors.Direction, Factors.Factor, Factors.FactorValue
		FROM [dbo].[SectionWithFactors], [dbo].[Sections], [dbo].[Factors]
		WHERE Sections.Section_ID = SectionWithFactors.Section_ID and SectionWithFactors.Factor_ID = Factors.Factor_ID and Sections.Section = '{_sectionName}'";
			ConnectWithDataBase(sqlExpression, DataType.Factor);
		}

		public void PullSections()
		{
			string sqlExpression = @$"SELECT Sections.Section FROM [dbo].[Sections]";
			ConnectWithDataBase(sqlExpression, DataType.Section);
		}

		public void PullScheme()
		{
			string sqlExpression = @$"SELECT Schemes.Scheme, Schemes.Disturbance, Schemes.Automation
  FROM [dbo].[Schemes],[dbo].[Sections]
  WHERE Sections.Section_ID = Schemes.Section_ID and Sections.Section = '{_sectionName}'";
			ConnectWithDataBase(sqlExpression, DataType.Scheme);
		}

		private void ConnectWithDataBase(string sqlExpression, DataType dataType)
		{

			
            using (SqlConnection connection = new SqlConnection(StringConnect))
			{
				using (SqlCommand command = new SqlCommand(sqlExpression, connection))
				{
					connection.Open();
					using (SqlDataReader reader = command.ExecuteReader())
					{
						switch(dataType)
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


		private void SectionConvertToList(SqlDataReader reader)
		{
			while (reader.Read())
			{
				_sections.Add(reader.GetString(0));
			}
		}

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
				if(reader.GetString(0) != compareDirection)
				{
					if (!firstDirectionFlag)
					{
						_factors.Add((compareDirection, factors));
					}
					compareDirection = reader.GetString(0);
				}

				if (reader.GetString(1) != compareFactor)
				{
					if (!firstFactorFlag)
					{
						Array.Resize(ref factors, factors.Length + 1);
						factors[factors.Length - 1] = (compareFactor, factorValues);
					}
					compareFactor = reader.GetString(1);
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

		private void SchemeConvertToList(SqlDataReader reader)
		{
			(string, bool)[] disturbance = new (string, bool)[0];
			bool firstSchemeFlag = true;
			string compareScheme ="";
			while (reader.Read())
			{
				if( reader.GetString(0) != compareScheme )
				{
					if(!firstSchemeFlag)
					{
						_scheme.Add((compareScheme, (disturbance)));
					}
					compareScheme = reader.GetString(0);
				}

				if(reader.GetString(0) == compareScheme)
				{
					Array.Resize(ref disturbance, disturbance.Length + 1);
					disturbance[disturbance.Length - 1] = (reader.GetString(1), reader.GetBoolean(2));
				}

				firstSchemeFlag = false;
			}
			_scheme.Add((compareScheme, (disturbance)));
		}


	}
}
