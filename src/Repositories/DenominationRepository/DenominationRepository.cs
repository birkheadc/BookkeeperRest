using BookkeeperRest.Models;
using MySql.Data.MySqlClient;

namespace BookkeeperRest.Repositories;

public class DenominationRepository : CrudRepositoryBase, IDenominationRepository
{
    public DenominationRepository(IConfiguration configuration) : base(configuration, "denominations", "CREATE TABLE denominations ( value INT DEFAULT 1 NOT NULL PRIMARY KEY, isDefault BOOL DEFAULT 0 NOT NULL)") {}

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
            int isDefaultInt = denomination.IsDefault ? 1 : 0;
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
            IsDefault = Boolean.Parse(reader["isDefault"].ToString() ?? "false")
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
        using (MySqlConnection connection = GetConnection())
        {
            connection.Open();

            MySqlCommand command = new();
            command.Connection = connection;
            command.CommandText = "UPDATE denominations SET isDefault = @isDefault WHERE value = @value";
            int isDefaultInt = isDefault ? 1 : 0;
            command.Parameters.AddWithValue("@value", value);
            command.Parameters.AddWithValue("@isDefault", isDefaultInt);

            command.ExecuteNonQuery();

            connection.Close();
        }
    }
}