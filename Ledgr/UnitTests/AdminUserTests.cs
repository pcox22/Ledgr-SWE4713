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
    public void CreateEmployee_ValidUserInput_CreatesDatabaseEntry()
    {
        //arrange
        Admin TempAdmin = new Admin();
        string Actual = "";
        string Expected = "4TestTest00";
        
        //act
        TempAdmin.CreateEmployee(4, "Test", "Test", false, false);
        
        //assert
        var sql = "SELECT ID, FirstName, LastName, IsAdmin, IsManager " +
                  "FROM Employee " +
                  "WHERE ID = @ID";
        try
        {
            using var connection = new SqliteConnection($"Data Source="+Database.GetDatabasePath());
            connection.Open();

            using var command = new SqliteCommand(sql, connection);
            command.Parameters.AddWithValue("@ID", 4);
            using var reader = command.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    Actual += reader.GetString(0);
                    Actual += reader.GetString(1);
                    Actual += reader.GetString(2);
                    Actual += reader.GetString(3);
                    Actual += reader.GetString(4);
                }
            }

            connection.Close();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
        Assert.That(Actual, Is.EqualTo(Expected));
    }

    [Test]
    public void GenerateUsername_ValidUsername_ReturnsTrue()
    {
        //arrange
        Admin Temp = new Admin();
        string Expected = "RStraiton0922";
        
        //act
        string Actual = Temp.GenerateUsername("RJ", "Straiton");
        
        //assert
        Assert.That(Actual, Is.EqualTo(Expected));
    }

    [Test]
    public void PromoteToAdmin_ValidInput_UpdatesDatabase()
    {
        //arrange
        Admin Temp = new Admin();
        string Expected = "1";
        string Actual = "";
        string sql = "Select IsAdmin FROM EMPLOYEE WHERE ID = @ID";
        
        //act
        Temp.PromoteToAdmin(4);
        try
        {
            using var connection = new SqliteConnection($"Data Source="+Database.GetDatabasePath());
            connection.Open();

            using var command = new SqliteCommand(sql, connection);
            command.Parameters.AddWithValue("@ID", 4);
            using var reader = command.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    Actual += reader.GetString(0);
                }
            }

            connection.Close();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
        
        //Assert
        Assert.That(Actual, Is.EqualTo(Expected));
    }
    
    [Test]
    public void DemoteFromAdmin_ValidInput_UpdatesDatabase()
    {
        //arrange
        Admin Temp = new Admin();
        string Expected = "0";
        string Actual = "";
        string sql = "Select IsAdmin FROM EMPLOYEE WHERE ID = @ID";
        
        //act
        Temp.DemoteFromAdmin(4);
        try
        {
            using var connection = new SqliteConnection($"Data Source="+Database.GetDatabasePath());
            connection.Open();

            using var command = new SqliteCommand(sql, connection);
            command.Parameters.AddWithValue("@ID", 4);
            using var reader = command.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    Actual += reader.GetString(0);
                }
            }

            connection.Close();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
        
        //Assert
        Assert.That(Actual, Is.EqualTo(Expected));
    }
    
    [Test]
    public void PromoteToAdmin_ValidInput_ReturnsTrue()
    {
        //arrange
        Admin Temp = new Admin();
        bool Expected = true;
        bool Actual;
        string sql = "Select IsAdmin FROM EMPLOYEE WHERE ID = @ID";
        
        //act
        Actual = Temp.PromoteToAdmin(4);
    
        //assert
        Assert.That(Actual, Is.EqualTo(Expected));
    }
    [Test]
    public void DemoteFromAdmin_ValidInput_ReturnsTrue()
    {
        //arrange
        Admin Temp = new Admin();
        bool Expected = true;
        bool Actual;
        string sql = "Select IsAdmin FROM EMPLOYEE WHERE ID = @ID";
        
        //act
        Actual = Temp.DemoteFromAdmin(4);
    
        //assert
        Assert.That(Actual, Is.EqualTo(Expected));
    }
    
    [Test]
    public void PromoteToManager_ValidInput_ReturnsTrue()
    {
        //arrange
        Admin Temp = new Admin();
        bool Expected = true;
        bool Actual;
        string sql = "Select IsManager FROM EMPLOYEE WHERE ID = @ID";
        
        //act
        Actual = Temp.PromoteToManager(4);
    
        //assert
        Assert.That(Actual, Is.EqualTo(Expected));
    }
    
    [Test]
    public void DemoteFromManager_ValidInput_ReturnsTrue()
    {
        //arrange
        Admin Temp = new Admin();
        bool Expected = true;
        bool Actual;
        string sql = "Select IsManager FROM EMPLOYEE WHERE ID = @ID";
        
        //act
        Actual = Temp.DemoteFromManager(4);
    
        //assert
        Assert.That(Actual, Is.EqualTo(Expected));
    }
}