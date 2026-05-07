using System;
using System.Collections.Generic;
using System.Text;
using DbManager.Parser;
using DbManager.Security;

namespace DbManager
{
 
    public class DropSecurityProfile : MiniSqlQuery
    {
        public string ProfileName { get; set; }

        public DropSecurityProfile(string profileName)
        {
            //TODO DEADLINE 4: Initialize member variables
            ProfileName = profileName;
        }
        public string Execute(Database database)
        {
            //TODO DEADLINE 5: Run the query and return the appropriate message
            //UsersProfileIsNotGrantedRequiredPrivilege, SecurityProfileDoesNotExistError, DropSecurityProfileSuccess
            Profile profileObj = database.SecurityManager.ProfileByName(ProfileName);
            if (profileObj == null)
            {
                return Constants.SecurityProfileDoesNotExistError;
            }
            if (!(database.SecurityManager.IsUserAdmin()))
            {
                return Constants.UsersProfileIsNotGrantedRequiredPrivilege;
            }

            database.SecurityManager.RemoveProfile(ProfileName);
            return Constants.DropSecurityProfileSuccess;
            
        }

        public override bool Equals(object obj)
        {
            DropSecurityProfile other = (DropSecurityProfile)obj;
            return (ProfileName == other.ProfileName);
        }

    }
}
