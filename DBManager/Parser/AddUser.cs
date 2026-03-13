using System;
using System.Collections.Generic;
using System.Text;
using DbManager.Parser;
using DbManager.Security;

namespace DbManager
{
 
    public class AddUser : MiniSqlQuery
    {
        public string Username { get; private set; }
        public string Password { get; private set; }
        public string ProfileName { get; private set; }


        public AddUser(string username, string password, string profileName)
        {
            //TODO DEADLINE 4: Initialize member variables
            Username= username;
            Password=password;
            ProfileName=profileName;
            
        }
        public string Execute(Database database)
        {
            //TODO DEADLINE 5: Run the query and return the appropriate message
            //UsersProfileIsNotGrantedRequiredPrivilege, SecurityProfileDoesNotExistError, AddUserSuccess
            Profile profile= database.SecurityManager.ProfileByName(Username);

            if(profile==null)
            {
               return "Error: Security profile does not exist";
            }

                User user= new User();
                user.Username= Username;
                user.EncryptedPassword=Password;

                profile.Users.Add(user);

                database.SecurityManager.AddProfile(profile);
                
             if (!database.SecurityManager.IsUserAdmin())
            {
                return "Error: The security profile of the user does not have the required privilege to perform the operation";
            }
                
             if(database.LastErrorMessage!=null)
            {
                return database.LastErrorMessage;
            }

            
            return "User added";
            
        }

    }
}
