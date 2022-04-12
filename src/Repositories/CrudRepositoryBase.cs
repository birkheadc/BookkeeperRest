using MySql.Data.MySqlClient;

namespace BookkeeperRest.Repositories;

public abstract class CrudRepositoryBase
{
    internal string connectionString;

    internal readonly string tableName;
    private readonly string schema;

    public CrudRepositoryBase(IConfiguration configuration, string tableName, string schema)
    {
        connectionString = configuration.GetConnectionString("DefaultConnection");
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
        MySqlConnection connection = new(connectionString);
        return connection;
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
}