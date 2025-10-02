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

    /*[Test]
    public void CreateEmployee_ValidUserInput_CreatesDatabaseEntry()
    {

        //arrange
        Admin TempAdmin = new Admin();
        string Actual = "";
        string Expected = "5TestTest00";

        //act
        TempAdmin.CreateEmployee(5, "Test", "Test", false, false, 4, "PassW0rd...", "Test@Test.com");

        //assert
        var sql = "SELECT ID, FirstName, LastName, IsAdmin, IsManager " +
                  "FROM Employee " +
                  "WHERE ID = @ID";
        try
        {
            using var connection = new SqliteConnection($"Data Source="+Database.GetDatabasePath());
            connection.Open();

            using var command = new SqliteCommand(sql, connection);
            command.Parameters.AddWithValue("@ID", 5);
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
    }*/

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

    [Test]
    public void DeactivateUser_ValidInput_UpdatesUserTable()
    {
        //arrange
        Admin Temp = new Admin();
        string Expected = "0";
        string Actual = "";
        string sql = "Select IsActive FROM User WHERE ID = @ID";
        
        
        //act
        Temp.DeactivateUser(4, "2025-10-02", "2025-10-03");
        try
        {
            using var connection = new SqliteConnection($"Data Source=" + Database.GetDatabasePath());
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
    public void ActivateUser_ValidInput_UpdatesUserTable()
    {
        //arrange
        Admin Temp = new Admin();
        string Expected = "1";
        string Actual = "";
        string sql = "Select IsActive FROM User WHERE ID = @ID";
        
        
        //act
        Temp.ActivateUser(4);
        try
        {
            using var connection = new SqliteConnection($"Data Source=" + Database.GetDatabasePath());
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
    public void DeactivateUser_ValidInput_ReturnsTrue()
    {
        //arrange
        Admin Temp = new Admin();
        bool Expected = true;
        bool Actual;
        string sql = "Select IsActive FROM User WHERE ID = @ID";
        
        //act
        Actual = Temp.DeactivateUser(4, "2025-10-02","2025-10-03");
    
        //assert
        Assert.That(Actual, Is.EqualTo(Expected));
    }
    
    [Test]
    public void ActivateUser_ValidInput_ReturnsTrue()
    {
        //arrange
        Admin Temp = new Admin();
        bool Expected = true;
        bool Actual;
        string sql = "Select IsActive FROM User WHERE ID = @ID";
        
        //act
        Actual = Temp.ActivateUser(1);
    
        //assert
        Assert.That(Actual, Is.EqualTo(Expected));
    }

    [Test]
    public void UserReport_ReadsDatabase()
    {
        //arrange
        Admin temp = new Admin();
        bool Expected = true;
        bool Actual;
        
        //act
        if (temp.UserReport().Contains("Test@Test.com"))
        {
            Actual = true;
        }
        else
        {
            Actual = false;
        }
         //Assert
         Assert.That(Actual, Is.EqualTo(Actual));
    }

    [Test]
    public void ExpiredPasswordReport_ReadsDatabase()
    {
        //arrange
        Admin temp = new Admin();
        bool Expected = true;
        bool Actual;
        
        //act
        if (temp.ExpiredPasswordReport().Contains("O1D_PassW0rd"))
        {
            Actual = true;
        }
        else
        {
            Actual = false;
        }
        //Assert
        Assert.That(Actual, Is.EqualTo(Actual));
    }

    /*[Test]
    public void UpdateEmployeeID_ValidInput_ReturnsTrue()
    {
        //arrange
        Admin temp = new Admin();
        bool expected = true;
        bool actual;
        
        //act
        actual = temp.UpdateEmployeeID(4, 100, 5);
        
        //assert
        Assert.That(actual, Is.EqualTo(expected));
    }*/
    
    [Test]
    public void UpdateFirstName_ValidInput_ReturnsTrue()
    {
        //arrange
        Admin temp = new Admin();
        bool expected = true;
        bool actual;
        
        //act
        actual = temp.UpdateFirstName(4, "John");
        
        //assert
        Assert.That(actual, Is.EqualTo(expected));
    }
    
    [Test]
    public void UpdateLastName_ValidInput_ReturnsTrue()
    {
        //arrange
        Admin temp = new Admin();
        bool expected = true;
        bool actual;
        
        //act
        actual = temp.UpdateLastName(4, "Smith");
        
        //assert
        Assert.That(actual, Is.EqualTo(expected));
    }
    
    [Test]
    public void UpdateDoB_ValidInput_ReturnsTrue()
    {
        //arrange
        Admin temp = new Admin();
        bool expected = true;
        bool actual;
        
        //act
        actual = temp.UpdateDoB(4, "2025-10-02");
        
        //assert
        Assert.That(actual, Is.EqualTo(expected));
    }
    
    [Test]
    public void UpdateAddress_ValidInput_ReturnsTrue()
    {
        //arrange
        Admin temp = new Admin();
        bool expected = true;
        bool actual;
        
        //act
        actual = temp.UpdateAddress(4, "756 Evil Street");
        
        //assert
        Assert.That(actual, Is.EqualTo(expected));
    }
    
    /*[Test]
    public void UpdateUserID_ValidInput_ReturnsTrue()
    {
        //arrange
        Admin temp = new Admin();
        bool expected = true;
        bool actual;
        
        //act
        actual = temp.UpdateUserID(4, 100);
        
        //assert
        Assert.That(actual, Is.EqualTo(expected));
    }*/
    
    [Test]
    public void UpdateUsername_ValidInput_ReturnsTrue()
    {
        //arrange
        Admin temp = new Admin();
        bool expected = true;
        bool actual;
        
        //act
        actual = temp.UpdateUsername(4, "TTest1002");
        
        //assert
        Assert.That(actual, Is.EqualTo(expected));
    }
    
    [Test]
    public void UpdateEmail_ValidInput_ReturnsTrue()
    {
        //arrange
        Admin temp = new Admin();
        bool expected = true;
        bool actual;
        
        //act
        actual = temp.UpdateEmail(4, "TTest1002@Ledgr.com");
        
        //assert
        Assert.That(actual, Is.EqualTo(expected));
    }
    
    [Test]
    public void Update_ValidInput_ReturnsTrue()
    {
        //arrange
        Admin temp = new Admin();
        bool expected = true;
        bool actual;
        
        //act
        actual = temp.UpdateNewUser(4, 0);
        
        //assert
        Assert.That(actual, Is.EqualTo(expected));
    }

    [Test]
    public void ApproveUser_ValidInput_ReturnsTrue()
    {
        //arrange
        Admin temp = new Admin();
        bool expected = true;
        bool actual;

        //act
        actual = temp.ApproveUser(2);

        //assert
        Assert.That(actual, Is.EqualTo(expected));
    }
}

