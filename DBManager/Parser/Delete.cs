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


            bool result = database.DeleteWhere(Table,Where);

            if(result == true)
            {
                return "DeleteSuccess";
            }
            else
            {
                return database.LastErrorMessage;
            }


            
        }
        public override bool Equals(Object obj)
        {
            Delete other = (Delete)obj;
            //First we compare extreme cases where 'WHERE' clause is null in one side (we also discard different table names)
            if(Table != other.Table || (Where!=null&&other.Where==null || Where==null&&other.Where!=null))
            {
                return false;
            }
            //Then, we compare both null case -> true
            else if (other.Where==null&&Where==null)
            {
                return true;
            }
            //Last, with both null cases ruled out, we can compare Where clauses one by one
            else if(Where.ColumnName == other.Where.ColumnName && Where.LiteralValue == other.Where.LiteralValue &&
                Where.Operator == other.Where.Operator)
            {
                return true;
            }
            //Code didn't found a match, thus they are not equal
            else
            {
                return false;
            }
        }
    }
}
