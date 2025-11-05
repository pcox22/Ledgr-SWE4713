using System.Reflection.Metadata;
using Microsoft.Data.Sqlite;

namespace LedgrLogic;

public class Manager : User
{
    public Manager(string TempUsername, string TempPass, string TempEmail, int TempUserID, int TempEmployeeID, bool TempActive, bool TempNew) : base (TempUsername, TempPass, TempEmail, TempUserID, TempEmployeeID, TempActive, TempNew) {}
    
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
            var insertSql = "INSERT INTO JournalEntry VALUES(null, @DATE, @STATUS, @COMMENT, @REFERENCE, @USERID, 'R')";
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

            var selectSql = "SELECT ID FROM JournalEntry ORDER BY ID DESC LIMIT 1";

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
            var insertSql = "INSERT INTO JournalEntry(ID, Date, Status, Comment, UserID, Type) VALUES(null, @DATE, @STATUS, @COMMENT, @USERID, 'R')";
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

            var selectSql = "SELECT ID FROM JournalEntry ORDER BY ID DESC LIMIT 1";

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
            var insertSql = "INSERT INTO JournalEntry(ID, Date, Status, Reference, UserID, Type) VALUES(null, @DATE, @STATUS, @REFERENCE, @USERID, 'R')";
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

            var selectSql = "SELECT ID FROM JournalEntry ORDER BY ID DESC LIMIT 1";

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
                var insertSql = "INSERT INTO JournalEntry(ID, Date, Status, UserID, Type) VALUES(null, @DATE, @STATUS, @USERID,'R')";
                using var connection = new SqliteConnection($"Data Source=" + Database.GetDatabasePath());
                connection.Open();
            
                var insertCommand = new SqliteCommand(insertSql, connection);
                insertCommand.Parameters.AddWithValue("@DATE", date);
                insertCommand.Parameters.AddWithValue("@STATUS", 'P');
                insertCommand.Parameters.AddWithValue("@USERID", userID);

                insertCommand.ExecuteNonQuery();

                /*var selectSql =
                    "SELECT ID FROM JOURNALENTRY WHERE Date = @DATE AND Status = @STATUS AND Comment = @COMMENT AND Reference = @Reference";*/

                var selectSql = "SELECT ID FROM JournalEntry ORDER BY ID DESC LIMIT 1";

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
    
