using DbManager.Parser;
using System;
using System.Collections.Generic;
using System.Text;

namespace DbManager
{
    public class DropTable: MiniSqlQuery
    {
        public string Table { get; private set; }

        public DropTable(string table)
        {
            //TODO DEADLINE 2: Initialize member variables
            Table = table;
        }

        public string Execute(Database database)
        {
            //TODO DEADLINE 3: Run the query and return the appropriate message
            //DropTableSuccess or the last error in the database
            
            return null;
            
        }

        public override bool Equals(object obj)
        {
            DropTable other = (DropTable)obj;
            if (Table == other.Table)
            {
                return true;
            }
            return false;
        }
    }
}
