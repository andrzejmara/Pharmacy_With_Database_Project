using System;
using System.Data.SqlClient;

public class Prescription : ActiveRecord
{
	public Prescription(string customerName, string pESEL, int prescriptionNumber)
	{
		CustomerName = customerName;
		PESEL = pESEL;
		PrescriptionNumber = prescriptionNumber;
	}

	public string CustomerName { get; set; }
	public string PESEL { get; set; }
	public int PrescriptionNumber { get; set; }

	public static Prescription NewInstance()
	{
		Console.WriteLine("Customer name: ");
		string customerName = Console.ReadLine();
		Console.WriteLine("Pesel (10 digits): ");
		string pesel = Console.ReadLine();
		Console.WriteLine("Prescription number (6 digits): ");
		int prescNum = Int32.Parse(Console.ReadLine());
		Prescription pres = new Prescription(customerName, pesel, prescNum);
		return pres;
	}
	public void Save(Prescription presc)
	{
		Prescription.Open();

		try
		{

			var sqlCommand = new SqlCommand();
			sqlCommand.Connection = connection;
			sqlCommand.CommandText =
				@"INSERT INTO Prescriptions (CustomerName, PESEL, PrescriptionNumber)
			                             VALUES (@CustomerName, @PESEL, @PrescriptionNumber);";

			var sqlNameParam = new SqlParameter();
			sqlNameParam.DbType = System.Data.DbType.AnsiString;
			sqlNameParam.Value = presc.CustomerName;
			sqlNameParam.ParameterName = "@CustomerName";
			sqlCommand.Parameters.Add(sqlNameParam);

			var sqlPeselParam = new SqlParameter();
			sqlPeselParam.DbType = System.Data.DbType.AnsiString;
			sqlPeselParam.Value = presc.PESEL.ToString();
			sqlPeselParam.ParameterName = "@PESEL";
			sqlCommand.Parameters.Add(sqlPeselParam);

			var sqlPrescNumParam = new SqlParameter();
			sqlPrescNumParam.DbType = System.Data.DbType.Int32;
			sqlPrescNumParam.Value = presc.PrescriptionNumber;
			sqlPrescNumParam.ParameterName = "@PrescriptionNumber";
			sqlCommand.Parameters.Add(sqlPrescNumParam);


			sqlCommand.ExecuteNonQuery();
			Console.WriteLine();
			ConsoleEx.WriteLine("Prescription added succesfully!", ConsoleColor.Green);
			Console.ReadLine();

		}
		catch (Exception e)
		{
			ConsoleEx.WriteLine("Wrong input!", ConsoleColor.Red);
			Console.WriteLine(e);
			Console.ReadLine();
		}
		Prescription.Close();
	}
	public static void ShowAll(string nameOfTable)
	{
		Prescription.Open();
		var sqlReader = GetAll(nameOfTable).ExecuteReader();
		if (sqlReader.HasRows)
		{
			while (sqlReader.HasRows && sqlReader.Read())
			{
				ConsoleEx.Write($" ID: {sqlReader["ID"]}. ", ConsoleColor.Magenta);
				Console.WriteLine(
					$" {sqlReader["CustomerName"].ToString().PadRight(15)} " +
					$"| PESEL: {sqlReader["PESEL"].ToString().PadRight(10)} " +
					$"| Prescription Number: {sqlReader["PrescriptionNumber"].ToString().PadRight(6)} ");
			}
		}
		else
		{
			ConsoleEx.WriteLine("There are no entries", ConsoleColor.Red);
		}
		Prescription.Close();
		Console.ReadLine();
	}
	public static void Options()
	{
		Console.Clear();
		ConsoleEx.WriteLine("Available Options: ", System.ConsoleColor.Blue);
		Console.WriteLine("1. Show all prescriptions");
		Console.WriteLine("2. Save prescription");
		Console.WriteLine("3. Remove prescripton");
		Console.WriteLine("4. Reload a prescription");
		Console.WriteLine("5. Return");
		string command = Console.ReadLine();

		if (command == "1")
		{
			Prescription.ShowAll("Prescriptions");
			Prescription.Options();
		}
		else if (command == "2")
		{
			Prescription presc = Prescription.NewInstance();
			presc.Save(presc);
			Prescription.Options();
		}
		else if (command == "3")
		{
			Prescription.RemovePrescription();
		}
		else if (command == "4")
		{
			Prescription.Reload();
			Prescription.Options();
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
	public static void RemovePrescription()
	{
		Prescription.ShowAll("Prescriptions");
		try
		{
			Console.WriteLine("Choose ID of the entry you want to remove:");
			string id = Console.ReadLine();
			DoesEntryExists(id, "Prescriptions", "");
			Prescription.Open();
			Prescription.Remove(id, "Prescriptions", "ID");
			ConsoleEx.WriteLine("Successfully removed", ConsoleColor.Green);
		}
		catch (Exception e)
		{
			ConsoleEx.WriteLine("The remove action wasn't succesful.", ConsoleColor.Red);
			Console.WriteLine(e);
			Console.ReadLine();
		}
		Prescription.Close();
		Prescription.Options();
	}
	public static void Reload()
	{
		try
		{
			Prescription.ShowAll("Prescriptions");
			Console.WriteLine("Choose ID of the entry you want to reload:");
			string id = Console.ReadLine();
			DoesEntryExists(id, "Prescriptions", "");
			Prescription presc = Prescription.NewInstance();
			Prescription.Open();
			var sqlCommand = new SqlCommand();
			sqlCommand.Connection = connection;
			sqlCommand.CommandText =
				@"UPDATE Prescriptions SET CustomerName = @CustomerName, PESEL = @PESEL, PrescriptionNumber = @PrescriptionNumber 
			                     WHERE ID = @id;";

			var sqlNameParam = new SqlParameter();
			sqlNameParam.DbType = System.Data.DbType.AnsiString;
			sqlNameParam.Value = presc.CustomerName;
			sqlNameParam.ParameterName = "@CustomerName";
			sqlCommand.Parameters.Add(sqlNameParam);

			var sqlPeselParam = new SqlParameter();
			sqlPeselParam.DbType = System.Data.DbType.AnsiString;
			sqlPeselParam.Value = presc.PESEL.ToString();
			sqlPeselParam.ParameterName = "@PESEL";
			sqlCommand.Parameters.Add(sqlPeselParam);

			var sqlPrescNumParam = new SqlParameter();
			sqlPrescNumParam.DbType = System.Data.DbType.Int32;
			sqlPrescNumParam.Value = presc.PrescriptionNumber;
			sqlPrescNumParam.ParameterName = "@PrescriptionNumber";
			sqlCommand.Parameters.Add(sqlPrescNumParam);

			var sqlIDParam = new SqlParameter();
			sqlIDParam.DbType = System.Data.DbType.Int32;
			sqlIDParam.Value = id;
			sqlIDParam.ParameterName = "@id";
			sqlCommand.Parameters.Add(sqlIDParam);

			sqlCommand.ExecuteNonQuery();
			Console.WriteLine();
			ConsoleEx.WriteLine("Prescription reloaded succesfully!", ConsoleColor.Green);
			Console.ReadLine();
		}
		catch (Exception e)
		{
			ConsoleEx.WriteLine("The reload action wasn't succesful.", ConsoleColor.Red);
			Console.WriteLine(e);
			Prescription.Close();
			Console.ReadLine();
		}
		Prescription.Close();
	}
}