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
    public User VerifyLogin(string TempUsername, string TempPassword)
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
    
    /*
     public bool ChangePassword(string TempPassword)
     {
      //check if the given password is equal to current password or an older password, as well as if it satisfies the password requirements
     */
}