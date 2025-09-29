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
        Console.WriteLine(Email.SendEmail("", "pcox21@students.kennesaw.edu", "Ledgr Systems", "Patrick Cox", "Test Subject",
            "Implementing Test Body...", "ytglitchroxas@gmail.com", "ijdu oaja bryq jlut"));
    }
}