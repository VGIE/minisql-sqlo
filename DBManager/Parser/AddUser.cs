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
                User user= new User();
                user.Username= Username;
                user.EncryptedPassword=Password;

               //database.SecurityManager.AddUser(user);
            }
             if(database.LastErrorMessage!=null)
            {
                return database.LastErrorMessage;
            }

            
            return "AddUserSuccess";
            
        }

    }
}
