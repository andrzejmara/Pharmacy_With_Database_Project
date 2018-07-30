using System;
using System.Data.SqlClient;

public abstract class ActiveRecord
{
	//Here you can change the connection to the database
	static string connectionString = "Integrated Security=SSPI;" +
									 "Data Source=.\\SQLEXPRESS;" +
									 "Initial Catalog=Pharmacy;";
	protected static SqlConnection connection = new SqlConnection(connectionString);
	public readonly int ID;

	public virtual void Save() {}

	public  static void Reload(){ }

	public static void Remove(string id, string fromWhere, string whatToRemove)
	{
		try
		{
			var sqlCommand = new SqlCommand();
			sqlCommand.Connection = connection;
			sqlCommand.CommandText =
				$"DELETE FROM {fromWhere} WHERE {whatToRemove} = @id;";

			var sqlIdParam = new SqlParameter
			{
				DbType = System.Data.DbType.Int32,
				Value = Int32.Parse(id),
				ParameterName = "@id"
			};
			sqlCommand.Parameters.Add(sqlIdParam);
			sqlCommand.ExecuteNonQuery();
		}
		catch (Exception e)
		{
			ConsoleEx.WriteLine("Action wasn't succesful", ConsoleColor.Red);
			Console.WriteLine(e.Message);
		}
		Console.ReadKey();
	}
	protected static void Open()
	{
		try
		{
			connection.Open();
		}
		catch (Exception e)
		{
			Console.WriteLine(e);
			connection.Close();
			throw;
		}

	}
	protected static void Close()
	{
		connection.Close();
	}
	public static SqlCommand GetAll(string nameOfTable)
	{
		var sqlCommand = new SqlCommand();
		sqlCommand.Connection = connection;
		sqlCommand.CommandText = $"SELECT * FROM {nameOfTable};";
		return sqlCommand;

	}
	public virtual void ShowAll() { }
	public static void DoesEntryExists(string id, string fromWhere, string whatToRemove)
	{
		Open();
		var sqlCommand = new SqlCommand();
		sqlCommand.Connection = connection;
		sqlCommand.CommandText = $"Select * From {fromWhere} Where {whatToRemove}ID = {id};";
		var sqlReader = sqlCommand.ExecuteReader();
		if (!sqlReader.Read()) { throw new Exception("Entry doesn't exist!");}
		Close();
	}
}