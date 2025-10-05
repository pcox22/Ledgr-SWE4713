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
        string result = (Email.SendEmail("ledgrsystems@gmail.com", "pcox21@students.kennesaw.edu", "Ledgr Systems", "Patrick Cox", "Test Subject",
            "Implementing Test Body..."));

        string expected = "Success";
        
        Assert.That(result, Is.EqualTo(expected));
    }

    public void Send_Email_Async_Test()
    {
        
    }
}