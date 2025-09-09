namespace LedgrLogic;

public class Admin : User
{
    //Needs access to database, should be bool for user feedback
    public void CreateUser()
    {
        //Creates a new user object, and then updates the database to include new user
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