using System.Collections;
using System.Data;
using Microsoft.Data.Sqlite;

namespace LedgrLogic;

public class Admin : User
{
    
    public Admin(string TempUsername, string TempPass, string TempEmail, int TempUserID, int TempEmployeeID, bool TempActive, bool TempNew) : base (TempUsername, TempPass, TempEmail, TempUserID, TempEmployeeID, TempActive, TempNew) {}

    public Admin()
    {
    }

    //Admin approving a new user
    //Needs to query potential user table, grab the info, and create a new employee and a new user
    /*public bool CreateEmployee(int TempID, string TempFirst, string TempLast, bool Admin, bool Manager, int TempUserID, string TempPassword, string TempEmail)
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

        var sql = "INSERT INTO Employee (FirstName, LastName, IsAdmin, IsManager)" +
                  "VALUES (@TEMPID, @TEMPFIRST, @TEMPLAST, @TEMPADMIN, @TEMPMANAGER)";
        try
        {
            using var connection = new SqliteConnection($"Data Source="+Database.GetDatabasePath());
            connection.Open();

            using var command = new SqliteCommand(sql, connection);
            command.Parameters.AddWithValue("@TEMPFIRST", TempFirst);
            command.Parameters.AddWithValue("@TEMPLAST", TempLast);
            command.Parameters.AddWithValue("@TEMPADMIN", TempAdmin);
            command.Parameters.AddWithValue("@TEMPMANAGER", TempManager);

            command.ExecuteNonQuery();
            connection.Close();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            Successful = false;
        }
        
        //If the employee was successfully created, call CreateUser() and pass the EmployeeID to it to create a linked User
        if (Successful)
        {
            //Reassign successful in case CreateUser() returns false
            Successful = CreateUser( GenerateUsername(TempFirst, TempLast), TempPassword, TempID, TempEmail);
        }

        return Successful;

    }*/

    public bool ApproveUser(int TempUserID)
    {
        bool Successful = true;

        try
        {
            var PotentialUserSQL = "SELECT * FROM PotentialUser WHERE ID = @ID";
            using var connection = new SqliteConnection($"Data Source=" + Database.GetDatabasePath());
            connection.Open();

            using var PotentialUserCommand = new SqliteCommand(PotentialUserSQL, connection);
            PotentialUserCommand.Parameters.AddWithValue("@ID", TempUserID);

            using var reader = PotentialUserCommand.ExecuteReader();
            string Username = "";
            string Password = "";
            string Email = "";
            int NewUser = -1;
            int IsActive = -1;
            string FirstName = "";
            string LastName = "";
            string DoB = "";
            string Address = "";
            int IsAdmin = -1;
            int IsManager = -1;
            int EmployeeID = -1;
            //assigning stored values to local variables for querying other tables
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    Username = reader.GetString(1);
                    Password = reader.GetString(2);
                    Email = reader.GetString(3);
                    NewUser = int.Parse(reader.GetString(4));
                    IsActive = int.Parse(reader.GetString(5));
                    FirstName = reader.GetString(6);
                    LastName = reader.GetString(7);
                    DoB = reader.GetString(8);
                    Address = reader.GetString(9);
                    IsAdmin = int.Parse(reader.GetString(10));
                    IsManager = int.Parse(reader.GetString(11));
                }
            }
            
            //Inserting new Employee row into Table
            var EmployeeSQL = "INSERT INTO Employee " +
                              "VALUES (@FIRSTNAME, @LASTNAME, @DOB, @ADDRESS, @ISADMIN, @ISMANAGER)";
            using var InsertEmployeeCommand = new SqliteCommand(EmployeeSQL, connection);
            InsertEmployeeCommand.Parameters.AddWithValue("@FIRSTNAME", FirstName);
            InsertEmployeeCommand.Parameters.AddWithValue("@LASTNAME", LastName);
            InsertEmployeeCommand.Parameters.AddWithValue("@DOB", DoB);
            InsertEmployeeCommand.Parameters.AddWithValue("@ADDRESS", Address);
            InsertEmployeeCommand.Parameters.AddWithValue("@ISADMIN", IsAdmin);
            InsertEmployeeCommand.Parameters.AddWithValue("@ISMANAGER", IsManager);
            InsertEmployeeCommand.ExecuteNonQuery();

