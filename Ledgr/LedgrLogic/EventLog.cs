using System.Data;
using Microsoft.Data.Sqlite;

namespace LedgrLogic;

public static class EventLog
{
    //int responsibleUser refers to the userID of the person making the change in the system
    //gets all info from the Account table and then writes a new query to the event log
    //beforeAfter should either be "B" for before or "A" for After to indicate changes
    public static bool LogAccount(char beforeAfter,int accountNum, int responsibleUser)
    {
        try
        {
            int number = -1;
            string name = "";
            string desc = "";
            char normalSide = 'a';
            string category = "";
            string subCategory = "";
            double initialBalance = -1;
            double debit = -1;
            double credit = -1;
            double balance = -1;
            string dateCreated = "";
            int userID = -1;
            int order = -1;
            string statement = "";
            int active = -1;
            //Query the account table and get the information
            using var connection = new SqliteConnection($"Data Source=" + Database.GetDatabasePath());
            var accountSql = "SELECT * FROM Account WHERE Number = @NUMBER";
            var accountCommand = new SqliteCommand(accountSql, connection);
            accountCommand.Parameters.AddWithValue("@NUMBER", accountNum);
            connection.Open();
            using var reader = accountCommand.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    number = reader.GetInt32(0);
                    name = reader.GetString(1);
                    desc = reader.GetString(2);
                    normalSide = reader.GetChar(3);
                    category = reader.GetString(4);
                    subCategory = reader.GetString(5);
                    initialBalance = reader.GetDouble(6);
                    debit = reader.GetDouble(7);
                    credit = reader.GetDouble(8);
                    balance = reader.GetDouble(9);
                    dateCreated = reader.GetString(10);
                    userID = reader.GetInt32(11);
                    order = reader.GetInt32(12);
                    statement = reader.GetString(13);
                    active = reader.GetInt32(14);
                }
            }
            //if the value of number has not been reassigned
            if (number == -1)
            {
                throw new InvalidAccountNumberException("No such account exists with this account number");
            }
            
            //Getting current time and date for @TimeOfChange value in the table
            string dateTime = GetDateTime();
            
            var eventLogSql =
                "INSERT INTO AccountEventLog VALUES (NULL, @BEFOREAFTER, @NUMBER, @NAME, @DESC, @NORMALSIDE, @CATEGORY, @SUBCATEGORY, @INITIALBALANCE, @DEBIT, @CREDIT, @BALANCE, @DATECREATED, @USERID, @ORDER, @STATEMENT, @ACTIVE, @TIMEOFCHANGE, @RESPONSIBLEUSER)";
            var command = new SqliteCommand(eventLogSql, connection);
            //adding EVERY value :(
            command.Parameters.AddWithValue("@BEFOREAFTER", beforeAfter);
            command.Parameters.AddWithValue("@NUMBER", number);
            command.Parameters.AddWithValue("@NAME", name);
            command.Parameters.AddWithValue("@DESC", desc);
            command.Parameters.AddWithValue("@NORMALSIDE", normalSide);
            command.Parameters.AddWithValue("@CATEGORY", category);
            command.Parameters.AddWithValue("@SUBCATEGORY", subCategory);
            command.Parameters.AddWithValue("@INITIALBALANCE", initialBalance);
            command.Parameters.AddWithValue("@DEBIT", debit);
            command.Parameters.AddWithValue("@CREDIT", credit);
            command.Parameters.AddWithValue("@BALANCE", balance);
            command.Parameters.AddWithValue("@DATECREATED", dateCreated);
            command.Parameters.AddWithValue("@USERID", userID);
            command.Parameters.AddWithValue("@ORDER", order);
            command.Parameters.AddWithValue("@STATEMENT", statement);
            command.Parameters.AddWithValue("@ACTIVE", active);
            command.Parameters.AddWithValue("@TIMEOFCHANGE", dateTime);
            command.Parameters.AddWithValue("@RESPONSIBLEUSER", responsibleUser);

