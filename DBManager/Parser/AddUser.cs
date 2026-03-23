using System;
using System.Collections.Generic;
using System.Text;
using DbManager.Parser;

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
            Username = username;
            Password = password;
            ProfileName = profileName;
            
        }
        public string Execute(Database database)
        {
            //TODO DEADLINE 5: Run the query and return the appropriate message
            //UsersProfileIsNotGrantedRequiredPrivilege, SecurityProfileDoesNotExistError, AddUserSuccess
            
            return null;
            
        }

        public override bool Equals(object obj)
        {
            AddUser other = (AddUser)obj;
            return (Username == other.Username && Password == other.Password && ProfileName == other.ProfileName);
        }
    }
}
