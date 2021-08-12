using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;

namespace WorkWithDataSource
{
	/// <summary>
	/// Класс необходимый для санкционированного подключения к базе данных
	/// </summary>
	internal class DataBaseAutentification
	{
		private SqlConnectionStringBuilder _sqlConnectionBuilder;
		
		/// <summary>
		/// Конструктор необходимый для тестов 
		/// </summary>
		internal DataBaseAutentification()
		{
			_sqlConnectionBuilder = new SqlConnectionStringBuilder();
			_sqlConnectionBuilder.DataSource = @"LAPTOP-EFSS8TJ6\SQLEXPRESS";
			_sqlConnectionBuilder.UserID = @"ParusMDP";
			_sqlConnectionBuilder.Password = "1234567890";
			_sqlConnectionBuilder.InitialCatalog = "PARUS-MDP";
		}
		
		/// <summary>
		/// Конструктор для формирования строки подключения
		/// </summary>
		/// <param name="DataSource">Сервер, где находится необходима БД</param>
		/// <param name="UserID">Логин</param>
		/// <param name="Password">Пароль</param>
		/// <param name="InitialCatalog">Необходимая БД</param>
		DataBaseAutentification(string DataSource, string UserID, string Password, string InitialCatalog)
		{
			_sqlConnectionBuilder = new SqlConnectionStringBuilder();
			_sqlConnectionBuilder.DataSource = DataSource;
			_sqlConnectionBuilder.UserID = UserID;
			_sqlConnectionBuilder.Password = Password;
			_sqlConnectionBuilder.InitialCatalog = InitialCatalog;
		}
		
		/// <summary>
		/// Метод генерирующий строку подключения на основе исходных данных
		/// </summary>
		/// <returns>Строка подключения</returns>
		internal string GetStringForConnect()
		{
			return _sqlConnectionBuilder.ConnectionString;
		}
	}
}
