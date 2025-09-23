using Microsoft.Data.Sqlite;

namespace LedgrLogic;

public class Admin : User
{
    //Needs access to database, should be bool for user feedback
    public void CreateEmployee(int TempID, string TempFirst, string TempLast, bool Admin, bool Manager)
    {
        int TempAdmin;
        int TempManager;
        bool Successful = true;
        //Turning bool into integer as SQL cannot store a bool, only 0 or 1
        if (Admin)
        {
            TempAdmin = 1;
        }
        else
        {
            TempAdmin = 0;
        }

        if (Manager)
        {
            TempManager = 1;
        }
        else
        {
            TempManager = 0;
        }

        var sql = "INSERT INTO Employee (ID, FirstName, LastName, IsAdmin, IsManager)" +
                  "VALUES (@TEMPID, @TEMPFIRST, @TEMPLAST, @TEMPADMIN, @TEMPMANAGER)";
        try
        {
            using var connection = new SqliteConnection($"Data Source="+Database.GetDatabasePath());
            connection.Open();

            using var command = new SqliteCommand(sql, connection);
            command.Parameters.AddWithValue("@TEMPID", TempID);
            command.Parameters.AddWithValue("@TEMPFIRST", TempFirst);
            command.Parameters.AddWithValue("@TEMPLAST", TempLast);
            command.Parameters.AddWithValue("@TEMPADMIN", TempAdmin);
            command.Parameters.AddWithValue("@TEMPMANAGER", TempManager);

            command.ExecuteNonQuery();
            connection.Close();
        }
        catch (Exception e)
        {
            Successful = false;
        }
        //If the employee was successfully created, call CreateUser() and pass the EmployeeID to it to create a linked User
    }

    public string GenerateUsername(string TempFirst, string TempLast)
    {
        //Adds first letter of firstname to lastname
        string Username = TempFirst.ToCharArray()[0] + TempLast;
        string Today = DateTime.Now.ToString("yy-MM-dd");
        
        //Adding just the month and day to the username (MM DD)
        Username += "" +Today.ToCharArray()[3] + "" +Today.ToCharArray()[4] + "" +Today.ToCharArray()[6] + ""+ Today.ToCharArray()[7] + "";

        return Username;
    }
    public void CreateUser(int TempUserID,string TempUsername, string TempPassword, int TempEmployeeID)
    {
        //Calls database to create a User, linked to an Employee
    }
    
    //Needs access to database, should be bool for user feedback
    public bool ActivateUser(int TempUserID)
    {
        //Changes the IsActive attribute on a user type to true, updates the database
        //Deletes entry from SuspendedUser Table
        var UserSQL = "UPDATE User" +
                      "IsActive = 1" +
                      "WHERE ID = (@ID)";
        var SuspendedUserSQL = "DELETE FROM SUSPENDEDUSER" +
                               "WHERE UserID = @ID";
        bool Successful = false;
        try
        {
            using var connection = new SqliteConnection($"Data Source=" + Database.GetDatabasePath());
            connection.Open();

            using var UserTableCommand = new SqliteCommand(UserSQL, connection);
            UserTableCommand.Parameters.AddWithValue("@ID", TempUserID);

            using var SuspendedTableCommand = new SqliteCommand(SuspendedUserSQL, connection);
            SuspendedTableCommand.Parameters.AddWithValue("@ID", TempUserID);

            //Successful will only be true if no errors are thrown by the queries
            Successful = true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }

        return Successful;
    }
    //Dates given to this method should be formatted as a string (YYYY-MM-DD)
    public bool DeactivateUser(int TempUserID, string TempStartDate, string TempEndDate)
    {
        //Changes the IsActive attribute on a user type to false
        //Creates a new entry in SuspendedUser Table
        var UserSQL = "UPDATE User" +
                  "IsActive = 0" +
                  "WHERE ID = (@ID)";
        var SuspendedUserSQL = "INSERT INTO SuspendedUserVALUES" +
                               "(@TEMPID, @START, @END, @USERID)";
        bool Successful = false;
        try
        {
            using var connection = new SqliteConnection($"Data Source=" + Database.GetDatabasePath());
            connection.Open();

            using var UserTableCommand = new SqliteCommand(UserSQL, connection);
            UserTableCommand.Parameters.AddWithValue("@ID", TempUserID);

            using var SuspendedTableCommand = new SqliteCommand(SuspendedUserSQL, connection);
            SuspendedTableCommand.Parameters.AddWithValue("@USERID", TempUserID);
            SuspendedTableCommand.Parameters.AddWithValue("@START", TempStartDate);
            SuspendedTableCommand.Parameters.AddWithValue("@END", TempEndDate);

            UserTableCommand.ExecuteNonQuery();
            SuspendedTableCommand.ExecuteNonQuery();
            
            //Successful will only be true if no errors are thrown by the queries
            Successful = true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }

        return Successful;
    }

    public bool PromoteToManager(int TempEmployeeID)
    {
        bool Successful = false;
        var sql = "UPDATE Employee SET IsManager = 1 WHERE ID = @ID";
        try
        {
            using var connection = new SqliteConnection($"Data Source=" + Database.GetDatabasePath());
            connection.Open();

            using var command = new SqliteCommand(sql, connection);
            command.Parameters.AddWithValue("@ID", TempEmployeeID);

            command.ExecuteNonQuery();
            Successful = true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }

        return Successful;
    }
    
    public bool PromoteToAdmin(int TempEmployeeID)
    {
        bool Successful = false;
        var sql = "UPDATE Employee SET IsAdmin = 1 WHERE ID = @ID";
                  
        try
        {
            using var connection = new SqliteConnection($"Data Source=" + Database.GetDatabasePath());
            connection.Open();

            using var command = new SqliteCommand(sql, connection);
            command.Parameters.AddWithValue("@ID", TempEmployeeID);

            command.ExecuteNonQuery();
            Successful = true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }

        return Successful;
    }
    
    public bool DemoteFromAdmin(int TempEmployeeID)
    {
        bool Successful = false;
        var sql = "UPDATE Employee SET IsAdmin = 0 WHERE ID = @ID";
        try
        {
            using var connection = new SqliteConnection($"Data Source=" + Database.GetDatabasePath());
            connection.Open();

            using var command = new SqliteCommand(sql, connection);
            command.Parameters.AddWithValue("@ID", TempEmployeeID);

            command.ExecuteNonQuery();
            Successful = true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }

        return Successful;
    }
    
    public bool DemoteFromManager(int TempEmployeeID)
    {
        bool Successful = false;
        var sql = "UPDATE Employee SET IsManager = 0 WHERE ID = @ID";
        try
        {
            using var connection = new SqliteConnection($"Data Source=" + Database.GetDatabasePath());
            connection.Open();

            using var command = new SqliteCommand(sql, connection);
            command.Parameters.AddWithValue("@ID", TempEmployeeID);

            command.ExecuteNonQuery();
            Successful = true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }

        return Successful;
    }
    
}