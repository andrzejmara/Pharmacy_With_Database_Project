using System;
using System.Collections.Generic;
using System.Data.SqlClient;

public class Medicine : ActiveRecord
{
	public Medicine(string name, string manufacturer, decimal price, int amount, bool withPrescription)
	{
		Name = name;
		Manufacturer = manufacturer;
		Price = price;
		Amount = amount;
		WithPrescription = withPrescription;
	}
	public Medicine()
	{

	}

	public string Name { get; set; }
	public string Manufacturer { get; set; }
	public decimal Price { get; set; }
	public int Amount { get; set; }
	public bool WithPrescription { get; set; }
	public static Medicine NewInstance()
	{
		Console.WriteLine("Medicine name: ");
		string name = Console.ReadLine();
		Console.WriteLine("Manufacturer name: ");
		string manufacturer = Console.ReadLine();
		Console.WriteLine("Price: ");
		decimal price = Decimal.Parse(Console.ReadLine());
		Console.WriteLine("Amount: ");
		int amount = Int32.Parse(Console.ReadLine());
		Console.WriteLine("With Prescription?(true/false):");
		bool withPrescription = bool.Parse(Console.ReadLine().ToLower());

		Medicine med = new Medicine(name, manufacturer, price, amount, withPrescription);
		return med;
	}
	public static void RemoveMedicine()
	{
		Medicine.ShowAll("Medicines");
		try
		{
			Console.WriteLine("Choose ID of the entry you want to remove:");
			string id = Console.ReadLine();
			DoesEntryExists(id, "Medicines", "");
			Medicine.Open();
			Medicine.Remove(id, "Medicines", "ID");
			ConsoleEx.WriteLine("Successfully removed", ConsoleColor.Green);
			Console.ReadLine();
		}
		catch (Exception e)
		{
			ConsoleEx.WriteLine("The remove action wasn't succesful.", ConsoleColor.Red);
			Console.WriteLine(e);
			Console.ReadLine();
		}
		Medicine.Close();
		Medicine.Options();
	}
	public static void ShowAll(string nameOfTable)
	{
		Medicine.Open();
		var sqlReader = GetAll(nameOfTable).ExecuteReader();
		if (sqlReader.HasRows)
		{
			while (sqlReader.HasRows && sqlReader.Read())
			{
				ConsoleEx.Write($" ID: {sqlReader["ID"]}. ", ConsoleColor.Magenta);
				Console.WriteLine(
					$"{sqlReader["Name"].ToString().PadRight(15)} " +
					$"|{sqlReader["Manufacturer"].ToString().PadRight(20)} " +
					$"| Price: {sqlReader["Price"].ToString().PadRight(6)} " +
					$"| Amount: {sqlReader["Amount"].ToString().PadRight(6)} " +
					$"| Needs Prescription: {sqlReader["WithPrescription"]}");
			}
		}
		else
		{
			ConsoleEx.WriteLine("There are no entries", ConsoleColor.Red);
		}
		Medicine.Close();
		Console.ReadLine();
	}
	public void Save(Medicine med)
	{
		Medicine.Open();

		try
		{

			var sqlCommand = new SqlCommand();
			sqlCommand.Connection = connection;
			sqlCommand.CommandText =
				@"INSERT INTO Medicines (Name, Manufacturer, Price, Amount, WithPrescription)
			                             VALUES (@Name, @Manufacturer, @Price, @Amount, @WithPrescription);";

			var sqlNameParam = new SqlParameter();
			sqlNameParam.DbType = System.Data.DbType.AnsiString;
			sqlNameParam.Value = med.Name;
			sqlNameParam.ParameterName = "@Name";
			sqlCommand.Parameters.Add(sqlNameParam);

			var sqlManufacturerParam = new SqlParameter();
			sqlManufacturerParam.DbType = System.Data.DbType.AnsiString;
			sqlManufacturerParam.Value = med.Manufacturer;
			sqlManufacturerParam.ParameterName = "@Manufacturer";
			sqlCommand.Parameters.Add(sqlManufacturerParam);

			var sqlPriceParam = new SqlParameter();
			sqlPriceParam.DbType = System.Data.DbType.Decimal;
			sqlPriceParam.Value = med.Price;
			sqlPriceParam.ParameterName = "@Price";
			sqlCommand.Parameters.Add(sqlPriceParam);

			var sqlAmountParam = new SqlParameter();
			sqlAmountParam.DbType = System.Data.DbType.Int32;
			sqlAmountParam.Value = med.Amount;
			sqlAmountParam.ParameterName = "@Amount";
			sqlCommand.Parameters.Add(sqlAmountParam);

			var sqlWithPrescriptionParam = new SqlParameter();
			sqlWithPrescriptionParam.DbType = System.Data.DbType.Boolean;
			sqlWithPrescriptionParam.Value = med.WithPrescription;
			sqlWithPrescriptionParam.ParameterName = "@WithPrescription";
			sqlCommand.Parameters.Add(sqlWithPrescriptionParam);

			sqlCommand.ExecuteNonQuery();
			Console.WriteLine();
			ConsoleEx.WriteLine("Medicine added succesfully!", ConsoleColor.Green);
			Console.ReadLine();

		}
		catch (Exception e)
		{
			Console.WriteLine(e);
			Console.ReadLine();
			throw;
		}
		Medicine.Close();
	}


