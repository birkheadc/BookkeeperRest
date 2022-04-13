using BookkeeperRest.Models;
using MySql.Data.MySqlClient;

namespace BookkeeperRest.Repositories;

public class TransactionTypeRepository : CrudRepositoryBase, ITransactionTypeRepository
{
    public TransactionTypeRepository(IConfiguration configuration) : base(configuration, "transaction_types", "CREATE TABLE transaction_types ( name VARCHAR(255) DEFAULT 'default' NOT NULL PRIMARY KEY, polarity TINYINT DEFAULT 1 NOT NULL, isDefault BOOL DEFAULT 0 NOT NULL)") {}

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
            command.CommandText = "INSERT INTO transaction_types (name, polarity, isDefault) VALUES (@name, @polarity, @isDefault)";

            command.Parameters.AddWithValue("@name", type.Name);
            int isDefaultInt = 0;
            if (type.IsDefault == true) {
                isDefaultInt = 1;
            }
            command.Parameters.AddWithValue("@isDefault", isDefaultInt);
            command.Parameters.AddWithValue("@polarity", type.Polarity);
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
            command.CommandText = "SELECT * FROM transaction_types WHERE name = @name";
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
            command.CommandText = "SELECT * FROM transaction_types";

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
            IsDefault = Boolean.Parse(reader["isDefault"].ToString() ?? "false")
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
            command.CommandText = "DELETE FROM transaction_types WHERE name = @name";
            command.Parameters.AddWithValue("@name", name);

            command.ExecuteNonQuery();

            connection.Close();
        }
    }

    public void UpdateByName(string name, bool isDefault)
    {
        using (MySqlConnection connection = GetConnection())
        {
            connection.Open();

            MySqlCommand command = new();
            command.Connection = connection;
            command.CommandText = "UPDATE transaction_types SET isDefault = @isDefault WHERE name = @name";
            int isDefaultInt = isDefault ? 1 : 0;
            command.Parameters.AddWithValue("@isDefault", isDefaultInt);
            command.Parameters.AddWithValue("@name", name);
            command.ExecuteNonQuery();

            connection.Close();
        }
    }
}