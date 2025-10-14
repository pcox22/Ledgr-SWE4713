using System.Collections;
using System.Data;
using System.Threading.Channels;
using Microsoft.Data.Sqlite;

namespace LedgrLogic;

public class User
{
    protected string Username;
    protected string Password;
    protected string Email;
    protected int UserID;
    protected int EmployeeID;
    protected bool IsActive;
    protected bool NewUser;

    public User(string TempUsername, string TempPass, string TempEmail, int TempUserID, int TempEmployeeID, bool TempActive, bool TempNew)
    {
        Username = TempUsername;
        Password = TempPass;
        UserID = TempUserID;
        EmployeeID = TempEmployeeID;
        IsActive = TempActive;
        NewUser = TempNew;
        Email = TempEmail;
    }
    
    public User()
    {
        Username = "";
        Password = "";
        UserID = 0;
        NewUser = true;
    }
    
    //Getters and Setters
    public string GetUserName()
    {
        return Username;
    }
    
    public string GetPassword()
    {
        return Password;
    }

    public int GetUserID()
    {
        return UserID;
    }

    public bool GetIsActive()
    {
        return IsActive;
    }

    public bool GetNewUser()
    {
        return NewUser;
    }

    public void SetUserName(string TempUsername)
    {
        Username = TempUsername;
    }
    
    //Just a regular Setter for password attribute, not equivalent to a change password method
    public void SetPassword(string TempPassword)
    {
        Password = TempPassword;
    }

    public void SetUserID(int TempUserID)
    {
        UserID = TempUserID;
    }

    public void SetIsActive(bool TempActive)
    {
        IsActive = TempActive;
    }

    public void SetNewUser(bool Temp)
    {
        NewUser = Temp;
    }

    public string GetEmail()
    {
        return Email;
    }

    public string GetFirstName()
    {
        try
        {
            using var connection = new SqliteConnection($"Data Source=" + Database.GetDatabasePath());
            connection.Open();

            // Get employee ID for later
            var empIDsql = "SELECT EmployeeID From User WHERE ID = @ID";
            var empIDcommand = new SqliteCommand(empIDsql, connection);
            empIDcommand.Parameters.AddWithValue("@ID", UserID);

            int employeeID = -1;
            using var reader = empIDcommand.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    employeeID = reader.GetInt32(0);
                }
            }

            var sql = "Select FirstName From Employee Where ID = @EmployeeID";
            var command = new SqliteCommand(sql, connection);
            command.Parameters.AddWithValue("@EmployeeID", employeeID);

            using var reader2 = command.ExecuteReader();
            string firstName = "";
            if (reader2.HasRows)
            {
                while (reader2.Read())
                {
                    firstName = reader2.GetString(0);
                }
            }

