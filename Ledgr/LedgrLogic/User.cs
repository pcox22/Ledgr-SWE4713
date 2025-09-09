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

    //Default constructor, to be used when Admin is creating a new user
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
    
    /*
    public bool Login(string TempUsername, string TempPassword)
    {
     //Takes in a username and password, queries the database for matching username, if none is found return false
     //If a matching user is found, see if Password given and stored password are the same
    }
    */
    
    /*
     public bool ChangePassword(string TempPassword)
     {
      //check if the given password is equal to current password or an older password, as well as if it satisfies the password requirements
     */
}