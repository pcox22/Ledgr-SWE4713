using LedgrLogic;
using NUnit.Framework.Internal.Execution;

namespace UnitTests;

//All unit tests relating to the Accountant Class
public class EventLogUnitTests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void LogUser_ValidInput_ReturnsTrue()
    {
        //arrange
        string newEmail = "mliu10@Ledgr.com";
        bool expected = true;
        bool before;
        bool after;
        bool actual;
        Admin temp = new Admin();
        
        //act
        before = EventLog.LogUser('b', 13, 1);
        temp.UpdateEmail(13, newEmail, "pcox0930");
        after = EventLog.LogUser('a', 13, 1);

        if (before && after)
        {
            actual = true;
        }
        else
        {
            actual = false;
        }

        //assert
        Assert.That(actual, Is.EqualTo(expected));
    }
    
    [Test]
    public void LogEmployee_ValidInput_ReturnsTrue()
    {
        //arrange
        string newLastName = "Log-Employee";
        bool expected = true;
        bool before;
        bool after;
        bool actual;
        Admin temp = new Admin();
        
        //act
        before = EventLog.LogEmployee('b', 4, 1);
        temp.UpdateLastName(4, newLastName, "pcox0930");
        after = EventLog.LogEmployee('a', 4, 1);

        if (before && after)
        {
            actual = true;
        }
        else
        {
            actual = false;
        }

        //assert
        Assert.That(actual, Is.EqualTo(expected));
    }
    
    [Test]
    public void LogAccount_ValidInput_ReturnsTrue()
    {
        //arrange
        string newCategory = "Log-Account-Test";
        bool expected = true;
        bool before;
        bool after;
        bool actual;
        Admin temp = new Admin();
        
        //act
        before = EventLog.LogAccount('b', 1, 1);
        temp.EditAccountCategory(1, newCategory, "pcox0930");
        after = EventLog.LogAccount('a', 1, 1);

        if (before && after)
        {
            actual = true;
        }
        else
        {
            actual = false;
        }

        //assert
        Assert.That(actual, Is.EqualTo(expected));
    }
}