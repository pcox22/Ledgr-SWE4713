using System.Reflection.Metadata;
using Microsoft.Data.Sqlite;

namespace LedgrLogic;

public class Accountant : User
{
    public Accountant(string tempUsername, string tempPass, string tempEmail, int tempUserID, int tempEmployeeID, bool tempActive, bool tempNew) : base (tempUsername, tempPass, tempEmail, tempUserID, tempEmployeeID, tempActive, tempNew) {}
 
    //Create journal entries (Done) (Not tested)
    //Because a journal entry can contain any number of debits or credits, creating a journal entry will be broken up into multiple methods
    
    //CreateJournalEntry() creates a new entry in the JournalEntry table, and returns the ID of that new entry
    //Comments and reference docs are optional, so there will be multiple methods but with different paramaters
    public static int CreateJournalEntry(string date, string comment, Blob reference, string username)
    {
        int userID = GetUserFromUserName(username).Result.GetUserID();
        int journalEntryID = -1;
        try
        {
            var insertSql = "INSERT INTO JournalEntry VALUES(null, @DATE, @STATUS, @COMMENT, @REFERENCE, @USERID)";
            using var connection = new SqliteConnection($"Data Source=" + Database.GetDatabasePath());
            connection.Open();
            
            var insertCommand = new SqliteCommand(insertSql, connection);
            insertCommand.Parameters.AddWithValue("@DATE", date);
            insertCommand.Parameters.AddWithValue("@STATUS", 'P');
            insertCommand.Parameters.AddWithValue("@COMMENT", comment);
            insertCommand.Parameters.AddWithValue("@REFERENCE", reference);
            insertCommand.Parameters.AddWithValue("@USERID", userID);

            insertCommand.ExecuteNonQuery();

            /*var selectSql =
                "SELECT ID FROM JOURNALENTRY WHERE Date = @DATE AND Status = @STATUS AND Comment = @COMMENT AND Reference = @Reference";*/

            var selectSql = "SELECT ID FROM JournalEntry ORDER BY DESC LIMIT 1";

            var selectCommand = new SqliteCommand(selectSql, connection);
            /*insertCommand.Parameters.AddWithValue("@DATE", date);
            insertCommand.Parameters.AddWithValue("@STATUS", 'P');
            insertCommand.Parameters.AddWithValue("@COMMENT", comment);
            insertCommand.Parameters.AddWithValue("@REFERENCE", reference);*/

            using var reader = selectCommand.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    journalEntryID = reader.GetInt32(0);
                }
            }
            connection.Close();
        }
        catch(Exception e)
        {
            Console.WriteLine(e);
            //throw some error here
        }

        return journalEntryID;
    }

    public static int CreateJournalEntry(string date, string comment, string username)
    {
        int userID = GetUserFromUserName(username).Result.GetUserID();
        int journalEntryID = -1;
        try
        {
            var insertSql = "INSERT INTO JournalEntry(ID, Date, Status, Comment, UserID) VALUES(null, @DATE, @STATUS, @COMMENT, @USERID)";
            using var connection = new SqliteConnection($"Data Source=" + Database.GetDatabasePath());
            connection.Open();
            
            var insertCommand = new SqliteCommand(insertSql, connection);
            insertCommand.Parameters.AddWithValue("@DATE", date);
            insertCommand.Parameters.AddWithValue("@STATUS", 'P');
            insertCommand.Parameters.AddWithValue("@COMMENT", comment);
            insertCommand.Parameters.AddWithValue("@USERID", userID);
            
            insertCommand.ExecuteNonQuery();

            /*var selectSql =
                "SELECT ID FROM JOURNALENTRY WHERE Date = @DATE AND Status = @STATUS AND Comment = @COMMENT AND Reference = @Reference";*/

            var selectSql = "SELECT ID FROM JournalEntry ORDER BY DESC LIMIT 1";

            var selectCommand = new SqliteCommand(selectSql, connection);
            /*insertCommand.Parameters.AddWithValue("@DATE", date);
            insertCommand.Parameters.AddWithValue("@STATUS", 'P');
            insertCommand.Parameters.AddWithValue("@COMMENT", comment);
            insertCommand.Parameters.AddWithValue("@REFERENCE", reference);*/

            using var reader = selectCommand.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    journalEntryID = reader.GetInt32(0);
                }
            }
            connection.Close();
        }
        catch(Exception e)
        {
            Console.WriteLine(e);
            //throw some error here
        }

        return journalEntryID;
    }

    public static int CreateJournalEntry(string date, Blob reference, string username)
    {
        int userID = GetUserFromUserName(username).Result.GetUserID();
        int journalEntryID = -1;
        try
        {
            var insertSql = "INSERT INTO JournalEntry(ID, Date, Status, Reference, UserID) VALUES(null, @DATE, @STATUS, @REFERENCE, @USERID)";
            using var connection = new SqliteConnection($"Data Source=" + Database.GetDatabasePath());
            connection.Open();
            
            var insertCommand = new SqliteCommand(insertSql, connection);
            insertCommand.Parameters.AddWithValue("@DATE", date);
            insertCommand.Parameters.AddWithValue("@STATUS", 'P');
            insertCommand.Parameters.AddWithValue("@REFERENCE", reference);
            insertCommand.Parameters.AddWithValue("@USERID", userID);

            insertCommand.ExecuteNonQuery();

            /*var selectSql =
                "SELECT ID FROM JOURNALENTRY WHERE Date = @DATE AND Status = @STATUS AND Comment = @COMMENT AND Reference = @Reference";*/

            var selectSql = "SELECT ID FROM JournalEntry ORDER BY DESC LIMIT 1";

            var selectCommand = new SqliteCommand(selectSql, connection);
            /*insertCommand.Parameters.AddWithValue("@DATE", date);
            insertCommand.Parameters.AddWithValue("@STATUS", 'P');
            insertCommand.Parameters.AddWithValue("@COMMENT", comment);
            insertCommand.Parameters.AddWithValue("@REFERENCE", reference);*/

            using var reader = selectCommand.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    journalEntryID = reader.GetInt32(0);
                }
            }
            connection.Close();
        }
        catch(Exception e)
        {
            Console.WriteLine(e);
            //throw some error here
        }

        return journalEntryID;
    }

    public static int CreateJournalEntry(string date, string username)
    {
        int userID = GetUserFromUserName(username).Result.GetUserID();
            int journalEntryID = -1;
            try
            {
                var insertSql = "INSERT INTO JournalEntry(ID, Date, Status, UserID) VALUES(null, @DATE, @STATUS, @USERID)";
                using var connection = new SqliteConnection($"Data Source=" + Database.GetDatabasePath());
                connection.Open();
            
                var insertCommand = new SqliteCommand(insertSql, connection);
                insertCommand.Parameters.AddWithValue("@DATE", date);
                insertCommand.Parameters.AddWithValue("@STATUS", 'P');
                insertCommand.Parameters.AddWithValue("@USERID", userID);

                insertCommand.ExecuteNonQuery();

                /*var selectSql =
                    "SELECT ID FROM JOURNALENTRY WHERE Date = @DATE AND Status = @STATUS AND Comment = @COMMENT AND Reference = @Reference";*/

                var selectSql = "SELECT ID FROM JournalEntry ORDER BY DESC LIMIT 1";

                var selectCommand = new SqliteCommand(selectSql, connection);
                /*insertCommand.Parameters.AddWithValue("@DATE", date);
                insertCommand.Parameters.AddWithValue("@STATUS", 'P');
                insertCommand.Parameters.AddWithValue("@COMMENT", comment);
                insertCommand.Parameters.AddWithValue("@REFERENCE", reference);*/

                using var reader = selectCommand.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        journalEntryID = reader.GetInt32(0);
                    }
                }
                connection.Close();
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
                //throw some error here
            }

            return journalEntryID;
    }
    
    //Inserting data into the JournalEntryDebit & JournalEntryCredit tables
    public static bool CreateJournalEntryDebit(int accountNum, double amount, int journalEntryID)
    {
        try
        {
            using var connection = new SqliteConnection($"Data Source=" + Database.GetDatabasePath());
            var sql = "INSERT INTO JournalEntryDebit VALUES(NULL, @ACCOUNTNUM, @AMOUNT, @JE)";
            connection.Open();
            
            var command = new SqliteCommand(sql, connection);
            command.Parameters.AddWithValue("@ACCOUNTNUM", accountNum);
            command.Parameters.AddWithValue("@AMOUNT", amount);
            command.Parameters.AddWithValue("@JE", journalEntryID);

            command.ExecuteNonQuery();
            connection.Close();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            //throw some error here
        }

        return true;
    }
    public static bool CreateJournalEntryCredit(int accountNum, double amount, int journalEntryID)
    {
        try
        {
            using var connection = new SqliteConnection($"Data Source=" + Database.GetDatabasePath());
            var sql = "INSERT INTO JournalEntryCredit VALUES(NULL, @ACCOUNTNUM, @AMOUNT, @JE)";
            connection.Open();
            
            var command = new SqliteCommand(sql, connection);
            command.Parameters.AddWithValue("@ACCOUNTNUM", accountNum);
            command.Parameters.AddWithValue("@AMOUNT", amount);
            command.Parameters.AddWithValue("@JE", journalEntryID);

            command.ExecuteNonQuery();
            connection.Close();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            //throw some error here
        }

        return true;
    }
    //view entries created by the manager or other accountants
    
    //view status of all journal entries by status (pending, approved, or rejected)
    public static List<string> ViewEntriesByStatus(char status)
    {
        List<string> entries = new List<string>();
        using var connection = new SqliteConnection($"Data Source=" + Database.GetDatabasePath());
        var sql = "";

        switch (status)
        {
            case ('P'):
                sql = "SELECT t1.ID, t1.Date, Debit.AccountNumber, Debit.Amount, Credit.AccountNumber, Credit.Amount, t1.Status, t1.Comment, t1.Reference, User.Username, Account.Name FROM JournalEntry as t1 INNER JOIN JournalEntryDebit as Debit on t1.ID = Debit.JournalEntryID INNER JOIN JournalEntryCredit as Credit ON Debit.JournalEntryID = Credit.JournalEntryID INNER JOIN User ON t1.UserID = User.ID INNER JOIN Account ON Debit.AccountNumber = Account.Number INNER JOIN Account ON Credit.AccountNumber WHERE t1.Status = 'P'";
                break;
            case('A'):
                sql = "SELECT t1.ID, t1.Date, Debit.AccountNumber, Debit.Amount, Credit.AccountNumber, Credit.Amount, t1.Status, t1.Comment, t1.Reference, User.Username FROM JournalEntry as t1 INNER JOIN JournalEntryDebit as Debit on t1.ID = Debit.JournalEntryID INNER JOIN JournalEntryCredit as Credit ON Debit.JournalEntryID = Credit.JournalEntryID INNER JOIN User ON t1.UserID = User.ID WHERE t1.Status = 'A'";
                break;
            case('R'):
                sql = "SELECT t1.ID, t1.Date, Debit.AccountNumber, Debit.Amount, Credit.AccountNumber, Credit.Amount, t1.Status, t1.Comment, t1.Reference, User.Username FROM JournalEntry as t1 INNER JOIN JournalEntryDebit as Debit on t1.ID = Debit.JournalEntryID INNER JOIN JournalEntryCredit as Credit ON Debit.JournalEntryID = Credit.JournalEntryID INNER JOIN User ON t1.UserID = User.ID WHERE t1.Status = 'R'";
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
                    for (int i = 0; i < 10; i++)
                    {
                        entries.Add(reader.GetString(i));
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

        return entries;
    }
    //filter journal entries displayed by status and by date
    
    //search a journal by account name, amount, or date
    public static List<string> SearchJournal(Object input)
    {
        List<string> result = new List<string>();
        var sql = "";

        if (input is string)
        {
            if (input.Equals("Name"))
            {
                //NOT DONE
                sql = "SELECT t1.ID, t1.Date, Debit.AccountNumber, Debit.Amount, Credit.AccountNumber, Credit.Amount, t1.Status, t1.Comment, t1.Reference, User.Username FROM JournalEntry as t1 INNER JOIN JournalEntryDebit as Debit on t1.ID = Debit.JournalEntryID INNER JOIN JournalEntryCredit as Credit ON Debit.JournalEntryID = Credit.JournalEntryID INNER JOIN User ON t1.UserID = User.ID WHERE Debit.";
            }
            else if (input.Equals("Date"))
            {
                sql = "";
            }
        }

        return result;
    }
    
    //view event logs for each account in the chart of accounts (DONE) (NOT TESTED)
    public static List<string> GetAccountEventLog(int accountNumber)
    {
        List<string> accountEventLog = new List<string>();
        try
        {
            var sql = "SELECT * FROM AccountEventLog WHERE Number = @ACCOUNTNUM";
            using var connection = new SqliteConnection($"Data Source=" + Database.GetDatabasePath());
            connection.Open();
            var command = new SqliteCommand(sql, connection);
            command.Parameters.AddWithValue("@ACCOUNTNUM", accountNumber);

            using var reader = command.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    for (int i = 0; i < 19; i++)
                    {
                        accountEventLog.Add(reader.GetString(i));
                    }
                }
            }
            connection.Close();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw new UnableToRetrieveException("Unable to retrieve this account's event log");
        }

        return accountEventLog;
    }
}