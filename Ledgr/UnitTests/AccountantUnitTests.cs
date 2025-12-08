using LedgrLogic;

namespace UnitTests;

//All unit tests relating to the Accountant Class
public class AccountantUnitTests
{
    [SetUp]
    public void Setup()
    {
    }
    
    //Accountant-SRS-REQ-001
    [Test]
    public void GetAccountEventLog_ReturnsCorrectList()
    {
        //arrange
        List<string> returnedValues = Accountant.GetAccountEventLog(1001).Result;
        
        //assert
        Assert.That(returnedValues.Contains("1001"));
    }

    //Accountant-SRS-REQ-002
    [Test]
    public void CreateJournalEntry_ValidInput_ReturnsJournalEntryID()
    {
        //arrange
        string username = "MTest1109";
        string date = "2025-11-09";
        int id = -2;
        
        //act
        id = Accountant.CreateJournalEntry(date, username);
        
        //assert
        Assert.That(!id.Equals(-2));
    }
    
    //Accountant-SRS-REQ-003
    [Test]
    public void CreateJournalEntryDetails_ValidInput_ReturnsTrue()
    {
        //arrange
        bool expected = true;
        bool actual = false;
        

        //act
        actual = Accountant.CreateJournalEntryDetails(1001, 0.00, "Debit", 1);

        //assert
        Assert.That(expected.Equals(actual));
    }
    
    //Accountant-SRS-REQ-004
    [Test]
    public void ViewEntriesByStatus_ValidInput_ReturnsProperList()
    {
        //arrange
        string expectedStatus = "A";
        char status = 'A';
        
        //act
        List<string> result = Accountant.ViewEntriesByStatus(status).Result;
        
        //assert
        if (result.Contains(expectedStatus))
        {
            Assert.Pass();
        }
        Assert.Fail();
    }
    
    //Accountant-SRS-REQ-005
    [Test]
    public void FilterJournalStatusDate_ValidInput_ReturnsProperList()
    {
        //arrange
        string date = "asc";
        char status = 'A';
        
        //act
        List<string> result = Accountant.FilterJournalStatusDate(status, date);
        
        //assert
        if (result.Contains("2025-11-06"))
        {
            Assert.Pass();
        }
        Assert.Fail();
    }
    
    //Accountant-SRS-REQ-006
    [Test]
    public void SearchJournal_ValidInput_ReturnsProperList()
    {
        //arrange
        string orderby = "Name";
        string userInput = "Cash";
        
        //act
        List<string> result = Accountant.SearchJournal(orderby, userInput);
        
        //assert
        if (result.Contains(userInput))
        {
            Assert.Pass();
        }
        Assert.Fail();
    }
    
    //Accountant-SRS-REQ-007
    [Test]
    public void GetLedger_ValidInput_ReturnsProperList()
    {
        
        //act
        List<string> result = Accountant.GetLedger();
        
        //assert
        if (result.Contains("Cash"))
        {
            Assert.Pass();
        }
        Assert.Fail();
    }
}