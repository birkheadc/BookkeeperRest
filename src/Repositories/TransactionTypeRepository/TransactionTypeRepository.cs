using BookkeeperRest.Models;
using MySql.Data.MySqlClient;

namespace BookkeeperRest.Repositories;

public class TransactionTypeRepository : ITransactionTypeRepository
{
    private string connectionString;

    public TransactionTypeRepository(IConfiguration configuration)
    {
        connectionString = configuration.GetConnectionString("DefaultConnection");
        InitializeDatabase();
    }

    private void InitializeDatabase()
    {
        if (DoesTableExist() == false)
        {
            CreateTable();
        }
    }

    private MySqlConnection GetConnection()
    {
        MySqlConnection connection = new(connectionString);
        return connection;
    }

    private bool DoesTableExist()
    {
        using (MySqlConnection connection = GetConnection())
        {
            MySqlCommand command = new();

            command.CommandText = "SELECT COUNT(*) FROM information_schema.TABLES WHERE (TABLE_SCHEMA='bookkeeper') AND (TABLE_NAME='transactiontypes')";

            command.Connection = connection;

            connection.Open();

            int n = GetCountFromScalarCommand(command);
            return n > 0;
        }
    }

    private int GetCountFromScalarCommand(MySqlCommand command)
    {
        return int.Parse(command.ExecuteScalar().ToString() ?? "0");
    }

    private void CreateTable()
    {
        using (MySqlConnection connection = GetConnection())
        {
            MySqlCommand command = new();
            
            command.CommandText = "CREATE TABLE transactiontypes ( name VARCHAR(255) DEFAULT 'default' NOT NULL PRIMARY KEY, polarity TINYINT DEFAULT 1 NOT NULL, isDefault BOOL DEFAULT 0 NOT NULL);";
            
            command.Connection = connection;
            
            connection.Open();
            
            command.ExecuteNonQuery();
        }
    }

    public void Add(TransactionType type)
    {

        if (DoesExistByName(type.Name) == true)
        {
            throw new DuplicateEntryException();
        }

        using (MySqlConnection connection = GetConnection())
        {
            connection.Open();

            MySqlCommand command = new();
            command.Connection = connection;
            command.CommandText = "INSERT INTO transactiontypes (name, polarity, isDefault) VALUES (@name, @polarity, @isDefault)";

            command.Parameters.AddWithValue("@name", type.Name);
            int isDefaultInt = 0;
            if (type.IsDefault == true) {
                isDefaultInt = 1;
            }
            command.Parameters.AddWithValue("@isDefault", isDefaultInt);
            command.Parameters.AddWithValue("@polarity", type.Polarity);
            Console.WriteLine(type);
            command.ExecuteNonQuery();
            

            connection.Close();
        }
    }

    public bool DoesExistByName(string name)
    {
        using (MySqlConnection connection = GetConnection())
        {
            connection.Open();

            MySqlCommand command = new();
            command.Connection = connection;
            command.CommandText = "SELECT * FROM transactiontypes WHERE name = @name";
            command.Parameters.AddWithValue("@name", name);

            if (command.ExecuteScalar() == null)
            {
                connection.Close();
                return false;
            }
            connection.Close();
            return true;
        }
    }

    public IEnumerable<TransactionType> GetAll()
    {
        List<TransactionType> types = new();

        using (MySqlConnection connection = GetConnection())
        {
            connection.Open();

            MySqlCommand command = new();
            command.Connection = connection;
            command.CommandText = "SELECT * FROM transactiontypes";

            using (MySqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    TransactionType type = GetTransactionTypeFromReaderLine(reader);
                    types.Add(type);
                }
            }

            connection.Close();
        }

        return types;
    }

    private TransactionType GetTransactionTypeFromReaderLine(MySqlDataReader reader)
    {
        TransactionType type = new()
        {
            Name = reader["name"].ToString() ?? "",
            Polarity = Int16.Parse(reader["polarity"].ToString() ?? "1"),
            IsDefault = reader["isDefault"].ToString() == "1"
        };

        return type;
    }

    public void RemoveAll()
    {
        throw new NotImplementedException();
    }

    public void RemoveByName(string name)
    {
        using (MySqlConnection connection = GetConnection())
        {
            connection.Open();

            MySqlCommand command = new();
            command.Connection = connection;
            command.CommandText = "DELETE FROM transactiontypes WHERE name = @name";
            command.Parameters.AddWithValue("@name", name);

            command.ExecuteNonQuery();

            connection.Close();
        }
    }

    public void UpdateByName(string name, bool isDefault)
    {
        throw new NotImplementedException();
    }
}