using BookkeeperRest.Models.Transaction;
using MySql.Data.MySqlClient;

namespace BookkeeperRest.Repositories.TransactionRepository;

public class TransactionRepository : ITransactionRepository
{
    private string connectionString;

    public TransactionRepository(IConfiguration configuration)
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

            command.CommandText = "SELECT COUNT(*) FROM information_schema.TABLES WHERE (TABLE_SCHEMA='bookkeeper') AND (TABLE_NAME='transactions')";

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
            
            command.CommandText = "CREATE TABLE transactions ( id CHAR(36) DEFAULT 0 NOT NULL PRIMARY KEY, date DATE DEFAULT (CURDATE()) NOT NULL, type VARCHAR(255) DEFAULT '__error__' NOT NULL, amount BIGINT DEFAULT 0 NOT NULL, note TEXT );";
            
            command.Connection = connection;
            
            connection.Open();
            
            command.ExecuteNonQuery();
        }
    }

    public void Add(Transaction transaction)
    {
        throw new NotImplementedException();
    }

    public void Add(IEnumerable<Transaction> transactions)
    {
        using (MySqlConnection connection = GetConnection())
        {
            connection.Open();

            foreach (Transaction transaction in transactions)
            {
                MySqlCommand command = new();
                command.Connection = connection;
                command.CommandText = "INSERT INTO transactions (id, date, type, amount, note) VALUES (@id, @date, @type, @amount, @note)";

                command.Parameters.AddWithValue("@id", transaction.Id.ToString());
                Console.WriteLine("LOOKFORME: " + transaction.Date.ToString("yyyy-MM-dd"));
                command.Parameters.AddWithValue("@date", transaction.Date.ToString("yyyy-MM-dd"));
                command.Parameters.AddWithValue("@type", transaction.Type);
                command.Parameters.AddWithValue("@amount", transaction.Amount);
                command.Parameters.AddWithValue("@note", transaction.Note);

                command.ExecuteNonQuery();
            }

            connection.Close();
        }
    }

    public IEnumerable<Transaction> FindAllByDateDesc()
    {
        List<Transaction> transactions = new();

        using (MySqlConnection connection = GetConnection())
        {
            connection.Open();

            MySqlCommand command = new();
            command.Connection = connection;
            command.CommandText = "SELECT * FROM transactions ORDER BY date DESC";

            using (MySqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    Transaction transaction = GetTransactionFromReaderLine(reader);
                    transactions.Add(transaction);
                }
            }

            connection.Close();
        }

        return transactions;
    }

    private Transaction GetTransactionFromReaderLine(MySqlDataReader reader)
    {
        Transaction transaction = new()
        {
            Id = Guid.Parse(reader["id"].ToString() ?? Guid.Empty.ToString()),
            Date = DateTime.Parse(reader["date"].ToString() ?? DateTime.UnixEpoch.ToString()),
            Type = reader["type"].ToString() ?? "__error__",
            Amount = long.Parse(reader["amount"].ToString() ?? "0"),
            Note = reader["note"].ToString() ?? ""
        };

        return transaction;
    }
}