	public static void Options()
	{
		Console.Clear();
		ConsoleEx.WriteLine("Available Options: ", System.ConsoleColor.Blue);
		Console.WriteLine("1. Show all medicines");
		Console.WriteLine("2. Save medicine");
		Console.WriteLine("3. Remove medicine");
		Console.WriteLine("4. Reload Medicine");
		Console.WriteLine("5. Return");
		string command = Console.ReadLine();

		if (command == "1")
		{
			Medicine.ShowAll("Medicines");
			Medicine.Options();
		}
		else if (command == "2")
		{
			Medicine med = Medicine.NewInstance();
			med.Save(med);
			Medicine.Options();

		}
		else if (command == "3")
		{
			Medicine.RemoveMedicine();
		}
		else if (command == "4")
		{
			Medicine.Reload();
			Medicine.Options();
		}
		else if (command == "5")
		{
			Console.Clear();
			Pharmacy_Main.Program.Main();
		}
		else
		{
			Console.WriteLine("Incorrect Command");
		}
	}



	public static void Reload()
	{
		Medicine.ShowAll("Medicines");
		Console.WriteLine("Choose ID of the entry you want to reload:");
		string id = Console.ReadLine();
		DoesEntryExists(id, "Medicines", "");
		try
		{
			Medicine med = Medicine.NewInstance();
			Medicine.Open();
			var sqlCommand = new SqlCommand();
			sqlCommand.Connection = connection;
			sqlCommand.CommandText =
				@"UPDATE Medicines SET Name = @Name, Manufacturer = @Manufacturer, Price = @Price, Amount = @Amount, WithPrescription = @WithPrescription
			                     WHERE MedicineID = @id;";
			var sqlNameParam = new SqlParameter();
			sqlNameParam.DbType = System.Data.DbType.AnsiString;
			sqlNameParam.Value = med.Name;
			sqlNameParam.ParameterName = "@Name";
			sqlCommand.Parameters.Add(sqlNameParam);

			var sqlManufacturerParam = new SqlParameter();
			sqlManufacturerParam.DbType = System.Data.DbType.AnsiString;
			sqlManufacturerParam.Value = med.Manufacturer;
			sqlManufacturerParam.ParameterName = "@Manufacturer";
			sqlCommand.Parameters.Add(sqlManufacturerParam);

			var sqlPriceParam = new SqlParameter();
			sqlPriceParam.DbType = System.Data.DbType.Decimal;
			sqlPriceParam.Value = med.Price;
			sqlPriceParam.ParameterName = "@Price";
			sqlCommand.Parameters.Add(sqlPriceParam);

			var sqlAmountParam = new SqlParameter();
			sqlAmountParam.DbType = System.Data.DbType.Int32;
			sqlAmountParam.Value = med.Amount;
			sqlAmountParam.ParameterName = "@Amount";
			sqlCommand.Parameters.Add(sqlAmountParam);

			var sqlWithPrescriptionParam = new SqlParameter();
			sqlWithPrescriptionParam.DbType = System.Data.DbType.Boolean;
			sqlWithPrescriptionParam.Value = med.WithPrescription;
			sqlWithPrescriptionParam.ParameterName = "@WithPrescription";
			sqlCommand.Parameters.Add(sqlWithPrescriptionParam);

			var sqlIDParam = new SqlParameter();
			sqlIDParam.DbType = System.Data.DbType.Int32;
			sqlIDParam.Value = id;
			sqlIDParam.ParameterName = "@id";
			sqlCommand.Parameters.Add(sqlIDParam);

			sqlCommand.ExecuteNonQuery();
			Console.WriteLine();
			ConsoleEx.WriteLine("Medicine reloaded succesfully!", ConsoleColor.Green);
			Console.ReadLine();
		}
		catch (Exception e)
		{
			ConsoleEx.WriteLine("The reload action wasn't succesful.", ConsoleColor.Red);
			Console.WriteLine(e);
			Prescription.Close();
			Console.ReadLine();
		}
	}
}