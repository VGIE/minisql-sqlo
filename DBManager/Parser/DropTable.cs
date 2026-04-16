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
            Table= table; 
            
        }

        public string Execute(Database database)
        {
            //TODO DEADLINE 3: Run the query and return the appropriate message
            //DropTableSuccess or the last error in the database
            if (!database.SecurityManager.IsUserAdmin())
            {
                return Constants.UsersProfileIsNotGrantedRequiredPrivilege;
            
            }
            database.DropTable(Table);
             if(database.LastErrorMessage!=null)
            {
                return database.LastErrorMessage;
            }
            
            
            return "Table dropped";
            
        }
    }
}
