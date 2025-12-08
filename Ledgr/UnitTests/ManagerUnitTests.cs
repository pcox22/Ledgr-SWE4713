using LedgrLogic;

namespace UnitTests;

//All unit tests relating to the Manager Class
public class ManagerUnitTests
{
    [SetUp]
    public void Setup()
    {
    }
    
    //Manager-SRS-REQ-001
    [Test]
    public void GetAccountEventLog_ReturnsCorrectList()
    {
        //arrange
        List<string> returnedValues = Manager.GetAccountEventLog(1001);
        
        //assert
        Assert.That(returnedValues.Contains("1001"));
    }

    //Manager-SRS-REQ-002
    [Test]
    public void CreateJournalEntry_ValidInput_ReturnsJournalEntryID()
    {
        //arrange
        string username = "MTest1109";
        string date = "2025-11-09";
        int id = -2;
        
        //act
        id = Manager.CreateJournalEntry(date, username);
        
        //assert
        Assert.That(!id.Equals(-2));
    }
    
    //Manager-SRS-REQ-003
    [Test]
    public void CreateJournalEntryDetails_ValidInput_ReturnsTrue()
    {
        //arrange
        bool expected = true;
        bool actual = false;
        

        //act
        actual = Manager.CreateJournalEntryDetails(1001, 0.00, "Debit", 1, "TTest01");

        //assert
        Assert.That(expected.Equals(actual));
    }
    
    //Manager-SRS-REQ-004
    [Test]
    public void ViewEntriesByStatus_ValidInput_ReturnsProperList()
    {
        //arrange
        string expectedStatus = "A";
        char status = 'A';
        
        //act
        List<string> result = Manager.ViewEntriesByStatus(status);
        
        //assert
        if (result.Contains(expectedStatus))
        {
            Assert.Pass();
        }
        Assert.Fail();
    }
    
    //Manager-SRS-REQ-005
    [Test]
    public void FilterJournalStatusDate_ValidInput_ReturnsProperList()
    {
        //arrange
        string date = "asc";
        char status = 'A';
        
        //act
        List<string> result = Manager.FilterJournalStatusDate(status, date);
        
        //assert
        if (result.Contains("2025-11-06"))
        {
            Assert.Pass();
        }
        Assert.Fail();
    }
    
    //Manager-SRS-REQ-006
    [Test]
    public void SearchJournal_ValidInput_ReturnsProperList()
    {
        //arrange
        string orderby = "Name";
        string userInput = "Cash";
        
        //act
        List<string> result = Manager.SearchJournal(orderby, userInput);
        
        //assert
        if (result.Contains(userInput))
        {
            Assert.Pass();
        }
        Assert.Fail();
    }
    
    //Manager-SRS-REQ-007
    [Test]
    public void GetLedger_ValidInput_ReturnsProperList()
    {
        
        //act
        List<string> result = Manager.GetLedger(1001);
        
        //assert
        if (result.Contains("Cash"))
        {
            Assert.Pass();
        }
        Assert.Fail();
    }
    
    //Manager-SRS-REQ-008
    [Test]
    public void GetIncomeStatement_ValidInput_ReturnsProperList()
    {
        //arrange
        string expectedNetIncome = "13425";
        
        //act
        List<string> result = Manager.GetIncomeStatement("2025-10-01", "2025-12-25");
        
        //assert
        if (result.Contains(expectedNetIncome))
        {
            Assert.Pass();
        }
        Assert.Fail();
    }
    
    //Manager-SRS-REQ-009
    [Test]
    public void GetTrialBalance_ValidInput_ReturnsProperList()
    {
        //arrange
        string expectedCash = "8875";
        
        //act
        List<string> result = Manager.GetTrialBalance("2025-10-01", "2025-12-25");
        
        //assert
        if (result.Contains(expectedCash))
        {
            Assert.Pass();
        }
        Assert.Fail();
    }
    
    //Manager-SRS-REQ-010
    [Test]
    public void GetBalanceSheet_ValidInput_ReturnsProperList()
    {
        //arrange
        string expectedCashBalance = "8875";
        
        //act
        List<string> result = Manager.GetBalanceSheet("2025-10-01", "2025-12-25");
        
        //assert
        if (result.Contains(expectedCashBalance))
        {
            Assert.Pass();
        }
        Assert.Fail();
    }
    
    //Manager-SRS-REQ-011
    [Test]
    public void ApproveJournalEntry_ValidInput_ReturnsTrue()
    {
        //arrange
        int id;
        bool expected = true;
        try
        {
            id = Manager.CreateJournalEntry("2025-12-07", "pcox0930");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        
        //act
        bool actual = Manager.ApproveJournalEntry(id);
        
        //assert
        Assert.That(actual.Equals(expected));
    }
    
    //Manager-SRS-REQ-011
    [Test]
    public void RejectJournalEntry_ValidInput_ReturnsTrue()
    {
        //arrange
        int id;
        bool expected = true;
        try
        {
            id = Manager.CreateJournalEntry("2025-12-07", "pcox0930");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        
        //act
        bool actual = Manager.RejectJournalEntry(id, "bad entry");
        
        //assert
        Assert.That(actual.Equals(expected));
    }
    
    //Manager-SRS-REQ-012
    [Test]
    public void ViewAdjustingEntriesByStatus_ValidInput_ReturnsProperList()
    {
        //arrange
        string expectedAmount = "980";
        
        //act
        List<string> result = Manager.ViewAdjustingEntriesByStatus('A');
        
        //assert
        if (result.Contains(expectedAmount))
        {
            Assert.Pass();
        }
        Assert.Fail();
    }
    
    //Manager-SRS-REQ-013
    [Test]
    public void CreateClosingJournalEntry_ValidInput_ReturnsID()
    {
        //arrange
        int id = -2;

        //act
        id = Manager.CreateClosingJournalEntry("2025-12-07", "pcox0930");

        //assert
        if (id != -2)
        {
            Assert.Pass();
        }

        Assert.Fail();
    }
    
    //Manager-SRS-REQ-014
    [Test]
    public void CreateReversingJournalEntry_ValidInput_ReturnsID()
    {
        //arrange
        int id = -2;

        //act
        id = Manager.CreateReversingJournalEntry("2025-12-07", "pcox0930");

        //assert
        if (id != -2)
        {
            Assert.Pass();
        }

        Assert.Fail();
    }
    
    //Manager-SRS-REQ-015
    [Test]
    public void ViewClosingEntriesByStatus_ValidInput_ReturnsProperList()
    {
        //arrange
        string expectedStatus = "2025-12-07";
        
        //act
        List<string> result = Manager.ViewClosingEntriesByStatus('P');
        
        //assert
        if (result.Contains(expectedStatus))
        {
            Assert.Pass();
        }
        Assert.Fail();
    }
    
    //Manager-SRS-REQ-016
    [Test]
    public void ViewReversingEntriesByStatus_ValidInput_ReturnsProperList()
    {
        //arrange
        string expectedStatus = "2025-12-07";
        
        //act
        List<string> result = Manager.ViewReversingEntriesByStatus('P');
        foreach (var element in result)
    
        //assert
        if (result.Contains(expectedStatus))
        {
            Assert.Pass();
        }
        Assert.Fail();
    }
}