            return firstName;
        }
        catch (SqliteException e)
        {
            Console.WriteLine(e.Message);
            return "fail";
        }
    }
    public string GetLastName()
    {
        try
        {
            using var connection = new SqliteConnection($"Data Source=" + Database.GetDatabasePath());
            connection.Open();

            // Get employee ID for later
            var empIDsql = "SELECT EmployeeID From User WHERE ID = @ID";
            var empIDcommand = new SqliteCommand(empIDsql, connection);
            empIDcommand.Parameters.AddWithValue("@ID", UserID);

            int employeeID = -1;
            using var reader = empIDcommand.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    employeeID = reader.GetInt32(0);
                }
            }

            var sql = "Select LastName From Employee Where ID = @EmployeeID";
            var command = new SqliteCommand(sql, connection);
            command.Parameters.AddWithValue("@EmployeeID", employeeID);

            using var reader2 = command.ExecuteReader();
            string lastName = "";
            if (reader2.HasRows)
            {
                while (reader2.Read())
                {
                    lastName = reader2.GetString(0);
                }
            }

            Console.WriteLine("Last Name: " + lastName);
            return lastName;
        }
        catch (SqliteException e)
        {
            Console.WriteLine(e.Message);
            return "fail";
        }
    }
    public string GetDoB()
    {
        try
        {
            using var connection = new SqliteConnection($"Data Source=" + Database.GetDatabasePath());
            connection.Open();

            // Get employee ID for later
            var empIDsql = "SELECT EmployeeID From User WHERE ID = @ID";
            var empIDcommand = new SqliteCommand(empIDsql, connection);
            empIDcommand.Parameters.AddWithValue("@ID", UserID);

            int employeeID = -1;
            using var reader = empIDcommand.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    employeeID = reader.GetInt32(0);
                }
            }

            var sql = "Select DoB From Employee Where ID = @EmployeeID";
            var command = new SqliteCommand(sql, connection);
            command.Parameters.AddWithValue("@EmployeeID", employeeID);

            using var reader2 = command.ExecuteReader();
            string DoB = "";
            if (reader2.HasRows)
            {
                while (reader2.Read())
                {
                    DoB = reader2.GetString(0);
                }
            }

            return DoB;
        }
        catch (SqliteException e)
        {
            Console.WriteLine(e.Message);
            return "fail";
        }
    }
    public string GetAddress()
    {
        try
        {
            using var connection = new SqliteConnection($"Data Source=" + Database.GetDatabasePath());
            connection.Open();
            
            // Get employee ID for later
            var empIDsql = "SELECT EmployeeID From User WHERE ID = @ID";
            var empIDcommand = new SqliteCommand(empIDsql, connection);
            empIDcommand.Parameters.AddWithValue("@ID", UserID);

            int employeeID = -1;
            using var reader = empIDcommand.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    employeeID = reader.GetInt32(0);
                }
            }

            var sql = "Select Address From Employee Where ID = @EmployeeID";
            var command = new SqliteCommand(sql, connection);
            command.Parameters.AddWithValue("@EmployeeID", employeeID);

            using var reader2 = command.ExecuteReader();
            string Address = "";
            if (reader2.HasRows)
            {
                while (reader2.Read())
                {
                    Address = reader2.GetString(0);
                }
            }

            Console.WriteLine("Address: " + Address);
            return Address;
        }
        catch (SqliteException e)
        {
            Console.WriteLine(e.Message);
            return "fail";
        }
    }
    public string GetRole()
    {
        try
        {
            using var connection = new SqliteConnection($"Data Source=" + Database.GetDatabasePath());
            connection.Open();

            // Get employee ID for later
            var empIDsql = "SELECT EmployeeID From User WHERE ID = @ID";
            var empIDcommand = new SqliteCommand(empIDsql, connection);
            empIDcommand.Parameters.AddWithValue("@ID", UserID);

            int employeeID = -1;
            using var reader = empIDcommand.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    employeeID = reader.GetInt32(0);
                }
            }

            var sql = "Select IsAdmin, IsManager From Employee Where ID = @EmployeeID";
            var command = new SqliteCommand(sql, connection);
            command.Parameters.AddWithValue("@EmployeeID", employeeID);

            using var reader2 = command.ExecuteReader();
            string isAdmin = "";
            string isManager = "";
            if (reader2.HasRows)
            {
                while (reader2.Read())
                {
                    isAdmin = reader2.GetString(0);
                    isManager = reader2.GetString(1);
                }
            }

            if (isAdmin == "1")
            {
                return "Admin";
            }
            else if (isManager == "1")
            {
                return "Manager";
            }
            else
            {
                return "Accountant";
            }

        }
        catch (SqliteException e)
        {
            Console.WriteLine(e.Message);
            return "fail";
        }
    }
    
    //VerifyLogin takes in a temp username and password, queries the database to find that username and,
    //if valid and user is not suspended, then instantiate and return a new User
    public static User VerifyLoginB(string TempUsername, string TempPassword)
    {
        string StoredPassword = "";
        int StoredUserID = -1;
        string StoredEmail = "";
        int StoredNew = -1;
        int StoredActive = -1;
        int StoredEmployeeID = -1;
        int TempAdmin = -1;
        int TempManager = -1;
        
        var UserSQL = "select * from User where Username = @USERNAME";
        var EmployeeSQL = "select IsAdmin, IsManager from Employee where ID = @EMPLOYEEID";
        try
        {
            using var Connection = new SqliteConnection($"Data Source=" + Database.GetDatabasePath());
            Connection.Open();

            using var UserCommand = new SqliteCommand(UserSQL, Connection);
            UserCommand.Parameters.AddWithValue("@USERNAME", TempUsername);

            using var reader = UserCommand.ExecuteReader();
            //If there was a match in username, read out the string and assign values to compare password,
            //and determine if the user is an admin or manager or neither
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    StoredUserID = Int32.Parse(reader.GetString(0));
                    StoredPassword = reader.GetString(2);
                    StoredEmail = reader.GetString(3);
                    StoredNew = Int32.Parse(reader.GetString(4));
                    StoredActive = Int32.Parse(reader.GetString(5));
                    StoredEmployeeID = Int32.Parse(reader.GetString(6));
                }
            }
            StoredPassword = LedgrLogic.Password.Decrypt(StoredPassword);
            
            //Querying the Employee table to see if user is an admin or a manager
            using var EmployeeCommand = new SqliteCommand(EmployeeSQL, Connection);
            EmployeeCommand.Parameters.AddWithValue("@EMPLOYEEID", StoredEmployeeID);

            using var EmployeeReader = EmployeeCommand.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    TempAdmin = Int32.Parse(EmployeeReader.GetString(0));
                    TempManager = Int32.Parse(EmployeeReader.GetString(1));
                }
            }
            Connection.Close();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
        //Turning the IsActive and NewUser into booleans as they are stored in the database as integers
        bool Active;
        bool NewUser;
        if (StoredActive == 1)
        {
            Active = true;
        }
        else
        {
            Active = false;
        }

        if (StoredNew == 1)
        {
            NewUser = true;
        }
        else
        {
            NewUser = false;
        }
        //If password is verified and the user is not inactive
        if (StoredPassword.Equals(TempPassword) && Active)
        {
            if (TempAdmin == 1)
            {
                return new Admin(TempUsername, TempPassword, StoredEmail, StoredUserID, StoredEmployeeID, Active, NewUser);
            }
            else if (TempManager == 1)
            {
                return new Manager(TempUsername, TempPassword, StoredEmail, StoredUserID, StoredEmployeeID, Active, NewUser);
            }

            return new User(TempUsername, TempPassword, StoredEmail, StoredUserID, StoredEmployeeID, Active, NewUser);
        }
        //If password didn't match return null? Should probably just throw an error
        return null;
    }
    
    public static async Task<User> VerifyLogin(string TempUsername, string TempPassword)
    {
        string StoredPassword = "";
        string StoredUsername = "";
        int StoredUserID = -1;
        string StoredEmail = "";
        int StoredNew = -1;
        int StoredActive = -1;
        int StoredEmployeeID = -1;
        int TempAdmin = -1;
        int TempManager = -1;
        
        var UserSQL = "select * from User where Username = @USERNAME";
        var EmployeeSQL = "Select * from Employee where ID = @EMPLOYEEID";
        try
        {
            using var Connection = new SqliteConnection($"Data Source=" + Database.GetDatabasePath());
            Connection.Open();

            using var UserCommand = new SqliteCommand(UserSQL, Connection);
            UserCommand.Parameters.AddWithValue("@USERNAME", TempUsername);

            using var reader = UserCommand.ExecuteReader();
            //If there was a match in username, read out the string and assign values to compare password,
            //and determine if the user is an admin or manager or neither
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    StoredUserID = Int32.Parse(reader.GetString(0));
                    StoredUsername = reader.GetString(1);
                    StoredPassword = reader.GetString(2);
                    StoredEmail = reader.GetString(3);
                    StoredNew = Int32.Parse(reader.GetString(4));
                    StoredActive = Int32.Parse(reader.GetString(5));
                    StoredEmployeeID = Int32.Parse(reader.GetString(6));
                }
            }

            if (StoredUsername != TempUsername)
            {
                throw new InvalidUsernameException("No user exists with that username.");
            }
            StoredPassword = LedgrLogic.Password.Decrypt(StoredPassword);
            
            //Querying the Employee table to see if user is an admin or a manager
            using var EmployeeCommand = new SqliteCommand(EmployeeSQL, Connection);
            EmployeeCommand.Parameters.AddWithValue("@EMPLOYEEID", StoredEmployeeID);

            using var EmployeeReader = EmployeeCommand.ExecuteReader();
            if (EmployeeReader.HasRows)
            {
                while (EmployeeReader.Read())
                {
                    TempAdmin = Int32.Parse(EmployeeReader.GetString(5));
                    TempManager = Int32.Parse(EmployeeReader.GetString(6));
                }
            }
            await Connection.CloseAsync();
        }
        catch (SqliteException e)
        {
            Console.WriteLine(e);
        }
        //Turning the IsActive and NewUser into booleans as they are stored in the database as integers
        bool Active;
        bool NewUser;
        if (StoredActive == 1)
        {
            Active = true;
        }
        else
        {
            Active = false;
            throw new InactiveUserException("The account you are trying to access is suspended. Please contract an administrator for support.");
        }

        if (StoredNew == 1)
        {
            NewUser = true;
        }
        else
        {
            NewUser = false;
        }
        //If password is verified and the user is not inactive
        if (StoredPassword.Equals(TempPassword) && Active)
        {
            if (TempAdmin == 1)
            {
                return new Admin(TempUsername, TempPassword, StoredEmail, StoredUserID, StoredEmployeeID, Active, NewUser);
            }
            else if (TempManager == 1)
            {
                return new Manager(TempUsername, TempPassword, StoredEmail, StoredUserID, StoredEmployeeID, Active, NewUser);
            }

            return new User(TempUsername, TempPassword, StoredEmail, StoredUserID, StoredEmployeeID, Active, NewUser);
        }
        //If password didn't match return null? Should probably just throw an error
        throw new InvalidPasswordException("Password is incorrect.");
    }
    
    public static string GenerateUsername(string TempFirst, string TempLast)
    {
        //Adds first letter of firstname to lastname
        string Username = TempFirst.ToCharArray()[0] + TempLast;
        string Today = DateTime.Now.ToString("yy-MM-dd");
        
        //Adding just the month and day to the username (MM DD)
        Username += "" +Today.ToCharArray()[3] + "" +Today.ToCharArray()[4] + "" +Today.ToCharArray()[6] + ""+ Today.ToCharArray()[7] + "";

        return Username;
    }

    public static async Task<User> GetUserFromUserName(string Username)
    {
        int StoredUserID = -1;
        string TempUsername = "";
        string StoredPassword = "";
        string StoredEmail = "";
        int StoredNew = -1;
        int StoredActive = -1;
        int StoredEmployeeID = -1;
        
        var sql = "SELECT * FROM USER WHERE USERNAME = @USERNAME";
        try
        {
            using var Connection = new SqliteConnection($"Data Source=" + Database.GetDatabasePath());
            Connection.Open();

            using var UserCommand = new SqliteCommand(sql, Connection);
            UserCommand.Parameters.AddWithValue("@USERNAME", Username);

            using var reader = UserCommand.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    StoredUserID = Int32.Parse(reader.GetString(0));
                    TempUsername = (reader.GetString(1));
                    StoredPassword = LedgrLogic.Password.Decrypt(reader.GetString(2));
                    StoredEmail = reader.GetString(3);
                    StoredNew = Int32.Parse(reader.GetString(4));
                    StoredActive = Int32.Parse(reader.GetString(5));
                    StoredEmployeeID = Int32.Parse(reader.GetString(6));
                }
            }
            
            bool New = StoredNew == 1;
            bool Active = StoredActive == 1;
            
            User userData = new User(TempUsername, StoredPassword, StoredEmail, StoredUserID, StoredEmployeeID, Active, New);
            await Connection.CloseAsync();
            return userData;

        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
        return null;
    }
    public static bool CreatePotentialUser(string tempUsername, string tempPassword, string tempEmail,
        string tempFirst, string tempLast, string tempDoB, string tempAddress, string Q1, string A1, string Q2, string A2, string Q3, string A3)
    {
        bool Successful = true;
        var sql = "INSERT INTO PotentialUser " +
                  "VALUES (NULL, @USERNAME, @PASSWORD, @EMAIL, @NEWUSER, @ISACTIVE, @FIRSTNAME, @LASTNAME, @DOB, @ADDRESS, @ISADMIN, @ISMANAGER, @Q1, @A1, @Q2, @A2, @Q3, @A3)";
        try
        {
            using var connection = new SqliteConnection($"Data Source=" + Database.GetDatabasePath());
            connection.Open();

            using var command = new SqliteCommand(sql, connection);
            command.Parameters.AddWithValue("@ID", 5);

            command.Parameters.AddWithValue("@USERNAME", tempUsername);
            command.Parameters.AddWithValue("@PASSWORD", tempPassword);
            command.Parameters.AddWithValue("@EMAIL", tempEmail);
            command.Parameters.AddWithValue("@NEWUSER", 1);
            command.Parameters.AddWithValue("@ISACTIVE", 0);
            command.Parameters.AddWithValue("@FIRSTNAME", tempFirst);
            command.Parameters.AddWithValue("@LASTNAME", tempLast);
            command.Parameters.AddWithValue("@DOB", tempDoB);
            command.Parameters.AddWithValue("@ADDRESS", tempAddress);
            command.Parameters.AddWithValue("@ISADMIN", 0);
            command.Parameters.AddWithValue("@ISMANAGER", 0);
            command.Parameters.AddWithValue("@Q1", Q1);
            command.Parameters.AddWithValue("@A1", A1);
            command.Parameters.AddWithValue("@Q2", Q2);
            command.Parameters.AddWithValue("@A2", A2);
            command.Parameters.AddWithValue("@Q3", Q3);
            command.Parameters.AddWithValue("@A3", A3);

            using var reader = command.ExecuteReader();
            
            connection.Close();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            Successful = false;
        }
        return Successful;
    }

    public static async Task RejectUser(int ID)
    {
        var sql = "DELETE FROM PotentialUser WHERE ID = @ID";
        try
        {
            using var connection = new SqliteConnection($"Data Source=" + Database.GetDatabasePath());
            connection.Open();
            
            var sqlCommand = new SqliteCommand(sql, connection);
            sqlCommand.Parameters.AddWithValue("@ID", ID);
            
            int rowsDel = sqlCommand.ExecuteNonQuery();
            Console.WriteLine("Deleted Row: " + rowsDel);

            await connection.CloseAsync();

        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
        
    }
    
    public static async Task<List<string>> GetSecurityQuestions(int tempUserID)
    {
        List<string> SecurityQuestions = new List<string>();
        var sql = "SELECT Question, Answer FROM SecurityQuestion WHERE UserID = @USERID";
        try
        {
            using var connection = new SqliteConnection($"Data Source=" + Database.GetDatabasePath());
            connection.Open();

            using var command = new SqliteCommand(sql, connection);
            command.Parameters.AddWithValue("@USERID", tempUserID);
            using var reader = command.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    for (int i = 0; i < 2; i++)
                    {
                        SecurityQuestions.Add(reader.GetString(i));
                    }
                }
                
            }
            
            await connection.CloseAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return null;
        }

        return SecurityQuestions;
    }

    public static bool CheckSecurityQuestion(string Answer, string UserInput)
    {
        if (Answer.Equals(UserInput))
        {
            return true;
        }

        return false;
    }
    
     public static bool ChangePassword(string TempUsername, string TempPassword)
     {
      bool Successful = true;
      string storedPassword = "";
      int StoredUserID = -1;
      try
      {
          var sql = "SELECT Password, ID From User WHERE Username = @USERNAME";
          using var connection = new SqliteConnection($"Data Source=" + Database.GetDatabasePath());
          connection.Open();

          using var command = new SqliteCommand(sql, connection);
          command.Parameters.AddWithValue("@USERNAME", TempUsername);
          using var reader = command.ExecuteReader();
          if (reader.HasRows)
          {
              while (reader.Read())
              {
                  storedPassword = LedgrLogic.Password.Decrypt(reader.GetString(0));
                  StoredUserID = int.Parse(reader.GetString(1));
              }
          }

          if (storedPassword.Equals(TempPassword))
          {
              throw new PasswordUsedBeforeException("Please enter a password that has not been used in the past");
          }
          
          var sqlEXP = "SELECT Password FROM ExpiredPassword WHERE UserID = @UserID";
          List<string> ExpiredPasswords = new List<string>();
          
          var expiredCommand = new SqliteCommand(sqlEXP, connection);
          expiredCommand.Parameters.AddWithValue("@UserID", StoredUserID);
          
          using var expiredReader = expiredCommand.ExecuteReader();
          if (expiredReader.HasRows)
          {
              while (expiredReader.Read())
              {
                  ExpiredPasswords.Add(expiredReader.GetString(0));
                  Console.WriteLine(expiredReader.GetString(0));
              }
          }
          
          connection.Close();

          foreach (string expiredPassword in ExpiredPasswords)
          {
              if (expiredPassword.Equals(TempPassword))
              {
                  Console.WriteLine("Password used before: " + TempPassword);
                  throw new PasswordUsedBeforeException("Please enter a password that has not been used in the past");
              }
          }
      }
      catch (SqliteException e)
      {
          Console.WriteLine(e);
          return false;
      }
      
      //Updating new password, old password now stored in expired password table
      try
      {
          string encryptedPassword = LedgrLogic.Password.Encrypt(TempPassword);
          var UserSQL = "UPDATE User SET Password = @NEWPASSWORD WHERE Username = @USERNAME";
          using var connection = new SqliteConnection($"Data Source=" + Database.GetDatabasePath());
          connection.Open();

          using var UserCommand = new SqliteCommand(UserSQL, connection);
          UserCommand.Parameters.AddWithValue("@NEWPASSWORD", encryptedPassword);
          UserCommand.Parameters.AddWithValue("@USERNAME", TempUsername);

          var ExpiredPasswordSQL = "INSERT INTO ExpiredPassword " +
                                   "VALUES (NULL, @STOREDPASSWORD, @USERID)";
          using var PasswordCommand = new SqliteCommand(ExpiredPasswordSQL, connection);
          PasswordCommand.Parameters.AddWithValue("@STOREDPASSWORD", storedPassword);
          PasswordCommand.Parameters.AddWithValue("@USERID", StoredUserID);

          UserCommand.ExecuteNonQuery();
          PasswordCommand.ExecuteNonQuery();
          
          connection.Close();
      }
      catch (Exception e)
      {
          Console.WriteLine(e);
          return false;
      }
      
      return Successful;
     }
     
     public static int GetUserID(string tempUsername)
     {
         int tempUserID = -1;
         var sql = "SELECT ID FROM User WHERE Username = @USERNAME";
         try
         {
             using var connection = new SqliteConnection($"Data Source=" + Database.GetDatabasePath());
             connection.Open();

             using var command = new SqliteCommand(sql, connection);
             command.Parameters.AddWithValue("@USERNAME", tempUsername);
             using var reader = command.ExecuteReader();
             if (reader.HasRows)
             {
                 while (reader.Read())
                 {
                     tempUserID = int.Parse(reader.GetString(0));
                 }
             }
             connection.Close();
         }
         catch (Exception e)
         {
             Console.WriteLine(e);
             return tempUserID;
         }

         return tempUserID;
     }

     public static async Task<bool> IdentifyUser(string Username, string Email)
     {
         var sql = "SELECT EMAIL FROM User WHERE Username = @USERNAME";
         try
         {
             using var connection = new SqliteConnection($"Data Source=" + Database.GetDatabasePath());
             connection.Open();

             using var command = new SqliteCommand(sql, connection);
             command.Parameters.AddWithValue("@USERNAME", Username);

             using var reader = command.ExecuteReader();
             if (reader.HasRows)
             {
                 while (reader.Read())
                 {
                     if (reader.GetString(0).Equals(Email))
                     {
                         return true;
                     }
                 }
             }
             
             await connection.CloseAsync();
         }
         catch (Exception e)
         {
             Console.WriteLine(e);
         }

         return false;
     }
     
     public string GetProfilePicturePath()
     {
         string path = "";
         string sql = "SELECT ImagePath FROM ProfilePicture WHERE UserID = @USERID";
         try
         {
             using var connection = new SqliteConnection($"Data Source=" + Database.GetDatabasePath());
             connection.Open();

             using var command = new SqliteCommand(sql, connection);
             command.Parameters.AddWithValue("@USERID", UserID);
             using var reader = command.ExecuteReader();
             if (reader.HasRows)
             {
                 path = reader.GetString(0);
             }
             connection.Close();
         }
         catch (Exception e)
         {
             Console.WriteLine(e);
             return path;
         }

         return path;
     }
     
}