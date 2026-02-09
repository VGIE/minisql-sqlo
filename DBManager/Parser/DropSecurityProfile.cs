using System;
using System.Collections.Generic;
using System.Text;
using DbManager.Parser;

namespace DbManager
{
 
    public class DropSecurityProfile : MiniSqlQuery
    {
        public string ProfileName { get; set; }

        public DropSecurityProfile(string profileName)
        {
            //TODO DEADLINE 4: Initialize member variables
            ProfileName= profileName;
            
        }
        public string Execute(Database database)
        {
            //TODO DEADLINE 5: Run the query and return the appropriate message
            //UsersProfileIsNotGrantedRequiredPrivilege, SecurityProfileDoesNotExistError, DropSecurityProfileSuccess
            bool remove= database.SecurityManager.RemoveProfile(ProfileName);

             if(database.LastErrorMessage!=null)
            {
                return database.LastErrorMessage;
            }
            
            if(!remove)
            {
                return "SecurityProfileDoesNotExistError";
            }

            
            return "DropSecurityProfileSuccess";
            
        }

    }
}
