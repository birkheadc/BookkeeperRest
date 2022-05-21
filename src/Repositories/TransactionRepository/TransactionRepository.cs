using BookkeeperRest.Models.Transaction;
using MySql.Data.MySqlClient;

namespace BookkeeperRest.Repositories.TransactionRepository;

public class TransactionRepository : CrudRepositoryBase, ITransactionRepository
{
    public TransactionRepository(IWebHostEnvironment env, IConfiguration configuration) : base(env, configuration, "transactions", "CREATE TABLE transactions ( id CHAR(36) DEFAULT 0 NOT NULL PRIMARY KEY, date DATE DEFAULT (CURDATE()) NOT NULL, type VARCHAR(255) DEFAULT '__error__' NOT NULL, amount BIGINT DEFAULT 0 NOT NULL, note TEXT )") {}

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
                command.Parameters.AddWithValue("@date", transaction.Date.ToString("yyyy-MM-dd"));
                command.Parameters.AddWithValue("@type", transaction.Type);
                command.Parameters.AddWithValue("@amount", transaction.Amount);
                command.Parameters.AddWithValue("@note", transaction.Note);

                command.ExecuteNonQuery();
            }

            connection.Close();
        }
    }

    public IEnumerable<Transaction> FindAllOrderByDateDesc()
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

    public IEnumerable<Transaction> FindByDate(DateTime date)
    {
        List<Transaction> transactions = new();

        using (MySqlConnection connection = GetConnection())
        {
            connection.Open();

            MySqlCommand command = new();
            command.Connection = connection;
            command.CommandText = "SELECT * FROM transactions WHERE date=@date ORDER BY date DESC";
            
            command.Parameters.AddWithValue("@date", date.ToString("yyyy-MM-dd"));

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

    public IEnumerable<Transaction> FindBetweenDates(DateTime startDate, DateTime endDate)
    {
        List<Transaction> transactions = new();

        using (MySqlConnection connection = GetConnection())
        {
            connection.Open();

            MySqlCommand command = new();
            command.Connection = connection;
            command.CommandText = "SELECT * FROM transactions WHERE date BETWEEN @start AND @end ORDER BY date DESC";
            
            command.Parameters.AddWithValue("@start", startDate.ToString("yyyy-MM-dd"));
            command.Parameters.AddWithValue("@end", endDate.ToString("yyyy-MM-dd"));

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

    public void DeleteById(string id)
    {
        using (MySqlConnection connection = GetConnection())
        {
            connection.Open();

            MySqlCommand command = new();
            command.Connection = connection;
            command.CommandText = "DELETE FROM transactions WHERE id=@id";
            command.Parameters.AddWithValue("@id", id);

            command.ExecuteNonQuery();
            
            connection.Close();
        }
    }

    public void UpdateMultiple(IEnumerable<Transaction> transactions)
    {
        using (MySqlConnection connection = GetConnection())
        {
            connection.Open();

            foreach (Transaction transaction in transactions)
            {
                MySqlCommand command = new();
                command.Connection = connection;
                command.CommandText = "UPDATE transactions SET amount = @amount, note = @note, type = @type WHERE id = @id";
                command.Parameters.AddWithValue("@amount", transaction.Amount);
                command.Parameters.AddWithValue("@note", transaction.Note);
                command.Parameters.AddWithValue("@type", transaction.Type);
                command.Parameters.AddWithValue("@id", transaction.Id.ToString());
                command.ExecuteNonQuery();
            }

            connection.Close();
        }
    }
}