using System.Collections;
using System.Runtime.InteropServices.JavaScript;
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
        string tempUsername = "TTest1003";
        string tempPassword = "PassW0rd...";
        string Expected = "TTest1003";
        
        //act
        User returnedUser = User.VerifyLoginB(tempUsername, tempPassword);
        string Actual = returnedUser.GetUserName();
        
        //assert
        Assert.That(Actual, Is.EqualTo(Expected));
    }
    
    [Test]
    public void GenerateUsername_ValidUsername_ReturnsTrue()
    {
        //arrange
        string Expected = "RStraiton1002";
        
        //act
        string Actual = User.GenerateUsername("RJ", "Straiton");
        
        //assert
        Assert.That(Actual, Is.EqualTo(Expected));
    }

    [Test]
    public void CreatePotentialUser_ValidInputs_ReturnsTrue()
    {
        //arrange
        string FirstName = "Please";
        string LastName = "Work";
        string Username = User.GenerateUsername(FirstName, LastName);
        string Password = LedgrLogic.Password.Encrypt("PassW0rd...");
        string email = "email@email.com";
        int NewUser = 0;
        int IsActive = 1;
        string DoB = "2025-10-10";
        string Address = "123 Main Street, Marietta GA";
        int Admin = 0;
        int Manager = 0;
        string q1 = "";
        string a1 = "";
        string q2 = "";
        string a2 = "";
        string q3 = "";
        string a3 = "";

        bool Expected = true;
        
        //act
        bool Actual = User.CreatePotentialUser(Username, Password, email, FirstName, LastName, DoB, Address, q1, a1, q2, a2, q3, a3);
        
        //assert
        Assert.That(Actual, Is.EqualTo(Expected));
    }

    [Test]
    public void GetUserID_ValidInput_ReturnsCorrectID()
    {
        //arrange
        int expected = 4;
        int actual;

        //act
        actual = User.GetUserID("TTest1003");

        //assert
        Assert.That(actual, Is.EqualTo(expected));
    }

    [Test]
    public void GetSecurityQuestions_ValidInput_ReturnsCorrectQuestion()
    {
        //arrange
        bool expected = true;
        
        //act
        ArrayList temp = User.GetSecurityQuestions(4);
        bool actual = temp.Contains("What was your first car?");
        
        //assert
        Assert.That(actual, Is.EqualTo(expected));
    }

    [Test]
    public void ChangePassword_ValidInput_ReturnsTrue()
    {
        //arrange
        string newPassword = "N3w_PassW0rd...";
        bool expected = true;

        //act
        bool actual = User.ChangePassword("TTest1003", newPassword);
        
        //assert
        Assert.That(actual, Is.EqualTo(expected));
    }
    
}