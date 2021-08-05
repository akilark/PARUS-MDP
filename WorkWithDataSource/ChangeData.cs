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
		private int _factorId;
		private string _factorValue;
		private string _scheme;
		private string _disturbance;
		private bool _automatics;
		private PullData _pullData;

		public string Section
		{
			set
			{
				_sectionName = value;
				_direction = null;
				_factor = null;
				_factorValue = null;
				_scheme = null;
				_disturbance = null;
				_automatics = false;
			}
		}

		public string Direction
		{
			set
			{
				_direction = value;
				_factor = null;
				_factorValue = null;
			}
		}

		public string Factor
		{
			set
			{
				_factor = value;
				_factorValue = null;
			}
		}

		public string FactorValue
		{
			set
			{
				_factorValue = value;
			}
		}

		public string Scheme
		{
			set
			{
				_scheme = value;
				_disturbance = null;
				_automatics = false;
			}
		}

		public string Disturbance
		{
			set
			{
				_disturbance = value;
				_automatics = false;
			}
		}

		public bool Automatic
		{
			set
			{
				_automatics = value;
			}
		}


		//Сделать несколько перегрузок под все потребности и удалить свойства
		public ChangeData(string SectionName)
		{
			_sectionName = SectionName;
			_pullData = new PullData(SectionName);
		}

		private DataType Validator()
		{
			if (_sectionName != null)
			{
				if (_direction != null)
				{
					if (_factor == null) throw new Exception("Необходимо задать значение фактора");

					if (_factorValue == null) throw new Exception("Необходимо задать значение фактора");

					return DataType.Factor;
				}
				if (_scheme != null)
				{
					if (_disturbance == null) throw new Exception("Необходимо задать возмущение");
					return DataType.Scheme;
				}

				return DataType.Section;			
			}
			else
			{
				throw new Exception("Название сечения не может быть null");
			}

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

		public void Exist()
		{
			//делать проверку существования, если проверка отрицательна, то создаем верхнюю папку, то того момента, пока конфликтов не будет возникать
		}

		public void Insert()
		{
			string sqlExpression;
			switch (Validator())
			{
				case DataType.Section:
					{
						int sectionId = ConnectWithDataBaseID(@"SELECT COUNT(Section_ID) FROM[dbo].[Sections]") + 1;
						sqlExpression = @$"INSERT INTO [dbo].[Sections] VALUES ({sectionId},'{_sectionName}')";
						ExecuteExpression(sqlExpression);
						break;
					}
				case DataType.Factor:
					{
						int sectionId = ConnectWithDataBaseID(@$"SELECT Section_ID FROM[dbo].[Sections] WHERE Section = '{_sectionName}'");
						int factorId;
						if(ConnectWithDataBaseID(@$"SELECT COUNT(Factor_ID) FROM[dbo].[Factors] 
							WHERE Factor = '{_factor}' and FactorValue = '{_factorValue}'") == 0)
						{
							factorId = ConnectWithDataBaseID(@"SELECT COUNT(Factor_ID) FROM[dbo].[Factors]") + 1;
						}
						else
						{
							factorId = ConnectWithDataBaseID(@$"SELECT Factor_ID FROM[dbo].[Factors] 
							WHERE Factor = '{_factor}' and FactorValue = '{_factorValue}'");
						}
						sqlExpression = @$"INSERT INTO [dbo].[Factors] VALUES('{factorId}','{_factor}','{_factorValue}')";
						ExecuteExpression(sqlExpression);

						sqlExpression = @$"INSERT INTO [dbo].[SectionWithFactors] VALUES({sectionId},'{_direction}',{factorId})";

						ExecuteExpression(sqlExpression);

						break;
					}
				case DataType.Scheme:
					{
						int sectionId = ConnectWithDataBaseID(@$"SELECT Section_ID FROM[dbo].[Sections] WHERE Section = '{_sectionName}'");
						sqlExpression = @$"INSERT INTO [dbo].[Schemes] VALUES ({sectionId},'{_scheme}','{_disturbance}',{_automatics})";
						break;
					}
			}

		}

		public void Delete()
		{
			string sqlExpression;
			switch (Validator())
			{
				case DataType.Section:
					{
						int sectionId = ConnectWithDataBaseID(@$"SELECT Section_ID FROM[dbo].[Sections] Where Section = '{_sectionName}");
						sqlExpression = $@"DELETE FROM [dbo].[SectionWithFactors] where Section_ID = '{sectionId}'";
						ExecuteExpression(sqlExpression);

						sqlExpression = @$"DELETE FROM [dbo].[Sections] WHERE Section = '{_sectionName}'";
						ExecuteExpression(sqlExpression);
						break;
					}
				case DataType.Factor:
					{
						int factorId = ConnectWithDataBaseID(@$"SELECT Factor_ID FROM[dbo].[Factors] 
							WHERE Factor = '{_factor}' and FactorValue = '{_factorValue}'");
						sqlExpression = $@"DELETE FROM [dbo].[SectionWithFactors] where Factor_ID = '{factorId}'";
						ExecuteExpression(sqlExpression);
						sqlExpression = @$"DELETE FROM [dbo].[Factors] WHERE Factor_ID = '{factorId}'";
						ExecuteExpression(sqlExpression);
						break;
					}
				case DataType.Scheme:
					{
						sqlExpression = @$"DELETE FROM [dbo].[Schemes] WHERE Scheme = '{_scheme}'";
						ExecuteExpression(sqlExpression);
						break;
					}
			}
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