            command.ExecuteNonQuery();
            connection.Close();
        }
        catch (Exception e)
        {
            throw new EventLogException("Unable to update the event log");
        }
        return true;
    }

    public static string GetDateTime()
    {
        //Sqlite stores DateTime as YYYY-MM-DD HH:MM:SS
        string dateTime = DateTime.Now.ToString("u");
        dateTime = dateTime.Remove(20);
        return dateTime;
    }

    public static bool LogEmployee(char beforeAfter, int employeeID, int responsibleUser)
    {
        try
        {
            int id = -1;
            string firstName = "";
            string lastName = "";
            string dateOfBirth = "";
            string address = "";
            int isAdmin = -1;
            int isManager = -1;
            
            //Query the employee table and get the information
            using var connection = new SqliteConnection($"Data Source=" + Database.GetDatabasePath());
            var employeeSql = "SELECT * FROM Employee WHERE ID = @ID";
            var employeeCommand = new SqliteCommand(employeeSql, connection);
            employeeCommand.Parameters.AddWithValue("@ID", employeeID);
            connection.Open();
            using var reader = employeeCommand.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    id = reader.GetInt32(0);
                    firstName = reader.GetString(1);
                    lastName = reader.GetString(2);
                    dateOfBirth = reader.GetString(3);
                    address = reader.GetString(4);
                    isAdmin = reader.GetInt32(5);
                    isManager = reader.GetInt32(6);
                }
            }
            //if the value of id has not been reassigned
            if (id == -1)
            {
                throw new InvalidEmployeeIDException("No such employee exists with this ID");
            }
            
            //Getting current time and date for @TimeOfChange value in the table
            string dateTime = GetDateTime();

            var eventLogSql =
                "INSERT INTO EmployeeEventLog VALUES (NULL, @BEFOREAFTER, @ID, @FIRST, @LAST, @DOB, @ADDRESS, @ADMIN, @MANAGER, @TOC, @RESPONSIBLEUSER)";
            var command = new SqliteCommand(eventLogSql, connection);
            command.Parameters.AddWithValue("@ID", id);
            command.Parameters.AddWithValue("@BEFOREAFTER", beforeAfter);
            command.Parameters.AddWithValue("@FIRST", firstName);
            command.Parameters.AddWithValue("@LAST", lastName);
            command.Parameters.AddWithValue("@DOB", dateOfBirth);
            command.Parameters.AddWithValue("@ADDRESS", address);
            command.Parameters.AddWithValue("@ADMIN", isAdmin);
            command.Parameters.AddWithValue("@MANAGER", isManager);
            command.Parameters.AddWithValue("@TOC", dateTime);
            command.Parameters.AddWithValue("@RESPONSIBLEUSER", responsibleUser);
            
            command.ExecuteNonQuery();
            connection.Close();
        }
        catch (Exception e)
        {
            throw new EventLogException("Unable to update the event log");
        }
        return true;
    }
    
    public static bool LogUser(char beforeAfter, int userID, int responsibleUser)
    {
        try
        {
            int id = -1;
            string username = "";
            string password = "";
            string email = "";
            int newUser = -1;
            int isActive = -1;
            int employeeID = -1;
            
            //Query the user table and get the information
            using var connection = new SqliteConnection($"Data Source=" + Database.GetDatabasePath());
            var userSql = "SELECT * FROM User WHERE ID = @ID";
            var userCommand = new SqliteCommand(userSql, connection);
            userCommand.Parameters.AddWithValue("@ID", userID);
            connection.Open();
            using var reader = userCommand.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    id = reader.GetInt32(0);
                    username = reader.GetString(1);
                    password = reader.GetString(2);
                    email = reader.GetString(3);
                    newUser = reader.GetInt32(4);
                    isActive = reader.GetInt32(5);
                    employeeID = reader.GetInt32(6);
                }
            }
            if (id == -1)
            {
                Console.WriteLine("ID did not change");
                throw new InvalidUserIDException("Invalid User ID");
            }
            
            //Getting current time and date for @TimeOfChange value in the table
            string dateTime = GetDateTime();

            var eventLogSql =
                "INSERT INTO UserEventLog VALUES (NULL, @BEFOREAFTER,@USERID, @USERNAME, @PASSWORD, @EMAIL, @NEWUSER, @ISACTIVE, @EMPLOYEEID, @TOC, @RESPONSIBLEUSER)";
            var command = new SqliteCommand(eventLogSql, connection);
            command.Parameters.AddWithValue("@BEFOREAFTER", beforeAfter);
            command.Parameters.AddWithValue("@USERID", userID);
            command.Parameters.AddWithValue("@USERNAME", username);
            command.Parameters.AddWithValue("@PASSWORD", password);
            command.Parameters.AddWithValue("@EMAIL", email);
            command.Parameters.AddWithValue("@NEWUSER", newUser);
            command.Parameters.AddWithValue("@ISACTIVE", isActive);
            command.Parameters.AddWithValue("@EMPLOYEEID", employeeID);
            command.Parameters.AddWithValue("@TOC", dateTime);
            command.Parameters.AddWithValue("@RESPONSIBLEUSER", responsibleUser);
            
            command.ExecuteNonQuery();
            connection.Close();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw new EventLogException("Unable to update the event log");
        }
        return true;
    }
}