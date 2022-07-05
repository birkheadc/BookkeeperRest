using BookkeeperRest.New.Models;
using MySql.Data.MySqlClient;

namespace BookkeeperRest.New.Repositories;

public class UserSettingRepository : CrudRepositoryBase, IUserSettingRepository
{
    public UserSettingRepository(IWebHostEnvironment env, IConfiguration configuration) : base(env, configuration, "settings_string", "CREATE TABLE settings_string ( name VARCHAR(255) DEFAULT '__blank__' NOT NULL PRIMARY KEY, value VARCHAR(255) DEFAULT '__blank__' NOT NULL )")
    {

    }

    public UserSettings GetAllSettings()
    {
        UserSettings userSettings = new();

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
                    UserSetting setting = GetUserSettingFromReader(reader);
                    // Todo
                }
            }
            
            connection.Close();
        }

        return settings;
    }

    public string GetValueByName(string name)
    {
        string value = "";

        using (MySqlConnection connection = GetConnection())
        {
            connection.Open();
            
            MySqlCommand command = new();
            command.Connection = connection;
            command.CommandText = "SELECT * FROM " + tableName + " WHERE name = @name LIMIT 1";
            command.Parameters.AddWithValue("@name", name);
            

            using (MySqlDataReader reader = command.ExecuteReader())
            {
                if (reader.Read() == false)
                {
                    throw new KeyNotFoundException();
                }
                value = reader["value"].ToString() ?? "";
            }
            
            connection.Close();
        }
        return value;
    }

    public void UpdateSetting(IEnumerable<UserSetting> settings)
    {
        foreach (UserSetting setting in settings)
            {
                if (DoesSettingExistByName(setting.Name) == true)
                {
                    UpdateSetting(setting);
                    return;
                }
                InsertSetting(setting);
            }
    }

    private UserSetting GetUserSettingFromReader(MySqlDataReader reader)
    {
        UserSetting setting = new()
        {
            Name = reader["name"].ToString() ?? "",
            Value = reader["value"].ToString() ?? "",
        };
        return setting;
    }

    private bool DoesSettingExistByName(string name)
    {
        using (MySqlConnection connection = GetConnection())
        {
            connection.Open();
            
            MySqlCommand command = new();
            command.Connection = connection;
            command.CommandText = "SELECT COUNT(*) FROM " + tableName + " WHERE name = @name";
            command.Parameters.AddWithValue("@name", name);
            int n = GetCountFromScalarCommand(command);
            
            connection.Close();

            return n > 0;
        }
    }

    private void InsertSetting(UserSetting setting)
    {
        using (MySqlConnection connection = GetConnection())
        {
            connection.Open();
            
            MySqlCommand command = new();
            command.Connection = connection;
            command.CommandText = "INSERT INTO " + tableName + " (name, value) VALUES (@name, @value)";
            command.Parameters.AddWithValue("@name", setting.Name);
            command.Parameters.AddWithValue("@value", setting.Value);
            command.ExecuteNonQuery();

            connection.Close();
        }
    }

    private void UpdateSetting(UserSetting setting)
    {
        using (MySqlConnection connection = GetConnection())
        {
            connection.Open();
            
            MySqlCommand command = new();
            command.Connection = connection;
            command.CommandText = "UPDATE " + tableName + " SET value = @value WHERE name = @name";
            command.Parameters.AddWithValue("@name", setting.Name);
            command.Parameters.AddWithValue("@value", setting.Value);
            command.ExecuteNonQuery();
            
            connection.Close();
        }
    }
}