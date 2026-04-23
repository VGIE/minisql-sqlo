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
            Privilege privilegeObj;
            Profile profileObj = database.SecurityManager.ProfileByName(ProfileName);

            switch (PrivilegeName)
            {
                case "Delete":
                    privilegeObj = Privilege.Delete;
                    break;
                case "Update":
                    privilegeObj = Privilege.Update;
                    break;
                case "Insert":
                    privilegeObj = Privilege.Insert;
                    break;
                default:
                    privilegeObj = Privilege.Select;
                    break;
            }
            if (profileObj.PrivilegesOn[TableName].Contains(privilegeObj))
            {
                //error 1
                return Constants.UsersProfileIsNotGrantedRequiredPrivilege;
            }
            else
            {
                //Execute 
                database.SecurityManager.RevokePrivilege(ProfileName, TableName, privilegeObj);
                return Constants.RevokePrivilegeSuccess;
            }

        }

        public override bool Equals(object obj)
        {
            Revoke other = (Revoke)obj;
            return (PrivilegeName == other.PrivilegeName && TableName == other.TableName && ProfileName == other.ProfileName);
        }
    }
}
