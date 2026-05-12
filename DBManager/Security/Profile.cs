using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DbManager.Security
{
    public class Profile
    {
        public const string AdminProfileName = "Admin";
        public string Name { get; set; }
        public List<User> Users { get; set; } = new List<User>();

        public Dictionary<string, List<Privilege>> PrivilegesOn { get; private set; } = new Dictionary<string, List<Privilege>>();

        public bool GrantPrivilege(string table, Privilege privilege)
        {
            //TODO DEADLINE 5: Grant this privilege on this table. Return false if there is an error, true otherwise
            if (PrivilegesOn.ContainsKey(table))
            {
                PrivilegesOn[table].Add(privilege);
                return true;
            }
            return false;
            
        }

        public bool RevokePrivilege(string table, Privilege privilege)
        {
            //TODO DEADLINE 5: Revoke this privilege on this table. Return false if there is an error, true otherwise

            if (PrivilegesOn.ContainsKey(table))
            {
                    PrivilegesOn[table].Remove(privilege);
                    return true;    
            }
            return false;
        }

        public bool IsGrantedPrivilege(string table, Privilege privilege)
        {
            //TODO DEADLINE 5: Return whether this profile is granted this privilege on this table
            if (Name.Equals(AdminProfileName))
            {
                return true;
            }
            if (PrivilegesOn[table].Contains(privilege))
            {
                return true;
            }
            return false;
        }
        public void SetListOfPrivileges4Testing(Dictionary<string, List<Privilege>> FakePrivilegesOn)
        {
            this.PrivilegesOn = FakePrivilegesOn;
        }
        public override bool Equals(object obj)
        {
            if (!(obj is Profile profile2))
            {
                return false;
            }

            if (this.Name != profile2.Name)
            {
                return false;
            }

            if (this.Users == null && profile2.Users != null) return false;
            if (this.Users != null && profile2.Users == null) return false;

            if (this.Users != null && profile2.Users != null)
            {
                if (this.Users.Count != profile2.Users.Count)
                {
                    return false;
                }

                for (int i = 0; i < this.Users.Count; i++)
                {
                    User user1 = this.Users[i];
                    User user2 = profile2.Users[i];

                    if (user1 == null && user2 != null) return false;
                    if (user1 != null && !user1.Equals(user2)) return false;
                }
            }

            if (this.PrivilegesOn == null && profile2.PrivilegesOn != null) return false;
            if (this.PrivilegesOn != null && profile2.PrivilegesOn == null) return false;

            if (this.PrivilegesOn != null && profile2.PrivilegesOn != null)
            {
                if (this.PrivilegesOn.Count != profile2.PrivilegesOn.Count)
                {
                    return false;
                }

                foreach (KeyValuePair<string, List<Privilege>> value in this.PrivilegesOn)
                {
                    string table = value.Key;
                    List<Privilege> misPrivilegios = value.Value;

                    if (!profile2.PrivilegesOn.ContainsKey(table))
                    {
                        return false;
                    }

                    List<Privilege> susPrivilegios = profile2.PrivilegesOn[table];

                    if (misPrivilegios == null && susPrivilegios != null) return false;
                    if (misPrivilegios != null && susPrivilegios == null) return false;

                    if (misPrivilegios != null && susPrivilegios != null)
                    {
                        if (misPrivilegios.Count != susPrivilegios.Count)
                        {
                            return false;
                        }

                        for (int i = 0; i < misPrivilegios.Count; i++)
                        {
                            Privilege p1 = misPrivilegios[i];
                            Privilege p2 = susPrivilegios[i];

                          
                            if (p1 != null && !p1.Equals(p2)) return false;
                        }
                    }
                }
            }

            return true;
        }
    }
}
