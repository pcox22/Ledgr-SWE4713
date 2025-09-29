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
        string result = (Email.SendEmail("", "pcox21@students.kennesaw.edu", "Ledgr Systems", "Patrick Cox", "Test Subject",
            "Implementing Test Body...", "ytglitchroxas@gmail.com", "ijdu oaja bryq jlut"));

        string expected = "Sucess";
        
        Assert.That(result, Is.EqualTo(expected));
    }
}