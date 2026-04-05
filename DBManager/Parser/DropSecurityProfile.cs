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


            if (!database.SecurityManager.IsUserAdmin())
            {
                return Constants.UsersProfileIsNotGrantedRequiredPrivilege;
            }

            Profile profile = database.SecurityManager.ProfileByName(ProfileName);

            if (profile == null)
            {
                return Constants.SecurityProfileDoesNotExistError;
            }


            bool remove = database.SecurityManager.RemoveProfile(ProfileName);

            if (!remove)
            {
                return Constants.SecurityProfileDoesNotExistError;
            }

            return Constants.DropSecurityProfileSuccess;

        }

    }
}
