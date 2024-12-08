﻿using Microsoft.Data.Sqlite;
using SQLitePCL;
using System.Collections.Generic;
using System.Globalization;

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
                        Environment.Exit(0);
                        break;
                    case "1":
                        GetAllRecords();
                        break;
                    case "2":
                        Insert();
                        break;
                    case "3":
                        Delete();
                        break;
                    case "4":
                        Update();
                        break;
                    default:
                        Console.WriteLine("\nInvalid command, choose 0 to 4.\n");
                        break;

                }
            }
        }

        private static void Update()
        {
            Console.Clear();
            GetAllRecords();

            var recordId = GetNumberInput("\n\nPlease type the record id you would like to update. Press 0 to return to main menu.");
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                var checkCmd = connection.CreateCommand();
                checkCmd.CommandText = $"SELECT EXISTS(SELECT 1 FROM drinking_water WHERE Id = '{recordId}')";
                int checkQuery = Convert.ToInt32(checkCmd.ExecuteScalar);

                if (checkQuery == 0)
                {
                    Console.WriteLine($"\n\nRecord with id {recordId} does not exist.\n\n");
                    connection.Close();
                    Update();
                }

                string date = GetDateInput();
                int quantity = GetNumberInput("\n\nPlease input the id of the record you wish to update. Type 0 to return to main menu.");
                
                var tableCmd = connection.CreateCommand();
                tableCmd.CommandText = $"UPDATE drinking_water SET date =  '{date}', quantity = {quantity} WHERE Id = '{recordId}'";

                tableCmd.ExecuteNonQuery();
                connection.Close();
            }
                
        }

        private static void Delete()
        {
            Console.Clear();
            GetAllRecords();

            var recordId = GetNumberInput("\n\nPlease type the ID of the record you wish to delete or type 0 to return to main menu\n\n");

            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();
                tableCmd.CommandText = $"DELETE FROM drinking_water where Id = '{recordId}'";

                int rowCount = tableCmd.ExecuteNonQuery();

                if (rowCount == 0)
                {
                    Console.WriteLine($"\n\nRecord with id {recordId} doesn't exist. \n\n");
                    Delete();
                }
            }

            Console.WriteLine($"\n\nRecord with Id {recordId} was deleted. \n\n");
            GetUserInput();
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

        private static void GetAllRecords()
        {
            Console.Clear();
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();
                tableCmd.CommandText = $"SELECT * FROM drinking_water";

                List<DrinkingWater> tableData = new();

                SqliteDataReader reader = tableCmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        tableData.Add(
                        new DrinkingWater
                        {
                            Id = reader.GetInt32(0),
                            Date = DateTime.ParseExact(reader.GetString(1), "dd-MM-yy", new CultureInfo("en-US")),
                            Quantity = reader.GetInt32(2)
                        }); ;

                    }

                }
                else
                {
                    Console.WriteLine("No rows found");
                }

                connection.Close();

                Console.WriteLine("---------------------------------------------\n");
                foreach (var dw in  tableData)
                {
                    Console.WriteLine($"{dw.Id} - {dw.Date.ToString("dd-MMM-yyyy")} - Quantity: {dw.Quantity}");
                }
                Console.WriteLine("---------------------------------------------\n");
            }
        }

        internal static string GetDateInput()
        {
            Console.WriteLine("\n\nPlease insert the date: (Format: dd-mm-yy) Type 0 to return to main menu");

            string dateInput = Console.ReadLine();

            if (dateInput == "0") GetUserInput();

            while (!DateTime.TryParseExact(dateInput, "dd-MM-yy", new CultureInfo("en-US"), DateTimeStyles.None, out _))
            {
                Console.WriteLine("\n\nInvalid date. (Format: dd-mm-yy). Type 0 to return to main manu or try again:\n\n");
                dateInput = Console.ReadLine();
            }

            return dateInput;
        }

        internal static int GetNumberInput(string message)
        {
            Console.WriteLine(message);

            string numberInput = Console.ReadLine();
            if (numberInput == "0") GetUserInput();

            while (!Int32.TryParse(numberInput, out _) || Convert.ToInt32(numberInput) < 0)
            {
                Console.WriteLine("\n\nInvalid number, try again.");
                numberInput = Console.ReadLine();
            }

            int finalInput = Convert.ToInt32(numberInput);

            return finalInput;
        }
    }

    public class DrinkingWater
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int Quantity { get; set; }
    }
}
