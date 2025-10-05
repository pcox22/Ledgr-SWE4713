using System.Collections;
using System.Data;
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
            await Connection.CloseAsync();
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
            Console.WriteLine("Login Successful");
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
        
        var sql = "SELECT FROM USER WHERE USERNAME = @USERNAME";
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
        string tempFirst, string tempLast, string tempDoB, string tempAddress)
    {
        bool Successful = true;
        var sql = "INSERT INTO PotentialUser " +
                  "VALUES (@ID, @USERNAME, @PASSWORD, @EMAIL, @NEWUSER, @ISACTIVE, @FIRSTNAME, @LASTNAME, @DOB, @ADDRESS, @ISADMIN, @ISMANAGER)";
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

    public static async void RejectUser(int ID)
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
    
    public static ArrayList GetSecurityQuestions(int tempUserID)
    {
        ArrayList SecurityQuestions = new ArrayList();
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
            
            connection.Close();
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
      //Validate the password first
      if (!LedgrLogic.Password.Validate(TempPassword).Equals("Success"))
      {
          return false;
      }
      
      //Check that the Password is not equal to current password
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
              return false;
          }
          connection.Close();
      }
      catch (Exception e)
      {
          Console.WriteLine(e);
          return false;
      }
      
      //Updating new password, old password now stored in expired password table
      try
      {
          var UserSQL = "UPDATE User SET Password = @NEWPASSWORD WHERE Username = @USERNAME";
          using var connection = new SqliteConnection($"Data Source=" + Database.GetDatabasePath());
          connection.Open();

          using var UserCommand = new SqliteCommand(UserSQL, connection);
          UserCommand.Parameters.AddWithValue("@NEWPASSWORD", TempPassword);
          UserCommand.Parameters.AddWithValue("@USERNAME", TempUsername);

          var ExpiredPasswordSQL = "INSERT INTO ExpiredPassword " +
                                   "VALUES (2, @STOREDPASSWORD, @USERID)";
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
    
    /*
     public bool ChangePassword(string TempPassword)
     {
      //check if the given password is equal to current password or an older password, as well as if it satisfies the password requirements
     */
}