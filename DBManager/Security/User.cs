using System;
using System.Security.Cryptography;
using System.Text;


namespace DbManager.Security
{
    public class User
    {
        public string Username { get; set; }
        public string EncryptedPassword { get; set; }

        public User(string username, string password)
        {
            //TODO DEADLINE 5: Initialize the member variables. We must encrypt the password
            Username = username;
            EncryptedPassword = Encryption.Encrypt(password);
        }

        public User() { }

        public override bool Equals(object obj)
        {
            User other = (User)obj;
            if(Username.Equals(other.Username) && (EncryptedPassword.Equals(other.EncryptedPassword)))
            {
                return true;
            }
            return false;
        }
    }
}