            //creating a new user will require the EmployeeID Foreign key, which is not created until the employee is created
            var GetEmployeeIDSQL = "SELECT ID FROM Employee WHERE FirstName = @FIRSTNAME)";
            using var GetEmployeeIDCommand = new SqliteCommand(GetEmployeeIDSQL, connection);
            GetEmployeeIDCommand.Parameters.AddWithValue("@FIRSTNAME", FirstName);
            using var EmployeeIDReader = GetEmployeeIDCommand.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    EmployeeID = int.Parse(reader.GetString(0));
                }
            }
            
            //Creating a new row in the User table
            var UserSQL = "INSERT INTO User " +
                          "VALUES (@USERNAME, @PASSWORD, @EMAIL, @NEWUSER, @ISACTIVE, @EMPLOYEEID)";
            using var UserSQLCommand = new SqliteCommand(UserSQL, connection);
            UserSQLCommand.Parameters.AddWithValue("@USERNAME", Username);
            UserSQLCommand.Parameters.AddWithValue("@PASSWORD", Password);
            UserSQLCommand.Parameters.AddWithValue("@EMAIL", Email);
            UserSQLCommand.Parameters.AddWithValue("@NEWUSER", NewUser);
            UserSQLCommand.Parameters.AddWithValue("ISACTIVE", IsActive);
            UserSQLCommand.Parameters.AddWithValue("@EMPLOYEEID", EmployeeID);
            UserSQLCommand.ExecuteNonQuery();
            
            connection.Close();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            Successful = false;
        }
        
        return Successful;
    }
    
    /*public bool CreateUser(string TempUsername, string TempPassword, int TempEmployeeID, string TempEmail)
    {
        //Calls database to create a User, linked to an Employee
        bool Successful = true;
        if (LedgrLogic.Password.Validate(TempPassword).Equals("Success"))
        {
            //Encrypt the password before storing to database
            TempPassword = LedgrLogic.Password.Encrypt(TempPassword);
            string sql = "INSERT INTO User VALUES (@USERNAME, @PASSWORD, @EMAIL, @NEWUSER, @ISACTIVE, @EMPLOYEEID)";
            try
            {
                using var connection = new SqliteConnection($"Data Source=" + Database.GetDatabasePath());
                connection.Open();

                using var command = new SqliteCommand(sql, connection);
                command.Parameters.AddWithValue("@USERNAME", TempUsername);
                command.Parameters.AddWithValue("@PASSWORD", TempPassword);
                command.Parameters.AddWithValue("@EMAIL", TempEmail);
                command.Parameters.AddWithValue("@NEWUSER", 1);
                command.Parameters.AddWithValue("@ISACTIVE", 1);
                command.Parameters.AddWithValue("@EMPLOYEEID", TempEmployeeID);
                command.ExecuteNonQuery();
                
                connection.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Successful = false;
            }
        }

        return Successful;
    }*/
    
    //Needs access to database, should be bool for user feedback
    public bool ActivateUser(int TempUserID)
    {
        //Changes the IsActive attribute on a user type to true, updates the database
        //Deletes entry from SuspendedUser Table
        var UserSQL = "UPDATE User SET IsActive = 1 WHERE ID = @ID";
        var SuspendedUserSQL = "DELETE FROM SUSPENDEDUSER WHERE UserID = @ID";
        bool Successful = false;
        try
        {
            using var connection = new SqliteConnection($"Data Source=" + Database.GetDatabasePath());
            connection.Open();

            using var UserTableCommand = new SqliteCommand(UserSQL, connection);
            UserTableCommand.Parameters.AddWithValue("@ID", TempUserID);

            using var SuspendedTableCommand = new SqliteCommand(SuspendedUserSQL, connection);
            SuspendedTableCommand.Parameters.AddWithValue("@ID", TempUserID);
            
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
    //Dates given to this method should be formatted as a string (YYYY-MM-DD)
    public bool DeactivateUser(int TempUserID, string TempStartDate, string TempEndDate)
    {
        //Changes the IsActive attribute on a user type to false
        //Creates a new entry in SuspendedUser Table
        var UserSQL = "UPDATE User SET IsActive = 0 WHERE ID = @ID";
        var SuspendedUserSQL = "INSERT INTO SuspendedUser VALUES (1, @START, @END, @USERID)";
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

    public ArrayList UserReport()
    {
        ArrayList UserReport = new ArrayList();
        try
        {
            var PotentialUserSQL = "SELECT * FROM User INNER JOIN Employee on User.EmployeeID = Employee.ID";
            using var connection = new SqliteConnection($"Data Source=" + Database.GetDatabasePath());
            connection.Open();

            using var PotentialUserCommand = new SqliteCommand(PotentialUserSQL, connection);

            using var reader = PotentialUserCommand.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    for (int i = 0; i < UserReport.Count; i++)
                    {
                        UserReport.Add(reader.GetString(i));
                    }
                }
            }
        }
        catch (Exception e)
        {
            return null;
        }
        return UserReport;
    }

    public ArrayList ExpiredPasswordReport()
    {
        ArrayList ExpiredPasswordReport = new ArrayList();
        try
        {
            var ExpiredPasswordSQL = "SELECT * FROM ExpiredPassword";
            using var connection = new SqliteConnection($"Data Source=" + Database.GetDatabasePath());
            connection.Open();

            using var PotentialUserCommand = new SqliteCommand(ExpiredPasswordSQL, connection);

            using var reader = PotentialUserCommand.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    for (int i = 0; i < ExpiredPasswordReport.Count; i++)
                    {
                        ExpiredPasswordReport.Add(reader.GetString(i));
                    }
                }
            }
        }
        catch (Exception e)
        {
            return null;
        }
        return ExpiredPasswordReport;
    }

    //Admin updating employee/user info
    public bool UpdateEmployeeID(int currentEmployeeID, int newEmployeeID, int tempUserID)
    {
        bool Successful = true;
        try
        {
            var EmployeeSQL = "UPDATE Employee SET ID = @NEWID WHERE ID = @CURRENTEMPLOYEEID";
            using var connection = new SqliteConnection($"Data Source=" + Database.GetDatabasePath());
            connection.Open();

            using var EmployeeCommand = new SqliteCommand(EmployeeSQL, connection);
            EmployeeCommand.Parameters.AddWithValue("@NEWID", newEmployeeID);
            EmployeeCommand.Parameters.AddWithValue("@CURRENTEMPLOYEEID", currentEmployeeID);

            EmployeeCommand.ExecuteNonQuery();

            //Foreign key EmployeeID in User must also get updated
            var UserSQL = "UPDATE User Set EmployeeID = @NEWID WHERE ID = @USERID";
            using var UserCommand = new SqliteCommand(UserSQL, connection);
            UserCommand.Parameters.AddWithValue("@NEWID", newEmployeeID);
            UserCommand.Parameters.AddWithValue("@USERID", tempUserID);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return false;
        }

        return Successful;
    }
    
    public bool UpdateFirstName(int EmployeeID, string tempFirst)
    {
        bool Successful = true;
        try
        {
            var sql = "UPDATE Employee SET FirstName = @NEWFIRST WHERE ID = @EMPLOYEEID";
            using var connection = new SqliteConnection($"Data Source=" + Database.GetDatabasePath());
            connection.Open();

            using var command = new SqliteCommand(sql, connection);
            command.Parameters.AddWithValue("@NEWFIRST", tempFirst);
            command.Parameters.AddWithValue("@EMPLOYEEID", EmployeeID);

            command.ExecuteNonQuery();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return false;
        }

        return Successful;
    }
    
    public bool UpdateLastName(int EmployeeID, string tempLast)
    {
        bool Successful = true;
        try
        {
            var sql = "UPDATE Employee SET LastName = @NEWLAST WHERE ID = @EMPLOYEEID";
            using var connection = new SqliteConnection($"Data Source=" + Database.GetDatabasePath());
            connection.Open();

            using var command = new SqliteCommand(sql, connection);
            command.Parameters.AddWithValue("@NEWFIRST", tempLast);
            command.Parameters.AddWithValue("@EMPLOYEEID", EmployeeID);

            command.ExecuteNonQuery();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return false;
        }

        return Successful;
    }
    
    public bool UpdateDoB(int EmployeeID, string tempDoB)
    {
        bool Successful = true;
        try
        {
            var sql = "UPDATE Employee SET DoB = @NEWDOB WHERE ID = @EMPLOYEEID";
            using var connection = new SqliteConnection($"Data Source=" + Database.GetDatabasePath());
            connection.Open();

            using var command = new SqliteCommand(sql, connection);
            command.Parameters.AddWithValue("@NEWDOB", tempDoB);
            command.Parameters.AddWithValue("@EMPLOYEEID", EmployeeID);

            command.ExecuteNonQuery();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return false;
        }

        return Successful;
    }
    public bool UpdateAddress(int EmployeeID, string tempAddress)
    {
        bool Successful = true;
        try
        {
            var sql = "UPDATE Employee SET Address = @NEWADDRESS WHERE ID = @EMPLOYEEID";
            using var connection = new SqliteConnection($"Data Source=" + Database.GetDatabasePath());
            connection.Open();

            using var command = new SqliteCommand(sql, connection);
            command.Parameters.AddWithValue("@NEWADDRESS", tempAddress);
            command.Parameters.AddWithValue("@EMPLOYEEID", EmployeeID);

            command.ExecuteNonQuery();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return false;
        }

        return Successful;
    }
    
    public bool UpdateUserID(int currentUserID, int newUserID)
    {
        bool Successful = true;
        try
        {
            var sql = "UPDATE User SET ID = @NEWID WHERE ID = @USERID";
            using var connection = new SqliteConnection($"Data Source=" + Database.GetDatabasePath());
            connection.Open();

            using var command = new SqliteCommand(sql, connection);
            command.Parameters.AddWithValue("@NEWID", newUserID);
            command.Parameters.AddWithValue("@USERID", currentUserID);

            command.ExecuteNonQuery();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return false;
        }

        return Successful;
    }
    
    public bool UpdateUsername(int currentUserID, string tempUsername)
    {
        bool Successful = true;
        try
        {
            var sql = "UPDATE User SET Username = @USERNAME WHERE ID = @USERID";
            using var connection = new SqliteConnection($"Data Source=" + Database.GetDatabasePath());
            connection.Open();

            using var command = new SqliteCommand(sql, connection);
            command.Parameters.AddWithValue("@NEWUSERNAME", tempUsername);
            command.Parameters.AddWithValue("@USERID", currentUserID);

            command.ExecuteNonQuery();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return false;
        }

        return Successful;
    }
    
    public bool UpdateEmail(int currentUserID, string tempEmail)
    {
        bool Successful = true;
        try
        {
            var sql = "UPDATE User SET Email = @NEWEMAIL WHERE ID = @USERID";
            using var connection = new SqliteConnection($"Data Source=" + Database.GetDatabasePath());
            connection.Open();

            using var command = new SqliteCommand(sql, connection);
            command.Parameters.AddWithValue("@NEWEMAIL", tempEmail);
            command.Parameters.AddWithValue("@USERID", currentUserID);

            command.ExecuteNonQuery();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return false;
        }

        return Successful;
    }
    
    public bool UpdateNewUser(int currentUserID, int tempNew)
    {
        bool Successful = true;
        try
        {
            var sql = "UPDATE User SET NewUser = @NEWUSER WHERE ID = @USERID";
            using var connection = new SqliteConnection($"Data Source=" + Database.GetDatabasePath());
            connection.Open();

            using var command = new SqliteCommand(sql, connection);
            command.Parameters.AddWithValue("@NEWUSER", tempNew);
            command.Parameters.AddWithValue("@USERID", currentUserID);

            command.ExecuteNonQuery();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return false;
        }

        return Successful;
    }
}