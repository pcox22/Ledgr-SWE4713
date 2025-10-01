using Microsoft.Data.Sqlite;

namespace LedgrLogic;

public class User
{
    protected string Username;
    protected string Password;
    protected int UserID;
    protected bool IsActive;
    protected bool NewUser;

    public User(string TempUsername, string TempPass, int TempID)
    {
        Username = TempUsername;
        Password = TempPass;
        UserID = TempID;
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
    //if its found, returns the password stored in the database (STILL NEEDS TO BE ENCRYPTED AND THEN DECRYPTED)
    public static async Task<bool> VerifyLogin(string TempUsername, string TempPassword)
    {
        string StoredPassword = "";
        int TempAdmin = -1;
        int TempManager = -1;
        int UserID;
        int TempActive = -1;
        var sql = "select PASSWORD, ISACTIVE from USER where ID = 1";
        try
        {
            //using var Connection = new SqliteConnection($"Data Source=" + Database.GetDatabasePath());
            //Connection.Open();
            string path = Directory.GetCurrentDirectory();
            var dbPath = Path.Combine(path, "LedgerDB.db");
            using var Connection = new SqliteConnection($"Data Source={dbPath}");
            Connection.Open();

            using var Command = new SqliteCommand(sql, Connection);
            Command.Parameters.AddWithValue("@USERNAME", TempUsername);

            using var reader = Command.ExecuteReader();
            //If there was a match in username, read out the string and assign values to compare password,
            //and determine if the user is an admin or manager or neither
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    StoredPassword = reader.GetString(0); 
                    //TempManager = Convert.ToInt32(reader.GetString(1));
                    //TempAdmin = Convert.ToInt32(reader.GetString(1));
                    TempActive = Convert.ToInt32(reader.GetString(1));
                    //UserID = Convert.ToInt32(reader.GetString(2));
                }
            }
            await Connection.CloseAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            //return false for failed login
            return false;
        }
        //If password is verified and the user is not inactive
        if (StoredPassword.Equals(TempPassword) && TempActive == 1)
        {
            Console.WriteLine("Login Successful");
            if (TempAdmin == 1)
            {
                
                return true;
            }
            else if (TempManager == 1)
            {
                //If the user is a manager, then call the manager login method, which will instantiate a manager, then return true
                return true;
            } 
            //If the user is neither an admin nor a manager, call the login method for a typical user then, return true
            return true;
        }
        //If password didn't match return false
        return false;
    }
    
    /*
     public bool ChangePassword(string TempPassword)
     {
      //check if the given password is equal to current password or an older password, as well as if it satisfies the password requirements
     */
}