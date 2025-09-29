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
    public void VerifyLogin_ValidInput_ReturnsTrue()
    {
        //arrange
        string tempUsername = "TTest0926";
        string tempPassword = "PassW0rd...";
        User tempUser = new User();
        bool Expected = true;
        
        //act
        bool Actual = tempUser.VerifyLogin(tempUsername, tempPassword);
        
        //assert
        Assert.That(Actual, Is.EqualTo(Expected));
    }
}