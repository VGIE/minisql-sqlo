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

            manager.AddProfile(p);
            Assert.True(p == manager.ProfileByName("a"));
            Assert.Null(manager.ProfileByName("b"));

            manager.AddProfile(p2);
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
            manager.AddProfile(p);
            Assert.True(manager.IsUserAdmin());

            User u2 = new User();
            u2.Username = "Admin";
            u2.EncryptedPassword = "password";
            p.Users.Add(u);

            Assert.True(manager.IsUserAdmin());
        }

        [Fact]

        public void IsPasswordCorrectTest()
        {
            User user = new User("AraGo", "abcd");
            Manager manager = new Manager("AraGo");
            Profile p = new Profile();
            p.Name = "AraGo";
            p.Users.Add(user);

            Assert.False(manager.IsPasswordCorrect("AraGo", "abcd"));

            manager.AddProfile(p);
            Assert.False(manager.IsPasswordCorrect("AraG", "adcd"));
            Assert.False(manager.IsPasswordCorrect("AraGo", "dcd"));
            Assert.True(manager.IsPasswordCorrect("AraGo", "abcd"));
        }

        [Fact]

        public void GrantPrivilegeTest()
        {
            User user = new User("AraGo", "abcd");
            User user2 = new User("Wiwiwiwi", "h100");

            Manager manager = new Manager("AraGo");
            Profile p = new Profile();
            p.Name = "AraGo";
            p.Users.Add(user);
            Profile p2 = new Profile();
            p2.Users.Add(user2);

            manager.GrantPrivilege("AraGo", "t", Privilege.Select);
            Assert.Equal(p.PrivilegesOn, p2.PrivilegesOn);
            manager.AddProfile(p);
            manager.GrantPrivilege("AraGo", "t", Privilege.Select);
            Assert.NotEqual(p.PrivilegesOn, p2.PrivilegesOn);

            manager.AddProfile(p2);

            manager.GrantPrivilege("Wiwiwiwi", null, Privilege.Select);
            Assert.NotEqual(p.PrivilegesOn, p2.PrivilegesOn);
            manager.GrantPrivilege("Wiwiwiwi", "t2", Privilege.Select);
            Assert.NotEqual(p.PrivilegesOn, p2.PrivilegesOn);
        }

        [Fact]

        public void RevokePrivilegeTest()
        {
            User user = new User("AraGo", "abcd");
            User user2 = new User("Wiwiwiwi", "h100");
            User user3 = new User("Lupita", "l1006");

            Manager manager = new Manager("lorolo");
            Profile p = new Profile();
            p.Users.Add(user);
            Profile p2 = new Profile();
            p2.Users.Add(user2);
            manager.AddProfile(p2);

            manager.GrantPrivilege("AraGo", "t", Privilege.Select);
            manager.GrantPrivilege("Wiwiwiwi", "t2", Privilege.Select);
            manager.GrantPrivilege("AraGo", "t2", Privilege.Select);

            manager.RevokePrivilege("AraGo", "t", Privilege.Select);
            Assert.Equal(p.PrivilegesOn, p2.PrivilegesOn);

            //asegurarme de que no peta al intentar un user q no esta en el manager
            manager.RevokePrivilege("Lupita", "t", Privilege.Select);

            manager.RevokePrivilege("AraGo", null, Privilege.Select);
            Assert.Equal(p.PrivilegesOn, p2.PrivilegesOn);
            manager.RevokePrivilege("AraGo", "t3", Privilege.Select);
            Assert.Equal(p.PrivilegesOn, p2.PrivilegesOn);

            Profile p3 = new Profile();
            p3.Users.Add(user3);
            manager.AddProfile(p3);

            manager.RevokePrivilege("AraGo", "t2", Privilege.Select);
            Assert.Equal(p.PrivilegesOn, p3.PrivilegesOn);
        }

        [Fact]

        public void IsGrantedPrivilegeTest()
        {
            //Profile by user no va
            User user = new User("AraGo", "abcd");

            Manager manager = new Manager("lorolo");

            Assert.False(manager.IsGrantedPrivilege("AraGo", "t", Privilege.Select));

            Profile p = new Profile();
            p.Name = "a";
            p.Users.Add(user);
            manager.AddProfile(p);

            manager.GrantPrivilege("a", "t", Privilege.Select);
            Assert.True(manager.IsGrantedPrivilege("AraGo", "t", Privilege.Select));

            manager.GrantPrivilege("a", "t2", Privilege.Delete);
            Assert.True(manager.IsGrantedPrivilege("AraGo", "t2", Privilege.Delete));

            Assert.False(manager.IsGrantedPrivilege("AraGo", "t", Privilege.Delete));
            Assert.False(manager.IsGrantedPrivilege("AraGo", "t2", Privilege.Select));

            manager.RevokePrivilege("a", "t", Privilege.Select);
            Assert.False(manager.IsGrantedPrivilege("AraGo", "t", Privilege.Select));
            Assert.False(manager.IsGrantedPrivilege("AraGo", null, Privilege.Select));
        }

        [Fact]

        public void ProfileByUserTest()
        {
            User user = new User("AraGo", "abcd");
            User user2 = new User("Wiwiwiwi", "h100");
            User user3 = new User("Lupita", "l1006");

            Manager manager = new Manager("lorolo");
            Profile p = new Profile();
            p.Users.Add(user);
            p.Name = "a";
            Profile p2 = new Profile();
            p2.Users.Add(user2);
            p.Name = "b";
            //el addProfile separa los profiles por nombre
            //si los dos no tienen nombre (name=null), no añade el segundo
            manager.AddProfile(p);
            Assert.Equal(p, manager.ProfileByUser("AraGo"));

            manager.AddProfile(p2);
            Assert.Equal(p2, manager.ProfileByUser("Wiwiwiwi"));
            Assert.Null(manager.ProfileByName("Lupita"));
        }

        [Fact]

        public void RemoveProfileTest()
        {

        }

    }
}
