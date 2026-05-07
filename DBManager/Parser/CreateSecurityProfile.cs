using DbManager.Parser;
using DbManager.Security;
using System;
using System.Collections.Generic;
using System.Text;

namespace DbManager
{
 
    public class CreateSecurityProfile : MiniSqlQuery
    {
        public string ProfileName { get; set; }

        public CreateSecurityProfile(string profileName)
        {
            //TODO DEADLINE 4: Initialize member variables
            ProfileName = profileName;
            
        }
        public string Execute(Database database)
        {
            //TODO DEADLINE 5: Run the query and return the appropriate message
            //UsersProfileIsNotGrantedRequiredPrivilege, CreateSecurityProfileSuccess
            Profile profileObj = database.SecurityManager.ProfileByName(ProfileName);
            if (profileObj == null)
            {
                return Constants.SecurityProfileDoesNotExistError;
            }
            if (!(database.SecurityManager.IsUserAdmin()))
            {
                return Constants.UsersProfileIsNotGrantedRequiredPrivilege;
            }

            Profile profile = new Profile() { Name = ProfileName };
            database.SecurityManager.AddProfile(profile);
            return Constants.CreateSecurityProfileSuccess;
            
        }

        public override bool Equals(object obj)
        {
            CreateSecurityProfile other = (CreateSecurityProfile)obj;
            return (ProfileName == other.ProfileName);
        }
    }
}
