using DbManager.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OurTests.SecurityTest
{
    public class ManagerTest
    {
        [Fact]
        public void ProfileByNameTest()
        {
            Profile p = new Profile { Name = "a" };
            Profile p2 = new Profile { Name = "b" };
            Manager manager = new Manager("c");

            Assert.Null(manager.ProfileByName("a"));

            manager.Profiles.Add(p);
            Assert.True(p == manager.ProfileByName("a"));
            Assert.Null(manager.ProfileByName("b"));

            manager.Profiles.Add(p2);
            Assert.True(p2 == manager.ProfileByName("b"));
            Assert.Null(manager.ProfileByName("c"));
        }

        [Fact]
        public void IsUserAdminTest()
        {
            Profile p = new Profile { Name = "Admin" };
            User u = new User();
            u.Username = "Admin";
            u.EncryptedPassword = "password";
            p.Users.Add(u);

            Manager manager = new Manager("Admin");
            Assert.False(manager.IsUserAdmin());
            manager.Profiles.Add(p);
            Assert.True(manager.IsUserAdmin());

            User u2 = new User();
            u2.Username = "Admin";
            u2.EncryptedPassword = "password";
            p.Users.Add(u2);

            Assert.True(manager.IsUserAdmin());
        }

        [Fact]

        public void IsPasswordCorrectTest()
        {
            User user = new User("Admin", "abcd");
            Manager manager = new Manager("Admin");
            Profile p = new Profile();
            p.Name = "Admin";
            p.Users.Add(user);

            Assert.False(manager.IsPasswordCorrect("Admin", "abcd"));

            manager.Profiles.Add(p);
            Assert.False(manager.IsPasswordCorrect("AraG", "adcd"));
            Assert.False(manager.IsPasswordCorrect("Admin", "dcd"));
            Assert.True(manager.IsPasswordCorrect("Admin", "abcd"));
        }

        [Fact]

        public void GrantPrivilegeTest()
        {
            User user = new User("Admin", "abcd");
            User user2 = new User("Wiwiwiwi", "h100");

            Manager manager = new Manager("Admin");
            Profile p = new Profile();
            p.Name = "Admin";
            p.Users.Add(user);
            Profile p2 = new Profile();
            p2.Users.Add(user2);

            manager.GrantPrivilege("Admin", "t", Privilege.Select);
            Assert.Equal(p.PrivilegesOn, p2.PrivilegesOn);
            manager.Profiles.Add(p);
            manager.GrantPrivilege("Admin", "t", Privilege.Select);
            Assert.NotEqual(p.PrivilegesOn, p2.PrivilegesOn);

            manager.Profiles.Add(p2);

            manager.GrantPrivilege("Wiwiwiwi", null, Privilege.Select);
            Assert.NotEqual(p.PrivilegesOn, p2.PrivilegesOn);
            manager.GrantPrivilege("Wiwiwiwi", "t2", Privilege.Select);
            Assert.NotEqual(p.PrivilegesOn, p2.PrivilegesOn);
        }

        [Fact]

        public void RevokePrivilegeTest()
        {
            User user = new User("Admin", "abcd");
            User user2 = new User("Wiwiwiwi", "h100");
            User user3 = new User("Lupita", "l1006");

            Manager manager = new Manager("Admin");
            Profile p = new Profile();
            p.Users.Add(user);
            Profile p2 = new Profile();
            p2.Users.Add(user2);
            manager.Profiles.Add(p2);

            manager.GrantPrivilege("Admin", "t", Privilege.Select);
            manager.GrantPrivilege("Wiwiwiwi", "t2", Privilege.Select);
            manager.GrantPrivilege("Admin", "t2", Privilege.Select);

            manager.RevokePrivilege("Admin", "t", Privilege.Select);
            Assert.Equal(p.PrivilegesOn, p2.PrivilegesOn);

            //asegurarme de que no peta al intentar un user q no esta en el manager
            manager.RevokePrivilege("Lupita", "t", Privilege.Select);

            manager.RevokePrivilege("Admin", null, Privilege.Select);
            Assert.Equal(p.PrivilegesOn, p2.PrivilegesOn);
            manager.RevokePrivilege("Admin", "t3", Privilege.Select);
            Assert.Equal(p.PrivilegesOn, p2.PrivilegesOn);

            Profile p3 = new Profile();
            p3.Users.Add(user3);
            manager.Profiles.Add(p3);

            manager.RevokePrivilege("Admin", "t2", Privilege.Select);
            Assert.Equal(p.PrivilegesOn, p3.PrivilegesOn);
        }

        [Fact]

        public void IsGrantedPrivilegeTest()
        {
            //Profile by user no va
            User user = new User("Admin", "abcd");

            Manager manager = new Manager("Admin");

            Assert.False(manager.IsGrantedPrivilege("Admin", "t", Privilege.Select));

            Profile p = new Profile();
            p.Name = "p1";
            p.Users.Add(user);
            manager.Profiles.Add(p);

            Assert.False(manager.IsGrantedPrivilege(null, "t", Privilege.Select));
            Assert.False(manager.IsGrantedPrivilege("Admin", null, Privilege.Delete));
            Assert.False(manager.IsGrantedPrivilege("Arago", "t", Privilege.Delete));
            Assert.False(manager.IsGrantedPrivilege("Admin", "t", Privilege.Delete));

            p.GrantPrivilege("t",Privilege.Select);
            Assert.True(manager.IsGrantedPrivilege("Admin", "t", Privilege.Select));
            Assert.False(manager.IsGrantedPrivilege("Admin", null, Privilege.Select));
            Assert.False(manager.IsGrantedPrivilege("Admin", "t", Privilege.Delete));
        }

        [Fact]

        public void ProfileByUserTest()
        {
            User user = new User("Admin", "abcd");
            User user2 = new User("Wiwiwiwi", "h100");
            User user3 = new User("Lupita", "l1006");

            Manager manager = new Manager("Admin");
            Profile p = new Profile();
            p.Users.Add(user);
            p.Name = "a";
            Profile p2 = new Profile();
            p2.Users.Add(user2);
            p.Name = "b";
            //el addProfile separa los profiles por nombre
            //si los dos no tienen nombre (name=null), no añade el segundo
            manager.Profiles.Add(p);
            Assert.Equal(p, manager.ProfileByUser("Admin"));

            manager.Profiles.Add(p2);
            Assert.Equal(p2, manager.ProfileByUser("Wiwiwiwi"));
            Assert.Null(manager.ProfileByName("Lupita"));
        }

        [Fact]

        public void RemoveProfileTest()
        {

        }

    }
}
