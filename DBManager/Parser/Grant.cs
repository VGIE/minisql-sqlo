using System;
using System.Collections.Generic;
using System.Text;
using DbManager.Parser;
using DbManager.Security;

namespace DbManager
{
 
    public class Grant : MiniSqlQuery
    {
        public string PrivilegeName { get; set; }
        public string TableName { get; set; }
        public string ProfileName { get; set; }

        public Grant(string privilegeName, string tableName, string profileName)
        {
            //TODO DEADLINE 4: Initialize member variables
            PrivilegeName= privilegeName;
            TableName= tableName;
            ProfileName= profileName;
            
        }
        public string Execute(Database database)
        {
            //TODO DEADLINE 5: Run the query and return the appropriate message
            //UsersProfileIsNotGrantedRequiredPrivilege, SecurityProfileDoesNotExistError, PrivilegeDoesNotExistError, GrantPrivilegeSuccess, ProfileAlreadyHasPrivilege
            database.SecurityManager.GrantPrivilege(PrivilegeName, TableName,PrivilegeUtils.FromPrivilegeName(ProfileName));
            if(database.LastErrorMessage!=null)
            {
                return database.LastErrorMessage;
            }

            
            return "GrantPrivilegeSuccess";
            
        }

    }
}
