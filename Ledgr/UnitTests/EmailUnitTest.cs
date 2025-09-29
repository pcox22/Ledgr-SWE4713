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
        Email.SendEmail("", "10jonathancox@gmail.com", "Ledgr Systems", "Patrick Cox", "Test Subject",
            "Implementing Test Body...", "ytglitchroxas@gmail.com", "ijdu oaja bryq jlut");
    }
}