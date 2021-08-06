using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;

namespace WorkWithDataSource
{
	public class ChangeData
	{
		private string _sectionName;
		private string _direction;
		private string _factor;
		private string _factorValue;
		private string _scheme;
		private string _disturbance;
		private int _automatics;
		private DataType _dataType;
		private PullData _pullData;

		public ChangeData(string SectionName)
		{
			_sectionName = SectionName;
			_pullData = new PullData(SectionName);
			_dataType = DataType.Section;
		}

		public ChangeData(string SectionName, string Direction, string Factor, string FactorValue)
		{
			_sectionName = SectionName;
			_pullData = new PullData(SectionName);
			_direction = Direction;
			_factor = Factor;
			_factorValue = FactorValue;
			_dataType = DataType.Factor;
		}

		public ChangeData(string SectionName, string Scheme, string Disturbance, int Automatics)
		{
			_sectionName = SectionName;
			_pullData = new PullData(SectionName);
			_scheme = Scheme;
			_disturbance = Disturbance;
			if(Automatics != 0)
			{
				_automatics = 1;
			}
			else
			{
				_automatics = 0;
			}
			_dataType = DataType.Scheme;
		}

		private int ConnectWithDataBaseID(string sqlExpression)
		{
			using (SqlConnection connection = new SqlConnection(_pullData.StringConnect))
			{
				connection.Open();
				SqlCommand command = new SqlCommand(sqlExpression, connection);
				return Convert.ToInt32(command.ExecuteScalar());
			}

		}

		public void ExistSection()
		{
			if (ConnectWithDataBaseID(@$"SELECT COUNT(Section_ID) FROM[dbo].[Sections] WHERE Section = '{_sectionName}'") == 0)
			{
				InsertSection();
			}
		}

		public void Insert()
		{
			
			switch (_dataType)
			{
				case DataType.Section:
					{
						InsertSection();
						break;
					}
				case DataType.Factor:
					{
						InsertFactors();
						break;
					}
				case DataType.Scheme:
					{
						InsertScheme();
						break;
					}
			}

		}

		private void InsertSection()
		{
			if(ConnectWithDataBaseID(@$"SELECT COUNT(Section_ID) FROM[dbo].[Sections] WHERE Section = '{_sectionName}'") == 0)
			{
				int sectionId;
				if (ConnectWithDataBaseID(@$"SELECT COUNT(Section_ID) FROM[dbo].[Sections]") != 0)
				{
					sectionId = ConnectWithDataBaseID(@"SELECT MAX(Section_ID) FROM[dbo].[Sections]") + 1;
				}
				else
				{
					sectionId = 1;
				}
				var sqlExpression = @$"INSERT INTO [dbo].[Sections] VALUES ({sectionId},'{_sectionName}')";
				ExecuteExpression(sqlExpression);
			}			
		}

		private void InsertFactors()
		{
			ExistSection();
			var sectionId = ConnectWithDataBaseID(@$"SELECT Section_ID FROM[dbo].[Sections] WHERE Section = '{_sectionName}'");
			if (ConnectWithDataBaseID(@$"SELECT COUNT(Section_ID) FROM[dbo].[Factors] WHERE
				Section_ID = {sectionId} and 
				Direction = '{_direction}' and 
				Factor = '{_factor}' and 
				FactorValue = '{_factorValue}'") == 0)
			{
				var sqlExpression = @$"INSERT INTO [dbo].[Factors] VALUES({sectionId},'{_direction}','{_factor}','{_factorValue}')";
				ExecuteExpression(sqlExpression);
			}
		}

		private void InsertScheme()
		{
			ExistSection();
			var sectionId = ConnectWithDataBaseID(@$"SELECT Section_ID FROM[dbo].[Sections] WHERE Section = '{_sectionName}'");
			if (ConnectWithDataBaseID(@$"SELECT COUNT(Section_ID) FROM[dbo].[Schemes] WHERE
				Section_ID = {sectionId} and 
				Scheme = '{_scheme}' and 
				Disturbance = '{_disturbance}' and 
				Automation = '{_automatics}'") == 0)
			{
				var sqlExpression = @$"INSERT INTO [dbo].[Schemes] VALUES ({sectionId},'{_scheme}','{_disturbance}',{_automatics})";
				ExecuteExpression(sqlExpression);
			}
		}

		public void Delete()
		{
			switch (_dataType)
			{
				case DataType.Section:
					{
						DeleteSection();
						break;
					}
				case DataType.Factor:
					{
						DeleteFactor();
						break;
					}
				case DataType.Scheme:
					{
						DeleteScheme();
						break;
					}
			}
		}

		private void DeleteSection()
		{
			if (ConnectWithDataBaseID(@$"SELECT COUNT(Section_ID) FROM[dbo].[Sections] WHERE Section = '{_sectionName}'") != 0)
			{
				var sqlExpression = @$"DELETE FROM [dbo].[Sections] WHERE Section = '{_sectionName}'";
				ExecuteExpression(sqlExpression);
			}
			else
			{
				throw new Exception("Такого сечения не существует в базе");
			}
		}

		private void DeleteFactor()
		{
			var sectionId = ConnectWithDataBaseID(@$"SELECT Section_ID FROM[dbo].[Sections] WHERE Section = '{_sectionName}'");
			var sqlExpression = @$"DELETE FROM [dbo].[Factors] WHERE Section_ID = {sectionId} and 
				Direction = '{_direction}' and 
				Factor = '{_factor}' and 
				FactorValue = '{_factorValue}'";
			ExecuteExpression(sqlExpression);
		}

		private void DeleteScheme()
		{
			var sectionId = ConnectWithDataBaseID(@$"SELECT Section_ID FROM[dbo].[Sections] WHERE Section = '{_sectionName}'");
			var sqlExpression = @$"DELETE FROM [dbo].[Schemes] WHERE Section_ID = {sectionId} and 
				Scheme = '{_scheme}' 
				and Disturbance = '{_disturbance}'";
			ExecuteExpression(sqlExpression);
		}

		private void ExecuteExpression(string sqlExpression)
		{
			using (SqlConnection connection = new SqlConnection(_pullData.StringConnect))
			{
				connection.Open();
				SqlCommand command = new SqlCommand(sqlExpression, connection);
				command.ExecuteNonQuery();
			}
		}
	}
}
