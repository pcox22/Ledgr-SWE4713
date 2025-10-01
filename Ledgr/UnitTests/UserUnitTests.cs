using LedgrLogic;

namespace UnitTests;

//All unit tests relating to the User Class
public class UserUnitTests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void VerifyLogin_ValidInput_ReturnsCorrectUsername()
    {
        //arrange
        string tempUsername = "TTest0926";
        string tempPassword = "PassW0rd...";
        string Expected = "TTest0926";
        
        //act
        User returnedUser = User.VerifyLogin(tempUsername, tempPassword);
        string Actual = returnedUser.GetUserName();
        
        //assert
        Assert.That(Actual, Is.EqualTo(Expected));
    }
    
    [Test]
    public void GenerateUsername_ValidUsername_ReturnsTrue()
    {
        //arrange
        string Expected = "RStraiton1001";
        
        //act
        string Actual = User.GenerateUsername("RJ", "Straiton");
        
        //assert
        Assert.That(Actual, Is.EqualTo(Expected));
    }

    [Test]
    public void CreatePotentialUser_ValidInputs_ReturnsTrue()
    {
        //arrange
        string FirstName = "Michael";
        string LastName = "Liu";
        string Username = User.GenerateUsername(FirstName, LastName);
        string Password = "PassW0rd...";
        string email = "email@email.com";
        int NewUser = 0;
        int IsActive = 1;
        string DoB = "2025-10-10";
        string Address = "123 Main Street, Marietta GA";
        int Admin = 0;
        int Manager = 0;

        bool Expected = true;
        
        //act
        bool Actual = User.CreatePotentialUser(Username, Password, email, NewUser, IsActive, FirstName, LastName, DoB,
            Address, Admin, Manager);
        
        //assert
        Assert.That(Actual, Is.EqualTo(Expected));
    }
}