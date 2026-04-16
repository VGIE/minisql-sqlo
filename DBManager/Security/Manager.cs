using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DbManager.Security
{
    public class Manager
    {
        public List<Profile> Profiles { get; private set; } = new List<Profile>();

        private string m_username;

        public Manager(string username)
        {
            m_username = username;
        }

        public bool IsUserAdmin()
        {
            //TODO DEADLINE 5: Return true if the user logged-in (m_username) is the admin, false otherwise
            Profile p = ProfileByUser(m_username);
            if (p == null) 
            { 
                return false;
            }

            return p.Name == Profile.AdminProfileName;
        }

        public bool IsPasswordCorrect(string username, string password)
        {
            //TODO DEADLINE 5: Return true if the user's password is correct. The given password should be encrypted before comparing with the saved one
            User u = UserByName(username);
            if (u == null) { return false; }
            string contraseña = Encryption.Encrypt(password);

            return u.EncryptedPassword == contraseña;

        }

        public void GrantPrivilege(string profileName, string table, Privilege privilege)
        {
            //TODO DEADLINE 5: Add this privilege on this table to the profile with this name
            //If the profile or the table don't exist, do nothing
            if (IsUserAdmin())
            {
                Profile p = ProfileByName(profileName);
                if (p == null) { return; }
                if (table == null) { return; }
                p.GrantPrivilege(table, privilege);
            }

        }

        public void RevokePrivilege(string profileName, string table, Privilege privilege)
        {
            //TODO DEADLINE 5: Remove this privilege on this table to the profile with this name
            //If the profile or the table don't exist, do nothing
            if (IsUserAdmin())
            {
                Profile p = ProfileByName(profileName);
                if (p == null) { return; }
                if (table == null) { return; }
                p.RevokePrivilege(table, privilege);
            }

        }

        public bool IsGrantedPrivilege(string username, string table, Privilege privilege)
        {
            //TODO DEADLINE 5: Return true if the username has this privilege on this table. False otherwise (also in case of error)
            
            if (username == null) { return false; }
            if (ProfileByUser(username) != null)
            {
                Profile p = ProfileByUser(username);

                if (p.Name == Profile.AdminProfileName) { return true; }

                //Preguntar a borja si la tabla puede ser null
                if (table == null)
                {
                    return false;
                }
                return p.IsGrantedPrivilege(table, privilege);
            }
            return false;
        }

        public void AddProfile(Profile profile)
        {
            //TODO DEADLINE 5: Add this profile
            if (IsUserAdmin())
            {
                if (profile != null)
                {
                    if (ProfileByName(profile.Name) == null)
                    {
                        Profiles.Add(profile);
                    }

                }
            }

        }

        public User UserByName(string username)
        {
            //TODO DEADLINE 5: Return the user by name. If it doesn't exist, return null

            foreach (var p in Profiles)
            {
                foreach (User u in p.Users)
                {
                    if (u.Username == username)
                    {
                        return u;
                    }
                }
            }
            return null;

        }

        public Profile ProfileByName(string profileName)
        {
            //TODO DEADLINE 5: Return the profile by name. If it doesn't exist, return null

            foreach (var Profile in Profiles)
            {
                if (Profile.Name == profileName)
                {
                    return Profile;
                }
            }
            return null;

        }

        public Profile ProfileByUser(string username)
        {
            //TODO DEADLINE 5: Return the profile by user. If the user doesn't exist, return null

            foreach (var p in Profiles)
            {
                foreach (User u in p.Users)
                {
                    if (username == u.Username) { return p; }
                }
            }

            return null;

        }

        public bool RemoveProfile(string profileName)
        {
            //TODO DEADLINE 5: Remove this profile
            if (IsUserAdmin())
            {
                Profile p = ProfileByName(profileName);
                if (p == null) { return false; }
                Profiles.Remove(p);
                return true;
            }
            return false;
        }

        public static Manager Load(string databaseName, string username)
        {
            //TODO DEADLINE 5: Load all the profiles and users saved for this database. The Manager instance should be created with the given username
            
            string filePath= databaseName+ "_Security.json";

            Manager manager = new Manager(username);

            if (File.Exists(filePath))
            {
                try
                {
                    string jsonString= File.ReadAllText(filePath);
                    var options= new JsonSerializerOptions
                    {
                        IncludeFields = true,
                        PropertyNameCaseInsensitive = true
                    };

                    var Listprofiles= JsonSerializer.Deserialize<List<Profile>>(jsonString, options);
                    if(Listprofiles != null)
                    {
                        manager.Profiles.Clear();
                        //AddRange añade a la lista del perfil concreto todos los perfiles ya deserializados
                        manager.Profiles.AddRange(Listprofiles);
                    }
                    
                }
                catch (Exception ex)
                {
                    throw;   
                }
            }
            
            return manager;
        }

        public void Save(string databaseName)
        {
             //TODO DEADLINE 5: Save all the profiles and users/passwords created for this database.

            try
            {
                string filePath= databaseName+ "_Security.json";

                var options= new JsonSerializerOptions
                {
                    //Para que el json sea legible añadiendo espacios y saltos de línea
                    WriteIndented = true, 
                    //Para incluir los campos privados y que no se pierda ningún dato 
                    IncludeFields = true,
                    //Para mayúsculas y minúsculas sin que importe
                    PropertyNameCaseInsensitive = true
                };
                
                //JsonSerializer.Serialize se encarga de recorrer la lista de perfiles y sus usuarios
                //y transforma toda la información de nombres, constraseñas etc en formato json 
                string jsonString= JsonSerializer.Serialize(this.Profiles, options);
                File.WriteAllText(filePath, jsonString);
            }
            catch (Exception ex)
            {
                throw;        
            }
        }
    }
}
