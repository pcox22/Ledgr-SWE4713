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
    
    //User-SRS-REQ-002
    [Test]
    public void VerifyLogin_ValidInput_ReturnsCorrectUsername()
    {
        //arrange
        string tempUsername = "pcox0930";
        string tempPassword = "AppD0m4!N";
        string Expected = "pcox0930";
        
        //act
        User returnedUser = User.VerifyLoginB(tempUsername, tempPassword);
        string Actual = returnedUser.GetUserName();
        
        //assert
        Assert.That(Actual, Is.EqualTo(Expected));
    }
    
    //User-SRS-REQ-001
    [Test]
    public void GenerateUsername_ValidUsername_ReturnsTrue()
    {
        //arrange
        string Expected = "RStraiton1207";
        
        //act
        string Actual = User.GenerateUsername("RJ", "Straiton");
        
        //assert
        Assert.That(Actual, Is.EqualTo(Expected));
    }

    //User-SRS-REQ-009
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
    
    //User-SRS-REQ-010
    [Test]
    public void GetSecurityQuestions_ValidInput_ReturnsCorrectQuestion()
    {
        //arrange
        bool expected = true;
        
        //act
        List<String> temp = User.GetSecurityQuestions(4).Result;
        bool actual = temp.Contains("What was your first car?");
        
        //assert
        Assert.That(actual, Is.EqualTo(expected));
    }
    
    
    //User-SRS-REQ-006
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
    
    //User-SRS-REQ-011
    [Test]
    public void GetCurrentRatio_ReturnsCorrectRatio()
    {
        //expected
        double expected = 17.64;
        
        //act
        double actual = User.GetCurrentRatio();
        actual = Math.Round(actual, 2);
        
        //assert
        Assert.That(actual.Equals(expected));
    }
    
    //User-SRS-REQ-012
    [Test]
    public void GetQuickRatio_ReturnsCorrectRatio()
    {
        //expected
        double expected = 17.64;
        
        //act
        double actual = User.GetQuickRatio();
        actual = Math.Round(actual, 2);
        
        //assert
        Assert.That(actual.Equals(expected));
    }
    
    //User-SRS-REQ-013
    [Test]
    public void GetWorkingCapital_ReturnsCorrectAmount()
    {
        //expected
        double expected = 16975;
        
        //act
        double actual = User.GetWorkingCapital();
        actual = Math.Round(actual, 2);
        
        //assert
        Assert.That(actual.Equals(expected));
    }
    
    //User-SRS-REQ-014
    [Test]
    public void GetNetProfitMargin_ReturnsCorrectRatio()
    {
        //expected
        double expected = 0.34;
        
        //act
        double actual = User.GetNetProfitMargin();
        actual = Math.Round(actual, 2);
        
        //assert
        Assert.That(actual.Equals(expected));
    }
    
    //User-SRS-REQ-015
    [Test]
    public void GetRoA_ReturnsCorrectRatio()
    {
        //expected
        double expected = 0.16;
        
        //act
        double actual = User.GetRoA();
        actual = Math.Round(actual, 2);
        
        //assert
        Assert.That(actual.Equals(expected));
    }
    
    //User-SRS-REQ-016
    [Test]
    public void GetRoE_ReturnsCorrectRatio()
    {
        //expected
        double expected = 0.22;
        
        //act
        double actual = User.GetRoE();
        actual = Math.Round(actual, 2);
        
        //assert
        Assert.That(actual.Equals(expected));
    }
    
    //User-SRS-REQ-017
    [Test]
    public void GetDebtRatio_ReturnsCorrectRatio()
    {
        //expected
        double expected = 0.07;
        
        //act
        double actual = User.GetDebtRatio();
        actual = Math.Round(actual, 2);
        
        //assert
        Assert.That(actual.Equals(expected));
    }
    
    //User-SRS-REQ-018
    [Test]
    public void GetDtE_ReturnsCorrectRatio()
    {
        //expected
        double expected = 0.10;
        
        //act
        double actual = User.GetDtE();
        actual = Math.Round(actual, 2);
        
        //assert
        Assert.That(actual.Equals(expected));
    }
    
}