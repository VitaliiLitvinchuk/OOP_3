using System.Text;
using Dapper;
using Microsoft.Data.SqlClient;
using Npgsql;
using Task.Features.Homework11;
using Task.Homework.h_t_22_10_2024.Task.Abstract;
using Task.Homework.h_t_22_10_2024.Task.DatabaseBuilder;
using Task.Homework.h_t_22_10_2024.Task.Entities;
using Task.Homework.h_t_22_10_2024.Task.Providers;
using static Task.TasksWorkers;

namespace Task.Homework
{
    public class Homework11 : ITask
    {
        public void Start()
        {
            using var pgConnection = new NpgsqlConnection("Host=localhost;Port=5433;Database=mydefaultdb;Username=myuser;Password=mypassword");
            pgConnection.Open();

            TestData.SetupDatabase(pgConnection);
            TestData.InsertTestData(pgConnection);

            var postgreSQL = new PostgreSQLProvider();
            var pgQuery = SqlBuilder.Select<User>(postgreSQL)
                                    .OrderBy<User>("Id")
                                    .Take(5);

            try
            {
                var pgResults = pgConnection.Query<User>(pgQuery.CommandText).ToList();

                Console.WriteLine("PostgreSQL Results:");

                foreach (var user in pgResults)
                    Console.WriteLine($"User ID: {user.Id}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error executing PostgreSQL query: {ex.Message}");
            }

            try
            {
                using var msConnection = new SqlConnection("Server=localhost,1434;Database=mydefaultdb;User Id=sa;Password=MySecurePassword1!;TrustServerCertificate=True;");
                msConnection.Open();

                TestData.SetupDatabase(msConnection);
                TestData.InsertTestData(msConnection);

                var msSQL = new MSSQLProvider();
                var msQuery = SqlBuilder.Select<User>(msSQL)
                                        .OrderBy<User>("Id")
                                        .Take(10);

                var msResults = msConnection.Query<User>(msQuery.CommandText);

                Console.WriteLine("\nMS SQL Results:");

                foreach (var user in msResults)
                    Console.WriteLine($"User ID: {user.Id}");
            }
            catch (Exception exception)
            {
                Console.WriteLine($"Error executing MS SQL query: {exception.Message}");
            }
        }
    }
}

namespace Task.Homework.h_t_22_10_2024
{
    namespace Task
    {
        namespace Abstract
        {
            public interface IProvider
            {
                string OrderByQuery(string columnName);
                string TakeQuery(string count);
            }
        }

        namespace Providers
        {
            public class PostgreSQLProvider : IProvider
            {
                public string OrderByQuery(string columnName) => $"ORDER BY {columnName}";
                public string TakeQuery(string count) => $"LIMIT {count}";
            }

            public class MSSQLProvider : IProvider
            {
                public string OrderByQuery(string columnName) => $"ORDER BY {columnName}";
                public string TakeQuery(string count) => $"TOP {count}";
            }
        }

        namespace Entities
        {
            public class User
            {
                public int Id { get; set; }
            }
        }

        namespace DatabaseBuilder
        {
            public class SqlBuilder(IProvider provider)
            {
                private string _table = "";
                private string _orderBy = "";
                private int? _take = null;
                private readonly IProvider _provider = provider;
                public static SqlBuilder Select<T>(IProvider provider)
                {
                    var builder = new SqlBuilder(provider)
                    {
                        _table = $"{typeof(T).Name}s"
                    };

                    return builder;
                }

                public SqlBuilder OrderBy<T>(string columnName)
                {
                    _orderBy = columnName;

                    return this;
                }

                public SqlBuilder Take(int count)
                {
                    _take = count;

                    return this;
                }

                public string CommandText
                {
                    get
                    {
                        var query = new StringBuilder($"SELECT * FROM {_table}");

                        if (!string.IsNullOrEmpty(_orderBy))
                            query.Append($" {_provider.OrderByQuery(_orderBy)}");

                        if (_take is not null)
                            query.Append($" {_provider.TakeQuery($"{_take}")}");

                        return query.ToString();
                    }
                }
            }
        }
    }
}