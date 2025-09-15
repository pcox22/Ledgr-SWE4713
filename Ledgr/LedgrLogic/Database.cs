using System;
using System.IO;
using System.Collections;
using Microsoft.Data.Sqlite;
namespace LedgrLogic;

/*
 Collection of methods needed to query database, static because there will never be a need to 
 instantiate a Database object
 */
public static class Database
{
    //Returns the path needed to access the Database
    public static string GetDatabasePath()
    {
        string BaseDirectory = AppDomain.CurrentDomain.BaseDirectory;
        string rootDB = Path.GetFullPath("LedgerDB.db");
        //string DBPath = Path.Combine(ProjectRoot, "LedgerDB.db");
        
        
        Console.WriteLine(Environment.CurrentDirectory);
        //Console.WriteLine("P Root: " + ProjectRoot);

        return rootDB;
    }
    
    
}
