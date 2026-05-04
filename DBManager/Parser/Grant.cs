using DbManager.Parser;
using DbManager.Security;
using System;
using System.Collections.Generic;
using System.Text;

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
            PrivilegeName = privilegeName;
            TableName = tableName;
            ProfileName = profileName;
        }
        public string Execute(Database database)
        {
            //TODO DEADLINE 5: Run the query and return the appropriate message
            //UsersProfileIsNotGrantedRequiredPrivilege, SecurityProfileDoesNotExistError, PrivilegeDoesNotExistError, GrantPrivilegeSuccess, ProfileAlreadyHasPrivilege

            if (database.SecurityManager.ProfileByName(ProfileName) == null)
            {
                return Constants.SecurityProfileDoesNotExistError;
            }
            Profile profileObj = database.SecurityManager.ProfileByName(ProfileName);
            Privilege privilegeObj = PrivilegeUtils.FromPrivilegeName(PrivilegeName);

            if (profileObj.PrivilegesOn[TableName].Contains(privilegeObj))
            {
                //error 1
                return Constants.UsersProfileIsNotGrantedRequiredPrivilege;
            }
            else
            {
                //Execute 
                database.SecurityManager.GrantPrivilege(ProfileName, TableName, privilegeObj);
                return Constants.GrantPrivilegeSuccess;
            }

        }

        public override bool Equals(object obj)
        {
            Grant other = (Grant)obj;
            return (PrivilegeName == other.PrivilegeName && TableName == other.TableName && ProfileName == other.ProfileName);
        }
    }
}