    public static bool CreateJournalEntryDetails(int accountNum, double amount, string debitCredit, int journalEntryID, string username)
    {
        int userID = GetUserFromUserName(username).Result.GetUserID();
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
            if (debitCredit.Equals("Debit"))
            {
                Account.UpdateAccountDebit(accountNum, amount, username);
            }
            else if (debitCredit.Equals("Credit"))
            {
                Account.UpdateAccountCredit(accountNum, amount, username);
            }
            else
            {
                throw new Exception("Debit or Credit must be selected");
            }
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
    
    //can approve or reject journal entries made by an accountant, if a journal entry is rejected the manager must explain why
    //(DONE) (NOT TESTED)
    public static bool ApproveJournalEntry(int journalEntryID)
    {
        var sql = "UPDATE JournalEntry SET Status = 'A' WHERE ID = @JOURNALENTRYID";
        try
        {
            using var connection = new SqliteConnection($"Data Source=" + Database.GetDatabasePath());
            connection.Open();

            var command = new SqliteCommand(sql, connection);
            command.Parameters.AddWithValue("@JOURNALENTRYID", journalEntryID);

            command.ExecuteNonQuery();
            
            //need to get info to update the account 
            var retrieveSql =
                "SELECT JED.AccountNumber, JED.Amount, JED.DebitCredit, User.Username FROM JournalEntryDetails AS JED INNER JOIN JournalEntry AS JE ON JED.JournalEntryID = JE.ID INNER JOIN User ON JE.UserID = User.Username WHERE ID = @ID";
            var retrieveCommand = new SqliteCommand(retrieveSql, connection);
            
            using var reader = command.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    int accountNum = reader.GetInt32(0);
                    double amount = reader.GetDouble(1);
                    string debitCredit = reader.GetString(2);
                    string username = reader.GetString(3);

                    if (debitCredit.Equals("Debit"))
                    {
                        Account.UpdateAccountDebit(accountNum, amount, username);
                    }
                    else if (debitCredit.Equals("Credit"))
                    {
                        Account.UpdateAccountCredit(accountNum, amount, username);
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

        return true;
    }
    public static bool RejectJournalEntry(int journalEntryID, string comment)
    {
        var sql = "UPDATE JournalEntry SET Status = 'R', Comment = @COMMENT WHERE ID = @JOURNALENTRYID";
        try
        {
            using var connection = new SqliteConnection($"Data Source=" + Database.GetDatabasePath());
            connection.Open();

            var command = new SqliteCommand(sql, connection);
            command.Parameters.AddWithValue("@JOURNALENTRYID", journalEntryID);
            command.Parameters.AddWithValue("@COMMENT", comment);

            command.ExecuteNonQuery();
            
            connection.Close();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }

        return true;
    }
    //view all journal entries by status (DONE) (NOT TESTED)
     public static List<string> ViewEntriesByStatus(char status)
    {
        List<string> entries = new List<string>();
        using var connection = new SqliteConnection($"Data Source=" + Database.GetDatabasePath());
        var sql = "";

        switch (status)
        {
            case ('P'):
                //sql = "SELECT t1.ID, t1.Date, Debit.AccountNumber, Debit.Amount, Credit.AccountNumber, Credit.Amount, t1.Status, t1.Comment, t1.Reference, User.Username, Account.Name FROM JournalEntry as t1 INNER JOIN JournalEntryDebit as Debit on t1.ID = Debit.JournalEntryID INNER JOIN JournalEntryCredit as Credit ON Debit.JournalEntryID = Credit.JournalEntryID INNER JOIN User ON t1.UserID = User.ID INNER JOIN Account ON Debit.AccountNumber = Account.Number INNER JOIN Account ON Credit.AccountNumber WHERE t1.Status = 'P'";
                sql =
                    "SELECT t1.ID, t1.Date, t3.Name, t2.DebitCredit, t2. Amount, t1.Status, t1.Comment, t1.Reference, t4.Username FROM JournalEntry as t1 INNER JOIN JournalEntryDetails as t2 on t1.ID = t2.JournalEntryID INNER JOIN Account AS t3 ON t2.AccountNumber = t3.Number INNER JOIN User AS t4 ON t1.UserID = t4.ID WHERE t1.status = 'P' AND t1.Type = 'R'";
                break;
            case('A'):
                //sql = "SELECT t1.ID, t1.Date, Debit.AccountNumber, Debit.Amount, Credit.AccountNumber, Credit.Amount, t1.Status, t1.Comment, t1.Reference, User.Username FROM JournalEntry as t1 INNER JOIN JournalEntryDebit as Debit on t1.ID = Debit.JournalEntryID INNER JOIN JournalEntryCredit as Credit ON Debit.JournalEntryID = Credit.JournalEntryID INNER JOIN User ON t1.UserID = User.ID WHERE t1.Status = 'A'";
                sql =
                    "SELECT t1.ID, t1.Date, t3.Name, t2.DebitCredit, t2. Amount, t1.Status, t1.Comment, t1.Reference, t4.Username FROM JournalEntry as t1 INNER JOIN JournalEntryDetails as t2 on t1.ID = t2.JournalEntryID INNER JOIN Account AS t3 ON t2.AccountNumber = t3.Number INNER JOIN User AS t4 ON t1.UserID = t4.ID WHERE t1.status = 'A' AND t1.Type = 'R'";
                break;
            case('R'):
                //sql = "SELECT t1.ID, t1.Date, Debit.AccountNumber, Debit.Amount, Credit.AccountNumber, Credit.Amount, t1.Status, t1.Comment, t1.Reference, User.Username FROM JournalEntry as t1 INNER JOIN JournalEntryDebit as Debit on t1.ID = Debit.JournalEntryID INNER JOIN JournalEntryCredit as Credit ON Debit.JournalEntryID = Credit.JournalEntryID INNER JOIN User ON t1.UserID = User.ID WHERE t1.Status = 'R'";
                sql =
                    "SELECT t1.ID, t1.Date, t3.Name, t2.DebitCredit, t2. Amount, t1.Status, t1.Comment, t1.Reference, t4.Username FROM JournalEntry as t1 INNER JOIN JournalEntryDetails as t2 on t1.ID = t2.JournalEntryID INNER JOIN Account AS t3 ON t2.AccountNumber = t3.Number INNER JOIN User AS t4 ON t1.UserID = t4.ID WHERE t1.status = 'R' AND t1.Type = 'R'";
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
            connection.Close();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }

        return entries;
    }
    
    //filter journal entries displayed by status and by date (DONE) (NOT TESTED)
    //returns journal entries ordered by status (P || A || R) and date (asc||desc)
    public static List<string> FilterJournalStatusDate(char status, string dateOrderBy)
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
            connection.Close();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }

        return results;
    }
    
    //search a journal by account name, amount, or date (DONE) (NOT TESTED)
    public static List<string> SearchJournal(string searchBy, Object input)
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
            connection.Close();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        return result;
    }
    
