using System.Data;
using Dapper;

namespace Task.Features.Homework11;

public static class TestData
{
    public static void SetupDatabase(IDbConnection connection)
    {
        const string createTableQuery = @"
            CREATE TABLE IF NOT EXISTS Users (
                id SERIAL PRIMARY KEY
            );
        ";

        try
        {
            connection.Execute(createTableQuery);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error creating table: {ex.Message}");
        }
    }
    public static void InsertTestData(IDbConnection connection)
    {
        const string insertQuery = "INSERT INTO Users (Id) VALUES (@Id)";

        var usersToInsert = Enumerable.Range(1, 10).Select(id => new { Id = id });

        try
        {
            foreach (var user in usersToInsert)
            {
                Console.WriteLine($"Inserting user: {user.Id}");
                connection.Execute(insertQuery, user);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error inserting test data: {ex.Message}");
        }
    }
}
