using BookkeeperRest.New.Models;
using MySql.Data.MySqlClient;

namespace BookkeeperRest.New.Repositories;

public class ExpenseRepository : CrudRepositoryBase, IExpenseRepository
{
    public ExpenseRepository(IWebHostEnvironment env, IConfiguration configuration) : base(env, configuration, "expenses", "CREATE TABLE expenses ( id CHAR(36) DEFAULT 0 NOT NULL PRIMARY KEY, date DATE DEFAULT (CURDATE()) NOT NULL, category VARCHAR(255) DEFAULT '__blank__' NOT NULL, amount BIGINT DEFAULT 0 NOT NULL, note TEXT, wasTakenFromCash BOOL DEFAULT 0 NOT NULL )")
    {

    }
    public void AddExpenses(IEnumerable<Expense> expenses)
    {
        using (MySqlConnection connection = GetConnection())
        {
            connection.Open();
            foreach (Expense expense in expenses)
            {
                if (expense.Amount != 0)
                {
                    MySqlCommand command = new();
                    command.Connection = connection;
                    command.CommandText = "INSERT INTO " + tableName + " (id, date, category, amount, note, wasTakenFromCash) VALUES (@id, @date, @category, @amount, @note, @wasTakenFromCash)";
                    command.Parameters.AddWithValue("@id", expense.Id);
                    command.Parameters.AddWithValue("@date", expense.Date.ToString("yyyy-MM-dd"));
                    command.Parameters.AddWithValue("@category", FormatCategoryName(expense.Category));
                    command.Parameters.AddWithValue("@amount", expense.Amount >= 0 ? expense.Amount : 0); 
                    command.Parameters.AddWithValue("@note", expense.Note);
                    command.Parameters.AddWithValue("@wasTakenFromCash", expense.WasTakenFromCash ? 1 : 0);

                    command.ExecuteNonQuery();
                }
            }
            connection.Close();
        }
    }

    public void RemoveExpensesByDate(DateTime date)
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

    public IEnumerable<Expense> GetExpensesByDate(DateTime date)
    {
        List<Expense> expenses = new();
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
                    Expense expense = GetExpenseFromReader(reader);
                    expenses.Add(expense);
                }
            }
            
            connection.Close();
        }
        return expenses;
    }

    private Expense GetExpenseFromReader(MySqlDataReader reader)
    {
        Expense expense = new()
        {
            Id = Guid.Parse(reader["id"].ToString() ?? ""),
            Category = reader["category"].ToString() ?? "",
            Amount = long.Parse(reader["amount"].ToString() ?? ""),
            Date = DateTime.Parse(reader["date"].ToString() ?? ""),
            Note = reader["note"].ToString() ?? "",
            WasTakenFromCash = Boolean.Parse(reader["wasTakenFromCash"].ToString() ?? "false")
        };
        return expense;
    }

    public void RemoveAll()
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

    public IEnumerable<Expense> GetAll()
    {
        List<Expense> expenses = new();
        using (MySqlConnection connection = GetConnection())
        {
            connection.Open();
            
            MySqlCommand command = new();
            command.Connection = connection;
            command.CommandText = "SELECT * FROM " + tableName + " ORDER BY date DESC";

            using (MySqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    Expense expense = GetExpenseFromReader(reader);
                    expenses.Add(expense);
                }
            }
            
            connection.Close();
        }
        return expenses;
    }
}