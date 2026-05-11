using DbManager.Parser;
using DbManager.Security;
using System;
using System.Collections.Generic;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

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
            PrivilegeName = privilegeName;
            TableName = tableName;
            ProfileName = profileName;
        }
        public string Execute(Database database)
        {
            //TODO DEADLINE 5: Run the query and return the appropriate message
            //UsersProfileIsNotGrantedRequiredPrivilege, SecurityProfileDoesNotExistError, RevokePrivilegeSuccess, 
            if (database.SecurityManager.ProfileByName(ProfileName) == null)
            {
                return Constants.SecurityProfileDoesNotExistError;
            }
            if (!database.SecurityManager.IsUserAdmin())
            {
                return Constants.UsersProfileIsNotGrantedRequiredPrivilege;
            }
            Privilege privilegeObj;
            Profile profileObj = database.SecurityManager.ProfileByName(ProfileName);

            bool error = false;
            switch (PrivilegeName)
            {
                case "DELETE":
                    privilegeObj = Privilege.Delete;
                    break;
                case "UPDATE":
                    privilegeObj = Privilege.Update;
                    break;
                case "INSERT":
                    privilegeObj = Privilege.Insert;
                    break;
                default:
                    privilegeObj = Privilege.Select;
                    break;
            }
            if (error)
            {
                return Constants.PrivilegeDoesNotExistError;
            }


            //Execute 
            database.SecurityManager.RevokePrivilege(ProfileName, TableName, privilegeObj);
            return Constants.RevokePrivilegeSuccess;

        }

        public override bool Equals(object obj)
        {
            Revoke other = (Revoke)obj;
            return (PrivilegeName == other.PrivilegeName && TableName == other.TableName && ProfileName == other.ProfileName);
        }
    }
}
