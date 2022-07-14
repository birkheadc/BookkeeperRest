using BookkeeperRest.New.Models;
using MySql.Data.MySqlClient;

namespace BookkeeperRest.New.Repositories;

public class EarningRepository : CrudRepositoryBase, IEarningRepository
{
    public EarningRepository(IWebHostEnvironment env, IConfiguration configuration) : base(env, configuration, "earnings", "CREATE TABLE earnings ( id CHAR(36) DEFAULT 0 NOT NULL PRIMARY KEY, date DATE DEFAULT (CURDATE()) NOT NULL, category VARCHAR(255) DEFAULT '__blank__' NOT NULL, amount BIGINT DEFAULT 0 NOT NULL )")
    {

    }
    public void AddEarnings(IEnumerable<Earning> earnings)
    {
        using (MySqlConnection connection = GetConnection())
        {
            connection.Open();
            foreach (Earning earning in earnings)
            {
                MySqlCommand command = new();
                command.Connection = connection;
                command.CommandText = "INSERT INTO " + tableName + " (id, date, category, amount) VALUES (@id, @date, @category, @amount)";
                command.Parameters.AddWithValue("@id", earning.Id);
                command.Parameters.AddWithValue("@date", earning.Date.ToString("yyyy-MM-dd"));
                command.Parameters.AddWithValue("@category", FormatCategoryName(earning.Category));
                command.Parameters.AddWithValue("@amount", earning.Amount >= 0 ? earning.Amount : 0);

                command.ExecuteNonQuery();
            }
            connection.Close();
        }
    }

    public void RemoveEarningsByDate(DateTime date)
    {
        using (MySqlConnection connection = GetConnection())
        {
            connection.Open();

            MySqlCommand command = new();
            command.Connection = connection;
            command.CommandText = "DELETE FROM " + tableName + " WHERE date = @date";
            command.Parameters.AddWithValue("@date", date.ToString("yyyy-MM-dd"));

            command.ExecuteNonQuery();

            connection.Close();
        }
    }

    public IEnumerable<Earning> GetEarningsByDate(DateTime date)
    {
        List<Earning> earnings = new();
        using (MySqlConnection connection = GetConnection())
        {
            connection.Open();
            
            MySqlCommand command = new();
            command.Connection = connection;
            command.CommandText = "SELECT * FROM " + tableName + " WHERE date = @date";
            command.Parameters.AddWithValue("@date", date.ToString("yyyy-MM-dd"));

            using (MySqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    Earning earning = GetEarningFromReader(reader);
                    earnings.Add(earning);
                }
            }
            
            connection.Close();
        }
        return earnings;
    }

    private Earning GetEarningFromReader(MySqlDataReader reader)
    {
        Earning earning = new()
        {
            Id = Guid.Parse(reader["id"].ToString() ?? ""),
            Category = reader["category"].ToString() ?? "",
            Amount = long.Parse(reader["amount"].ToString() ?? ""),
            Date = DateTime.Parse(reader["date"].ToString() ?? "")
        };
        return earning;
    }

    public void RemoveAll()
    {
        
    }
}