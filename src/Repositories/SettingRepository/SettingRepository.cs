using BookkeeperRest.Models;
using MySql.Data.MySqlClient;

namespace BookkeeperRest.Repositories;

public class SettingRepository : CrudRepositoryBase, ISettingRepository
{
    public SettingRepository(IConfiguration configuration) : base(configuration, "settings", "CREATE TABLE settings ( key_string VARCHAR(255) DEFAULT '__' NOT NULL PRIMARY KEY, value_string VARCHAR(255) DEFAULT '__' NOT NULL )") {}
    public void Add(Setting setting)
    {
        using (MySqlConnection connection = GetConnection())
        {
            connection.Open();

            MySqlCommand command = new();
            command.Connection = connection;
            command.CommandText = "INSERT INTO settings (key_string, value_string) VALUES (@key, @value)";
            command.Parameters.AddWithValue("@key", setting.Key);
            command.Parameters.AddWithValue("@value", setting.Value);

            command.ExecuteNonQuery();

            connection.Close();
        }
    }

    public void DeleteByKey(string key)
    {
        using (MySqlConnection connection = GetConnection())
        {
            connection.Open();

            MySqlCommand command = new();
            command.Connection = connection;
            command.CommandText = "DELETE FROM settings WHERE key_string = @key";
            command.Parameters.AddWithValue("@key", key);
            command.ExecuteNonQuery();
            
            connection.Close();          
        }
    }

    public IEnumerable<Setting> GetAll()
    {
        List<Setting> settings = new();

        using (MySqlConnection connection = GetConnection())
        {
            connection.Open();

            MySqlCommand command = new();
            command.Connection = connection;
            command.CommandText = "SELECT * FROM settings";
            
            using (MySqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    Setting setting = GetSettingFromReader(reader);
                    settings.Add(setting);
                }
            }

            connection.Close();
        }

        return settings;
    }

    private Setting GetSettingFromReader(MySqlDataReader reader)
    {
        Setting setting = new()
        {
            Key = reader["key_string"].ToString() ?? "",
            Value = reader["value_string"].ToString() ?? ""
        };
        return setting;
    }

    public void UpdateByKey(string key, string value)
    {
        using (MySqlConnection connection = GetConnection())
        {
            connection.Open();

            MySqlCommand command = new();

            command.Connection = connection;
            command.CommandText = "UPDATE settings SET value_string = @value WHERE key_string = @key";
            command.Parameters.AddWithValue("@key", key);
            command.Parameters.AddWithValue("@value", value);

            command.ExecuteNonQuery();

            connection.Close();
        }
    }
}