using System;
using System.Collections.Generic;
using System.Text;
using DbManager.Parser;
using DbManager.Security;

namespace DbManager
{
 
    public class CreateSecurityProfile : MiniSqlQuery
    {
        public string ProfileName { get; set; }

        public CreateSecurityProfile(string profileName)
        {
            //TODO DEADLINE 4: Initialize member variables
            ProfileName=profileName;
            
        }
        public string Execute(Database database)
        {
            //TODO DEADLINE 5: Run the query and return the appropriate message
            //UsersProfileIsNotGrantedRequiredPrivilege, CreateSecurityProfileSuccess
            
            Profile profile= new Profile();
            profile.Name= ProfileName;

             if (!database.SecurityManager.IsUserAdmin())
            {
                return "Error: The security profile of the user does not have the required privilege to perform the operation";
            }

            database.SecurityManager.AddProfile(profile);
            
            if(database.LastErrorMessage!=null)
            {
                return database.LastErrorMessage;
            }
          
            
            return "Security profile created";
            
        }

    }
}
