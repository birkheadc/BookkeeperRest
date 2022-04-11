using BookkeeperRest.Models;
using MySql.Data.MySqlClient;

namespace BookkeeperRest.Repositories;

public class DenominationRepository : IDenominationRepository
{

    private string connectionString;

    public DenominationRepository(IConfiguration configuration)
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

            command.CommandText = "SELECT COUNT(*) FROM information_schema.TABLES WHERE (TABLE_SCHEMA='bookkeeper') AND (TABLE_NAME='denominations')";

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
            
            command.CommandText = "CREATE TABLE denominations ( value INT DEFAULT 1 NOT NULL PRIMARY KEY, isDefault BOOL DEFAULT 0 NOT NULL);";
            
            command.Connection = connection;
            
            connection.Open();
            
            command.ExecuteNonQuery();
        }
    }

    public void Add(Denomination denomination)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<Denomination> GetAll()
    {
        throw new NotImplementedException();
    }

    public void RemoveAll()
    {
        throw new NotImplementedException();
    }

    public void RemoveByValue(int value)
    {
        throw new NotImplementedException();
    }

    public void UpdateByValue(int value, bool isDefault)
    {
        throw new NotImplementedException();
    }
}