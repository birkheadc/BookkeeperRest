using BookkeeperRest.New.Models;
using MySql.Data.MySqlClient;

namespace BookkeeperRest.New.Repositories;

public class EarningCategoryRepository : CrudRepositoryBase, IEarningCategoryRepository
{
    public EarningCategoryRepository(IWebHostEnvironment env, IConfiguration configuration) : base(env, configuration, "earning_categories", "CREATE TABLE earning_categories ( name VARCHAR(255) DEFAULT '__blank__' NOT NULL PRIMARY KEY, isDefault BOOL DEFAULT 0 NOT NULL)")
    {

    }

    public void AddCategoriesByEarnings(IEnumerable<Earning> earnings)
    {
        foreach (Earning earning in earnings)
        {
            if (DoesCategoryExistByName(earning.Category) == false)
            {
                Category category = new()
                {
                    Name = earning.Category,
                    IsDefault = false
                };
                InsertCategory(category);
            }
        }
    }

    public void UpdateCategories(IEnumerable<Category> categories)
    {
        DeleteAll();
        foreach (Category category in categories)
        {
            if (DoesCategoryExistByName(category.Name) == false)
            {
                InsertCategory(category);
            }
            else
            {
                UpdateCategory(category);
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

    private bool DoesCategoryExistByName(string name)
    {
        using (MySqlConnection connection = GetConnection())
        {
            connection.Open();
            
            MySqlCommand command = new();
            command.Connection = connection;
            command.CommandText = "SELECT COUNT(*) FROM " + tableName + " WHERE name = @name";
            command.Parameters.AddWithValue("@name", FormatCategoryName(name));
            int n = GetCountFromScalarCommand(command);
            
            connection.Close();

            return n > 0;
        }
    }

    private void InsertCategory(Category category)
    {
        using (MySqlConnection connection = GetConnection())
        {
            connection.Open();
            
            MySqlCommand command = new();
            command.Connection = connection;
            command.CommandText = "INSERT INTO " + tableName + " (name, isDefault) VALUES (@name, @isDefault)";
            command.Parameters.AddWithValue("@name", FormatCategoryName(category.Name));
            command.Parameters.AddWithValue("@isDefault", category.IsDefault ? 1 : 0);
            
            
            command.ExecuteNonQuery();
            
            connection.Close();
        }
    }

    private void UpdateCategory(Category category)
    {
        using (MySqlConnection connection = GetConnection())
        {
            connection.Open();
            
            MySqlCommand command = new();
            command.Connection = connection;
            command.CommandText = "UPDATE " + tableName + " SET isDefault = @isDefault WHERE name = @name";
            command.Parameters.AddWithValue("@isDefault", category.IsDefault ? 1 : 0);
            command.Parameters.AddWithValue("@name", FormatCategoryName(category.Name));

            command.ExecuteNonQuery();
            
            connection.Close();
        }
    }

    public IEnumerable<Category> GetAllCategories()
    {

        List<Category> categories = new();

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
                    Category category = GetCategoryFromReader(reader);
                    categories.Add(category);
                }
            }
            
            connection.Close();
        }

        return categories;
    }

    private Category GetCategoryFromReader(MySqlDataReader reader)
    {
        Category category = new()
        {
            Name = reader["name"].ToString() ?? "",
            IsDefault = Boolean.Parse(reader["isDefault"].ToString() ?? "false")
        };
        return category;
    }

    public void DeleteByNames(IEnumerable<string> names)
    {
        foreach (string name in names)
        {
            DeleteByName(name);
        }
    }

    private void DeleteByName(string name)
    {
        using (MySqlConnection connection = GetConnection())
        {
            connection.Open();
            
            MySqlCommand command = new();
            command.Connection = connection;
            command.CommandText = "DELETE FROM " + tableName + " WHERE name = @name";
            command.Parameters.AddWithValue("@name", FormatCategoryName(name));
            command.ExecuteNonQuery();

            connection.Close();
        }
    }
}