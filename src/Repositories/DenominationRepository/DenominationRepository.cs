using BookkeeperRest.Models;
using MySql.Data.MySqlClient;

namespace BookkeeperRest.Repositories;

public class DenominationRepository : IDenominationRepository
{

    private string connectionString;

    public DenominationRepository(IConfiguration configuration)
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

            command.CommandText = "SELECT COUNT(*) FROM information_schema.TABLES WHERE (TABLE_SCHEMA='bookkeeper') AND (TABLE_NAME='denominations')";

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
            
            command.CommandText = "CREATE TABLE denominations ( value INT DEFAULT 1 NOT NULL PRIMARY KEY, isDefault BOOL DEFAULT 0 NOT NULL);";
            
            command.Connection = connection;
            
            connection.Open();
            
            command.ExecuteNonQuery();
        }
    }

    public void Add(Denomination denomination)
    {

        if (DoesExistByValue(denomination.Value) == true)
        {
            throw new DuplicateEntryException();
        }

        using (MySqlConnection connection = GetConnection())
        {
            connection.Open();

            MySqlCommand command = new();
            command.Connection = connection;
            command.CommandText = "INSERT INTO denominations (value, isDefault) VALUES (@value, @isDefault)";

            command.Parameters.AddWithValue("@value", denomination.Value);
            int isDefaultInt = 0;
            if (denomination.IsDefault == true) {
                isDefaultInt = 1;
            }
            command.Parameters.AddWithValue("@isDefault", isDefaultInt);
            command.ExecuteNonQuery();
            

            connection.Close();
        }
    }

    public bool DoesExistByValue(int value)
    {
        using (MySqlConnection connection = GetConnection())
        {
            connection.Open();

            MySqlCommand command = new();
            command.Connection = connection;
            command.CommandText = "SELECT * FROM denominations WHERE value = @value";
            command.Parameters.AddWithValue("@value", value);

            if (command.ExecuteScalar() == null)
            {
                connection.Close();
                return false;
            }
            connection.Close();
            return true;
        }
    }

    public IEnumerable<Denomination> GetAll()
    {
        List<Denomination> denominations = new();

        using (MySqlConnection connection = GetConnection())
        {
            connection.Open();

            MySqlCommand command = new();
            command.Connection = connection;
            command.CommandText = "SELECT * FROM denominations";

            using (MySqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    Denomination denomination = GetDenominationFromReaderLine(reader);
                    denominations.Add(denomination);
                }
            }

            connection.Close();
        }

        return denominations;
    }

    private Denomination GetDenominationFromReaderLine(MySqlDataReader reader)
    {
        Denomination denomination = new()
        {
            Value = Int32.Parse(reader["value"].ToString() ?? "1"),
            IsDefault = reader["isDefault"].ToString() == "1"
        };

        return denomination;
    }

    public void RemoveAll()
    {
        throw new NotImplementedException();
    }

    public void RemoveByValue(int value)
    {
        using (MySqlConnection connection = GetConnection())
        {
            connection.Open();

            MySqlCommand command = new();
            command.Connection = connection;
            command.CommandText = "DELETE FROM denominations WHERE value = @value";
            command.Parameters.AddWithValue("@value", value);

            command.ExecuteNonQuery();

            connection.Close();
        }   
    }

    public void UpdateByValue(int value, bool isDefault)
    {
        throw new NotImplementedException();
    }
}