using System;
using System.Collections.Generic;
using System.Text;
using DbManager.Parser;
using DbManager.Security;

namespace DbManager
{
 
    public class DeleteUser : MiniSqlQuery
    {
        public string Username { get; private set; }

        public DeleteUser(string username)
        {
            //TODO DEADLINE 4: Initialize member variables
            Username= username;
            
        }
        public string Execute(Database database)
        {
            //TODO DEADLINE 5: Run the query and return the appropriate message
            //UsersProfileIsNotGrantedRequiredPrivilege, UserDoesNotExistError, DeleteUserSuccess

            User user= database.SecurityManager.UserByName(Username);

            if(user==null)
            {
                return "Error: Security profile does not exist";
            }           


            if (!database.SecurityManager.IsUserAdmin())
            {
                return "Error: The security profile of the user does not have the required privilege to perform the operation";
            }
            
            bool remove= database.SecurityManager.RemoveProfile(Username);

              if(database.LastErrorMessage!=null)
            {
                return database.LastErrorMessage;
            }
            
            if(!remove)
            {
                return "Security profile does not exist";
            }

            
            return "User deleted";
            
        }

    }
}
