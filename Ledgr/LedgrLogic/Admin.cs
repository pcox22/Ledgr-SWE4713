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

    public static List<string> GetAllUsers()
    {
        List<string> users = new List<string>();
        try
        {
            var sql = "SELECT * FROM User";
            using var connection = new SqliteConnection($"Data Source=" + Database.GetDatabasePath());
            connection.Open();

            using var sqlCommand = new SqliteCommand(sql, connection);
            using var reader = sqlCommand.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    for (int i = 0; i < 7; i++)
                    {
                        users.Add(reader.GetString(i));
                    }
                }
            }
            return users;
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
        return null;
    }

    public static List<string> GetAllPotentialUsers()
    {
        List<string> users = new List<string>();
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

        try
        {
            var sql = "SELECT * FROM PotentialUser";
            using var connection = new SqliteConnection($"Data Source=" + Database.GetDatabasePath());
            connection.Open();

            using var sqlCommand = new SqliteCommand(sql, connection);
            using var reader = sqlCommand.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    for (int i = 0; i < 12; i++)
                    {
                        users.Add(reader.GetString(i));
                    }
                }
            }
            return users;
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
        return null;
    }
    
    public static bool ApproveUser(int TempUserID)
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
            int PotentialUserID = -1;
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

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    PotentialUserID = Convert.ToInt32(reader.GetInt32(0));
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

            int empID = 1;
            var sql = "SELECT * FROM EMPLOYEE";
            using var GetIDSQLCommand = new SqliteCommand(sql, connection);
            
            using var GetIDReader = GetIDSQLCommand.ExecuteReader();
            if (GetIDReader.HasRows)
            {
                while (GetIDReader.Read())
                {
                    empID++;
                }
            }
            
            
            //Inserting new Employee row into Table
            var EmployeeSQL = "INSERT INTO Employee " +
                              "VALUES (@ID, @FIRSTNAME, @LASTNAME, @DOB, @ADDRESS, @ISADMIN, @ISMANAGER)";
            using var InsertEmployeeCommand = new SqliteCommand(EmployeeSQL, connection);
            InsertEmployeeCommand.Parameters.AddWithValue("@ID", empID);
            InsertEmployeeCommand.Parameters.AddWithValue("@FIRSTNAME", FirstName);
            InsertEmployeeCommand.Parameters.AddWithValue("@LASTNAME", LastName);
            InsertEmployeeCommand.Parameters.AddWithValue("@DOB", DoB);
            InsertEmployeeCommand.Parameters.AddWithValue("@ADDRESS", Address);
            InsertEmployeeCommand.Parameters.AddWithValue("@ISADMIN", IsAdmin);
            InsertEmployeeCommand.Parameters.AddWithValue("@ISMANAGER", IsManager);
            InsertEmployeeCommand.ExecuteNonQuery();

            //creating a new user will require the EmployeeID Foreign key, which is not created until the employee is created
            /*
            var GetEmployeeIDSQL = "SELECT ID FROM Employee WHERE FirstName = @FIRSTNAME)";
            using var GetEmployeeIDCommand = new SqliteCommand(GetEmployeeIDSQL, connection);
            GetEmployeeIDCommand.Parameters.AddWithValue("@FIRSTNAME", FirstName);
            using var EmployeeIDReader = GetEmployeeIDCommand.ExecuteReader();
            if (EmployeeIDReader.HasRows)
            {
                while (EmployeeIDReader.Read())
                {
                    EmployeeID = int.Parse(reader.GetString(0));
                }
            }
            */
            
            int userID = 1;
            var userSQL = "SELECT * FROM EMPLOYEE";
            using var GetUserIDSQLCommand = new SqliteCommand(sql, connection);
            
            using var GetUserIDReader = GetUserIDSQLCommand.ExecuteReader();
            if (GetUserIDReader.HasRows)
            {
                while (GetUserIDReader.Read())
                {
                    userID++;
                }
            }
            
            var UserSQL = "INSERT INTO User " +
                          "VALUES (@ID, @USERNAME, @PASSWORD, @EMAIL, @NEWUSER, @ISACTIVE, @EMPLOYEEID)";
            using var UserSQLCommand = new SqliteCommand(UserSQL, connection);
            UserSQLCommand.Parameters.AddWithValue("@ID", userID);
            UserSQLCommand.Parameters.AddWithValue("@USERNAME", Username);
            UserSQLCommand.Parameters.AddWithValue("@PASSWORD", Password);
            UserSQLCommand.Parameters.AddWithValue("@EMAIL", Email);
            UserSQLCommand.Parameters.AddWithValue("@NEWUSER", NewUser);
            UserSQLCommand.Parameters.AddWithValue("ISACTIVE", IsActive);
            UserSQLCommand.Parameters.AddWithValue("@EMPLOYEEID", empID);
            UserSQLCommand.ExecuteNonQuery();
            
            var RemoveSQL = "DELETE FROM PotentialUser WHERE ID = @ID";
            using var RemoveSQLCommand = new SqliteCommand(RemoveSQL, connection);

            RemoveSQLCommand.Parameters.AddWithValue("@ID", PotentialUserID);
            int rows = RemoveSQLCommand.ExecuteNonQuery();
            Console.WriteLine("Users Removed: " + rows);

            LedgrLogic.Email.SendEmail("ledgrsystems@gmail.com", Email, "Ledgr", (FirstName + " " + LastName), "Login Verified", $"You may log in using {Username} as your username, and {Password} as your password.");
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
    public static bool DeactivateUser(string TempUsername, string TempStartDate, string TempEndDate)
    {
        
        //Changes the IsActive attribute on a user type to false
        //Creates a new entry in SuspendedUser Table
        var UserSQL = "UPDATE User SET IsActive = 0 WHERE Username = @USERNAME";
        var SuspendedUserSQL = "INSERT INTO SuspendedUser VALUES (1, @START, @END, @USERID)";
        bool Successful = false;
        try
        {
            int targetID = -1;
            
            
            using var connection = new SqliteConnection($"Data Source=" + Database.GetDatabasePath());
            connection.Open();

            using var UserTableCommand = new SqliteCommand(UserSQL, connection);
            UserTableCommand.Parameters.AddWithValue("@USERNAME", TempUsername);
            
            var GetUserIDSQL = "SELECT ID FROM User WHERE Username = @USERNAME";
            var GetUserIDCommand = new SqliteCommand(GetUserIDSQL, connection);
            
            GetUserIDCommand.Parameters.AddWithValue("@USERNAME", TempUsername);
            
            var reader = GetUserIDCommand.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    targetID = int.Parse(reader.GetString(0));
                }
            }

            using var SuspendedTableCommand = new SqliteCommand(SuspendedUserSQL, connection);
            SuspendedTableCommand.Parameters.AddWithValue("@START", TempStartDate);
            SuspendedTableCommand.Parameters.AddWithValue("@END", TempEndDate);
            SuspendedTableCommand.Parameters.AddWithValue("@USERID", targetID);

            Console.WriteLine("Target ID: " + targetID);

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
    
    public static List<string> ExpiredPasswordReport()
    {
        List<string> ExpiredPasswordReport = new List<string>();
        try
        {
            var sql = "SELECT * FROM ExpiredPassword";
            using var connection = new SqliteConnection($"Data Source=" + Database.GetDatabasePath());
            connection.Open();

            using var PotentialUserCommand = new SqliteCommand(sql, connection);

            using var reader = PotentialUserCommand.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    ExpiredPasswordReport.Add(reader.GetString(0));
                    ExpiredPasswordReport.Add(reader.GetString(1));
                    ExpiredPasswordReport.Add(reader.GetString(2));
                }
            }
            connection.Close();
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return null;
        }
        return ExpiredPasswordReport;
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