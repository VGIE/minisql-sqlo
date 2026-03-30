using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
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
            return false;
        }

        public bool IsPasswordCorrect(string username, string password)
        {
            //TODO DEADLINE 5: Return true if the user's password is correct. The given password should be encrypted before comparing with the saved one
            return false;
            
        }

        public void GrantPrivilege(string profileName, string table, Privilege privilege)
        {
            //TODO DEADLINE 5: Add this privilege on this table to the profile with this name
            //If the profile or the table don't exist, do nothing
            
        }

        public void RevokePrivilege(string profileName, string table, Privilege privilege)
        {
            //TODO DEADLINE 5: Remove this privilege on this table to the profile with this name
            //If the profile or the table don't exist, do nothing
            
        }

        public bool IsGrantedPrivilege(string username, string table, Privilege privilege)
        {
            //TODO DEADLINE 5: Return true if the username has this privilege on this table. False otherwise (also in case of error)
            
            return false;
            
        }

        public void AddProfile(Profile profile)
        {
            //TODO DEADLINE 5: Add this profile
            
        }

        public User UserByName(string username)
        {
            //TODO DEADLINE 5: Return the user by name. If it doesn't exist, return null
            
            return null;
            
        }

        public Profile ProfileByName(string profileName)
        {
            //TODO DEADLINE 5: Return the profile by name. If it doesn't exist, return null
            
            return null;
            
        }

        public Profile ProfileByUser(string username)
        {
            //TODO DEADLINE 5: Return the profile by user. If the user doesn't exist, return null
            foreach (Profile p in Profiles)
            {
                foreach (User u in p.Users)
                {
                    if (u.Username.Equals(username))
                    {
                        return p;
                    }
                }
            }
            return null;
            
        }

        public bool RemoveProfile(string profileName)
        {
            //TODO DEADLINE 5: Remove this profile
            foreach (Profile p in Profiles)
            {

                if (p.Name.Equals(profileName))
                {
                    Profiles.Remove(p);
                }
            }
            return false;
        }

        public static Manager Load(string databaseName, string username)
        {
            //TODO DEADLINE 5: Load all the profiles and users saved for this database. The Manager instance should be created with the given username

            Manager manager = new Manager(username);
            if (!Directory.Exists(databaseName))
            {
                return null;
            }
            String[] files = Directory.GetFiles(databaseName, "*-profile.txt");
            foreach(String file in files)
            {
                
                StreamReader reader = new StreamReader(file);
                String[] fullFileName = Path.GetFileNameWithoutExtension(file).Split("-");
                String profileName = fullFileName[0];
                Profile p = new Profile();
                p.Name = profileName;
                String line;
                List<Profile> profiles = new List<Profile>();
                line = reader.ReadLine();
                if (line.Equals("{TABLE} || {PRIVILEGES}"))
                {
                    break;
                }
                while ((line = reader.ReadLine())!="{USERNAME} || {PASSWORD}")
                {
                    String[] parts = line.Split(":");
                    List<Privilege> privilegesList = new List<Privilege>();
                    String[] privileges = parts[1].Split(" ");
                    foreach(String s in privileges)
                    {
                        Privilege privilegeparse;
                        switch (s)
                        {
                            case "Delete":
                                 privilegeparse = Privilege.Delete;
                                break;
                            case "Insert":
                                privilegeparse = Privilege.Insert;
                                break;
                            case "Update":
                                privilegeparse = Privilege.Update;
                                break;
                            case "Select":
                                privilegeparse = Privilege.Select;
                                break;
                            default:
                                privilegeparse = Privilege.Select;
                                break;
                        }
                        privilegesList.Add(privilegeparse);
                    }
                    p.PrivilegesOn.Add(parts[0], privilegesList);
                }
                while (reader.ReadLine() != null)
                {
                    //ME SALTO UN USUARIO ENTRE EL FINAL DEL ANTERIOR WHILE Y EL COMIENZO DE ESTE
                }
            }
            
            
        }

        public void Save(string databaseName)
        {
            //TODO DEADLINE 5: Save all the profiles and users/passwords created for this database.
            if (Directory.Exists(databaseName))
            {
                Directory.CreateDirectory(databaseName);
            }
            foreach(Profile p in Profiles)
            {
                TextWriter writer = null;
                try
                {
                    writer = File.CreateText(databaseName + "\\" + p.Name + "-profile.txt");
                    writer.WriteLine("{TABLE} || {PRIVILEGES}");
                    foreach (var privilege in p.PrivilegesOn)
                    {
                        foreach(Privilege pr in privilege.Value)
                        {
                            writer.WriteLine(privilege.Key+":");
                            writer.Write(pr+" ");
                        }
                        
                    }
                    writer.WriteLine("{USERNAME} || {PASSWORD}");
                    foreach (User u in p.Users)
                    {
                        writer.WriteLine(u.Username + " || " + u.EncryptedPassword);
                    }
                    writer.Close();
                }
                catch (Exception ex) { }
            }
        }
    }
}
