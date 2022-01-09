using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;

namespace WorkWithDataSource
{
	/// <summary>
	/// Класс необходимый для санкционированного подключения к базе данных
	/// </summary>
	[Serializable]
	public class DataBaseAutentification
	{
		private SqlConnectionStringBuilder _sqlConnectionBuilder;

		/// <summary>
		/// Конструктор необходимый для тестов 
		/// </summary>
		public DataBaseAutentification()
		{
			_sqlConnectionBuilder = new SqlConnectionStringBuilder();
			//_sqlConnectionBuilder.DataSource = @"LAPTOP-EFSS8TJ6\SQLEXPRESS";
			//_sqlConnectionBuilder.UserID = @"ParusMDP";
			//_sqlConnectionBuilder.Password = "1234567890";
			//_sqlConnectionBuilder.InitialCatalog = "PARUS-MDP";
		}

		/// <summary>
		/// Конструктор для формирования строки подключения
		/// </summary>
		/// <param name="DataSource">Имя сервера, где находится необходима БД</param>
		/// <param name="UserID">Логин</param>
		/// <param name="Password">Пароль</param>
		/// <param name="InitialCatalog">Необходимая БД</param>
		public DataBaseAutentification(string DataSource, string UserID, string Password)
		{
			_sqlConnectionBuilder = new SqlConnectionStringBuilder();
			_sqlConnectionBuilder.DataSource = DataSource;
			_sqlConnectionBuilder.UserID = UserID;
			_sqlConnectionBuilder.Password = Password;
			_sqlConnectionBuilder.InitialCatalog = "PARUS-AFTP";
		}

		/// <summary>
		/// Строка подключения
		/// </summary>
		public string StringForConnect
		{
			get 
			{
				return _sqlConnectionBuilder.ConnectionString;
			}
			set
			{
				_sqlConnectionBuilder.ConnectionString = value;
			}

		}
	}
}
