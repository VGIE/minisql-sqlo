using DbManager.Parser;
using DbManager.Security;
using System;
using System.Collections.Generic;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

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
            //UsersProfileIsNotGrantedRequiredPrivilege, SecurityProfileDoesNotExistError, AddUserSuccessç

            Profile profile = database.SecurityManager.ProfileByName(ProfileName);

            if (profile == null)
            {
                return Constants.SecurityProfileDoesNotExistError;
            }

            if (!database.SecurityManager.IsUserAdmin())
            {
                return Constants.UsersProfileIsNotGrantedRequiredPrivilege;
            }

            User user = new User(Username, Password);
            profile.Users.Add(user);

            return Constants.AddUserSuccess;

        }
    }
}
