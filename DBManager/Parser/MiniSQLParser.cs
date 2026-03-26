using DbManager.Parser;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace DbManager
{
    public class MiniSQLParser
    {
        public static MiniSqlQuery Parse(string miniSQLQuery)
        {
            //TODO DEADLINE 2
            const string selectPattern = @"SELECT\s+([\w]+(?:,[\w]+)*)\s+FROM\s+(\w+)(?:\s+WHERE\s+(\w+)\s*(<|>|=)\s*'(-?\d+(?:\.\d+)?|[a-zA-Z]+)')?";
            
            const string insertPattern = @"INSERT\s+INTO\s+(\w+)\s+VALUES\s*\('(-?\d+|-?\d+\.\d+|(?:\w+(?:\s+\w+)*))'(?:,'(-?\d+|-?\d+\.\d+|\w+(\s+\w+)*)')*\)";
            
            const string dropTablePattern = @"DROP\s+TABLE\s+([\w+]+)";
            
            //Note: The parsing of CREATE TABLE should accept empty columns "()"
            //And then, an execution error should be given if a CreateTable without columns is executed
            const string createTablePattern = @"CREATE\s+TABLE\s+([\w+]+)\s+\(([\w]+\s+(?:INT|DOUBLE|TEXT)(?:,[\w+]+\s+(?:INT|DOUBLE|TEXT))*)\)";

            string updateTablePattern = @"UPDATE\s+(\w+)\s+SET\s+([\w]+='[\w.-]+'(?:,\s*[\w]+='[\w.-]+')*)\s+WHERE\s+(\w+)([<>=])'([\w.-]+)'";


            const string deletePattern = @"DELETE\s+FROM\s+(\w+)\s+WHERE\s+(\w+)(=|<|>)'(-?\d+|-?\d+\.\d+|(?:\w+(?:\s+\w+)*))'";

            //TODO DEADLINE 4
            const string createSecurityProfilePattern = @"CREATE\s+SECURITY\s+PROFILE\s+([a-zA-Z]+)";
            
            const string dropSecurityProfilePattern = @"DROP\s+SECURITY\s+PROFILE\s+([a-zA-Z]+)";
            
            const string grantPattern = @"GRANT\s+(DELETE|INSERT|SELECT|UPDATE)\s+ON\s+(\w+)\s+TO\s+([a-zA-Z]+)";
            
            const string revokePattern = @"REVOKE\s+(DELETE|INSERT|SELECT|UPDATE)\s+ON\s+(\w+)\s+TO\s+([a-zA-Z]+)";
            
            const string addUserPattern = @"ADD\s+USER\s+\(([a-zA-Z]+),(\w+),([a-zA-Z]+)\)";
            
            const string deleteUserPattern = @"DELETE\s+USER\s+([a-zA-Z]+)";


            //TODO DEADLINE 2
            //Parse query using the regular expressions above one by one. If there is a match, create an instance of the query with the parsed parameters
            //For example, if the query is a "SELECT ...", there should be a match with selectPattern. We would create and return an instance of Select
            //initialized with the table name, the columns, and (possibly) an instance of Condition.
            //If there is no match, it means there is a syntax error. We will return null.
            
            //update case
            Match match;
            match = Regex.Match(miniSQLQuery, updateTablePattern);
            if (match.Success && match.Length == miniSQLQuery.Length)
            {
                string table = match.Groups[1].Value;
                List<SetValue> values = new List<SetValue>();
                List<string> valuesPre = CommaSeparatedNames(match.Groups[2].Value);

                for (int i = 0; i < valuesPre.Count; i++)
                {
                    string[] words = valuesPre[i].Split('=');

                    string columnName = words[0].Trim();
                    string columnValue = words[1].Trim(' ', '\'', '"');

                    values.Add(new SetValue(columnName, columnValue));
                }
                string whereColumn = match.Groups[3].Value;
                string whereOperator = match.Groups[4].Value;
                string whereValue = match.Groups[5].Value.Trim(' ', '\'', '"');

                Condition cond = new Condition(whereColumn, whereOperator, whereValue);
                return new Update(table, values, cond);
            }
            match = Regex.Match(miniSQLQuery, selectPattern);
            
            if (match.Success && match.Length == miniSQLQuery.Length)
            {
                if (match.Groups[3].Value == "")
                {
                    return new Select(match.Groups[2].Value, match.Groups[1].Value.Split(",").ToList(), null);
                }
                Condition condition;
                condition = new Condition(match.Groups[3].Value, match.Groups[4].Value, match.Groups[5].Value);
                return new Select(match.Groups[2].Value, match.Groups[1].Value.Split(",").ToList(), condition);
            }


            //CREATE TABLE CASE
            match = Regex.Match(miniSQLQuery, createTablePattern);
            if (match.Success && match.Length == miniSQLQuery.Length)
            {
                List<ColumnDefinition> columns = new List<ColumnDefinition>();
                string[] columnWithValue = match.Groups[2].Value.Split(",");
                foreach (string s in columnWithValue)
                {
                    string[] parts = s.Split(" ");
                    string name = parts[0];
                    string type = parts[1];

                    ColumnDefinition.DataType datatype;
                    switch (type.ToLower())
                    {
                        case "int":
                            datatype = ColumnDefinition.DataType.Int;
                            break;
                        case "double":
                            datatype = ColumnDefinition.DataType.Double;
                            break;
                        case "text":
                            datatype = ColumnDefinition.DataType.String;
                            break;
                        default:
                            datatype = ColumnDefinition.DataType.String;
                            break;
                    }
                    columns.Add(new ColumnDefinition(datatype, name));
                }
                return new CreateTable(match.Groups[1].Value, columns);
            }

            match = Regex.Match(miniSQLQuery, dropTablePattern);
            if (match.Success && match.Length == miniSQLQuery.Length)
            {
                return new DropTable(match.Groups[1].Value);
            }

            match = Regex.Match(miniSQLQuery, insertPattern);
            if(match.Success && match.Length == miniSQLQuery.Length)
            {
                List<string> valores2 = new List<string>();
                for (int i=2;i<match.Groups.Count;i++)
                {

                    valores2.Add(match.Groups[i].Value);
                }
                return new Insert(match.Groups[1].Value, valores2);
            }


            //delete case
            match = Regex.Match(miniSQLQuery, deletePattern);
            if(match.Success && match.Length == miniSQLQuery.Length)
            {
                return new Delete(match.Groups[1].Value, new Condition(match.Groups[2].Value, match.Groups[3].Value, match.Groups[4].Value));
            }




            //TODO DEADLINE 4
            //Do the same for the security queries (CREATE SECURITY PROFILE, ...)

            //GRANT case
            match = Regex.Match(miniSQLQuery, grantPattern);
            if (match.Success && match.Length == miniSQLQuery.Length)
            {
                return new Grant(match.Groups[1].Value, match.Groups[2].Value, match.Groups[3].Value);
            }

            //REVOKE case
            match = Regex.Match(miniSQLQuery, revokePattern);
            if (match.Success && match.Length == miniSQLQuery.Length)
            {
                return new Revoke(match.Groups[1].Value, match.Groups[2].Value, match.Groups[3].Value);
            }

            //ADD USER case

            match = Regex.Match(miniSQLQuery, addUserPattern);
            if (match.Success && match.Length == miniSQLQuery.Length)
            {
                return new AddUser(match.Groups[1].Value, match.Groups[2].Value, match.Groups[3].Value);
            }

            //DELETE USER case
            match = Regex.Match(miniSQLQuery, deleteUserPattern);
            if (match.Success && match.Length == miniSQLQuery.Length)
            {
                return new DeleteUser(match.Groups[1].Value);
            }

            //CREATE PROFILE case
            match = Regex.Match(miniSQLQuery, createSecurityProfilePattern);
            if (match.Success && match.Length == miniSQLQuery.Length)
            {
                return new CreateSecurityProfile(match.Groups[1].Value);
            }

            //DROP PROFILE case
            match = Regex.Match(miniSQLQuery, dropSecurityProfilePattern);
            if (match.Success && match.Length == miniSQLQuery.Length)
            {
                return new DropSecurityProfile(match.Groups[1].Value);
            }


            return null;
           
        }

        static List<string> CommaSeparatedNames(string text)
        {
            string[] textParts = text.Split(",", System.StringSplitOptions.RemoveEmptyEntries);
            List<string> commaSeparator = new List<string>();
            for(int i=0; i < textParts.Length; i++)
            {
                commaSeparator.Add(textParts[i]);
            }
            return commaSeparator;
        }
        
    }
}
