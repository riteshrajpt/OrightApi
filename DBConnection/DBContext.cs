using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System;
using System.Data;

public class DBContext
{
    private readonly string _connectionString;

    public DBContext(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection");
    }

    public MySqlConnection GetConnection()
    {
        MySqlConnection connection = new MySqlConnection(_connectionString);

        try
        {
            connection.Open();

            // Check if the connection state is open
            if (connection.State == ConnectionState.Open)
            {
                Console.WriteLine("Database connection established successfully.");
            }
            else
            {
                Console.WriteLine("Failed to connect to the database.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error connecting to the database: {ex.Message}");
        }

        return connection;
    }
}
