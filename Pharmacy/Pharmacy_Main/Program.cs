using System;
using System.Collections.Generic;
using System.Reflection;
using System.Xml.Linq;

namespace Pharmacy_Main
{
	class Program
	{
		public static void Main()
		{

			ConsoleEx.WriteLine("Hi! This is a program that lets you run a Pharmacy!", ConsoleColor.Green);
			var options = new List<string>() { "1. Medicines", "2. Orders", "3. Prescriptions", "4. Exit" };
			ConsoleEx.WriteLine("This is the list of available commands: ", ConsoleColor.Green);
			foreach (var option in options)
			{
				Console.WriteLine(option);
			}
			Console.WriteLine();




			string command;
			do
			{
				ConsoleEx.WriteLine("Write your command: ", ConsoleColor.Yellow);
				command = Console.ReadLine();
				command = command.Trim();


				if (command == "1")
				{
				Console.Clear();
				Medicine.Options();

				}
				else if (command == "2")
				{
				Console.Clear();
				Order.Options();
				}
				else if (command == "3")
				{
					Console.Clear();
					Prescription.Options();
				}
				else if (command == "4")
				{
					break;
				}
				else if (command.ToLower() != "exit")
				{
					Console.WriteLine("Incorrect Command");
				}
				Console.WriteLine();
			} while (command.ToLower() != "exit");
			Console.ReadLine();
		}
	}
}
