using System;
using System.Collections.Generic;
using System.Text;
using DbManager.Parser;

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
            bool remove= database.SecurityManager.RemoveProfile(Username);

              if(database.LastErrorMessage!=null)
            {
                return database.LastErrorMessage;
            }
            
            if(!remove)
            {
                return "UserDoesNotExistError";
            }

            
            return "DeleteUserSucces";
            
        }

    }
}
