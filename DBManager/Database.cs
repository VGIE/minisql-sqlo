using DbManager.Parser;
using DbManager.Security;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
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
           m_username= adminUsername;

            
        }

        public bool AddTable(Table table)
        {
            //DEADLINE 1.B: Add a new table to the database
            Tables.Add(table);

            return true;
            
        }

        public Table TableByName(string tableName)
        {
            //DEADLINE 1.B: Find and return the table with the given name
            for(int i=0; i<Tables.Count; i++)
            {
                if(Tables[i].Name==tableName)
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

            for (int i= 0; i<Tables.Count; i++){

                if (Tables[i].Name == tableName)
                {
                    LastErrorMessage= Constants.TableAlreadyExistsError;
                    return false; 
                }
                else if(ColumnDefinition==null)
                {
                    LastErrorMessage= Constants.TableAlreadyExistsError;
                    return false; 
                }
                else
                {
                    Table newTable= new Table(tableName, ColumnDefinition);

                }

            }
            
            return false;
        }

        public bool DropTable(string tableName)
        {
            //DEADLINE 1.B: Delete the table with the given name. If the table doesn't exist, return false and set LastErrorMessage
            //If everything goes ok, return true and set LastErrorMessage with the appropriate success message (Check Constants.cs)

            for (int i= Tables.Count-1; i>=0; i--)
            {
                if(Tables[i].Name==tableName)
                {
                    Tables.RemoveAt(i);
                    LastErrorMessage= Constants.DeleteSuccess;
                    return true;
                }
            }
            LastErrorMessage= Constants.TableDoesNotExistError;
            return false; 
        }

        public bool Insert(string tableName, List<string> values)
        {
            //DEADLINE 1.B: Insert a new row to the table. If it doesn't exist return false and set LastErrorMessage appropriately
            //If everything goes ok, set LastErrorMessage with the appropriate success message (Check Constants.cs)
            
            for(int i=0; i<Tables.Count; i++)
            {
                if(Tables[i].Name==tableName)
                {
                    Tables[i].Insert(values);
                    
                }
                else
                {
                    LastErrorMessage= Constants.TableDoesNotExistError;
                    return false;
                }
            }   
            return false; 
        }

        public Table Select(string tableName, List<string> columns, Condition condition)
        {
            //DEADLINE 1.B: Return the result of the select. If the table doesn't exist return null and set LastErrorMessage appropriately (Check Constants.cs)
            //If any of the requested columns doesn't exist, return null and set LastErrorMessage (Check Constants.cs)
            //If everything goes ok, return the table
             for(int i=0; i<Tables.Count; i++)
            {
                if(Tables[i].Name==tableName)
                {
                    Table table= Tables[i];

                    for(int c=0; c<columns.Count; c++)
                    {
                        bool found= false; 

                        for(int j=0; j<table.NumColumns(); j++)
                        {
                            if(table.GetColumn(j).Name==columns[c])
                            {
                                found=true;
                                break;
                            }
                        }
                    
                    if(found!=true)
                    {
                        LastErrorMessage= Constants.TableDoesNotExistError;
                        return null;
                    }
                    }
                    return table.Select(columns, condition);

                    
                }
                else
                {
                    LastErrorMessage= Constants.ColumnDoesNotExistError;
                }
            }   
            return null; 
        }
            

        public bool DeleteWhere(string tableName, Condition columnCondition)
        {
            //DEADLINE 1.B: Delete all the rows where the condition is true. 
            //If the table or the column in the condition don't exist, return null and set LastErrorMessage (Check Constants.cs)
            //If everything goes ok, return true
             for(int i=0; i<Tables.Count; i++)
            {
                if(Tables[i].Name==tableName)
                {
                    Table table= Tables[i];
                    bool columnExists= false; 

                    for(int j=0; j<table.NumColumns();j++)
                    {
                        if(table.GetColumn(j).Name==columnCondition.ColumnName)
                        {
                            columnExists=true;
                            break; 
                            
                        }
                    }
                    if(!columnExists)
                    {
                        LastErrorMessage= Constants.ColumnDoesNotExistError;
                        return false;
                    }
                    table.DeleteWhere(columnCondition);
                    return true; 
                }
            }
           return false;
        }

        public bool Update(string tableName, List<SetValue> columnNames, Condition columnCondition)
        {
            //DEADLINE 1.B: Update in the given table all the rows where the condition is true using the SetValues
            //If the table or the column in the condition don't exist, return null and set LastErrorMessage (Check Constants.cs)
            //If everything goes ok, return true
            
             for(int i=0; i<Tables.Count; i++)
            {
                if(Tables[i].Name==tableName)
                {
                    Table table= Tables[i];

                    bool columnConditionExists= false; 

                    for(int j=0; j<table.NumColumns();j++)
                    {
                        if(table.GetColumn(j).Name==columnCondition.ColumnName)
                        {
                            columnConditionExists=true;
                            break; 
                            
                        }
                    }
                    if(!columnConditionExists)
                    {
                        LastErrorMessage= Constants.ColumnDoesNotExistError;
                        return false;
                    }
                    for(int c=0; c<columnNames.Count; c++)
                    {
                        bool columnExists= false;

                        for(int j=0; j<table.NumColumns(); j++)
                        {
                            if(table.GetColumn(j).Name==columnNames[c].ColumnName)
                            {
                                columnExists=true; 
                                break;
                                
                            }
                        }
                        if(!columnExists)
                        {
                            LastErrorMessage= Constants.ColumnDoesNotExistError;
                            return false; 
                        }
                    }
                    table.Update(columnNames, columnCondition);
                    return true;  
                }
            }
           return false;            
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

                    for(int i=0; i<Tables.Count; i++)
                    {
                        Table table= Tables[i];

                        writer.Write(table.Name);

                        writer.Write(table.NumColumns());

                        for(int c=0; c<table.NumColumns(); c++)
                        {
                            ColumnDefinition col= table.GetColumn(c);
                            writer.Write(col.Name);
                            writer.Write(col.Type.ToString());
                            
                        }

                        writer.Write(table.NumRows());
                        for(int r=0; r<table.NumRows();r++)
                        {
                            Row row= table.GetRow(r);
                            List<string> rowValues= row.Values;
                            writer.Write(rowValues.Count);

                            for(int v=0; v<rowValues.Count; v++)
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
            

            
            return null;
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





