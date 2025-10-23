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
    public static async Task<int> CreateJournalEntry(string date, string comment, Blob reference, string username)
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
            await connection.CloseAsync();
        }
        catch(Exception e)
        {
            Console.WriteLine(e);
            //throw some error here
        }

        return journalEntryID;
    }

    public static async Task<int> CreateJournalEntry(string date, string comment, string username)
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
            await connection.CloseAsync();
        }
        catch(Exception e)
        {
            Console.WriteLine(e);
            //throw some error here
        }

        return journalEntryID;
    }

    public static async Task<int> CreateJournalEntry(string date, Blob reference, string username)
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
            
            var selectSql = "SELECT ID FROM JournalEntry ORDER BY DESC LIMIT 1";

            var selectCommand = new SqliteCommand(selectSql, connection);


            using var reader = selectCommand.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    journalEntryID = reader.GetInt32(0);
                }
            }
            await connection.CloseAsync();
        }
        catch(Exception e)
        {
            Console.WriteLine(e);
            //throw some error here
        }

        return journalEntryID;
    }

    public static async Task<int> CreateJournalEntry(string date, string username)
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

                using var reader = selectCommand.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        journalEntryID = reader.GetInt32(0);
                    }
                }
                await connection.CloseAsync();
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
            }

            return journalEntryID;
    }

    public static async Task<bool> CreateJournalEntryDetails(int accountNum, double amount, string debitCredit, int journalEntryID)
    {
        try
        {
            using var connection = new SqliteConnection($"Data Source=" + Database.GetDatabasePath());
            var sql = "INSERT INTO JournalEntryDetails VALUES(NULL, @ACCOUNTNUM, @AMOUNT, @JE, @DC)";
            connection.Open();
            
            var command = new SqliteCommand(sql, connection);
            command.Parameters.AddWithValue("@ACCOUNTNUM", accountNum);
            command.Parameters.AddWithValue("@AMOUNT", amount);
            command.Parameters.AddWithValue("@JE", journalEntryID);
            command.Parameters.AddWithValue("@DC", debitCredit);

            command.ExecuteNonQuery();
            await connection.CloseAsync();
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
    public static async Task<List<string>> ViewEntriesByStatus(char status)
    {
        List<string> entries = new List<string>();
        using var connection = new SqliteConnection($"Data Source=" + Database.GetDatabasePath());
        var sql = "";

        switch (status)
        {
            case ('P'):
                //sql = "SELECT t1.ID, t1.Date, Debit.AccountNumber, Debit.Amount, Credit.AccountNumber, Credit.Amount, t1.Status, t1.Comment, t1.Reference, User.Username, Account.Name FROM JournalEntry as t1 INNER JOIN JournalEntryDebit as Debit on t1.ID = Debit.JournalEntryID INNER JOIN JournalEntryCredit as Credit ON Debit.JournalEntryID = Credit.JournalEntryID INNER JOIN User ON t1.UserID = User.ID INNER JOIN Account ON Debit.AccountNumber = Account.Number INNER JOIN Account ON Credit.AccountNumber WHERE t1.Status = 'P'";
                sql =
                    "SELECT t1.ID, t1.Date, t3.Name, t2.DebitCredit, t2. Amount, t1.Status, t1.Comment, t1.Reference, t4.Username FROM JournalEntry as t1 INNER JOIN JournalEntryDetails as t2 on t1.ID = t2.JournalEntryID INNER JOIN Account AS t3 ON t2.AccountNumber = t3.Number INNER JOIN User AS t4 ON t1.UserID = t4.ID WHERE t1.status = 'P'";
                break;
            case('A'):
                //sql = "SELECT t1.ID, t1.Date, Debit.AccountNumber, Debit.Amount, Credit.AccountNumber, Credit.Amount, t1.Status, t1.Comment, t1.Reference, User.Username FROM JournalEntry as t1 INNER JOIN JournalEntryDebit as Debit on t1.ID = Debit.JournalEntryID INNER JOIN JournalEntryCredit as Credit ON Debit.JournalEntryID = Credit.JournalEntryID INNER JOIN User ON t1.UserID = User.ID WHERE t1.Status = 'A'";
                sql =
                    "SELECT t1.ID, t1.Date, t3.Name, t2.DebitCredit, t2. Amount, t1.Status, t1.Comment, t1.Reference, t4.Username FROM JournalEntry as t1 INNER JOIN JournalEntryDetails as t2 on t1.ID = t2.JournalEntryID INNER JOIN Account AS t3 ON t2.AccountNumber = t3.Number INNER JOIN User AS t4 ON t1.UserID = t4.ID WHERE t1.status = 'A'";
                break;
            case('R'):
                //sql = "SELECT t1.ID, t1.Date, Debit.AccountNumber, Debit.Amount, Credit.AccountNumber, Credit.Amount, t1.Status, t1.Comment, t1.Reference, User.Username FROM JournalEntry as t1 INNER JOIN JournalEntryDebit as Debit on t1.ID = Debit.JournalEntryID INNER JOIN JournalEntryCredit as Credit ON Debit.JournalEntryID = Credit.JournalEntryID INNER JOIN User ON t1.UserID = User.ID WHERE t1.Status = 'R'";
                sql =
                    "SELECT t1.ID, t1.Date, t3.Name, t2.DebitCredit, t2. Amount, t1.Status, t1.Comment, t1.Reference, t4.Username FROM JournalEntry as t1 INNER JOIN JournalEntryDetails as t2 on t1.ID = t2.JournalEntryID INNER JOIN Account AS t3 ON t2.AccountNumber = t3.Number INNER JOIN User AS t4 ON t1.UserID = t4.ID WHERE t1.status = 'R'";
                break;
            default:
                sql =
                    "SELECT t1.ID, t1.Date, t3.Name, t2.DebitCredit, t2. Amount, t1.Status, t1.Comment, t1.Reference, t4.Username FROM JournalEntry as t1 INNER JOIN JournalEntryDetails as t2 on t1.ID = t2.JournalEntryID INNER JOIN Account AS t3 ON t2.AccountNumber = t3.Number INNER JOIN User AS t4 ON t1.UserID = t4.ID WHERE t1.status = 'P'";
                break;
        }
        try
        {
            using var command = new SqliteCommand(sql, connection);
            connection.Open();
            using var reader = command.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    for (int i = 0; i < 9; i++)
                    {
                        if (!reader.IsDBNull(i))
                        {
                            entries.Add(reader.GetString(i));
                        }
                        else
                        {
                            entries.Add("");
                        }
                    }
                }
            }
            await connection.CloseAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            //throw;
        }

        return entries;
    }
    //filter journal entries displayed by status and by date (DONE) (NOT TESTED)
    //returns journal entries ordered by status (P || A || R) and date (asc||desc)
    public static async Task<List<string>> FilterJournalStatusDate(char status, string dateOrderBy)
    {
        List<string> results = new List<string>();
        var sql = "";
        if (status == 'A' && dateOrderBy.Equals("asc"))
        {
            sql = "SELECT t1.ID, t1.Date, t3.Name, t2.DebitCredit, t2. Amount, t1.Status, t1.Comment, t1.Reference, t4.Username FROM JournalEntry as t1 INNER JOIN JournalEntryDetails as t2 on t1.ID = t2.JournalEntryID INNER JOIN Account AS t3 ON t2.AccountNumber = t3.Number INNER JOIN User AS t4 ON t1.UserID = t4.ID WHERE Status = 'A' ORDER BY DATE ASC";
        }
        else if (status == 'A' && dateOrderBy.Equals("desc"))
        {
            sql = "SELECT t1.ID, t1.Date, t3.Name, t2.DebitCredit, t2. Amount, t1.Status, t1.Comment, t1.Reference, t4.Username FROM JournalEntry as t1 INNER JOIN JournalEntryDetails as t2 on t1.ID = t2.JournalEntryID INNER JOIN Account AS t3 ON t2.AccountNumber = t3.Number INNER JOIN User AS t4 ON t1.UserID = t4.ID WHERE Status = 'A' ORDER BY DATE DESC";
        }
        else if (status == 'P' && dateOrderBy.Equals("asc"))
        {
            sql = "SELECT t1.ID, t1.Date, t3.Name, t2.DebitCredit, t2. Amount, t1.Status, t1.Comment, t1.Reference, t4.Username FROM JournalEntry as t1 INNER JOIN JournalEntryDetails as t2 on t1.ID = t2.JournalEntryID INNER JOIN Account AS t3 ON t2.AccountNumber = t3.Number INNER JOIN User AS t4 ON t1.UserID = t4.ID WHERE Status = 'P' ORDER BY DATE ASC";
        }
        else if (status == 'P' && dateOrderBy.Equals("desc"))
        {
            sql = "SELECT t1.ID, t1.Date, t3.Name, t2.DebitCredit, t2. Amount, t1.Status, t1.Comment, t1.Reference, t4.Username FROM JournalEntry as t1 INNER JOIN JournalEntryDetails as t2 on t1.ID = t2.JournalEntryID INNER JOIN Account AS t3 ON t2.AccountNumber = t3.Number INNER JOIN User AS t4 ON t1.UserID = t4.ID WHERE Status = 'P' ORDER BY DATE DESC";
        }
        else if (status == 'R' && dateOrderBy.Equals("asc"))
        {
            sql = "SELECT t1.ID, t1.Date, t3.Name, t2.DebitCredit, t2. Amount, t1.Status, t1.Comment, t1.Reference, t4.Username FROM JournalEntry as t1 INNER JOIN JournalEntryDetails as t2 on t1.ID = t2.JournalEntryID INNER JOIN Account AS t3 ON t2.AccountNumber = t3.Number INNER JOIN User AS t4 ON t1.UserID = t4.ID WHERE Status = 'P' ORDER BY DATE ASC";
        }
        else if (status == 'R' && dateOrderBy.Equals("desc"))
        {
            sql = "SELECT t1.ID, t1.Date, t3.Name, t2.DebitCredit, t2. Amount, t1.Status, t1.Comment, t1.Reference, t4.Username FROM JournalEntry as t1 INNER JOIN JournalEntryDetails as t2 on t1.ID = t2.JournalEntryID INNER JOIN Account AS t3 ON t2.AccountNumber = t3.Number INNER JOIN User AS t4 ON t1.UserID = t4.ID WHERE Status = 'P' ORDER BY DATE DESC";
        }
        try
        {
            using var connection = new SqliteConnection();
            var command = new SqliteCommand(sql, connection);
            connection.Open();
            using var reader = command.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    for (int i = 0; i < 10; i++)
                    {
                        if (!reader.IsDBNull(i))
                        {
                            results.Add(reader.GetString(i));
                        }
                        else
                        {
                            results.Add("");
                        }
                    }
                }
            }
            await connection.CloseAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }

        return results;
    }
    
    //search a journal by account name, amount, or date (DONE) (NOT TESTED)
    public static async Task<List<string>> SearchJournal(string searchBy, Object input)
    {
        List<string> result = new List<string>();
        var sql = "";
        switch(searchBy){
            case "Name":
                sql =
                    "SELECT t1.ID, t1.Date, t3.Name, t2.DebitCredit, t2. Amount, t1.Status, t1.Comment, t1.Reference, t4.Username FROM JournalEntry as t1 INNER JOIN JournalEntryDetails as t2 on t1.ID = t2.JournalEntryID INNER JOIN Account AS t3 ON t2.AccountNumber = t3.Number INNER JOIN User AS t4 ON t1.UserID = t4.ID WHERE t3.Name = @INPUT";
                break;
            case "Amount":
                sql =
                    "SELECT t1.ID, t1.Date, t3.Name, t2.DebitCredit, t2. Amount, t1.Status, t1.Comment, t1.Reference, t4.Username FROM JournalEntry as t1 INNER JOIN JournalEntryDetails as t2 on t1.ID = t2.JournalEntryID INNER JOIN Account AS t3 ON t2.AccountNumber = t3.Number INNER JOIN User AS t4 ON t1.UserID = t4.ID WHERE t2.Amount = @INPUT";
                break;
            case "Date":
                sql =
                    "SELECT t1.ID, t1.Date, t3.Name, t2.DebitCredit, t2. Amount, t1.Status, t1.Comment, t1.Reference, t4.Username FROM JournalEntry as t1 INNER JOIN JournalEntryDetails as t2 on t1.ID = t2.JournalEntryID INNER JOIN Account AS t3 ON t2.AccountNumber = t3.Number INNER JOIN User AS t4 ON t1.UserID = t4.ID WHERE t1.Date = @INPUT";
                break;
        }

        try
        {
            using var connection = new SqliteConnection($"Data Source=" + Database.GetDatabasePath());
            connection.Open();

            var command = new SqliteCommand(sql, connection);

            using var reader = command.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    for (int i = 0; i < 9; i++)
                    {
                        if (!reader.IsDBNull(i))
                        {
                            result.Add(reader.GetString(i));
                        }
                        else
                        {
                            result.Add("");
                        }
                    }
                }
            }
            await connection.CloseAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        return result;
    }
    
    //view event logs for each account in the chart of accounts (DONE) (NOT TESTED)
    public static async Task<List<string>> GetAccountEventLog(int accountNumber)
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
            await connection.CloseAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw new UnableToRetrieveException("Unable to retrieve this account's event log");
        }

        return accountEventLog;
    }

    //GetLedger returns all approved journal entries for a specific account, orders them by Date (asc)/ also gets acct info
    //To get Normal side for an account, you can use Account.GetAccountFromAccountNumber and get the 3 element in the list
    //(DONE) (NOT TESTED)
    public static async Task<List<string>> GetLedger()
    {
        List<string> ledger = new List<string>();
        try
        {
            var sql =
                "SELECT Acct.Name, Acct.Number, JE.Date, JE.Comment, JED.DebitCredit, JED.Amount FROM JournalEntry AS JE INNER JOIN JournalEntryDetails AS JED ON JE.ID = JED.JournalEntryID INNER JOIN Account AS Acct ON JED.AccountNumber = Acct.Number WHERE JE.Status = 'A' ORDER BY JED.DebitCredit DESC, JE.Date ASC";
            using var connection = new SqliteConnection($"Data Source=" + Database.GetDatabasePath());
            connection.Open();
            
            var command = new SqliteCommand(sql, connection);
            using var reader = command.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    for (int i = 0; i < 6; i++)
                    {
                        ledger.Add(reader.GetString(i));
                    }
                }
            }
            await connection.CloseAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }

        return ledger;
    }
}