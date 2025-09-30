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
}