using System;
using DbManager;

namespace DBManagerConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("DBManager console client");

            string databaseName = null, username = null, password = null;

            Database database = null;

            while (database == null)
            {
                Console.Write("Database: ");
                databaseName = Console.ReadLine();
                Console.Write("Username: ");
                username = Console.ReadLine();
                Console.Write("Password: ");
                password = Console.ReadLine();

                database= Database.Load(databaseName, username, password);
            }
            

            Console.WriteLine("Database loaded. Press Enter after each MiniSQL query to see the result");
            while (true)
            {
                Console.Write($"{databaseName}> ");
                string query = Console.ReadLine();
                string result = database.ExecuteMiniSQLQuery(query);
                Console.WriteLine(result);
            }
        }
    }
}
