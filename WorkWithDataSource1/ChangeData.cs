using System;
using System.Data.SqlClient;

namespace WorkWithDataSource
{
	/// <summary>
	/// Метод необходимый для изменения данных в БД
	/// </summary>
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
		private DataBaseAutentification _sqlConnection;

		/// <summary>
		/// Конструктор класса для добавления/удаления сечения в БД
		/// </summary>
		/// <param name="SectionName">Название сечения</param>
		public ChangeData(string SectionName, DataBaseAutentification dataBaseAutentification)
		{
			_sectionName = SectionName;
			_dataType = DataType.Section;
			_sqlConnection = dataBaseAutentification;
		}

		/// <summary>
		/// Конструктор класса для добавления/удаления факторов в БД
		/// </summary>
		/// <param name="SectionName">Название сечения</param>
		/// <param name="Direction">Направление</param>
		/// <param name="Factor">Влияющий фактор</param>
		/// <param name="FactorValue">Значения фактора</param>
		public ChangeData(
			string SectionName,
			string Direction,
			string Factor,
			string FactorValue, DataBaseAutentification dataBaseAutentification)
		{
			_sectionName = SectionName;
			_direction = Direction;
			_factor = Factor;
			_factorValue = FactorValue;
			_dataType = DataType.Factor;
			_sqlConnection = dataBaseAutentification;
		}

		/// <summary>
		/// Конструктор класса для добавления/удаления схем в БД
		/// </summary>
		/// <param name="SectionName">Название сечения</param>
		/// <param name="Scheme">Схема</param>
		/// <param name="Disturbance">Возмущение</param>
		/// <param name="Automatics">Предусмотренна ли ПА для 
		/// данного возмущения</param>
		public ChangeData(
			string SectionName,
			string Scheme,
			string Disturbance,
			bool Automatics, DataBaseAutentification dataBaseAutentification)
		{
			_sectionName = SectionName;
			_scheme = Scheme;
			_disturbance = Disturbance;
			if (Automatics)
			{
				_automatics = 1;
			}
			else
			{
				_automatics = 0;
			}
			_dataType = DataType.Scheme;
			_sqlConnection = dataBaseAutentification;
		}

		/// <summary>
		/// Метод позволяющий получить численное значение из БД
		/// </summary>
		/// <param name="sqlExpression">SQL запрос</param>
		/// <returns>Численное значение</returns>
		private int ConnectWithDataBaseID(string sqlExpression)
		{
			using (SqlConnection connection = new SqlConnection(_sqlConnection.StringForConnect))
			{
				connection.Open();
				SqlCommand command = new SqlCommand(sqlExpression, connection);
				return Convert.ToInt32(command.ExecuteScalar());
			}

		}

		/// <summary>
		/// Метод для проверки существования сечения, если сечения не существует -
		/// инициирует его создание
		/// </summary>
		public void ExistSection()
		{
			if (ConnectWithDataBaseID(@$"SELECT COUNT(Section_ID) " +
				@$"FROM[dbo].[Sections] WHERE Section = '{_sectionName}'") == 0)
			{
				InsertSection();
			}
		}

		/// <summary>
		/// Метод для добавления данных в БД
		/// </summary>
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

		/// <summary>
		/// Метод для добавления сечения в БД
		/// </summary>
		private void InsertSection()
		{
			if (ConnectWithDataBaseID(@$"SELECT COUNT(Section_ID) " +
				@$"FROM[dbo].[Sections] WHERE Section = '{_sectionName}'") == 0)
			{
				int sectionId;
				if (ConnectWithDataBaseID(@$"SELECT COUNT(Section_ID) " +
					@$"FROM[dbo].[Sections]") != 0)
				{
					sectionId = ConnectWithDataBaseID(@"SELECT MAX(Section_ID) " +
						@$"FROM[dbo].[Sections]") + 1;
				}
				else
				{
					sectionId = 1;
				}

				ExecuteExpression(@$"INSERT INTO [dbo].[Sections] " +
					@$"VALUES ({sectionId},'{_sectionName}')");
			}
		}

