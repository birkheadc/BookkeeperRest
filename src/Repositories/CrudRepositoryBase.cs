using MySql.Data.MySqlClient;

namespace BookkeeperRest.New.Repositories;

public abstract class CrudRepositoryBase
{
    protected string connectionString;

    protected readonly string tableName;
    protected readonly string schema;

    public CrudRepositoryBase(IWebHostEnvironment env, IConfiguration configuration, string tableName, string schema)
    {
        // If this connection string does not work the app will simply crash. Eventually I hope to fix that but for now sorry.
        if (env.IsDevelopment())
        {
            connectionString = configuration["ConnectionString"];
        }
        else
        {
            connectionString = Environment.GetEnvironmentVariable("ASPNETCORE_CONNECTIONSTRING") ?? "";
        }

        this.tableName = tableName;
        this.schema = schema;
        InitializeDatabase();
    }

    internal void InitializeDatabase()
    {
        if (DoesTableExist() == false)
        {
            CreateTable();
        }
    }

    internal MySqlConnection GetConnection()
    {
        try
        {
            MySqlConnection connection = new(connectionString);
            return connection;
        }
        catch
        {
            // At the moment my application is completely dependent on being supplied a valid connection string.
            // In other words, if the connection string is bad, the entire thing will crash to a halt, because of the poor way I wrote it.
            // Fix will probably take a bit of time, since every method that uses this method to establish a connection will probably need a rewrite,
            // and the general application flow will need some kind of reimagining to compensate for an inability to connect to the database.

            // This exception is not handled. It will crash the application. Make sure the connection string is always valid or else. Sorry.
            throw new Exception("Fatal Error: Could not connect to database.");
        }
    }

    internal bool DoesTableExist()
    {
        using (MySqlConnection connection = GetConnection())
        {
            MySqlCommand command = new();

            command.CommandText = "SELECT COUNT(*) FROM information_schema.TABLES WHERE (TABLE_SCHEMA='bookkeeper') AND (TABLE_NAME='" + tableName + "')";

            command.Connection = connection;

            connection.Open();

            int n = GetCountFromScalarCommand(command);
            return n > 0;
        }
    }

    internal int GetCountFromScalarCommand(MySqlCommand command)
    {
        return int.Parse(command.ExecuteScalar().ToString() ?? "0");
    }

    internal void CreateTable()
    {
        using (MySqlConnection connection = GetConnection())
        {
            MySqlCommand command = new();
            
            command.CommandText = schema;
            
            command.Connection = connection;
            
            connection.Open();
            
            command.ExecuteNonQuery();
        }
    }

    internal string FormatCategoryName(string name)
    {
        string newName = name.Replace(' ', '_').ToLower();
        return newName;
    }
}