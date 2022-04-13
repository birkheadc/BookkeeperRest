using MySql.Data.MySqlClient;

namespace BookkeeperRest.Repositories.PasswordRepository;

public class PasswordRepository : CrudRepositoryBase, IPasswordRepository
{
    private const string DEFAULT_PASSWORD = "";


    public PasswordRepository(IConfiguration configuration) : base(configuration, "password", "CREATE TABLE password (password VARCHAR(255))") {}

    private bool IsPasswordSet()
    {
        using (MySqlConnection connection = GetConnection())
        {
            MySqlCommand command = new();

            command.CommandText = "SELECT COUNT(password) FROM password";

            command.Connection = connection;

            connection.Open();

            int n = GetCountFromScalarCommand(command);
            return n > 0;
        }
    }

    private void InsertPassword(string password)
    {
        using (MySqlConnection connection = GetConnection())
        {
            MySqlCommand command = new();

            command.Parameters.AddWithValue("@password", password);

            command.CommandText = "INSERT INTO password (password) values (@password)";

            command.Connection = connection;

            connection.Open();

            command.ExecuteNonQuery();

            connection.Close();
        }
    }
    public void Change(string newHashedPassword)
    {
        if (IsPasswordSet() == false)
        {
            InsertPassword(newHashedPassword);
            return;
        }
        using (MySqlConnection connection = GetConnection())
        {
            connection.Open();

            MySqlCommand command = new();
            command.Connection = connection;
            command.Parameters.AddWithValue("@password", newHashedPassword);
            command.CommandText = "UPDATE password SET password=@password";
            command.ExecuteNonQuery();
            
            connection.Close();
        }

    }

    public string Get()
    {
        string password;
        using (MySqlConnection connection = GetConnection())
        {
            MySqlCommand command = new();

            command.Connection = connection;

            command.CommandText = "SELECT password FROM password LIMIT 1";

            connection.Open();

            using (MySqlDataReader reader = command.ExecuteReader())
            {
                if (reader.Read() == false)
                {
                    connection.Close();
                    return DEFAULT_PASSWORD;
                }
                password = reader["password"].ToString() ?? DEFAULT_PASSWORD;
            }

            connection.Close();
        }
        return password;
    }
}