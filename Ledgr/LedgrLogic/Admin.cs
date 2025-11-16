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
            Console.WriteLine(e.Message);
        }
        return null;
    }
    
    public static async Task<bool> ApproveUser(int TempUserID)
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
            Successful = false;
        }
        
        return Successful;
    }
    
    public static async Task ActivateUser(int TempUserID)
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
            
            await connection.CloseAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }
    //Dates given to this method should be formatted as a string (YYYY-MM-DD)
    public static async Task DeactivateUser(string TempUsername, string TempStartDate, string TempEndDate)
    {
        
        //Changes the IsActive attribute on a user type to false
        //Creates a new entry in SuspendedUser Table
        var UserSQL = "UPDATE User SET IsActive = 0 WHERE Username = @USERNAME";
        var SuspendedUserSQL = "INSERT INTO SuspendedUser VALUES (NULL, @START, @END, @USERID)";
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
            await connection.CloseAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
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
            connection.Close();
        }
        catch (Exception e)
        {
            return null;
        }
        return UserReport;
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
            
            connection.Close();
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
            command.Parameters.AddWithValue("@NEWLAST", tempLast);
            command.Parameters.AddWithValue("@EMPLOYEEID", EmployeeID);

            command.ExecuteNonQuery();
            
            connection.Close();
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
            
            connection.Close();
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
            
            connection.Close();
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
            command.Parameters.AddWithValue("@USERNAME", tempUsername);
            command.Parameters.AddWithValue("@USERID", currentUserID);

            command.ExecuteNonQuery();
            
            connection.Close();
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
            
            connection.Close();
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
            
            connection.Close();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return false;
        }

        return Successful;
    }

    //Admin Edit Account methods
    public bool EditAccountName(int tempAccountNumber, string tempAccountName)
    {
        try
        {
            var sql = "UPDATE Account SET Name = @NAME WHERE Number = @ID";
            using var connection = new SqliteConnection($"Data Source=" + Database.GetDatabasePath());

            var command = new SqliteCommand(sql, connection);
            command.Parameters.AddWithValue("@NAME", tempAccountName);
            command.Parameters.AddWithValue("@ID", tempAccountNumber);
            
            connection.Open();

            command.ExecuteNonQuery();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return false;
        }
        return true;
    }
    
    public bool EditAccountDescription(int tempAccountNumber, string tempAccountDesc)
    {
        try
        {
            var sql = "UPDATE Account SET Description = @DESC WHERE Number = @ID"; 
            using var connection = new SqliteConnection($"Data Source=" + Database.GetDatabasePath());

            var command = new SqliteCommand(sql, connection);
            command.Parameters.AddWithValue("@DESC", tempAccountDesc);
            command.Parameters.AddWithValue("@ID", tempAccountNumber);
            
            connection.Open();

            command.ExecuteNonQuery();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return false;
        }
        return true;
    }
    
    //Because Normal Side has to be either left or right (L or R in the database) ensure the char given is L or R
    public bool EditAccountNormalSIde(int tempAccountNumber, char tempNormalSide)
    {
        if (tempNormalSide != 'L' || tempNormalSide != 'R')
        {
            return false;
        }
        try
        {
            var sql = "UPDATE Account SET NormalSide = @NORMALSIDE WHERE Number = @ID"; 
            using var connection = new SqliteConnection($"Data Source=" + Database.GetDatabasePath());

            var command = new SqliteCommand(sql, connection);
            command.Parameters.AddWithValue("@NORMALSIDE", tempNormalSide);
            command.Parameters.AddWithValue("@ID", tempAccountNumber);
            
            connection.Open();

            command.ExecuteNonQuery();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return false;
        }
        return true;
    }
    
    public bool EditAccountCategory(int tempAccountNumber, string tempAccountCategory)
    {
        try
        {
            var sql = "UPDATE Account SET Category = @CATEGORY WHERE Number = @ID"; 
            using var connection = new SqliteConnection($"Data Source=" + Database.GetDatabasePath());

            var command = new SqliteCommand(sql, connection);
            command.Parameters.AddWithValue("@CATEGORY", tempAccountCategory);
            command.Parameters.AddWithValue("@ID", tempAccountNumber);
            
            connection.Open();

            command.ExecuteNonQuery();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return false;
        }
        return true;
    }
    
    public bool EditAccountSubCategory(int tempAccountNumber, string tempAccountSubCategory)
    {
        try
        {
            var sql = "UPDATE Account SET SubCategory = @SUBCATEGORY WHERE Number = @ID"; 
            using var connection = new SqliteConnection($"Data Source=" + Database.GetDatabasePath());

            var command = new SqliteCommand(sql, connection);
            command.Parameters.AddWithValue("@CATEGORY", tempAccountSubCategory);
            command.Parameters.AddWithValue("@ID", tempAccountNumber);
            
            connection.Open();

            command.ExecuteNonQuery();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return false;
        }
        return true;
    }
    
    //Need to call method to turn given double into a value with only two decimal spaces
    public bool EditAccountInitialBalance(int tempAccountNumber, double tempInitialBalance)
    {
        try
        {
            var sql = "UPDATE Account SET InitialBalance = @INITIALBALANCE WHERE Number = @ID"; 
            using var connection = new SqliteConnection($"Data Source=" + Database.GetDatabasePath());

            var command = new SqliteCommand(sql, connection);
            command.Parameters.AddWithValue("@INITIALBALANCE", tempInitialBalance);
            command.Parameters.AddWithValue("@ID", tempAccountNumber);
            
            connection.Open();

            command.ExecuteNonQuery();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return false;
        }
        return true;
    }
    
    public bool EditAccountDebit(int tempAccountNumber, double tempDebit)
    {
        try
        {
            var sql = "UPDATE Account SET Debit = @DEBIT WHERE Number = @ID"; 
            using var connection = new SqliteConnection($"Data Source=" + Database.GetDatabasePath());

            var command = new SqliteCommand(sql, connection);
            command.Parameters.AddWithValue("@DEBIT", tempDebit);
            command.Parameters.AddWithValue("@ID", tempAccountNumber);
            
            connection.Open();

            command.ExecuteNonQuery();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return false;
        }
        return true;
    }
    
    public bool EditAccountCredit(int tempAccountNumber, double tempCredit)
    {
        try
        {
            var sql = "UPDATE Account SET Credit = @CREDIT WHERE Number = @ID"; 
            using var connection = new SqliteConnection($"Data Source=" + Database.GetDatabasePath());

            var command = new SqliteCommand(sql, connection);
            command.Parameters.AddWithValue("@CREDIT", tempCredit);
            command.Parameters.AddWithValue("@ID", tempAccountNumber);
            
            connection.Open();

            command.ExecuteNonQuery();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return false;
        }
        return true;
    }
    public bool EditAccountBalance(int tempAccountNumber, double tempBalance)
    {
        try
        {
            var sql = "UPDATE Account SET Balance = @BALANCE WHERE Number = @ID"; 
            using var connection = new SqliteConnection($"Data Source=" + Database.GetDatabasePath());

            var command = new SqliteCommand(sql, connection);
            command.Parameters.AddWithValue("@BALANCE", tempBalance);
            command.Parameters.AddWithValue("@ID", tempAccountNumber);
            
            connection.Open();

            command.ExecuteNonQuery();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return false;
        }
        return true;
    }
    public bool EditAccountOrder(int tempAccountNumber, int tempOrder)
    {
        try
        {
            var sql = "UPDATE Account SET Order = @ORDER WHERE Number = @ID"; 
            using var connection = new SqliteConnection($"Data Source=" + Database.GetDatabasePath());

            var command = new SqliteCommand(sql, connection);
            command.Parameters.AddWithValue("@ORDER", tempOrder);
            command.Parameters.AddWithValue("@ID", tempAccountNumber);
            
            connection.Open();

            command.ExecuteNonQuery();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return false;
        }
        return true;
    }
    
    public bool EditAccountStatement(int tempAccountNumber, string tempStatement)
    {
        if (tempStatement != "IS" && tempStatement != "BS" && tempStatement != "RE")
        {
            return false;
        }
        try
        {
            var sql = "UPDATE Account SET Statement = @STATEMENT WHERE Number = @ID"; 
            using var connection = new SqliteConnection($"Data Source=" + Database.GetDatabasePath());

            var command = new SqliteCommand(sql, connection);
            command.Parameters.AddWithValue("@Statement", tempStatement);
            command.Parameters.AddWithValue("@ID", tempAccountNumber);
            
            connection.Open();

            command.ExecuteNonQuery();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return false;
        }
        return true;
    }
    
    //Deactivate an account
    //Needs to check if account balance != 0
    public static async Task DeactivateAccount(int tempAccountNumber)
    {
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
                        //Needs to throw error
                        throw new AccountBalanceGreaterThanZeroException("Cannot deactivate an account with a balance greater than zero");
                    }
                }
            }

            deactivateCommand.ExecuteNonQuery();
        }
        catch (SqliteException e)
        {
            //needs to throw error
            Console.WriteLine(e);
        }
    }
    public bool ActivateAccount(int tempAccountNumber)
    {
        try
        {
            var sql = "UPDATE Account SET Active = 1 WHERE Number = @ID"; 
            using var connection = new SqliteConnection($"Data Source=" + Database.GetDatabasePath());

            var command = new SqliteCommand(sql, connection);
            command.Parameters.AddWithValue("@ID", tempAccountNumber);
            
            connection.Open();

            command.ExecuteNonQuery();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return false;
        }
        return true;
    }
    
    public static async Task<List<string>> GetEventLog(string eventLogTable)
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

    public static bool CreateAccount(int tempAccountNum, string tempName, string tempDesc, char tempNormalSide,
        string category, string subCategory, double tempInitBalance, double tempDebit, double tempCredit,
        double tempBalance, string tempDate, int tempUserID, int tempOrder, string tempStatement, string adminUsername)
    {
        //getting admin ID for event log
        User temp = User.GetUserFromUserName(adminUsername).Result;
        int adminID = temp.GetUserID();

        if (!UniqueAccountName(tempName))
        {
            throw new UniqueAccountNameException("The account name entered already exists");
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

    public static async Task UpdateUserEmployee(int userID, string email, bool active, string FirstName, string LastName, string DoB, string Address, string Role)
    {
        try
        {
            var connection = new SqliteConnection($"Data Source=" + Database.GetDatabasePath());
            await connection.OpenAsync();
            
            // Get employee ID for later
            var empIDsql = "SELECT EmployeeID From User WHERE ID = @ID";
            var empIDcommand = new SqliteCommand(empIDsql, connection);
            empIDcommand.Parameters.AddWithValue("@ID", userID);

            int employeeID = -1;
            using var reader = empIDcommand.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    employeeID = reader.GetInt32(0);
                }
            }
            
            // Update Email
            var sql = "UPDATE User Set Email = @EMAIL WHERE ID = @ID";
        
            var command = new SqliteCommand(sql, connection);
            command.Parameters.AddWithValue("@ID", userID);
            command.Parameters.AddWithValue("@EMAIL", email);
            command.ExecuteNonQuery();
            
            // Update Active / Suspension
            
            sql = "UPDATE User SET IsActive = @ACTIVE WHERE ID = @ID";
            var command1 = new SqliteCommand(sql, connection);
            command1.Parameters.AddWithValue("@ACTIVE", active);
            command1.Parameters.AddWithValue("@ID", userID);
            command1.ExecuteNonQuery();
            
            // Update First name
            sql = "Update Employee Set FirstName = @FIRSTNAME Where ID = @EMPLOYEEID";
            var command2 = new SqliteCommand(sql, connection);
            command2.Parameters.AddWithValue("@EMPLOYEEID", employeeID);
            command2.Parameters.AddWithValue("@FIRSTNAME", FirstName);
            command2.ExecuteNonQuery();
            
            // Update Last Name
            sql = "Update Employee Set LastName = @LASTNAME Where ID = @EMPLOYEEID";
            var command3 = new SqliteCommand(sql, connection);
            command3.Parameters.AddWithValue("@LASTNAME", LastName);
            command3.Parameters.AddWithValue("@EMPLOYEEID", employeeID);
            command3.ExecuteNonQuery();
            
            // dob address role
            sql = "Update Employee Set DoB = @DOB WHERE ID = @EMPLOYEEID";
            var command4 = new SqliteCommand(sql, connection);
            command4.Parameters.AddWithValue("@DOB", DoB);
            command4.Parameters.AddWithValue("@EMPLOYEEID", employeeID);
            command4.ExecuteNonQuery();
            
            // Address
            sql = "Update Employee Set Address = @ADDRESS WHERE ID = @EMPLOYEEID";
            var command5 = new SqliteCommand(sql, connection);
            command5.Parameters.AddWithValue("@ADDRESS", Address);
            command5.Parameters.AddWithValue("@EMPLOYEEID", employeeID);
            command5.ExecuteNonQuery();
            
            // Role
            int isAdmin = -1;
            int isManager = -1;
            
            if (Role == "Admin")
            {
                isAdmin = 1;
                isManager = 0;
            }
            else if (Role == "Manager")
            {
                isAdmin = 0;
                isManager = 1;
            }
            else
            {
                isAdmin = 0;
                isManager = 0;
            }
            
            sql = "UPDATE Employee SET IsAdmin = @ISADMIN WHERE ID = @EMPLOYEEID";
            var sql2 = "UPDATE Employee SET IsManager = @ISMANAGER WHERE ID = @EMPLOYEEID";
            
            var command6 = new SqliteCommand(sql, connection);
            command6.Parameters.AddWithValue("@ISADMIN", isAdmin);
            command6.Parameters.AddWithValue("@EMPLOYEEID", employeeID);
            command6.ExecuteNonQuery();
            
            var command7 = new SqliteCommand(sql2, connection);
            command7.Parameters.AddWithValue("@ISMANAGER", isManager);
            command7.Parameters.AddWithValue("@EMPLOYEEID", employeeID);
            Console.WriteLine("Permissions Updated: " + command7.ExecuteNonQuery());
            
            await connection.CloseAsync();

        }
        catch (SqliteException e)
        {
            Console.WriteLine(e);
        }
        
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
    
    
}