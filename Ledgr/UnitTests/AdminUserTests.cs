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
    public void CreateEmployee_ValidUserInput_WritesToDatabase()
    {
        //arrange
        Admin TempAdmin = new Admin();
        string Row = "";
        
        //act
        TempAdmin.CreateEmployee(3, "Michael", "Liu", true, false);
        
        //assert
        var sql = "SELECT ID, FirstName, LastName, IsAdmin, IsMananger FROM Employee WHERE ID = @ID";
        try
        {
            Console.WriteLine("Hello from Line 28 in Test");
            using var connection = new SqliteConnection($"Data Source="+Database.GetDatabasePath());
            Console.WriteLine("Hello from Line 30 in Test");
            connection.Open();

            using var command = new SqliteCommand(sql, connection);
            command.Parameters.AddWithValue("@ID", 1);
            Console.WriteLine("Hello from Line 33 in Test");
            using var reader = command.ExecuteReader();
            Console.WriteLine("Hello from Line 35 in Test");
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    Console.WriteLine("Hello from Line 39 in Test");
                    Row += reader.GetString(0);
                    Console.WriteLine("Hello from Line 42 in Test");
                    Row += reader.GetString(1);
                    Row += reader.GetString(2);
                    Row += reader.GetString(3);
                    Row += reader.GetString(4);
                }
            }

            connection.Close();
        }
        catch (Exception e)
        {
            Console.WriteLine("Maaaaan wtf going on");
        }
        Assert.That(Row, Is.EqualTo("1 RJ Straiton 1 0"));
    }
}