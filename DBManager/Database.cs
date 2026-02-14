using DbManager.Parser;
using DbManager.Security;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Enumeration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace DbManager
{
    public class Database
    {
        private List<Table> Tables = new List<Table>();
        private string m_username;

        public string LastErrorMessage { get; private set; }

        public Manager SecurityManager
        {
            get;
            private set;
        }

        //This constructor should only be used from Load (without needing to set a password for the user). It cannot be used from any other class
        private Database()
        {

        }

        public Database(string adminUsername, string adminPassword)
        {
            //DEADLINE 1.B: Initalize the member variables
            m_username = adminUsername;
            SecurityManager = new Manager(m_username);

        }

        public bool AddTable(Table table)
        {
            //DEADLINE 1.B: Add a new table to the database

            if (table == null)
                return false;

            Tables.Add(table);
            return true;

        }

        public Table TableByName(string tableName)
        {
            //DEADLINE 1.B: Find and return the table with the given name
            for (int i = 0; i < Tables.Count; i++)
            {
                if (Tables[i].Name == tableName)
                {
                    return Tables[i];
                }
            }

            return null;
        }

        public bool CreateTable(string tableName, List<ColumnDefinition> ColumnDefinition)
        {
            //DEADLINE 1.B: Create a new table with the given name and columns. If there is already a table with that name,
            //return false and set LastErrorMessage with the appropriate error (Check Constants.cs)
            //Do the same if no column is provided
            //If everything goes ok, set LastErrorMessage with the appropriate success message (Check Constants.cs)


            if (TableByName(tableName) != null)
            {
                LastErrorMessage = Constants.TableAlreadyExistsError;
                return false;
            }
            else if (tableName == null || ColumnDefinition.Count() == 0)
            {
                LastErrorMessage = Constants.DatabaseCreatedWithoutColumnsError;
                return false;
            }
            else
            {
                Table newTable = new Table(tableName, ColumnDefinition);
                Tables.Add(newTable);
                LastErrorMessage = Constants.CreateTableSuccess;
                return true;
            }

        }

        public bool DropTable(string tableName)
        {
            //DEADLINE 1.B: Delete the table with the given name. If the table doesn't exist, return false and set LastErrorMessage
            //If everything goes ok, return true and set LastErrorMessage with the appropriate success message (Check Constants.cs)

            if (TableByName(tableName) == null)
            {
                LastErrorMessage = Constants.TableDoesNotExistError;
                return false;
            }

            List<Table> remove = new List<Table>();
            remove.Add(TableByName(tableName));

            foreach (Table table in remove)
            {
                Tables.Remove(table);
            }
            LastErrorMessage = Constants.DropTableSuccess;
            return true;

        }

        public bool Insert(string tableName, List<string> values)
        {
            //DEADLINE 1.B: Insert a new row to the table. If it doesn't exist return false and set LastErrorMessage appropriately
            //If everything goes ok, set LastErrorMessage with the appropriate success message (Check Constants.cs)

            if (TableByName(tableName) == null)
            {
                LastErrorMessage = Constants.TableDoesNotExistError;
                return false;
            }
            if (values == null || values.Count != TableByName(tableName).NumColumns())
            {
                LastErrorMessage = Constants.ColumnCountsDontMatch;
                return false;
            }

            TableByName(tableName).Insert(values);
            LastErrorMessage = Constants.InsertSuccess;
            return true;

        }

        public Table Select(string tableName, List<string> columns, Condition condition)
        {
            //DEADLINE 1.B: Return the result of the select. If the table doesn't exist return null and set LastErrorMessage appropriately (Check Constants.cs)
            //If any of the requested columns doesn't exist, return null and set LastErrorMessage (Check Constants.cs)
            //If everything goes ok, return the table

            if (TableByName(tableName) == null)
            {
                LastErrorMessage = Constants.TableDoesNotExistError;
                return null;
            }

            for (int i = 0; i < columns.Count; i++)
            {
                if (TableByName(tableName).ColumnByName(columns[i]) == null)
                {
                    LastErrorMessage = Constants.ColumnDoesNotExistError;
                    return null;
                }
            }
            Table table = TableByName(tableName).Select(columns, condition);
            return table;

        }

        public bool DeleteWhere(string tableName, Condition columnCondition)
        {
            //DEADLINE 1.B: Delete all the rows where the condition is true. 
            //If the table or the column in the condition don't exist, return null and set LastErrorMessage (Check Constants.cs)
            //If everything goes ok, return true

            if (TableByName(tableName) == null)
            {
                LastErrorMessage = Constants.TableDoesNotExistError;
                return false;
            }
            else if (TableByName(tableName).ColumnByName(columnCondition.ColumnName) == null)
            {
                LastErrorMessage = Constants.ColumnDoesNotExistError;
                return false;
            }

            TableByName(tableName).DeleteWhere(columnCondition);
            LastErrorMessage = Constants.DeleteSuccess;
            return true;

        }

        public bool Update(string tableName, List<SetValue> columnNames, Condition columnCondition)
        {
            //DEADLINE 1.B: Update in the given table all the rows where the condition is true using the SetValues
            //If the table or the column in the condition don't exist, return null and set LastErrorMessage (Check Constants.cs)
            //If everything goes ok, return true

            if (TableByName(tableName) == null)
            {
                LastErrorMessage = Constants.TableDoesNotExistError;
                return false;
            }
            else if (TableByName(tableName).ColumnByName(columnCondition.ColumnName) == null)
            {
                LastErrorMessage = Constants.ColumnDoesNotExistError;
                return false;
            }

            TableByName(tableName).Update(columnNames, columnCondition);
            LastErrorMessage = Constants.UpdateSuccess;
            return true;

        }

        public bool Save(string databaseName)
        {
            //DEADLINE 1.C: Save this database to disk with the given name
            //If everything goes ok, return true, false otherwise.
            //DEADLINE 5: Save the SecurityManager so that it can be loaded with the database in Load()

            try
            {
                using (FileStream fs = new FileStream(databaseName, FileMode.Create))
                using (BinaryWriter writer = new BinaryWriter(fs))
                {
                    writer.Write(m_username);

                    writer.Write(Tables.Count);

                    for (int i = 0; i < Tables.Count; i++)
                    {
                        Table table = Tables[i];

                        writer.Write(table.Name);

                        writer.Write(table.NumColumns());

                        for (int c = 0; c < table.NumColumns(); c++)
                        {
                            ColumnDefinition col = table.GetColumn(c);

                            writer.Write(col.Name);
                            writer.Write(col.Type.ToString());

                        }

                        writer.Write(table.NumRows());
                        for (int r = 0; r < table.NumRows(); r++)
                        {
                            Row row = table.GetRow(r);
                            List<string> rowValues = row.Values;
                            writer.Write(rowValues.Count);

                            for (int v = 0; v < rowValues.Count; v++)
                            {
                                writer.Write(rowValues[v]);
                            }
                        }
                    }
                }
                SecurityManager.Save(databaseName);
                return true;

            }
            catch
            {
                return false;
            }



        }

        public static Database Load(string databaseName, string username, string password)
        {
            //DEADLINE 1.C: Load the (previously saved) database of name databaseName
            //If everything goes ok, return the loaded database (a new instance), null otherwise.
            //DEADLINE 5: When the Database object is created, set the username (create a new method if you must)
            //After loading the database, load the SecurityManager and check the password is correct. If it's not, return null. If it is return the database


            try
            {
            
                Database db= new Database();
                using (FileStream fs = new FileStream(databaseName, FileMode.Open))
                using (BinaryReader reader = new BinaryReader(fs))
                {

                    db.m_username= reader.ReadString();

                    int numTables= reader.ReadInt32();

                    for(int i=0; i<numTables; i++)
                    {
                        string tableName= reader.ReadString();

                        int columnCount= reader.ReadInt32();

                        List<ColumnDefinition> columns= new List<ColumnDefinition>();

                        for (int c=0; c<columnCount; c++)
                        {

                        string columnName= reader.ReadString();
                        string columnTypeString= reader.ReadString(); 

                        ColumnDefinition.DataType type= (ColumnDefinition.DataType)Enum.Parse(typeof(ColumnDefinition.DataType), columnTypeString);
                        columns.Add(new ColumnDefinition(type, columnName));
                    }

                    Table table= new Table(tableName, columns);

                    int rowCount= reader.ReadInt32();

                    for(int r=0; r<rowCount; r++)
                        {
                            int valueCount= reader.ReadInt32();

                            List<string> values= new List<string>();

                            for(int v=0; v<valueCount; v++)
                            {
                                values.Add(reader.ReadString());
                                
                            }
                            table.Insert(values);
                        }

                        db.Tables.Add(table);

                    }

                }
                /* descomentar solo para DEADLINE 5
                Manager manager= Manager.Load(databaseName, username);

                if(manager==null)
                {
                    return null;
                }

                if(!manager.IsPasswordCorrect(username, password))
                {
                    return null;
                }
                
                db.SecurityManager=manager;
                */
                return db;


            }
            catch
            {
                return null;
            }
           
        }

        public string ExecuteMiniSQLQuery(string query)
        {
            //Parse the query
            MiniSqlQuery miniSQLQuery = MiniSQLParser.Parse(query);

            //If the parser returns null, there must be a syntax error (or the parser is failing)
            if (miniSQLQuery == null)
                return Constants.SyntaxError;

            //Once the query is parsed, we run it on this database
            return miniSQLQuery.Execute(this);
        }


        public bool IsUserAdmin()
        {
            return SecurityManager.IsUserAdmin();
        }





        //All these methods are ONLY FOR TESTING. Use them to simplify creating unit tests:
        public const string AdminUsername = "admin";
        public const string AdminPassword = "adminPassword";
        public static Database CreateTestDatabase()
        {
            Database database = new Database(AdminUsername, AdminPassword);

            database.Tables.Add(Table.CreateTestTable());

            return database;
        }

        public void AddTuplesForTesting(string tableName, List<List<string>> rows)
        {
            Table table = TableByName(tableName);
            foreach (List<string> row in rows)
            {
                table.Insert(row);
            }
        }

        public void CheckForTesting(string tableName, List<List<string>> rows)
        {
            Table table = TableByName(tableName);

            table.CheckForTesting(rows);
        }
    }
}






