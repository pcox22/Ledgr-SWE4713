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

    public static List<string> GetAccountFromAccountNumber(int accountNum)
    {
        List<string> Account = new List<string>();
        try
        {
            string sql = "SELECT * FROM Account where Number = @ACCOUNTNUM";
            using var connection = new SqliteConnection($"Data Source=" + Database.GetDatabasePath());

            using var command = new SqliteCommand(sql, connection);

            using var reader = command.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    for (int i = 0; i < 15; i++)
                    {
                        Account.Add(reader.GetString(i));
                    }
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
        return Account;
    }

    public static bool UpdateAccountCredit(int accountNum, double tempAmount, string username)
    {
        //getting user ID for event log
        User temp = User.GetUserFromUserName(username).Result;
        int userID = temp.GetUserID();

        try
        {
            //updating event log before change
            EventLog.LogAccount('b', accountNum, userID);

            var sql = "UPDATE Account SET Credit = @CREDIT WHERE Number = @ID";
            using var connection = new SqliteConnection($"Data Source=" + Database.GetDatabasePath());

            var command = new SqliteCommand(sql, connection);
            command.Parameters.AddWithValue("@CREDIT", tempAmount);
            command.Parameters.AddWithValue("@ID", accountNum);

            connection.Open();

            command.ExecuteNonQuery();

            //updating event log after change
            EventLog.LogAccount('a', accountNum, userID);
            
            //update balance in database
            UpdateAccountBalance(accountNum, username);

            connection.Close();
        }
        catch (Exception e)
        {
            throw new InvalidChangeException("No such account exists");
        }

        return true;
    }
    
    public static bool UpdateAccountDebit(int accountNum, double tempAmount, string username)
    {
        //getting user ID for event log
        User temp = User.GetUserFromUserName(username).Result;
        int userID = temp.GetUserID();

        try
        {
            //updating event log before change
            EventLog.LogAccount('b', accountNum, userID);

            var sql = "UPDATE Account SET Debit = @DEBIT WHERE Number = @ID";
            using var connection = new SqliteConnection($"Data Source=" + Database.GetDatabasePath());

            var command = new SqliteCommand(sql, connection);
            command.Parameters.AddWithValue("@DEBIT", tempAmount);
            command.Parameters.AddWithValue("@ID", accountNum);

            connection.Open();

            command.ExecuteNonQuery();

            //updating event log after change
            EventLog.LogAccount('a', accountNum, userID);
            
            //update balance in database
            UpdateAccountBalance(accountNum, username);
            
            connection.Close();
        }
        catch (Exception e)
        {
            throw new InvalidChangeException("No such account exists");
        }

        return true;
    }

    public static bool UpdateAccountBalance(int accountNum, string username)
    {
        //getting user ID for event log
        User temp = User.GetUserFromUserName(username).Result;
        int userID = temp.GetUserID();
        
        try
        {
            var sql = "SELECT NormalSide, Debit, Credit, Balance FROM Account WHERE Number = @NUM";
            using var connection = new SqliteConnection($"Data Source=" + Database.GetDatabasePath());
            connection.Open();
            
            var command = new SqliteCommand(sql, connection);
            command.Parameters.AddWithValue("@NUM", accountNum);

            using var reader = command.ExecuteReader();

            char normalSide = 'U';
            double debit = 0;
            double credit = 0;
            double balance = 0;

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    normalSide = reader.GetChar(0);
                    debit = reader.GetDouble(1);
                    credit = reader.GetDouble(2);
                    balance = reader.GetDouble(3);
                }
            }
            
            //Updating the balance before reassigning its value in the database
            if (normalSide == 'L')
            {
                balance = balance + (debit - credit);
            }
            else if (normalSide == 'R')
            {
                balance = balance + (credit - debit);
            }
            else
            {
                throw new UnableToRetrieveException(
                    "Unable to retrieve this account's normal side, debit, credit, or balance");
            }
            
            //saving the before image in the event log
            EventLog.LogAccount('b', accountNum, userID);
            
            //Updating balance stored in the database
            var updateSql = "UPDATE Account SET Balance = @BALANCE WHERE Number = @NUM";
            var updateCommand = new SqliteCommand(updateSql, connection);
            updateCommand.Parameters.AddWithValue("@BALANCE", balance);
            updateCommand.Parameters.AddWithValue("@NUM", accountNum);

            command.ExecuteNonQuery();
            
            //saving the after image in the event log
            EventLog.LogAccount('b', accountNum, userID);
            
            connection.Close();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }

        return true;
    }
}