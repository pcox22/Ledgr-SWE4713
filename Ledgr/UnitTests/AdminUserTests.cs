using LedgrLogic;
using Microsoft.Data.Sqlite;

namespace UnitTests;

//All unit tests relating to the Admin Class
public class AdminUnitTests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    //TEST ID: Admin-SRS-REQ-001
    public void ExpiredPasswordReport_ReturnsValidList()
    {
        //arrange
        List<string> storedPasswords;
        string expiredPassword = "PassW0rd...";
        
        //act
        storedPasswords = Admin.ExpiredPasswordReport();
        
        //assert
        if (storedPasswords.Contains(expiredPassword))
        {
            Assert.Pass();
        }
        Assert.Fail();
    }

    [Test]
    //TEST ID: Admin-SRS-REQ-002
    public void UserReport_ReturnsValidList()
    {
        //arrange
        List<string> storedUsers;
        string knownUsername = "pcox0930";
        
        //act
        storedUsers = Admin.UserReport();
        
        //assert
        if (storedUsers.Contains(knownUsername))
        {
            Assert.Pass();
        }
        Assert.Fail();
    }

    [Test]
    //TEST ID: Admin-SRS-REQ-003
    public void UpdateUsername_ValidInput_ReturnsTrue()
    {
        //arrange
        Admin temp = new Admin();
        string newUsername = "ATest1013";
        bool expected = true;
        bool actual;
        
        //act
        actual = temp.UpdateUsername(4, newUsername, "pcox0930");
        
        //assert
        Assert.That(actual, Is.EqualTo(expected));
    }

    [Test]
    //TEST ID: Admin-SRS-REQ-004
    public void UpdateEmail_ValidInput_ReturnsTrue()
    {
        //arrange
        Admin temp = new Admin();
        string newEmail = "ATest1013@Ledgr.com";
        bool expected = true;
        bool actual;
        
        //act
        actual = temp.UpdateEmail(4, newEmail, "pcox0930");
        
        //assert
        Assert.That(actual, Is.EqualTo(expected));
    }

    [Test]
    //TEST ID: Admin-SRS-REQ-005
    public void UpdateFirstName_ValidInput_ReturnsTrue()
    {
        //arrange
        Admin temp = new Admin();
        string newFirstName = "Robert";
        bool expected = true;
        bool actual;
        
        //act
        actual = temp.UpdateFirstName(1005, newFirstName, "pcox0930");
        
        //assert
        Assert.That(actual, Is.EqualTo(expected));
    }

    [Test]
    //TEST ID: Admin-SRS-REQ-006
    public void UpdateLastName_ValidInput_ReturnsTrue()
    {
        //arrange
        Admin temp = new Admin();
        string newLastName = "Straiton";
        bool expected = true;
        bool actual;
        
        //act
        actual = temp.UpdateLastName(1005, newLastName, "pcox0930");
        
        //assert
        Assert.That(actual, Is.EqualTo(expected));
    }
    
    [Test]
    //TEST ID: Admin-SRS-REQ-007
    public void UpdateDoB_ValidInput_ReturnsTrue()
    {
        //arrange
        Admin temp = new Admin();
        string newDoB = "2003-06-07";
        bool expected = true;
        bool actual;
        
        //act
        actual = temp.UpdateDoB(1005, newDoB, "pcox0930");
        
        //assert
        Assert.That(actual, Is.EqualTo(expected));
    }
    
    [Test]
    //TEST ID: Admin-SRS-REQ-008
    public void UpdateAddress_ValidInput_ReturnsTrue()
    {
        //arrange
        Admin temp = new Admin();
        string newAddress = "123 Main Street NE";
        bool expected = true;
        bool actual;
        
        //act
        actual = temp.UpdateAddress(1005, newAddress, "pcox0930");
        
        //assert
        Assert.That(actual, Is.EqualTo(expected));
    }
    
    [Test]
    //TEST ID: Admin-SRS-REQ-009
    public void ApproveUser_ValidInput_ReturnsTrue()
    {
        //arrange
        int tempID = 15;
        bool expected = true;
        bool actual;
        
        //act
        actual = Admin.ApproveUser(15, "pcox0930").Result;
        
        //assert
        Assert.That(actual, Is.EqualTo(expected));
    }
    
    [Test]
    //TEST ID: Admin-SRS-REQ-010
    public void DeactivateUser_ValidInput_ReturnsTrue()
    {
        //arrange
        bool expected = true;
        bool actual;
        
        //act
        actual = Admin.DeactivateUser("ecox1008", "2025-10-13", "2025-10-15","pcox0930");
        
        //assert
        Assert.That(actual, Is.EqualTo(expected));
    }
    
    [Test]
    //TEST ID: Admin-SRS-REQ-011
    public void ActivateUser_ValidInput_ReturnsTrue()
    {
        //arrange
        bool expected = true;
        bool actual;
        
        //act
        actual = Admin.ActivateUser(13, "pcox0930");
        
        //assert
        Assert.That(actual, Is.EqualTo(expected));
    }
    
    [Test]
    //TEST ID: Admin-SRS-REQ-012
    public void PromoteToManager_ValidInput_ReturnsTrue()
    {
        //arrange
        Admin temp = new Admin();
        bool expected = true;
        bool actual;
        
        //act
        actual = temp.PromoteToManager(1003, "pcox0930");
        
        //assert
        Assert.That(actual, Is.EqualTo(expected));
    }
    
    [Test]
    //TEST ID: Admin-SRS-REQ-013
    public void PromoteToAdmin_ValidInput_ReturnsTrue()
    {
        //arrange
        Admin temp = new Admin();
        bool expected = true;
        bool actual;
        
        //act
        actual = temp.PromoteToAdmin(1003, "pcox0930");
        
        //assert
        Assert.That(actual, Is.EqualTo(expected));
    }
    
    [Test]
    //TEST ID: Admin-SRS-REQ-014
    public void DemoteFromManager_ValidInput_ReturnsTrue()
    {
        //arrange
        Admin temp = new Admin();
        bool expected = true;
        bool actual;
        
        //act
        actual = temp.DemoteFromManager(1003, "pcox0930");
        
        //assert
        Assert.That(actual, Is.EqualTo(expected));
    }
    
    [Test]
    //TEST ID: Admin-SRS-REQ-015
    public void DemoteFromAdmin_ValidInput_ReturnsTrue()
    {
        //arrange
        Admin temp = new Admin();
        bool expected = true;
        bool actual;
        
        //act
        actual = temp.DemoteFromAdmin(1003, "pcox0930");
        
        //assert
        Assert.That(actual, Is.EqualTo(expected));
    }
    
    [Test]
    //TEST ID: Admin-SRS-REQ-016
    public void EditAccountName_ValidInput_ReturnsTrue()
    {
        //arrange
        Admin temp = new Admin();
        bool expected = true;
        bool actual;
        
        //act
        actual = temp.DemoteFromAdmin(1003, "pcox0930");
        
        //assert
        Assert.That(actual, Is.EqualTo(expected));
    }
}