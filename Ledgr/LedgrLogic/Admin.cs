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
            /*using var connection = new SqliteConnection($"Data Source=" + Database.GetDatabasePath());
            connection.Open();*/
            
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
            Console.WriteLine("Fuck");
            Successful = false;
        }
        //If the employee was successfully created, call CreateUser() and pass the EmployeeID to it to create a linked User
    }
    public void CreateUser(int TempUserID,string TempUsername, string TempPassword, int TempEmployeeID)
    {
        //Calls database to create a User, linked to an Employee
    }
    
    //Needs access to database, should be bool for user feedback
    public void ActivateUser()
    {
        //Changes the IsActive attribute on a user type to true, updates the database
    }
    //Needs access to database, should be bool for user feedback
    public void DeactivateUser()
    {
        //Changes the IsActive attribute on a user type to false, updates the database
    }
    
}