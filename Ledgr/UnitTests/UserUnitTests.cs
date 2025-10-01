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
        User tempUser = new User();
        string Expected = "TTest0926";
        
        //act
        User returnedUser = tempUser.VerifyLogin(tempUsername, tempPassword);
        string Actual = returnedUser.GetUserName();
        
        //assert
        Assert.That(Actual, Is.EqualTo(Expected));
    }
    
    [Test]
    public void GenerateUsername_ValidUsername_ReturnsTrue()
    {
        //arrange
        User Temp = new User();
        string Expected = "RStraiton0930";
        
        //act
        string Actual = Temp.GenerateUsername("RJ", "Straiton");
        
        //assert
        Assert.That(Actual, Is.EqualTo(Expected));
    }
}