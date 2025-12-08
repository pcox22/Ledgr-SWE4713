using System.Collections;
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
        string knownUsername = "ecox1008";
        
        //act
        ArrayList storedUsers = Admin.UserReport();
        
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
        actual = temp.UpdateUsername(4, newUsername);
        
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
        actual = temp.UpdateEmail(4, newEmail);
        
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
        actual = temp.UpdateFirstName(1005, newFirstName);
        
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
        actual = temp.UpdateLastName(1005, newLastName);
        
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
        actual = temp.UpdateDoB(1005, newDoB);
        
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
        actual = temp.UpdateAddress(1005, newAddress);
        
        //assert
        Assert.That(actual, Is.EqualTo(expected));
    }
    
    [Test]
    //TEST ID: Admin-SRS-REQ-009
    public void ApproveUser_ValidInput_ReturnsTrue()
    {
        //arrange
        int tempID = 19;
        bool expected = true;
        bool actual;
        
        //act
        actual = Admin.ApproveUser(tempID).Result;
        
        //assert
        Assert.That(actual, Is.EqualTo(expected));
    }
    
    [Test]
    //TEST ID: Admin-SRS-REQ-010
    public  void DeactivateUser_ValidInput_ReturnsTrue()
    {
        //arrange
        bool expected = true;
        
        //act
        bool actual = Admin.DeactivateUser("ecox1008", "2025-12-07", "2025-12-15").IsCompletedSuccessfully;
        
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
        actual = Admin.ActivateUser(15).IsCompletedSuccessfully;
        
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
        actual = temp.PromoteToManager(1003);
        
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
        actual = temp.PromoteToAdmin(1003);
        
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
        actual = temp.DemoteFromManager(1003);
        
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
        actual = temp.DemoteFromAdmin(1003);
        
        //assert
        Assert.That(actual, Is.EqualTo(expected));
    }
    
    [Test]
    //TEST ID: Admin-SRS-REQ-016
    public void EditAccountName_ValidInput_ReturnsTrue()
    {
        //arrange
        Admin temp = new Admin();
        string newAccountName = "Inventory Merchandising";
        bool expected = true;
        bool actual;
        
        //act
        actual = temp.EditAccountName(12345, newAccountName);
        
        //assert
        Assert.That(actual, Is.EqualTo(expected));
    }
    
    [Test]
    //TEST ID: Admin-SRS-REQ-017
    public void EditAccountDescription_ValidInput_ReturnsTrue()
    {
        //arrange
        Admin temp = new Admin();
        string newAccountDesc = "";
        bool expected = true;
        bool actual;
        
        //act
        actual = temp.EditAccountDescription(4001, newAccountDesc);
        
        //assert
        Assert.That(actual, Is.EqualTo(expected));
    }
    
    [Test]
    //TEST ID: Admin-SRS-REQ-018
    public void EditAccountNormalSide_ValidInput_ReturnsTrue()
    {
        //arrange
        Admin temp = new Admin();
        char newNormalSide = 'L';
        bool expected = true;
        bool actual;
        
        //act
        actual = temp.EditAccountNormalSIde(1001, newNormalSide);
        
        //assert
        Assert.That(actual, Is.EqualTo(expected));
    }
    
    [Test]
    //TEST ID: Admin-SRS-REQ-019
    public void EditAccountCategory_ValidInput_ReturnsTrue()
    {
        //arrange
        Admin temp = new Admin();
        string newCategory = "Liability";
        bool expected = true;
        bool actual;
        
        //act
        actual = temp.EditAccountCategory(2001, newCategory);
        
        //assert
        Assert.That(actual, Is.EqualTo(expected));
    }
    
    [Test]
    //TEST ID: Admin-SRS-REQ-020
    public void EditAccountSubCategory_ValidInput_ReturnsTrue()
    {
        //arrange
        Admin temp = new Admin();
        string newSubCategory = "Current Asset";
        bool expected = true;
        bool actual;
        
        //act
        actual = temp.EditAccountSubCategory(1001, newSubCategory);
        
        //assert
        Assert.That(actual, Is.EqualTo(expected));
    }
    
    [Test]
    //TEST ID: Admin-SRS-REQ-021
    public void EditAccountInitialBalance_ValidInput_ReturnsTrue()
    {
        //arrange
        Admin temp = new Admin();
        double newInitBalance = 1234.56;
        bool expected = true;
        bool actual;
        
        //act
        actual = temp.EditAccountInitialBalance(12345, newInitBalance);
        
        //assert
        Assert.That(actual, Is.EqualTo(expected));
    }
    
    [Test]
    //TEST ID: Admin-SRS-REQ-022
    public void EditAccountDebit_ValidInput_ReturnsTrue()
    {
        //arrange
        Admin temp = new Admin();
        double newDebit = 500.56;
        bool expected = true;
        bool actual;
        
        //act
        actual = temp.EditAccountDebit(12345, newDebit);
        
        //assert
        Assert.That(actual, Is.EqualTo(expected));
    }
    
    [Test]
    //TEST ID: Admin-SRS-REQ-023
    public void EditAccountCredit_ValidInput_ReturnsTrue()
    {
        //arrange
        Admin temp = new Admin();
        double newCredit = 850.00;
        bool expected = true;
        bool actual;
        
        //act
        actual = temp.EditAccountCredit(12345, newCredit);
        
        //assert
        Assert.That(actual, Is.EqualTo(expected));
    }
    
    [Test]
    //TEST ID: Admin-SRS-REQ-024
    public void EditAccountBalance_ValidInput_ReturnsTrue()
    {
        //arrange
        Admin temp = new Admin();
        double newBalance = 500.56;
        bool expected = true;
        bool actual;
        
        //act
        actual = temp.EditAccountBalance(12345, newBalance);
        
        //assert
        Assert.That(actual, Is.EqualTo(expected));
    }
    
    [Test]
    //TEST ID: Admin-SRS-REQ-025
    public void EditAccountOrder_ValidInput_ReturnsTrue()
    {
        //arrange
        Admin temp = new Admin();
        int newOrder = 1;
        bool expected = true;
        bool actual;
        
        //act
        actual = temp.EditAccountOrder(5001, newOrder);
        
        //assert
        Assert.That(actual, Is.EqualTo(expected));
    }
    
    [Test]
    //TEST ID: Admin-SRS-REQ-026
    public void EditAccountStatement_ValidInput_ReturnsTrue()
    {
        //arrange
        Admin temp = new Admin();
        string newStatement = "BS";
        bool expected = true;
        bool actual;
        
        //act
        actual = temp.EditAccountStatement(1001, newStatement);
        
        //assert
        Assert.That(actual, Is.EqualTo(expected));
    }
    
    [Test]
    //TEST ID: Admin-SRS-REQ-027
    public void DeactivateAccount_AccountBalanceGreaterThanZero_ThrowsInvalidChangeException()
    {
        
        //assert
        Assert.Throws<InvalidChangeException>(() => Admin.DeactivateAccount(1001));
    }
    
    [Test]
    //TEST ID: Admin-SRS-REQ-028
    public void DeactivateAccount_AccountBalanceEqualToZero_ReturnsTrue()
    {
        //arrange
        Admin temp = new Admin();
        temp.EditAccountBalance(1001, 0);
        bool expected = true;
        bool actual;
        
        //act
       actual = Admin.DeactivateAccount(12345).Equals(true);
            
        //assert
        Assert.That(actual, Is.EqualTo(expected));
    }
    
    [Test]
    //TEST ID: Admin-SRS-REQ-029
    public void ActivateAccount_ValidInput_ReturnsTrue()
    {
        //arrange
        Admin temp = new Admin();
        bool expected = true;
        bool actual;
        
        //act
        actual = temp.ActivateAccount(1001);
        
        //assert
        Assert.That(actual, Is.EqualTo(expected));
    }
    
    [Test]
    //TEST ID: Admin-SRS-REQ-030
    public void GetEventLog_Account_ReturnsTrue()
    {
        //arrange
        List<string> accountEventLog;
        string knownAccountName = "Cash";
        
        //act
        accountEventLog = Admin.GetEventLog("Account").Result;
        
        //assert
        if (accountEventLog.Contains(knownAccountName))
        {
            Assert.Pass();
        }
        else
        {
            Assert.Fail();
        }
    }
    
    [Test]
    //TEST ID: Admin-SRS-REQ-031
    public void GetEventLog_User_ReturnsTrue()
    {
        //arrange
        List<string> userEventLog;
        string knownUsername = "MLiu1001";
        
        //act
        userEventLog = Admin.GetEventLog("User").Result;
        
        //assert
        if (userEventLog.Contains(knownUsername))
        {
            Assert.Pass();
        }
        else
        {
            Assert.Fail();
        }
    }
    
    [Test]
    //TEST ID: Admin-SRS-REQ-032
    public void GetEventLog_Employee_ReturnsTrue()
    {
        //arrange
        List<string> employeeEventLog;
        string knownAddress = "1100 South Marietta Parkway";
        
        //act
        employeeEventLog = Admin.GetEventLog("Employee").Result;
        
        //assert
        if (employeeEventLog.Contains(knownAddress))
        {
            Assert.Pass();
        }
        else
        {
            Assert.Fail();
        }
    }
    
    [Test]
    //TEST ID: Admin-SRS-REQ-033
    public void CreateAccount_ValidInput_ReturnsTrue()
    {
        //arrange
        bool expected = true;
        bool actual;
        
        //act
        actual = Admin.CreateAccount(2003, "Salaries Payable", "", 'R', "Liability",
            "Current Liability", 0.00, 0.00,0.00, 0.00, "2025-10-14", 1, 2, "BS", "pcox0930");
        
        //assert
       Assert.That(actual, Is.EqualTo(expected));
    }
}