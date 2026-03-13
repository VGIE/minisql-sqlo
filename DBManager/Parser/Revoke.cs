using System;
using System.Collections.Generic;
using System.Text;
using DbManager.Parser;
using DbManager.Security;

namespace DbManager
{
 
    public class Revoke : MiniSqlQuery
    {
        public string PrivilegeName { get; set; }
        public string TableName { get; set; }
        public string ProfileName { get; set; }

        public Revoke(string privilegeName, string tableName, string profileName)
        {
            //TODO DEADLINE 4: Initialize member variables
            PrivilegeName= privilegeName;
            TableName=tableName;
            ProfileName=profileName; 
            
        }
        public string Execute(Database database)
        {
            //TODO DEADLINE 5: Run the query and return the appropriate message
            //UsersProfileIsNotGrantedRequiredPrivilege, SecurityProfileDoesNotExistError, RevokePrivilegeSuccess, 

            Profile profile= database.SecurityManager.ProfileByName(ProfileName);

            if (profile==null)
            {
                return "Error: Security profile does not exist";
            }
    
            Privilege revokePrivilege= PrivilegeUtils.FromPrivilegeName(PrivilegeName);

            if (!profile.IsGrantedPrivilege(TableName, revokePrivilege))
            {
                return "Error: The security profile of the user does not have the required privilege to perform the operation";
            }
            
            database.SecurityManager.RevokePrivilege(PrivilegeName, TableName, revokePrivilege);

            if(database.LastErrorMessage!=null)
            {
                return database.LastErrorMessage;

            }
            
            return "Security privilege revoked"; 
        }
        
    }

}
