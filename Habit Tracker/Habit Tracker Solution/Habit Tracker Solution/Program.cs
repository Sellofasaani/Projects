﻿using Microsoft.Data.Sqlite;
using SQLitePCL;

namespace Habit_Tracker_Solution
{
    internal class Program
    {

        static string connectionString = @"Data Source=Habit-Tracker.db";
        static void Main(string[] args)
        {
            Batteries.Init();
            

            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();

                tableCmd.CommandText =
                    @"CREATE TABLE IF NOT EXISTS drinking_water(
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        Date TEXT,
                        Quantity INTEGER
                    )";
                tableCmd.ExecuteNonQuery();

                connection.Close();
            }

            GetUserInput();

        }


        static void GetUserInput()
        {
            Console.Clear();
            bool closeApp = false;
            while (closeApp == false)
            {
                Console.WriteLine("\n\nMAIN MENU");
                Console.WriteLine("\nWhat would you like to do?");
                Console.WriteLine("\nType 0 to Close the application.");
                Console.WriteLine("Type 1 to View all records.");
                Console.WriteLine("Type 2 to Insert record.");
                Console.WriteLine("Type 3 to Delete record.");
                Console.WriteLine("Type 4 to Update record.");
                Console.WriteLine("--------------------------------------------\n");

                string commandInput = Console.ReadLine();

                switch (commandInput)
                {
                    case "0":
                        Console.WriteLine("\nGoodbye\n");
                        closeApp = true;
                        break;
                    //case 1:
                    //    GetAllRecords();
                    //    break;
                    case "2":
                        Insert();
                        break;
                    //case 3:
                    //    Delete();
                    //    break;
                    //case 4:
                    //    Update();
                    //    break;
                    default:
                        Console.WriteLine("\nInvalid command, choose 0 to 4.\n");
                        break;

                }
            }
        }
        private static void Insert()
        {
            string date = GetDateInput();
            int quantity = GetNumberInput("\n\nPlease insert number of glasses");

            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();
                tableCmd.CommandText = $"INSERT INTO drinking_water(date, quantity) VALUES('{date}', {quantity})";

                tableCmd.ExecuteNonQuery();

                connection.Close();

            }
        }

        internal static string GetDateInput()
        {
            Console.WriteLine("\n\nPlease insert the date: (Format: dd-mm-yyyy) Type 0 to return to main menu");

            string dateInput = Console.ReadLine();

            if (dateInput == "0") GetUserInput();

            return dateInput;
        }

        internal static int GetNumberInput(string message)
        {
            Console.WriteLine(message);

            string numberInput = Console.ReadLine();
            if (numberInput == "0") GetUserInput();

            int finalInput = Convert.ToInt32(numberInput);

            return finalInput;
        }
    }
}