    //view event logs for each account in CoA (DONE) (NOT TESTED)
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
    
    //GetLedger returns all approved journal entries for a specific account, orders them by Date (asc)/ also gets acct info
    //To get Normal side for an account, you can use Account.GetAccountFromAccountNumber and get the 3rd element in the list
    //(DONE) (NOT TESTED)
    public static List<string> GetLedger(int accountNum)
    {
        List<string> ledger = new List<string>();
        try
        {
            var sql =
                "SELECT Acct.Name, Acct.Number, JE.Date, JE.Comment, JED.DebitCredit, JED.Amount FROM JournalEntry AS JE INNER JOIN JournalEntryDetails AS JED ON JE.ID = JED.JournalEntryID INNER JOIN Account AS Acct ON JED.AccountNumber = Acct.Number WHERE JE.Status = 'A' AND Acct.Number = @NUM ORDER BY JED.DebitCredit DESC, JE.Date ASC";
            using var connection = new SqliteConnection($"Data Source=" + Database.GetDatabasePath());
            connection.Open();
            
            var command = new SqliteCommand(sql, connection);
            command.Parameters.AddWithValue("@NUM", accountNum);
            using var reader = command.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    for (int i = 0; i < 6; i++)
                    {
                        if (!reader.IsDBNull(i))
                        {
                            ledger.Add(reader.GetString(i));
                        }
                        else
                        {
                            ledger.Add("");
                        }
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

        return ledger;
    }
    
    public static List<string> GetLedgerByDateRange(string toDate, string fromDate, int accountNum)
    {
        List<string> ledger = new List<string>();
        try
        {
            var sql =
                "SELECT Acct.Name, Acct.Number, JE.Date, JE.Comment, JED.DebitCredit, JED.Amount FROM JournalEntry AS JE INNER JOIN JournalEntryDetails AS JED ON JE.ID = JED.JournalEntryID INNER JOIN Account AS Acct ON JED.AccountNumber = Acct.Number WHERE JE.Status = 'A' AND Acct.Number = @ACCOUNT AND JE.Date BETWEEN @FIRST AND @LAST BY JED.DebitCredit DESC, JE.Date ASC";
            using var connection = new SqliteConnection($"Data Source=" + Database.GetDatabasePath());
            connection.Open();
            
            var command = new SqliteCommand(sql, connection);
            command.Parameters.AddWithValue("@ACCOUNT", accountNum);
            command.Parameters.AddWithValue("@FIRST", fromDate);
            command.Parameters.AddWithValue("@LAST", toDate);
            
            using var reader = command.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    for (int i = 0; i < 6; i++)
                    {
                        if (!reader.IsDBNull(i))
                        {
                            ledger.Add(reader.GetString(i));
                        }
                        else
                        {
                            ledger.Add("");
                        }
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

        return ledger;
    }
    
    //Ledger must allow filtering and search features
    
    //Adjusting journal entries
     //Create adjusting journal entries (Done) (Not tested)
    //Because a journal entry can contain any number of debits or credits, creating a journal entry will be broken up into multiple methods
    
    //CreateJournalEntry() creates a new entry in the JournalEntry table, and returns the ID of that new entry
    //Comments and reference docs are optional, so there will be multiple methods but with different paramaters
    public static int CreateAdjustingJournalEntry(string date, string comment, Blob reference, string username)
    { 
        int userID = GetUserFromUserName(username).Result.GetUserID();
        int journalEntryID = -1;
        try
        {
            var insertSql = "INSERT INTO JournalEntry(ID, Date, Status, Comment, UserID, Type) VALUES(null, @DATE, @STATUS, @COMMENT, @USERID, 'A')";
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

            var selectSql = "SELECT ID FROM JournalEntry ORDER BY ID DESC LIMIT 1";

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
    public static int CreateAdjustingJournalEntry(string date, string comment, string username)
    {
        int userID = GetUserFromUserName(username).Result.GetUserID();
        int journalEntryID = -1;
        try
        {
            var insertSql = "INSERT INTO JournalEntry(ID, Date, Status, Comment, UserID, Type) VALUES(null, @DATE, @STATUS, @COMMENT, @USERID, 'A')";
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

            var selectSql = "SELECT ID FROM JournalEntry ORDER BY ID DESC LIMIT 1";

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

    public static int CreateAdjustingJournalEntry(string date, Blob reference, string username)
    {
        int userID = GetUserFromUserName(username).Result.GetUserID();
        int journalEntryID = -1;
        try
        {
            var insertSql = "INSERT INTO JournalEntry(ID, Date, Status, Reference, UserID, Type) VALUES(null, @DATE, @STATUS, @REFERENCE, @USERID, 'A')";
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

            var selectSql = "SELECT ID FROM JournalEntry ORDER BY ID DESC LIMIT 1";

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

    public static int CreateAdjustingJournalEntry(string date, string username)
    {
        int userID = GetUserFromUserName(username).Result.GetUserID();
            int journalEntryID = -1;
            try
            {
                var insertSql = "INSERT INTO JournalEntry(ID, Date, Status, UserID, Type) VALUES(null, @DATE, @STATUS, @USERID,'A')";
                using var connection = new SqliteConnection($"Data Source=" + Database.GetDatabasePath());
                connection.Open();
            
                var insertCommand = new SqliteCommand(insertSql, connection);
                insertCommand.Parameters.AddWithValue("@DATE", date);
                insertCommand.Parameters.AddWithValue("@STATUS", 'P');
                insertCommand.Parameters.AddWithValue("@USERID", userID);

                insertCommand.ExecuteNonQuery();

                /*var selectSql =
                    "SELECT ID FROM JOURNALENTRY WHERE Date = @DATE AND Status = @STATUS AND Comment = @COMMENT AND Reference = @Reference";*/

                var selectSql = "SELECT ID FROM JournalEntry ORDER BY ID DESC LIMIT 1";

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
    
    
     public static List<string> ViewAdjustingEntriesByStatus(char status)
    {
        List<string> entries = new List<string>();
        using var connection = new SqliteConnection($"Data Source=" + Database.GetDatabasePath());
        var sql = "";

        switch (status)
        {
            case ('P'):
                //sql = "SELECT t1.ID, t1.Date, Debit.AccountNumber, Debit.Amount, Credit.AccountNumber, Credit.Amount, t1.Status, t1.Comment, t1.Reference, User.Username, Account.Name FROM JournalEntry as t1 INNER JOIN JournalEntryDebit as Debit on t1.ID = Debit.JournalEntryID INNER JOIN JournalEntryCredit as Credit ON Debit.JournalEntryID = Credit.JournalEntryID INNER JOIN User ON t1.UserID = User.ID INNER JOIN Account ON Debit.AccountNumber = Account.Number INNER JOIN Account ON Credit.AccountNumber WHERE t1.Status = 'P'";
                sql =
                    "SELECT t1.ID, t1.Date, t3.Name, t2.DebitCredit, t2. Amount, t1.Status, t1.Comment, t1.Reference, t4.Username FROM JournalEntry as t1 INNER JOIN JournalEntryDetails as t2 on t1.ID = t2.JournalEntryID INNER JOIN Account AS t3 ON t2.AccountNumber = t3.Number INNER JOIN User AS t4 ON t1.UserID = t4.ID WHERE t1.status = 'P' AND t1.Type = 'A'";
                break;
            case('A'):
                //sql = "SELECT t1.ID, t1.Date, Debit.AccountNumber, Debit.Amount, Credit.AccountNumber, Credit.Amount, t1.Status, t1.Comment, t1.Reference, User.Username FROM JournalEntry as t1 INNER JOIN JournalEntryDebit as Debit on t1.ID = Debit.JournalEntryID INNER JOIN JournalEntryCredit as Credit ON Debit.JournalEntryID = Credit.JournalEntryID INNER JOIN User ON t1.UserID = User.ID WHERE t1.Status = 'A'";
                sql =
                    "SELECT t1.ID, t1.Date, t3.Name, t2.DebitCredit, t2. Amount, t1.Status, t1.Comment, t1.Reference, t4.Username FROM AdjustingJournalEntry as t1 INNER JOIN AdjustingJournalEntryDetails as t2 on t1.ID = t2.JournalEntryID INNER JOIN Account AS t3 ON t2.AccountNumber = t3.Number INNER JOIN User AS t4 ON t1.UserID = t4.ID WHERE t1.status = 'A' AND t1.Type = 'A'";
                break;
            case('R'):
                //sql = "SELECT t1.ID, t1.Date, Debit.AccountNumber, Debit.Amount, Credit.AccountNumber, Credit.Amount, t1.Status, t1.Comment, t1.Reference, User.Username FROM JournalEntry as t1 INNER JOIN JournalEntryDebit as Debit on t1.ID = Debit.JournalEntryID INNER JOIN JournalEntryCredit as Credit ON Debit.JournalEntryID = Credit.JournalEntryID INNER JOIN User ON t1.UserID = User.ID WHERE t1.Status = 'R'";
                sql =
                    "SELECT t1.ID, t1.Date, t3.Name, t2.DebitCredit, t2. Amount, t1.Status, t1.Comment, t1.Reference, t4.Username FROM AdjustingJournalEntry as t1 INNER JOIN AdjustingJournalEntryDetails as t2 on t1.ID = t2.JournalEntryID INNER JOIN Account AS t3 ON t2.AccountNumber = t3.Number INNER JOIN User AS t4 ON t1.UserID = t4.ID WHERE t1.status = 'R' AND t1.Type = 'A'";
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
            connection.Close();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }

        return entries;
    }
    
    //Financial Statements
    public static List<string> GetIncomeStatement(string fromDate, string toDate)
    {
        List<string> incomeStatement = new List<string>();
        List<string> relevantEntries = new List<string>();
        try
        {
            //var sql = "Select Acct.Name, JED.Amount, JED.DebitCredit, Acct.NormalSide FROM JournalEntry AS JE INNER JOIN JournalEntryDetails AS JED ON JE.ID = JED.JournalEntryID INNER JOIN Account AS Acct ON JED.AccountNumber = Acct.Number WHERE Acct.Category = 'Revenue'  AND JE.Date BETWEEN '2025-10-01' AND '2025-11-03' OR Acct.Category = 'Expense'  AND JE.Date BETWEEN @START AND @LAST ORDER BY Acct.Category DESC, Acct.\"Order\" ASC";
            var sql =
                "SELECT Acct.Name, Acct.Number, Acct.NormalSide FROM JournalEntry AS JE INNER JOIN JournalEntryDetails AS JED ON JE.ID = JED.JournalEntryID INNER JOIN Account AS Acct ON JED.AccountNumber = Acct.Number WHERE Acct.Name = 'Revenue' AND JE.Date BETWEEN @FIRST AND @LAST OR Acct.Name = 'Expense' AND JE.Date BETWEEN @FIRST AND @LAST ORDER BY Acct.\"Order\" ASC";
            using var connection = new SqliteConnection($"Data Source=" + Database.GetDatabasePath());
            connection.Open();
            
            var command = new SqliteCommand(sql, connection);
            command.Parameters.AddWithValue("@START", fromDate);
            command.Parameters.AddWithValue("@LAST", toDate);

            command.ExecuteNonQuery();


            using var reader = command.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                        incomeStatement.Add(reader.GetString(0));
                        char normalSide = reader.GetChar(2);
                        relevantEntries = GetLedgerByDateRange(toDate, fromDate, reader.GetInt32(1));
                        List<string> temp = new List<string>();
                        
                        //creating a list containing only DebitCredit and Amount to get the total
                        for (int j = 0; j < relevantEntries.Count / 6; j++)
                        {
                            for (int k = 4; k < 6; k++)
                            {
                                temp.Add(relevantEntries[k]);
                            }
                        }

                        double balance = GetAccountBalance(temp, normalSide);
                        incomeStatement.Add("" + balance);
                }
            }
            connection.Close();
            
            
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }

        return incomeStatement;
    }

    //Takes in a list of entries for a single account, list contains Amount and DebitCredit
    public static double GetAccountBalance(List<string> entries, char normalSide)
    {
        double balance = 0.00;
        
            for (int i = 0; i < ((entries.Count / 2) - 1); i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    if (normalSide == 'R')
                    {
                        if (entries[j].Equals("Debit"))
                        {
                            j++;
                            balance -= double.Parse(entries[j]);
                        }
                        else
                        {
                            j++;
                            balance += double.Parse(entries[j]);
                        }
                    }
                    else
                    {
                        if (entries[j].Equals("Debit"))
                        {
                            j++;
                            balance -= double.Parse(entries[j]);
                        }
                        else
                        {
                            j++;
                            balance += double.Parse(entries[j]);
                        }
                    }
                }
            }
        return balance;
    }

    public static List<string> GetBalanceSheetCategory(string fromDate, string toDate, string category)
    {
        List<string> balanceSheet = new List<string>();
        var sql = "";
        switch (category)
        {
            case "Asset":
                sql =
                    "SELECT Acct.Name, Acct.Number, Acct.NormalSide FROM JournalEntry AS JE INNER JOIN JournalEntryDetails AS JED ON JE.ID = JED.JournalEntryID INNER JOIN Account AS Acct ON JED.AccountNumber = Acct.Number WHERE Acct.Name = 'Asset' AND JE.Date BETWEEN @FIRST AND @LAST ORDER BY Acct.\"Order\" ASC";
                break;
            case "Liability":
                sql =
                    "SELECT Acct.Name, Acct.Number, Acct.NormalSide FROM JournalEntry AS JE INNER JOIN JournalEntryDetails AS JED ON JE.ID = JED.JournalEntryID INNER JOIN Account AS Acct ON JED.AccountNumber = Acct.Number WHERE Acct.Name = 'Liability' AND JE.Date BETWEEN @FIRST AND @LAST ORDER BY Acct.\"Order\" ASC";
                break;
            case "Equity":
                sql =
                    "SELECT Acct.Name, Acct.Number, Acct.NormalSide FROM JournalEntry AS JE INNER JOIN JournalEntryDetails AS JED ON JE.ID = JED.JournalEntryID INNER JOIN Account AS Acct ON JED.AccountNumber = Acct.Number WHERE Acct.Name = 'Capital' AND JE.Date BETWEEN @FIRST AND @LAST ORDER BY Acct.\"Order\" ASC";
                break;
        }
        
        try
        {
            using var connection = new SqliteConnection($"Data Source=" + Database.GetDatabasePath());
            connection.Open();

            var command = new SqliteCommand(sql, connection);
            command.Parameters.AddWithValue("@START", fromDate);
            command.Parameters.AddWithValue("@LAST", toDate);

            command.ExecuteNonQuery();


            using var reader = command.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    List<string> returnedEntries = new List<string>();
                    balanceSheet.Add(reader.GetString(0));
                    char normalSide = reader.GetChar(2);
                    List<string> relevantEntries = GetLedgerByDateRange(toDate, fromDate, reader.GetInt32(1));
                    List<string> temp = new List<string>();

                    //creating a list containing only DebitCredit and Amount to get the total
                    for (int j = 0; j < relevantEntries.Count / 6; j++)
                    {
                        for (int k = 4; k < 6; k++)
                        {
                            temp.Add(relevantEntries[k]);
                        }
                    }

                    double balance = GetAccountBalance(temp, normalSide);
                    balanceSheet.Add("" + balance);
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }

        return balanceSheet;
    }

    public static List<string> GetBalanceSheet(string fromDate, string toDate)
    {
        List<string> balanceSheet = new List<string>();
        try
        {
            //getting assets first
            List<string> temp = new List<string>();
            temp = GetBalanceSheetCategory(fromDate, toDate, "Asset");
            //putting the assets into the balance sheet
            for (int i = 0; i < temp.Count; i++)
            {
                balanceSheet.Add(temp[i]); 
            }
            
            //getting liabilities next
            temp = GetBalanceSheetCategory(fromDate, toDate, "Liability");
            //putting the liabilities into the balance sheet
            for (int i = 0; i < temp.Count; i++)
            {
                balanceSheet.Add(temp[i]); 
            }
            
            //getting equity last
            temp = GetBalanceSheetCategory(fromDate, toDate, "Equity");
            //putting the equity into the balance sheet
            for (int i = 0; i < temp.Count; i++)
            {
                balanceSheet.Add(temp[i]); 
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }

        return balanceSheet;
    }
}