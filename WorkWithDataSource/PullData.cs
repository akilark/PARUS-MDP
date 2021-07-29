using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;

namespace WorkWithDataSource
{
	public class PullData
	{
		private const string sqlExpressionScheme = "";
		private const string sqlExpressionFactors = @"SELECT  Sections.Section, SectionWithFactors.Direction, Factors.Factor, Factors.FactorValue
			FROM [dbo].[SectionWithFactors], [dbo].[Sections], [dbo].[Factors]
			WHERE Sections.Section_ID = SectionWithFactors.Section_ID and SectionWithFactors.Factor_ID = Factors.Factor_ID";
		
		private void Pull(string sqlExpression)
		{
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            builder.DataSource = @"LAPTOP-EFSS8TJ6\SQLEXPRESS";
            builder.UserID = @"ParusMDP";
            builder.Password = "1234567890";
            builder.InitialCatalog = "PARUS-MDP";
            using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
			{
				using (SqlCommand command = new SqlCommand(sqlExpression, connection))
				{
					connection.Open();
					using (SqlDataReader reader = command.ExecuteReader())
					{
						PullScheme(reader);
					}
				}
			}
		}

		private void PullScheme(SqlDataReader reader)
		{
			while (reader.Read())
			{
				Console.WriteLine(@"{0} /\ {1} /\ {2} /\ {3}", reader.GetString(0), reader.GetString(1), reader.GetString(2), reader.GetString(3));
			}
		}

		public void PullFactors()
		{
			Pull(sqlExpressionFactors);
		}
	}
}
