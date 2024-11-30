using Microsoft.Data.Sqlite;
using SQLitePCL; // Required for initializing SQLite

Batteries.Init(); // Initialize SQLite provider

initializeDatabase();
insertData();
readData();


void initializeDatabase()
{

    var sql = @"CREATE TABLE IF NOT EXISTS drinking_water(
    id INTEGER PRIMARY KEY AUTOINCREMENT,
    Date TEXT NOT NULL,
    Quantity INTEGER
)";

    try
    {
        using var connection = new SqliteConnection(@"Data Source=habit_tracker.db");
        connection.Open();

        using var command = new SqliteCommand(sql, connection);
        command.ExecuteNonQuery();

        Console.WriteLine("Table 'authors' created successfully.");

    }
    catch (SqliteException ex)
    {
        Console.WriteLine(ex.Message);
    }

}

void insertData()
{
    var sql = "INSERT INTO drinking_water (Date, Quantity) " + "VALUES (@Date, @Quantity)";

    var Date = DateTime.Now;
    var Quantity = 10;

    try
    {
        using var connection = new SqliteConnection(@"Data Source=habit_tracker.db");
        connection.Open();

        using var command = new SqliteCommand(sql, connection);

        command.Parameters.AddWithValue("@Date", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
        command.Parameters.AddWithValue("@Quantity", Quantity);

        var rowInserted = command.ExecuteNonQuery();

        Console.WriteLine($"Water added to the list amount {Quantity} and date {Date}");
    }

    catch (SqliteException ex)
    {
        Console.WriteLine(ex.Message);
    }
}

void readData()
{

    var sql = "SELECT * FROM drinking_water";

    try
    {
        using var connection = new SqliteConnection(@"Data Source=habit_tracker.db");
        connection.Open();

        using var command = new SqliteCommand(sql, connection);

        using var reader = command.ExecuteReader();
        if (reader.HasRows)
        {
            while (reader.Read())
            {
                var id = reader.GetInt32(0);
                var Date = reader.GetDateTime(1);
                var Quantity = reader.GetInt32(2);
                Console.WriteLine($"{id}\t{Date}\t{Quantity}");
            }
        }
        else
        {
            Console.WriteLine("No logs found.");
        }

    }

    catch (SqliteException ex)
    {
        Console.WriteLine(ex.Message);
    }

}



