using MySql.Data.MySqlClient;

namespace BookkeeperRest.New.Repositories;

public class PasswordRepository : CrudRepositoryBase, IPasswordRepository
{
    public PasswordRepository(IWebHostEnvironment env, IConfiguration configuration) : base(env, configuration, "password", "CREATE TABLE password (password VARCHAR(255))")
    {

    }

    private bool IsPasswordSet()
    {
        using (MySqlConnection connection = GetConnection())
        {
            connection.Open();
            
            MySqlCommand command = new();
            command.Connection = connection;
            command.CommandText = "SELECT COUNT(*) FROM " + tableName;
            int n = GetCountFromScalarCommand(command);

            connection.Close();

            return n > 0;
        }
    }

    private void InsertPassword(string password)
    {
        using (MySqlConnection connection = GetConnection())
        {
            connection.Open();
            
            MySqlCommand command = new();
            command.Connection = connection;
            command.CommandText = "INSERT INTO " + tableName + " (password) VALUES (@password)";
            command.Parameters.AddWithValue("@password", password);
            command.ExecuteNonQuery();
            
            connection.Close();
        }
    }

    public void ChangePassword(string password)
    {
        if (IsPasswordSet() == false)
        {
            InsertPassword(password);
            return;
        }
        using (MySqlConnection connection = GetConnection())
        {
            connection.Open();
            
            MySqlCommand command = new();
            command.Connection = connection;
            command.CommandText = "UPDATE " + tableName + " SET password = @password";
            command.Parameters.AddWithValue("@password", password);
            command.ExecuteNonQuery();
            
            connection.Close();
        }
    }

    public string GetPassword()
    {
        string password = "";

        using (MySqlConnection connection = GetConnection())
        {
            connection.Open();
            
            MySqlCommand command = new();
            command.Connection = connection;
            command.CommandText = "SELECT password FROM " + tableName + " LIMIT 1";
            
            using (MySqlDataReader reader = command.ExecuteReader())
            {
                if (reader.Read() == true)
                {
                    password = reader["password"].ToString() ?? "";
                }
            }

            connection.Close();
        }

        return password;
    }
}