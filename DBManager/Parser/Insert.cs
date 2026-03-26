using DbManager.Parser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbManager
{
    public class Insert: MiniSqlQuery
    {
        public string Table { get; private set; }
        public List<string> Values { get; private set; }
        public Insert(string table, List<string> values)
        {
            //TODO DEADLINE 2: Initialize member variables
            Table = table;
            Values = values;
        }

        public string Execute(Database database)
        {
            //TODO DEADLINE 3: Run the query and return the appropriate message
            //InsertSuccess or the last error in the database
            if (database.Insert(Table, Values) == true)
            {
                return Constants.InsertSuccess;
            }
            return database.LastErrorMessage;
        }
        public override bool Equals(Object obj)
        {
            Insert other = (Insert)obj;
            //First we compare extreme cases where 'WHERE' clause is null in one side (we also discard different table names)
            if (Table != other.Table || (Values != null && other.Values == null || Values == null && other.Values != null))
            {
                return false;
            }
            //Then, we compare both null case -> true
            else if (other.Values == null && Values == null)
            {
                return true;
            }
            //Last, with both null cases ruled out, we can compare Where clauses one by one
            else
            {
                for(int i = 0; i<Values.Count;i++)
                {
                    if (Values[i] != other.Values[i])
                    {
                        return false;
                    }
                }
                return true;
            }
        }
    }
}
