using BookkeeperRest.New.Models;
using MySql.Data.MySqlClient;

namespace BookkeeperRest.New.Repositories;

public class DenominationRepository : CrudRepositoryBase, IDenominationRepository
{
    public DenominationRepository(IWebHostEnvironment env, IConfiguration configuration) : base(env, configuration, "denominations", "CREATE TABLE denominations ( value INT DEFAULT 1 NOT NULL PRIMARY KEY, isDefault BOOL DEFAULT 0 NOT NULL)")
    {
        
    }

    public IEnumerable<Denomination> GetAllDenominations()
    {
        List<Denomination> denominations = new();

        using (MySqlConnection connection = GetConnection())
        {
            connection.Open();
            
            MySqlCommand command = new();
            command.Connection = connection;
            command.CommandText = "SELECT * FROM " + tableName;

            using (MySqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    Denomination denomination = GetDenominationFromReader(reader);
                    denominations.Add(denomination);
                }
            }
            
            connection.Close();
        }

        return denominations;
    }

    public void UpdateAll(IEnumerable<Denomination> denominations)
    {
        DeleteAll();
        foreach (Denomination denomination in denominations)
        {
            if (DoesDenominationExist(denomination) == true)
            {
                UpdateDenomination(denomination);
            }
            else
            {
                InsertDenomination(denomination);
            }
        }
    }

    private void DeleteAll()
    {
        using (MySqlConnection connection = GetConnection())
        {
            connection.Open();
            
            MySqlCommand command = new();
            command.Connection = connection;
            command.CommandText = "DELETE FROM " + tableName;
            command.ExecuteNonQuery();
            
            connection.Close();
        }
    }

    public void DeleteByValues(IEnumerable<int> values)
    {
        foreach (int value in values)
        {
            using (MySqlConnection connection = GetConnection())
            {
                connection.Open();
                
                MySqlCommand command = new();
                command.Connection = connection;
                command.CommandText = "DELETE FROM " + tableName + " WHERE value = @value";
                command.Parameters.AddWithValue("@value", value);
                command.ExecuteNonQuery();
                
                connection.Close();
            }
        }
    }

    private void InsertDenomination(Denomination denomination)
    {
        using (MySqlConnection connection = GetConnection())
        {
            connection.Open();
            
            MySqlCommand command = new();
            command.Connection = connection;
            command.CommandText = "INSERT INTO " + tableName + " (value, isDefault) VALUES (@value, @isDefault)";
            command.Parameters.AddWithValue("@value", denomination.Value);
            command.Parameters.AddWithValue("@isDefault", denomination.IsDefault ? 1 : 0);
            command.ExecuteNonQuery();
            
            connection.Close();
        }
    }

    private void UpdateDenomination(Denomination denomination)
    {
        using (MySqlConnection connection = GetConnection())
        {
            connection.Open();
            
            MySqlCommand command = new();
            command.Connection = connection;
            command.CommandText = "UPDATE " + tableName + " SET isDefault = @isDefault WHERE value = @value";
            command.Parameters.AddWithValue("@value", denomination.Value);
            command.Parameters.AddWithValue("@isDefault", denomination.IsDefault ? 1 : 0);
            command.ExecuteNonQuery();
            
            connection.Close();
        }
    }

    private bool DoesDenominationExist(Denomination denomination)
    {
        using (MySqlConnection connection = GetConnection())
        {
            connection.Open();
            
            MySqlCommand command = new();
            command.Connection = connection;
            command.CommandText = "SELECT COUNT(*) FROM " + tableName + " WHERE value = @value";
            command.Parameters.AddWithValue("@value", denomination.Value);

            int n = GetCountFromScalarCommand(command);
            
            connection.Close();

            return n > 0;
        }
    }

    private Denomination GetDenominationFromReader(MySqlDataReader reader)
    {
        Denomination denomination = new()
        {
            Value = int.Parse(reader["value"].ToString() ?? "0"),
            IsDefault = Boolean.Parse(reader["isDefault"].ToString() ?? "false")
        };
        return denomination;
    }
}