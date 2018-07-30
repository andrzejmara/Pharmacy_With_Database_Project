using System;
using System.Data.SqlClient;

public class Order : ActiveRecord
{
	public Order(DateTime date, int amount, int prescriptionId, int medicineId)
	{
		Date = date;
		Amount = amount;
		PrescriptionId = prescriptionId;
		MedicineId = medicineId;
	}

	public DateTime Date { get; set; }
	public int Amount { get; set; }
	public int PrescriptionId { get; set; }
	public int MedicineId { get; set; }

	public static Order NewInstance()
	{
		DateTime date = DateTime.Now;
		Console.WriteLine("Amount: ");
		int amount = Int32.Parse(Console.ReadLine());
		Console.WriteLine("Prescription ID: ");
		int prescId = Int32.Parse(Console.ReadLine());
		Console.WriteLine("Medication ID:  ");
		int medId = Int32.Parse(Console.ReadLine());
		Order order = new Order(date, amount, prescId, medId);
		return order;
	}
	public void Save(Order order)
	{
		Prescription.Open();

		try
		{

			var sqlCommand = new SqlCommand();
			sqlCommand.Connection = connection;
			sqlCommand.CommandText =
				@"INSERT INTO Prescriptions (PrescriptionID, MedicineID, Date, Amount)
			                             VALUES (@PrescriptionID, @MedicineID, @Date, @Amount);";

			var sqlAmount = new SqlParameter();
			sqlAmount.DbType = System.Data.DbType.Int32;
			sqlAmount.Value = order.Amount;
			sqlAmount.ParameterName = "@Amount";
			sqlCommand.Parameters.Add(sqlAmount);

			var sqlPrescID = new SqlParameter();
			sqlPrescID.DbType = System.Data.DbType.Int32;
			sqlPrescID.Value = order.PrescriptionId;
			sqlPrescID.ParameterName = "@PrescriptionID";
			sqlCommand.Parameters.Add(sqlPrescID);

			var sqlMedicineID = new SqlParameter();
			sqlMedicineID.DbType = System.Data.DbType.Int32;
			sqlMedicineID.Value = order.MedicineId;
			sqlMedicineID.ParameterName = "@MedicineID";
			sqlCommand.Parameters.Add(sqlMedicineID);

			var sqlDate = new SqlParameter();
			sqlDate.DbType = System.Data.DbType.Date;
			sqlDate.Value = order.MedicineId;
			sqlDate.ParameterName = "@Date";
			sqlCommand.Parameters.Add(sqlDate);

			sqlCommand.ExecuteNonQuery();
			Console.WriteLine();
			ConsoleEx.WriteLine("Order added succesfully!", ConsoleColor.Green);
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

	public static void Options()
	{
		Console.Clear();
		ConsoleEx.WriteLine("Available Options: ", System.ConsoleColor.Blue);
		Console.WriteLine("1. Add Order");
		Console.WriteLine("2. Return");
		string command = Console.ReadLine();

		if (command == "1")
		{
			Order order = Order.NewInstance();
			order.Save(order);
			Order.Options();
		}
		else if (command == "2")
		{
			Console.Clear();
			Pharmacy_Main.Program.Main();
		}
		else
		{
			Console.WriteLine("Incorrect Command");
		}
	}

}