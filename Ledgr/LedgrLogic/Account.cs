using Microsoft.Data.Sqlite;

namespace LedgrLogic;

public static class Account
{
    /*public static List<string> GetAccountsByNumber()
    {
        List<string> tempAccounts = new List<string>();
        try
        {
            using var connection = new SqliteConnection($"Data Source:" + Database.GetDatabasePath());
            var sql = "SELECT * FROM ACCOUNT ORDER BY Number ASC";
            var command = new SqliteCommand(sql, connection);
            
            connection.Open();
            using var reader = command.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    for (int i = 0; i < 15; i++)
                    {
                        tempAccounts.Add(reader.GetString(i));
                    }
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }

        return tempAccounts;
    }*/

    public static List<string> GetAccounts(string howToOrder)
    {
        List<string> tempAccounts = new List<string>();
        using var connection = new SqliteConnection($"Data Source=" + Database.GetDatabasePath());
        var sql = "";

        switch (howToOrder)
        {
            case ("Number"):
                sql = "SELECT * FROM Account WHERE Active = 1 ORDER BY Number ASC";
                break;
            case("Name"):
                sql = "SELECT * FROM Account WHERE Active = 1 ORDER BY Name DESC";
                break;
            case("Category"):
                sql = "SELECT * FROM Account WHERE Active = 1 ORDER BY Category DESC";
                break;
            case("SubCategory"):
                sql = "SELECT * FROM Account WHERE Active = 1 ORDER BY SubCategory DESC";
                break;
            case("Balance"):
                sql = "SELECT * FROM Account WHERE Active = 1 ORDER BY Balance DESC";
                break;
        }

        try
        {
            var command = new SqliteCommand(sql, connection);
            connection.Open();
            using var reader = command.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    for (int i = 0; i < 15; i++)
                    {
                        tempAccounts.Add(reader.GetString(i));
                    }
                }
            }
            connection.Close();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }

        return tempAccounts;
    }
    
}