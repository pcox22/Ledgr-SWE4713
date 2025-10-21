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

    public static async Task<List<string>> GetAllUsers()
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

    public static async Task<List<string>> GetAllPotentialUsers()
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
            throw new UnableToRetrieveException("Unable to get potential users");
        }
    }
    
    public static async Task<bool> ApproveUser(int TempUserID, string adminUsername)
    {
        //getting admin ID for event log
        User temp = User.GetUserFromUserName(adminUsername).Result;
        int adminID = temp.GetUserID();
        
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
            string Question1 = "";
            string Answer1 = "";
            string Question2 = "";
            string Answer2 = "";
            string Question3 = "";
            string Answer3 = "";

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    PotentialUserID = reader.GetInt32(0);
                    Username += reader.GetString(1);
                    Password += reader.GetString(2);
                    Email += reader.GetString(3);
                    NewUser = int.Parse(reader.GetString(4));
                    IsActive = int.Parse(reader.GetString(5));
                    FirstName += reader.GetString(6);
                    LastName += reader.GetString(7);
                    DoB += reader.GetString(8);
                    Address += reader.GetString(9);
                    IsAdmin = int.Parse(reader.GetString(10));
                    IsManager = int.Parse(reader.GetString(11));
                    Question1 = reader.GetString(12);
                    Answer1 = reader.GetString(13);
                    Question2 = reader.GetString(14);
                    Answer2 = reader.GetString(15);
                    Question3 = reader.GetString(16);
                    Answer3 = reader.GetString(17);
                }
            }
            
            //Inserting new Employee row into Table
            var EmployeeSQL = "INSERT INTO Employee " +
                              "VALUES (NULL, @FIRSTNAME, @LASTNAME, @DOB, @ADDRESS, @ISADMIN, @ISMANAGER)";
            using var InsertEmployeeCommand = new SqliteCommand(EmployeeSQL, connection);
            InsertEmployeeCommand.Parameters.AddWithValue("@FIRSTNAME", FirstName);
            InsertEmployeeCommand.Parameters.AddWithValue("@LASTNAME", LastName);
            InsertEmployeeCommand.Parameters.AddWithValue("@DOB", DoB);
            InsertEmployeeCommand.Parameters.AddWithValue("@ADDRESS", Address);
            InsertEmployeeCommand.Parameters.AddWithValue("@ISADMIN", IsAdmin);
            InsertEmployeeCommand.Parameters.AddWithValue("@ISMANAGER", IsManager);
            InsertEmployeeCommand.ExecuteNonQuery();
            
            // Get that new Employee ID; we need a more viable system; no guarantee first, last name will work
            var getEmployeeIdSQL = "SELECT ID FROM Employee WHERE FirstName = @FIRSTNAME and LastName = @LASTNAME";
            var getEmployeeIdCommand = new SqliteCommand(getEmployeeIdSQL, connection);
            
            getEmployeeIdCommand.Parameters.AddWithValue("@FIRSTNAME", FirstName);
            getEmployeeIdCommand.Parameters.AddWithValue("@LASTNAME", LastName);
            
            using var getEmpIDReader = getEmployeeIdCommand.ExecuteReader();
            if (getEmpIDReader.HasRows)
            {
                while (getEmpIDReader.Read())
                {
                    EmployeeID = Convert.ToInt32(getEmpIDReader.GetInt32(0));
                }
            }
            
            //updating event log after change
            EventLog.LogEmployee('a', EmployeeID, adminID);
            
            var UserSQL = "INSERT INTO User " +
                          "VALUES (NULL, @USERNAME, @PASSWORD, @EMAIL, @NEWUSER, @ISACTIVE, @EMPLOYEEID)";
            using var UserSQLCommand = new SqliteCommand(UserSQL, connection);
            UserSQLCommand.Parameters.AddWithValue("@USERNAME", Username);
            UserSQLCommand.Parameters.AddWithValue("@PASSWORD", Password);
            UserSQLCommand.Parameters.AddWithValue("@EMAIL", Email);
            UserSQLCommand.Parameters.AddWithValue("@NEWUSER", NewUser);
            UserSQLCommand.Parameters.AddWithValue("ISACTIVE", IsActive);
            UserSQLCommand.Parameters.AddWithValue("@EMPLOYEEID", EmployeeID);
            UserSQLCommand.ExecuteNonQuery();
            
            var RemoveSQL = "DELETE FROM PotentialUser WHERE ID = @ID";
            using var RemoveSQLCommand = new SqliteCommand(RemoveSQL, connection);

            RemoveSQLCommand.Parameters.AddWithValue("@ID", PotentialUserID);
            await RemoveSQLCommand.ExecuteNonQueryAsync();

            LedgrLogic.Email.SendEmail("ledgrsystems@gmail.com", Email, "Ledgr", (FirstName + " " + LastName), "Login Verified", $"You may log in using {Username} as your username, and {LedgrLogic.Password.Decrypt(Password)} as your password.");

            var getUserIdSQL = "SELECT ID FROM User WHERE EmployeeID = @EMPID";
            var getUserIdCommand = new SqliteCommand(getUserIdSQL, connection);
            getUserIdCommand.Parameters.AddWithValue("@EMPID", EmployeeID);

            int userID = -1;
            using var getUserIDReader = getUserIdCommand.ExecuteReader();
            if (getUserIDReader.HasRows)
            {
                while (getUserIDReader.Read())
                {
                    userID = Convert.ToInt32(getUserIDReader.GetString(0));
                }
            }
            
            //updating event log after change
            EventLog.LogUser('a', userID, adminID);
            
            var SecQuestionsSQL1 = "Insert INTO SecurityQuestion Values(Null, @QUESTION, @ANSWER, @USERID)";
            var SecQuestionsSQL2 = "Insert INTO SecurityQuestion Values(Null, @QUESTION, @ANSWER, @USERID)";
            var SecQuestionsSQL3 = "Insert INTO SecurityQuestion Values(Null, @QUESTION, @ANSWER, @USERID)";
            
            var SQ1Command = new SqliteCommand(SecQuestionsSQL1, connection);
            SQ1Command.Parameters.AddWithValue("@QUESTION", Question1);
            SQ1Command.Parameters.AddWithValue("@ANSWER", Answer1);
            SQ1Command.Parameters.AddWithValue("@USERID", userID);
            
            var SQ2Command = new SqliteCommand(SecQuestionsSQL2, connection);
            SQ2Command.Parameters.AddWithValue("@QUESTION", Question2);
            SQ2Command.Parameters.AddWithValue("@ANSWER", Answer2);
            SQ2Command.Parameters.AddWithValue("@USERID", userID);

            var SQ3Command = new SqliteCommand(SecQuestionsSQL3, connection);
            SQ3Command.Parameters.AddWithValue("@QUESTION", Question3);
            SQ3Command.Parameters.AddWithValue("@ANSWER", Answer3);
            SQ3Command.Parameters.AddWithValue("@USERID", userID);

            SQ1Command.ExecuteNonQuery();
            SQ2Command.ExecuteNonQuery();
            SQ3Command.ExecuteNonQuery();

            await connection.CloseAsync();

        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw new UnableToApproveUserException("Unable to approve the user");
        }
        
        return Successful;
    }
    
    public static bool ActivateUser(int TempUserID, string adminUsername)
    {
        //getting admin ID for event log
        User temp = User.GetUserFromUserName(adminUsername).Result;
        int adminID = temp.GetUserID();
        
        //Changes the IsActive attribute on a user type to true, updates the database
        //Deletes entry from SuspendedUser Table
        var UserSQL = "UPDATE User SET IsActive = 1 WHERE ID = @ID";
        var SuspendedUserSQL = "DELETE FROM SuspendedUser WHERE UserID = @ID";
        bool Successful = false;
        try
        {
            //updating event log before change
            EventLog.LogUser('b', TempUserID, adminID);
            
            using var connection = new SqliteConnection($"Data Source=" + Database.GetDatabasePath());
            connection.Open();

            using var UserTableCommand = new SqliteCommand(UserSQL, connection);
            UserTableCommand.Parameters.AddWithValue("@ID", TempUserID);

            using var SuspendedTableCommand = new SqliteCommand(SuspendedUserSQL, connection);
            SuspendedTableCommand.Parameters.AddWithValue("@ID", TempUserID);
            
            UserTableCommand.ExecuteNonQuery();
            SuspendedTableCommand.ExecuteNonQuery();
            
            //updating event log after change
            EventLog.LogUser('a', TempUserID, adminID);

            //Successful will only be true if no errors are thrown by the queries
            Successful = true;
        }
        catch (Exception e)
        {
            throw new UnableToActivateUserException("Unable to activate user");
        }

        return Successful;
    }
    //Dates given to this method should be formatted as a string (YYYY-MM-DD)
    public static bool DeactivateUser(string TempUsername, string TempStartDate, string TempEndDate, string adminUsername)
    {
        //getting admin ID for event log
        User admin = User.GetUserFromUserName(adminUsername).Result;
        int adminID = admin.GetUserID();
        
        //Changes the IsActive attribute on a user type to false
        //Creates a new entry in SuspendedUser Table
        var UserSQL = "UPDATE User SET IsActive = 0 WHERE Username = @USERNAME";
        var SuspendedUserSQL = "INSERT INTO SuspendedUser VALUES (NULL, @START, @END, @USERID)";
        bool Successful = false;
        
        //getting deactivated user's id
        User temp = User.GetUserFromUserName(TempUsername).Result;
        int tempID = temp.GetUserID();
        try
        {
            //updating event log before change
            EventLog.LogUser('b', tempID, adminID);
            
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
            
            //updating event log after change
            EventLog.LogUser('a', tempID, adminID);
            
            connection.Close();
        }
        catch (Exception e)
        {
            throw new UnableToDeactivateUserException("Unable to deactivate this user");
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
            throw new UnableToRetrieveException("Unable to retrieve expired passwords");
        }
        return ExpiredPasswordReport;
    }

    public bool PromoteToManager(int TempEmployeeID, string adminUsername)
    {
        //getting admin ID for event log
        User temp = User.GetUserFromUserName(adminUsername).Result;
        int adminID = temp.GetUserID();
        
        bool Successful = false;
        var sql = "UPDATE Employee SET IsManager = 1 WHERE ID = @ID";
        try
        {
            //updating event log before change
            EventLog.LogEmployee('b', TempEmployeeID, adminID);
            
            using var connection = new SqliteConnection($"Data Source=" + Database.GetDatabasePath());
            connection.Open();

            using var command = new SqliteCommand(sql, connection);
            command.Parameters.AddWithValue("@ID", TempEmployeeID);

            command.ExecuteNonQuery();
            
            //updating event log after change
            EventLog.LogEmployee('a', TempEmployeeID, adminID);
            
            Successful = true;
        }
        catch (Exception e)
        {
            throw new InvalidChangeException("No such employee exists");
        }

        return Successful;
    }
    
    public bool PromoteToAdmin(int TempEmployeeID, string adminUsername)
    {
        //getting admin ID for event log
        User temp = User.GetUserFromUserName(adminUsername).Result;
        int adminID = temp.GetUserID();
        
        bool Successful = false;
        var sql = "UPDATE Employee SET IsAdmin = 1 WHERE ID = @ID";
                  
        try
        {
            //updating event log before change
            EventLog.LogEmployee('b', TempEmployeeID, adminID);
            
            using var connection = new SqliteConnection($"Data Source=" + Database.GetDatabasePath());
            connection.Open();

            using var command = new SqliteCommand(sql, connection);
            command.Parameters.AddWithValue("@ID", TempEmployeeID);

            command.ExecuteNonQuery();
            
            //updating event log after change
            EventLog.LogEmployee('a', TempEmployeeID, adminID);
            
            Successful = true;
        }
        catch (Exception e)
        {
            throw new InvalidChangeException("No such employee exists");
        }

        return Successful;
    }
    
    public bool DemoteFromAdmin(int TempEmployeeID, string adminUsername)
    {
        //getting admin ID for event log
        User temp = User.GetUserFromUserName(adminUsername).Result;
        int adminID = temp.GetUserID();
        
        bool Successful = false;
        var sql = "UPDATE Employee SET IsAdmin = 0 WHERE ID = @ID";
        try
        {
            //updating event log before change
            EventLog.LogEmployee('b', TempEmployeeID, adminID);
            
            using var connection = new SqliteConnection($"Data Source=" + Database.GetDatabasePath());
            connection.Open();

            using var command = new SqliteCommand(sql, connection);
            command.Parameters.AddWithValue("@ID", TempEmployeeID);

            command.ExecuteNonQuery();
            
            //updating event log after change
            EventLog.LogEmployee('a', TempEmployeeID, adminID);
            
            Successful = true;
        }
        catch (Exception e)
        {
            throw new InvalidChangeException("No such employee exists");
        }

        return Successful;
    }
    
    public bool DemoteFromManager(int TempEmployeeID, string adminUsername)
    {
        //getting admin ID for event log
        User temp = User.GetUserFromUserName(adminUsername).Result;
        int adminID = temp.GetUserID();
        
        bool Successful = false;
        var sql = "UPDATE Employee SET IsManager = 0 WHERE ID = @ID";
        try
        {
            //updating event log before change
            EventLog.LogEmployee('b', TempEmployeeID, adminID);
            
            using var connection = new SqliteConnection($"Data Source=" + Database.GetDatabasePath());
            connection.Open();

            using var command = new SqliteCommand(sql, connection);
            command.Parameters.AddWithValue("@ID", TempEmployeeID);

            command.ExecuteNonQuery();
            
            //updating event log after change
            EventLog.LogEmployee('a', TempEmployeeID, adminID);
            
            Successful = true;
        }
        catch (Exception e)
        {
            throw new InvalidChangeException("No such employee exists");
        }

        return Successful;
    }
    public static List<string> UserReport()
    {
        List<string> UserReport = new List<string>();
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
                    for (int i = 0; i < 13; i++)
                    {
                        if (!reader.IsDBNull(i))
                        {
                            UserReport.Add(reader.GetString(i));
                        }
                    }
                }
            }
            connection.Close();
        }
        catch (Exception e)
        {
            throw new UnableToRetrieveException("Unable to retrieve User Report");
        }
        return UserReport;
    }
    
    public bool UpdateFirstName(int EmployeeID, string tempFirst, string adminUsername)
    {
        //getting admin ID for event log
        User temp = User.GetUserFromUserName(adminUsername).Result;
        int adminID = temp.GetUserID();
        
        bool Successful = true;
        try
        {
            //updating event log before change
            EventLog.LogEmployee('b', EmployeeID, adminID);
            
            var sql = "UPDATE Employee SET FirstName = @NEWFIRST WHERE ID = @EMPLOYEEID";
            using var connection = new SqliteConnection($"Data Source=" + Database.GetDatabasePath());
            connection.Open();

            using var command = new SqliteCommand(sql, connection);
            command.Parameters.AddWithValue("@NEWFIRST", tempFirst);
            command.Parameters.AddWithValue("@EMPLOYEEID", EmployeeID);

            command.ExecuteNonQuery();
            
            //updating event log after change
            EventLog.LogEmployee('a', EmployeeID, adminID);
            
            connection.Close();
        }
        catch (Exception e)
        {
            throw new InvalidChangeException("No such employee exists");
        }

        return Successful;
    }
    
    public bool UpdateLastName(int EmployeeID, string tempLast, string adminUsername)
    {
        //getting admin ID for event log
        User temp = User.GetUserFromUserName(adminUsername).Result;
        int adminID = temp.GetUserID();
        
        bool Successful = true;
        try
        {
            //updating event log before change
            EventLog.LogEmployee('b', EmployeeID, adminID);
            
            var sql = "UPDATE Employee SET LastName = @NEWLAST WHERE ID = @EMPLOYEEID";
            using var connection = new SqliteConnection($"Data Source=" + Database.GetDatabasePath());
            connection.Open();

            using var command = new SqliteCommand(sql, connection);
            command.Parameters.AddWithValue("@NEWLAST", tempLast);
            command.Parameters.AddWithValue("@EMPLOYEEID", EmployeeID);

            command.ExecuteNonQuery();
            
            //updating event log after change
            EventLog.LogEmployee('a', EmployeeID, adminID);
            
            connection.Close();
        }
        catch (Exception e)
        {
            throw new InvalidChangeException("No such user exists");
        }

        return Successful;
    }
    
    public bool UpdateDoB(int EmployeeID, string tempDoB, string adminUsername)
    {
        //getting admin ID for event log
        User temp = User.GetUserFromUserName(adminUsername).Result;
        int adminID = temp.GetUserID();
        
        bool Successful = true;
        try
        {
            //updating event log before change
            EventLog.LogEmployee('b', EmployeeID, adminID);
            
            var sql = "UPDATE Employee SET DoB = @NEWDOB WHERE ID = @EMPLOYEEID";
            using var connection = new SqliteConnection($"Data Source=" + Database.GetDatabasePath());
            connection.Open();

            using var command = new SqliteCommand(sql, connection);
            command.Parameters.AddWithValue("@NEWDOB", tempDoB);
            command.Parameters.AddWithValue("@EMPLOYEEID", EmployeeID);

            command.ExecuteNonQuery();
            
            //updating event log after change
            EventLog.LogEmployee('a', EmployeeID, adminID);
            
            connection.Close();
        }
        catch (Exception e)
        {
            throw new InvalidChangeException("No such user exists");
        }

        return Successful;
    }
    public bool UpdateAddress(int EmployeeID, string tempAddress,string adminUsername)
    {
        //getting admin ID for event log
        User temp = User.GetUserFromUserName(adminUsername).Result;
        int adminID = temp.GetUserID();
        
        bool Successful = true;
        try
        {
            
            //updating event log before change
            EventLog.LogEmployee('b', EmployeeID, adminID);
            
            var sql = "UPDATE Employee SET Address = @NEWADDRESS WHERE ID = @EMPLOYEEID";
            using var connection = new SqliteConnection($"Data Source=" + Database.GetDatabasePath());
            connection.Open();

            using var command = new SqliteCommand(sql, connection);
            command.Parameters.AddWithValue("@NEWADDRESS", tempAddress);
            command.Parameters.AddWithValue("@EMPLOYEEID", EmployeeID);

            command.ExecuteNonQuery();
            
            //updating event log after change
            EventLog.LogEmployee('a', EmployeeID, adminID);
            
            connection.Close();
        }
        catch (Exception e)
        {
            throw new InvalidChangeException("No such user exists");
        }

        return Successful;
    }
    public bool UpdateUsername(int currentUserID, string tempUsername, string adminUsername)
    {
        //getting admin ID for event log
        User temp = User.GetUserFromUserName(adminUsername).Result;
        int adminID = temp.GetUserID();
        
        bool Successful = true;
        try
        {
            //updating event log before change
            EventLog.LogUser('b', currentUserID, adminID);
            
            var sql = "UPDATE User SET Username = @USERNAME WHERE ID = @USERID";
            using var connection = new SqliteConnection($"Data Source=" + Database.GetDatabasePath());
            connection.Open();

            using var command = new SqliteCommand(sql, connection);
            command.Parameters.AddWithValue("@USERNAME", tempUsername);
            command.Parameters.AddWithValue("@USERID", currentUserID);

            command.ExecuteNonQuery();
            
            //updating event log before change
            EventLog.LogUser('a', currentUserID, adminID);
            
            connection.Close();
        }
        catch (Exception e)
        {
            throw new InvalidChangeException("No such user exists");
        }

        return Successful;
    }
    
    public bool UpdateEmail(int currentUserID, string tempEmail, string adminUsername)
    {
        //getting admin ID for event log
        User temp = User.GetUserFromUserName(adminUsername).Result;
        int adminID = temp.GetUserID();
        
        bool Successful = true;
        try
        {
            //updating event log before change
            EventLog.LogEmployee('b', currentUserID, adminID);
            
            var sql = "UPDATE User SET Email = @NEWEMAIL WHERE ID = @USERID";
            using var connection = new SqliteConnection($"Data Source=" + Database.GetDatabasePath());
            connection.Open();

            using var command = new SqliteCommand(sql, connection);
            command.Parameters.AddWithValue("@NEWEMAIL", tempEmail);
            command.Parameters.AddWithValue("@USERID", currentUserID);

            command.ExecuteNonQuery();
            
            //updating event log after change
            EventLog.LogEmployee('a', currentUserID, adminID);
            
            connection.Close();
        }
        catch (Exception e)
        {
            throw new InvalidChangeException("No such user exists");
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
            
            connection.Close();
        }
        catch (Exception e)
        {
            throw new InvalidChangeException("No such user exists");
        }

        return Successful;
    }

    //Admin Edit Account methods
    public bool EditAccountName(int tempAccountNumber, string tempAccountName, string adminUsername)
    {
        //getting admin ID for event log
        User temp = User.GetUserFromUserName(adminUsername).Result;
        int adminID = temp.GetUserID();
        
        try
        {
            //updating event log before change
            EventLog.LogAccount('b', tempAccountNumber, adminID);
            
            var sql = "UPDATE Account SET Name = @NAME WHERE Number = @ID";
            using var connection = new SqliteConnection($"Data Source=" + Database.GetDatabasePath());

            var command = new SqliteCommand(sql, connection);
            command.Parameters.AddWithValue("@NAME", tempAccountName);
            command.Parameters.AddWithValue("@ID", tempAccountNumber);
            
            connection.Open();

            command.ExecuteNonQuery();
            
            //updating event log after change
            EventLog.LogAccount('a', tempAccountNumber, adminID);
            
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return false;
        }
        return true;
    }
    
    public bool EditAccountDescription(int tempAccountNumber, string tempAccountDesc, string adminUsername)
    {
        //getting admin ID for event log
        User temp = User.GetUserFromUserName(adminUsername).Result;
        int adminID = temp.GetUserID();
        
        try
        {
            //updating event log before change
            EventLog.LogAccount('b', tempAccountNumber, adminID);
            
            var sql = "UPDATE Account SET Description = @DESC WHERE Number = @ID"; 
            using var connection = new SqliteConnection($"Data Source=" + Database.GetDatabasePath());

            var command = new SqliteCommand(sql, connection);
            command.Parameters.AddWithValue("@DESC", tempAccountDesc);
            command.Parameters.AddWithValue("@ID", tempAccountNumber);
            
            connection.Open();

            command.ExecuteNonQuery();
            
            //updating event log after change
            EventLog.LogAccount('a', tempAccountNumber, adminID);
            
            connection.Close();
        }
        catch (Exception e)
        {
            throw new InvalidChangeException("No such account exists");
        }
        return true;
    }
    
    //Because Normal Side has to be either left or right (L or R in the database) ensure the char given is L or R
    public bool EditAccountNormalSIde(int tempAccountNumber, char tempNormalSide, string adminUsername)
    {
        if (tempNormalSide != 'L' && tempNormalSide != 'R')
        {
            return false;
        }
        
        //getting admin ID for event log
        User temp = User.GetUserFromUserName(adminUsername).Result;
        int adminID = temp.GetUserID();
        
        try
        {
            //updating event log before change
            EventLog.LogAccount('b', tempAccountNumber, adminID);
            
            var sql = "UPDATE Account SET NormalSide = @NORMALSIDE WHERE Number = @ID"; 
            using var connection = new SqliteConnection($"Data Source=" + Database.GetDatabasePath());

            var command = new SqliteCommand(sql, connection);
            command.Parameters.AddWithValue("@NORMALSIDE", tempNormalSide);
            command.Parameters.AddWithValue("@ID", tempAccountNumber);
            
            connection.Open();

            command.ExecuteNonQuery();
            
            //updating event log after change
            EventLog.LogAccount('a', tempAccountNumber, adminID);
            
            connection.Close();
        }
        catch (Exception e)
        {
            throw new InvalidChangeException("No such account exists");
        }
        return true;
    }
    
    public bool EditAccountCategory(int tempAccountNumber, string tempAccountCategory, string adminUsername)
    {
        //getting admin ID for event log
        User temp = User.GetUserFromUserName(adminUsername).Result;
        int adminID = temp.GetUserID();
        
        try
        {
            //updating event log before change
            EventLog.LogAccount('b', tempAccountNumber, adminID);
            
            var sql = "UPDATE Account SET Category = @CATEGORY WHERE Number = @ID"; 
            using var connection = new SqliteConnection($"Data Source=" + Database.GetDatabasePath());

            var command = new SqliteCommand(sql, connection);
            command.Parameters.AddWithValue("@CATEGORY", tempAccountCategory);
            command.Parameters.AddWithValue("@ID", tempAccountNumber);
            
            connection.Open();

            command.ExecuteNonQuery();
            
            //updating event log after change
            EventLog.LogAccount('a', tempAccountNumber, adminID);
            
            connection.Close();
        }
        catch (Exception e)
        {
            throw new InvalidChangeException("No such account exists");
        }
        return true;
    }
    
    public bool EditAccountSubCategory(int tempAccountNumber, string tempAccountSubCategory, string adminUsername)
    {
        //getting admin ID for event log
        User temp = User.GetUserFromUserName(adminUsername).Result;
        int adminID = temp.GetUserID();
        
        try
        {
            //updating event log before change
            EventLog.LogAccount('b', tempAccountNumber, adminID);
            
            var sql = "UPDATE Account SET SubCategory = @SUBCATEGORY WHERE Number = @ID"; 
            using var connection = new SqliteConnection($"Data Source=" + Database.GetDatabasePath());

            var command = new SqliteCommand(sql, connection);
            command.Parameters.AddWithValue("@SUBCATEGORY", tempAccountSubCategory);
            command.Parameters.AddWithValue("@ID", tempAccountNumber);
            
            connection.Open();

            command.ExecuteNonQuery();
            
            //updating event log after change
            EventLog.LogAccount('a', tempAccountNumber, adminID);
            
            connection.Close();
        }
        catch (Exception e)
        {
            throw new InvalidChangeException("No such account exists");
        }
        return true;
    }
    
    //Need to call method to turn given double into a value with only two decimal spaces
    public bool EditAccountInitialBalance(int tempAccountNumber, double tempInitialBalance, string adminUsername)
    {
        //getting admin ID for event log
        User temp = User.GetUserFromUserName(adminUsername).Result;
        int adminID = temp.GetUserID();
        
        try
        {
            //updating event log before change
            EventLog.LogAccount('b', tempAccountNumber, adminID);
            
            var sql = "UPDATE Account SET InitialBalance = @INITIALBALANCE WHERE Number = @ID"; 
            using var connection = new SqliteConnection($"Data Source=" + Database.GetDatabasePath());

            var command = new SqliteCommand(sql, connection);
            command.Parameters.AddWithValue("@INITIALBALANCE", tempInitialBalance);
            command.Parameters.AddWithValue("@ID", tempAccountNumber);
            
            connection.Open();

            command.ExecuteNonQuery();
            
            //updating event log after change
            EventLog.LogAccount('a', tempAccountNumber, adminID);
            
            connection.Close();
        }
        catch (Exception e)
        {
            throw new InvalidChangeException("No such account exists");
        }
        return true;
    }
    
    public bool EditAccountDebit(int tempAccountNumber, double tempDebit, string adminUsername)
    {
        //getting admin ID for event log
        User temp = User.GetUserFromUserName(adminUsername).Result;
        int adminID = temp.GetUserID();
        
        try
        {
            //updating event log before change
            EventLog.LogAccount('b', tempAccountNumber, adminID);
            
            var sql = "UPDATE Account SET Debit = @DEBIT WHERE Number = @ID"; 
            using var connection = new SqliteConnection($"Data Source=" + Database.GetDatabasePath());

            var command = new SqliteCommand(sql, connection);
            command.Parameters.AddWithValue("@DEBIT", tempDebit);
            command.Parameters.AddWithValue("@ID", tempAccountNumber);
            
            connection.Open();

            command.ExecuteNonQuery();
            
            //updating event log after change
            EventLog.LogAccount('a', tempAccountNumber, adminID);
            
            connection.Close();
        }
        catch (Exception e)
        {
            throw new InvalidChangeException("No such account exists");
        }
        return true;
    }
    
    public bool EditAccountCredit(int tempAccountNumber, double tempCredit, string adminUsername)
    {
        //getting admin ID for event log
        User temp = User.GetUserFromUserName(adminUsername).Result;
        int adminID = temp.GetUserID();
        
        try
        {
            //updating event log before change
            EventLog.LogAccount('b', tempAccountNumber, adminID);
            
            var sql = "UPDATE Account SET Credit = @CREDIT WHERE Number = @ID"; 
            using var connection = new SqliteConnection($"Data Source=" + Database.GetDatabasePath());

            var command = new SqliteCommand(sql, connection);
            command.Parameters.AddWithValue("@CREDIT", tempCredit);
            command.Parameters.AddWithValue("@ID", tempAccountNumber);
            
            connection.Open();

            command.ExecuteNonQuery();
            
            //updating event log after change
            EventLog.LogAccount('a', tempAccountNumber, adminID);
            
            connection.Close();
        }
        catch (Exception e)
        {
            throw new InvalidChangeException("No such account exists");
        }
        return true;
    }
    public bool EditAccountBalance(int tempAccountNumber, double tempBalance, string adminUsername)
    {
        //getting admin ID for event log
        User temp = User.GetUserFromUserName(adminUsername).Result;
        int adminID = temp.GetUserID();
        
        try
        {
            //updating event log before change
            EventLog.LogAccount('b', tempAccountNumber, adminID);
            
            var sql = "UPDATE Account SET Balance = @BALANCE WHERE Number = @ID"; 
            using var connection = new SqliteConnection($"Data Source=" + Database.GetDatabasePath());

            var command = new SqliteCommand(sql, connection);
            command.Parameters.AddWithValue("@BALANCE", tempBalance);
            command.Parameters.AddWithValue("@ID", tempAccountNumber);
            
            connection.Open();

            command.ExecuteNonQuery();
            
            //updating event log after change
            EventLog.LogAccount('a', tempAccountNumber, adminID);
            
            connection.Close();
        }
        catch (Exception e)
        {
            throw new InvalidChangeException("No such account exists");
        }
        return true;
    }
    public bool EditAccountOrder(int tempAccountNumber, int tempOrder, string adminUsername)
    {
        //getting admin ID for event log
        User temp = User.GetUserFromUserName(adminUsername).Result;
        int adminID = temp.GetUserID();
        
        try
        {
            //updating event log before change
            EventLog.LogAccount('b', tempAccountNumber, adminID);
            
            var sql = "UPDATE Account SET \"Order\" = @ORDER WHERE Number = @ID"; 
            using var connection = new SqliteConnection($"Data Source=" + Database.GetDatabasePath());

            var command = new SqliteCommand(sql, connection);
            command.Parameters.AddWithValue("@ORDER", tempOrder);
            command.Parameters.AddWithValue("@ID", tempAccountNumber);
            
            connection.Open();

            command.ExecuteNonQuery();
            
            //updating event log after change
            EventLog.LogAccount('a', tempAccountNumber, adminID);
            
            connection.Close();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw new InvalidChangeException("No such account exists");
        }
        return true;
    }
    
    public bool EditAccountStatement(int tempAccountNumber, string tempStatement, string adminUsername)
    {
        //getting admin ID for event log
        User temp = User.GetUserFromUserName(adminUsername).Result;
        int adminID = temp.GetUserID();
        
        if (tempStatement != "IS" && tempStatement != "BS" && tempStatement != "RE")
        {
            return false;
        }
        try
        {
            //updating event log before change
            EventLog.LogAccount('b', tempAccountNumber, adminID);
            
            var sql = "UPDATE Account SET Statement = @STATEMENT WHERE Number = @ID"; 
            using var connection = new SqliteConnection($"Data Source=" + Database.GetDatabasePath());

            var command = new SqliteCommand(sql, connection);
            command.Parameters.AddWithValue("@STATEMENT", tempStatement);
            command.Parameters.AddWithValue("@ID", tempAccountNumber);
            
            connection.Open();

            command.ExecuteNonQuery();
            
            //updating event log after change
            EventLog.LogAccount('a', tempAccountNumber, adminID);
            
            connection.Close();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw new InvalidChangeException("No such account exists");
        }
        return true;
    }
    
    //Deactivate an account
    public bool DeactivateAccount(int tempAccountNumber, string adminUsername)
    {
        //getting admin ID for event log
        User temp = User.GetUserFromUserName(adminUsername).Result;
        int adminID = temp.GetUserID();
        
        try
        {
            var deactivateSql = "UPDATE Account SET Active = 0 WHERE Number = @ID";
            var getBalanceSql = "SELECT BALANCE FROM  Account WHERE Number = @ID";
            using var connection = new SqliteConnection($"Data Source=" + Database.GetDatabasePath());
            
            var getBalanceCommand = new SqliteCommand(getBalanceSql, connection);
            getBalanceCommand.Parameters.AddWithValue("@ID", tempAccountNumber);
            
            var deactivateCommand = new SqliteCommand(deactivateSql, connection);
            deactivateCommand.Parameters.AddWithValue("@ID", tempAccountNumber);
            
            connection.Open();
            
            //checking that balance != 0
            using var reader = getBalanceCommand.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    double storedBalance = double.Parse(reader.GetString(0));
                    if (storedBalance > 0)
                    {
                        throw new InvalidChangeException("Account has a balance greater than $0.00");
                    }
                }
            }
            //updating event log before change
            EventLog.LogAccount('b', tempAccountNumber, adminID);
            
            deactivateCommand.ExecuteNonQuery();
            
            //updating event log after change
            EventLog.LogAccount('a', tempAccountNumber, adminID);
            
            connection.Close();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw new InvalidChangeException("No such account exists");
        }
        return true;
    }
    public bool ActivateAccount(int tempAccountNumber, string adminUsername)
    {
        //getting admin ID for event log
        User temp = User.GetUserFromUserName(adminUsername).Result;
        int adminID = temp.GetUserID();
        
        try
        {
            //updating event log before change
            EventLog.LogAccount('b', tempAccountNumber, adminID);
            
            var sql = "UPDATE Account SET Active = 1 WHERE Number = @ID"; 
            using var connection = new SqliteConnection($"Data Source=" + Database.GetDatabasePath());

            var command = new SqliteCommand(sql, connection);
            command.Parameters.AddWithValue("@ID", tempAccountNumber);
            
            connection.Open();

            command.ExecuteNonQuery();
            
            //updating event log after change
            EventLog.LogAccount('a', tempAccountNumber, adminID);
            
            connection.Close();
        }
        catch (Exception e)
        {
            throw new InvalidChangeException("No such account exists");
        }
        return true;
    }

    public List<string> GetEventLog(string eventLogTable)
    {
        List<string> tempEventLog = new List<string>();
        using var connection = new SqliteConnection($"Data Source=" + Database.GetDatabasePath());
        var sql = "";
        int columns = 0;

        switch (eventLogTable)
        {
            case ("Account"):
                sql = "SELECT * FROM AccountEventLog ORDER BY ID ASC";
                columns = 18;
                break;
            case("Employee"):
                sql = "SELECT * FROM EmployeeEventLog ORDER BY ID ASC";
                columns = 10;
                break;
            case("User"):
                sql = "SELECT * FROM UserEventLog ORDER BY ID ASC";
                columns = 10;
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
                    for (int i = 0; i < columns; i++)
                    {
                        tempEventLog.Add(reader.GetString(i));
                    }
                }
            }
        }
        catch (Exception e)
        {
            throw new UnableToRetrieveException("Unable to retrieve "+eventLogTable+" Event Log");
        }

        return tempEventLog;
    }

    public static bool UniqueAccountName(string tempName)
    {
        try
        {
            var sql = "SELECT Name FROM Account WHERE Name = @NAME";
            using var connection = new SqliteConnection($"Data Source=" + Database.GetDatabasePath());
            connection.Open();

            var command = new SqliteCommand(sql, connection);
            command.Parameters.AddWithValue("@NAME", tempName);

            using var reader = command.ExecuteReader();
            if (reader.HasRows)
            {
                return false;
            } 
            
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        
    }

    public static bool CreateAccount(int tempAccountNum, string tempName, string tempDesc, char tempNormalSide,
        string category, string subCategory, double tempInitBalance, double tempDebit, double tempCredit,
        double tempBalance, string tempDate, int tempUserID, int tempOrder, string tempStatement, string adminUsername)
    {
        //getting admin ID for event log
        User temp = User.GetUserFromUserName(adminUsername).Result;
        int adminID = temp.GetUserID();

        if (!UniqueAccountName(tempName))
        {
            return false;
        }

        try
        {
            using var connection = new SqliteConnection($"Data Source=" + Database.GetDatabasePath());
            var sql =
                "INSERT INTO Account VALUES(@NUMBER, @NAME, @DESC, @NS, @CATEGORY, @SUBCATEGORY, @INITBALANCE, @DEBIT, @CREDIT, @BALANCE, @DATE, @USERID, @ORDER, @STATEMENT, 1)";
            using var command = new SqliteCommand(sql, connection);

            command.Parameters.AddWithValue("@NUMBER", tempAccountNum);
            command.Parameters.AddWithValue("@NAME", tempName);
            command.Parameters.AddWithValue("@DESC", tempDesc);
            command.Parameters.AddWithValue("@NS", tempNormalSide);
            command.Parameters.AddWithValue("@CATEGORY", category);
            command.Parameters.AddWithValue("@SUBCATEGORY", subCategory);
            command.Parameters.AddWithValue("@INITBALANCE", tempInitBalance);
            command.Parameters.AddWithValue("@DEBIT", tempDebit);
            command.Parameters.AddWithValue("@CREDIT", tempCredit);
            command.Parameters.AddWithValue("@BALANCE", tempBalance);
            command.Parameters.AddWithValue("@DATE", tempDate);
            command.Parameters.AddWithValue("@USERID", tempUserID);
            command.Parameters.AddWithValue("@ORDER", tempOrder);
            command.Parameters.AddWithValue("@STATEMENT", tempStatement);

            connection.Open();
            command.ExecuteNonQuery();
            
            //updating event log after change
            EventLog.LogAccount('a', tempAccountNum, adminID);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return false;
        }
        return true;
    }

    public static List<string> GetAccountEventLog(int accountNumber)
    {
        List<string> accountEventLog = new List<string>();
        try
        {
            var sql = "SELECT * FROM AccountEventLog WHERE Number = @ACCOUNTNUM";
            using var connection = new SqliteConnection($"Data Source=" + Database.GetDatabasePath());

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
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw new UnableToRetrieveException("Unable to retrieve this account's event log");
        }

        return accountEventLog;
    }
}