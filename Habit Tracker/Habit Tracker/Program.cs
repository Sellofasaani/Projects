using Microsoft.Data.Sqlite;
using System.Globalization;
using SQLitePCL; // Required for initializing SQLite

Batteries.Init(); // Initialize SQLite provider
using var connection = new SqliteConnection(@"Data Source=habit_tracker.db");

initializeDatabase();
GetUserInput();


void initializeDatabase()
{

    var sql = @"CREATE TABLE IF NOT EXISTS drinking_water(
    id INTEGER PRIMARY KEY AUTOINCREMENT,
    Date TEXT NOT NULL,
    Quantity INTEGER
)";

    try
    {

        connection.Open();

        using var command = new SqliteCommand(sql, connection);
        command.ExecuteNonQuery();

        Console.WriteLine("Table 'drinking_water' created successfully.");

    }
    catch (SqliteException ex)
    {
        Console.WriteLine(ex.Message);
    }

}

void GetUserInput()
{
    Console.Clear();
    bool closeApp = false;

    while (closeApp == false)
    {
        Console.WriteLine("\n\nMain menu:");
        Console.WriteLine("\nWhat would you like to do?");
        Console.WriteLine("\nPress 0 to close the app");
        Console.WriteLine("Type 1 to View all records");
        Console.WriteLine("Type 2 to Insert record");
        Console.WriteLine("Type 3 to Update record");
        Console.WriteLine("Type 4 to Delete record");
        Console.WriteLine("-----------------------------\n\n");

        var command = Console.ReadLine();

        switch (command)
        {
            case "0":
                Console.WriteLine("Thanks for the fish, adios.");
                closeApp = true;
                Environment.Exit(0);
                break;
            case "1":
                ReadData();
                break;
            case "2":
                InsertData();
                break;
            case "3":
                UpdateData();
                break;
            default:
                Console.WriteLine("Invalid command, input 0 to 4");
                break;

        }
    }

}

void InsertData()
{
    var sql = "INSERT INTO drinking_water (Date, Quantity) " + "VALUES (@Date, @Quantity)";

    var Date = GetDateInput();
    var Quantity = GetNumberInput("Please input the quantity of water. Press 0 to return to main menu.");

    try
    {
        connection.Open();

        using var command = new SqliteCommand(sql, connection);

        command.Parameters.AddWithValue("@Date", Date);
        command.Parameters.AddWithValue("@Quantity", Quantity);

        var rowInserted = command.ExecuteNonQuery();

        Console.WriteLine($"Water added to the list amount {Quantity} and date {Date}");
    }

    catch (SqliteException ex)
    {
        Console.WriteLine(ex.Message);
    }
}

void UpdateData()
{
    ReadData();

    var sqlCheck = "SELECT EXISTS(SELECT 1 FROM drinking_water WHERE id = @id)";
    var sqlUpdate = "UPDATE drinking_water SET date = @date, quantity = @quantity WHERE id = @id";

    var recordId = GetNumberInput("\n\nPlease type Id of the record would like to update. Type 0 to return to main manu.\n\n");

    connection.Open();
    using var checkCommand = new SqliteCommand(sqlCheck, connection);
    checkCommand.Parameters.AddWithValue("@id", recordId);

    var recordExists = Convert.ToInt32(checkCommand.ExecuteScalar());
    if (recordExists == 0)
    {
        Console.WriteLine("No such ID exists, try again.");
        connection.Close();
        UpdateData();
    }

    string date = GetDateInput();
    int quantity = GetNumberInput("\n\nPlease insert number of glasses or other measure of your choice (no decimals allowed)\n\n");

    using var updateCommand = new SqliteCommand(sqlUpdate, connection);
    updateCommand.Parameters.AddWithValue("@date", date);
    updateCommand.Parameters.AddWithValue("@quantity", quantity);
    updateCommand.Parameters.AddWithValue("id", recordId);

    var rowsUpdated =  updateCommand.ExecuteNonQuery();
    Console.WriteLine($"The quantity has been updated successfully. Rows affected: {rowsUpdated}");

}

string GetDateInput()
{
    Console.WriteLine("Insert date in format: DD-MM-YY");
    var dateInput = Console.ReadLine();
    if (dateInput == "0") GetUserInput();
    while (!DateTime.TryParseExact(dateInput, "dd-MM-yy", new CultureInfo("en-GB"), DateTimeStyles.None, out _))
    {
        Console.WriteLine("\n\nInvalid date. (Format: dd-mm-yy). Type 0 to return to main manu or try again:\n\n");
        dateInput = Console.ReadLine();
    }
    return dateInput;
}

int GetNumberInput(string message)
{
    Console.WriteLine(message);
    var UserNumber = int.Parse(Console.ReadLine());
    return UserNumber;

}

void ReadData()
{
    Console.Clear();
    var sql = "SELECT * FROM drinking_water";

    try
    {
        connection.Open();

        using var command = new SqliteCommand(sql, connection);

        using var reader = command.ExecuteReader();
        List<DrinkingWater> tableData = [];

        if (reader.HasRows)
        {
            while (reader.Read())
            {
                tableData.Add(
                new DrinkingWater
                {

                    Id = reader.GetInt32(0),
                    Date = DateTime.ParseExact(reader.GetString(1), "dd-MM-yy", new CultureInfo("en-GB")),
                    Quantity = reader.GetInt32(2)
                });
            }
        }
        else
        {
            Console.WriteLine("No logs found.");
        }

        Console.WriteLine("------------------------------------------\n");
        foreach (var dw in tableData)
        {
            Console.WriteLine($"{dw.Id} - {dw.Date.ToString("dd-MMM-yyyy")} - Quantity: {dw.Quantity}");
        }
        Console.WriteLine("------------------------------------------\n");

    }

    catch (SqliteException ex)
    {
        Console.WriteLine(ex.Message);
    }

}

public class DrinkingWater
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public int Quantity { get; set; }
}



