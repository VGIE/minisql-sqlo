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

            Profile profile= database.SecurityManager.ProfileByName(ProfileName);

            if (!database.SecurityManager.IsUserAdmin())
            {
                return "Error: The security profile of the user does not have the required privilege to perform the operation";
            }

            if (profile==null)
            {
                return "Error: Security profile does not exist";
            }

            Privilege grantprivilege= PrivilegeUtils.FromPrivilegeName(PrivilegeName);

            if (grantprivilege==null)
            {
                return "Error: Privilege does not exist";
            }

            if (profile.IsGrantedPrivilege(TableName, grantprivilege))
            {
                return "Error: Profile already has privilege";
            }

            database.SecurityManager.GrantPrivilege(PrivilegeName, TableName,grantprivilege);

            if(database.LastErrorMessage!=null)
            {
                return database.LastErrorMessage;
            }
            
            return "Security privilege granted";
            
        }

    }
}
