using LedgrLogic;
using Microsoft.Data.Sqlite;

namespace UnitTests;

//All unit tests relating to the Admin Class
public class PasswordUnitTests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void Encrypt_ValidInput_ReturnsDifferentString()
    {
        //arrange
        string UserInput = "AppD0m4!N";
        
        //act
        string Actual = Password.Encrypt(UserInput);
        Console.WriteLine(Actual);
        
        //assert
        Assert.That(Actual, !Is.EqualTo(UserInput));
    }

    [Test]
    public void Decrypt_EncryptedPassword_ReturnsInitialString()
    {
        //arrange
        string Expected = "PassW0rd...";
        
        //act
        string Actual = Password.Decrypt(Password.Encrypt(Expected));
        
        //assert
        Assert.That(Actual, Is.EqualTo(Expected));
    }

    [Test]
    public void Validate_ValidPassword_ReturnsSuccess()
    {
        //arrange
        string Expected = "Success";
        string UserInput = "PassW0rd...";
        
        //act
        string Actual = LedgrLogic.Password.Validate(UserInput);
        
        //assert
        Assert.That(Actual, Is.EqualTo(Expected));
    }

    [Test]
    public void Validate_NumberAsFirstCharacter_ReturnsInvalid()
    {
        //arrange
        string Expected = "First character must be a letter";
        string UserInput = "1PassW0rd...";
        
        //act
        string Actual = Password.Validate(UserInput);
        
        //assert
        Assert.That(Actual, Is.EqualTo(Expected));
    }

    [Test]
    public void Validate_SevenCharacterString_ReturnsInvalid()
    {
        //arrange
        string Expected = "Password must be at least 8 characters long";
        string UserInput = "Seven7.";
        
        //act
        string Actual = Password.Validate(UserInput);
        
        //assert
        Assert.That(Actual,Is.EqualTo(Expected));
    }

    [Test]
    public void Validate_NoNumberInString_ReturnsInvalid()
    {
        //arrange
        string Expected = "Password must contain a number";
        string UserInput = "Password...";
        
        //act
        string Actual = Password.Validate(UserInput);
        
        //assert
        Assert.That(Actual, Is.EqualTo(Expected));
    }

    [Test]
    public void Validate_NoSpecialCharacterInString_ReturnsInvalid()
    {
        //arrange
        string Expected = "Password must contain a special character";
        string UserInput = "PassW0rd";
        
        //act
        string Actual = Password.Validate(UserInput);
        
        //assert
        Assert.That(Actual, Is.EqualTo(Expected));
    }
}
    
