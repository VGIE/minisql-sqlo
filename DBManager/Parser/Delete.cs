using System;
using System.Collections.Generic;
using System.Text;

namespace DbManager.Parser
{
    public class Delete : MiniSqlQuery
    {
        public string Table { get; private set; }
        public Condition Where { get; private set; }

        public Delete(string table, Condition where)
        {
            //TODO DEADLINE 2: Initialize member variables
            Table = table;
            Where = where;
        }

        public string Execute(Database database)
        {
            //TODO DEADLINE 3: Run the query and return the appropriate message
            //DeleteSuccess or the last error in the database
            
            return null;
            
        }
        public override bool Equals(Object obj)
        {
            Delete other = (Delete)obj;
            if(Table==other.Table && Where.ColumnName==other.Where.ColumnName &&
                Where.LiteralValue==other.Where.LiteralValue && Where.Operator==other.Where.Operator)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