		/// <summary>
		/// Метод для добавления фактора в БД
		/// </summary>
		private void InsertFactors()
		{
			ExistSection();
			var sectionId = ConnectWithDataBaseID(@$"SELECT Section_ID " +
				@$"FROM[dbo].[Sections] WHERE Section = '{_sectionName}'");
			if (ConnectWithDataBaseID(@$"SELECT COUNT(Section_ID) FROM[dbo].[Factors] WHERE
				Section_ID = {sectionId} and 
				Direction = '{_direction}' and 
				Factor = '{_factor}' and 
				FactorValue = '{_factorValue}'") == 0)
			{
				var sqlExpression = @$"INSERT INTO [dbo].[Factors] " +
					@$"VALUES({sectionId},'{_direction}','{_factor}','{_factorValue}')";
				ExecuteExpression(sqlExpression);
			}
		}

		/// <summary>
		/// Метод для добавления схемы в БД
		/// </summary>
		private void InsertScheme()
		{
			ExistSection();
			var sectionId = ConnectWithDataBaseID(@$"SELECT Section_ID " +
				@$"FROM[dbo].[Sections] WHERE Section = '{_sectionName}'");
			if (ConnectWithDataBaseID(@$"SELECT COUNT(Section_ID) FROM[dbo].[Schemes] WHERE
				Section_ID = {sectionId} and 
				Scheme = '{_scheme}' and 
				Disturbance = '{_disturbance}' and 
				Automation = '{_automatics}'") == 0)
			{
				var sqlExpression = @$"INSERT INTO [dbo].[Schemes] " +
					@$"VALUES ({sectionId},'{_scheme}','{_disturbance}',{_automatics})";
				ExecuteExpression(sqlExpression);
			}
		}

		/// <summary>
		/// Метод для удаления данных из БД
		/// </summary>
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

		/// <summary>
		/// Метод для удаления данных о сечении из БД
		/// </summary>
		private void DeleteSection()
		{
			if (ConnectWithDataBaseID(@$"SELECT COUNT(Section_ID) " +
				@$"FROM[dbo].[Sections] WHERE Section = '{_sectionName}'") != 0)
			{
				var sqlExpression = @$"DELETE FROM [dbo].[Sections] " +
					@$"WHERE Section = '{_sectionName}'";
				ExecuteExpression(sqlExpression);
			}
			else
			{
				throw new Exception("Такого сечения не существует в базе");
			}
		}

		/// <summary>
		/// Метод для удаления данных о факторе из БД
		/// </summary>
		private void DeleteFactor()
		{
			var sectionId = ConnectWithDataBaseID(@$"SELECT Section_ID FROM[dbo].[Sections] " +
				@$"WHERE Section = '{_sectionName}'");
			var sqlExpression = @$"DELETE FROM [dbo].[Factors] WHERE 
				Section_ID = {sectionId} and 
				Direction = '{_direction}' and 
				Factor = '{_factor}' and 
				FactorValue = '{_factorValue}'";
			ExecuteExpression(sqlExpression);
		}

		/// <summary>
		/// Метод для удаления данных о схеме из БД
		/// </summary>
		private void DeleteScheme()
		{
			var sectionId = ConnectWithDataBaseID(@$"SELECT Section_ID FROM[dbo].[Sections] " +
				@$"WHERE Section = '{_sectionName}'");
			var sqlExpression = @$"DELETE FROM [dbo].[Schemes] WHERE 
				Section_ID = {sectionId} and 
				Scheme = '{_scheme}' 
				and Disturbance = '{_disturbance}'";
			ExecuteExpression(sqlExpression);
		}

		/// <summary>
		/// Метод выполнения запроса в БД
		/// </summary>
		/// <param name="sqlExpression">SQL запрос</param>
		private void ExecuteExpression(string sqlExpression)
		{
			using (SqlConnection connection = new SqlConnection(_sqlConnection.StringForConnect))
			{
				connection.Open();
				SqlCommand command = new SqlCommand(sqlExpression, connection);
				command.ExecuteNonQuery();
			}
		}
	}
}