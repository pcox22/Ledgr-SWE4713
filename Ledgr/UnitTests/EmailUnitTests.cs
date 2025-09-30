using LedgrLogic;

namespace UnitTests;

public class EmailUnitTest
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void Send_Email_Test()
    {
        string result = (Email.SendEmail("10jonathancox@gmail.com", "pcox21@students.kennesaw.edu", "Ledgr Systems", "Patrick Cox", "Test Subject",
            "Implementing Test Body..."));

        string expected = "Success";
        
        Assert.That(result, Is.EqualTo(expected));
    }
}