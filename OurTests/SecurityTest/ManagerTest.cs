using DbManager;
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

            User user2 = new User("Nodmin", "abcd");
            Profile p2 = new Profile();
            p2.Name = "p2";
            p2.Users.Add(user2);
            p2.GrantPrivilege("t",Privilege.Select);
            Manager m = new Manager("Nodmin");
            m.Profiles.Add(p2);

            Assert.True(m.IsGrantedPrivilege("Nodmin", "t", Privilege.Select));
            Assert.False(m.IsGrantedPrivilege("Nodmin", null, Privilege.Select));
            
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
        
        public void testManagerSaveLoad()
        {
            string dbName= "Personas";
            string adminUser= "Admin";
            string adminPass= "1234";
            Manager manager = new Manager(adminUser);

            Profile readerP= new Profile { Name = "Reader" };
            User user= new User("Lupe","constraseña1106");
            readerP.Users.Add(user);

            readerP.GrantPrivilege("TablaX", Privilege.Select);
            manager.Profiles.Add(readerP);

            Profile adminProfile = new Profile { Name = Profile.AdminProfileName };
            adminProfile.Users.Add(new User(adminUser, Encryption.Encrypt(adminPass)));
            manager.Profiles.Add(adminProfile);

            manager.Save(dbName);

            Manager loadedManager= Manager.Load(dbName, adminUser);

            Assert.NotNull(loadedManager);
            Assert.True(loadedManager.IsPasswordCorrect("Lupe", "constraseña1106"));
            Assert.True(loadedManager.IsGrantedPrivilege("Lupe", "TablaX", Privilege.Select));
            Assert.False(loadedManager.IsGrantedPrivilege("Lupe", "TablaX", Privilege.Delete));
        }

        [Fact]

        public void testManagerSaveLoadMultiplePrivileges()
        {
            string dbName= "Numeros";
            
            Manager manager = new Manager("Admin");
            Profile editorP = new Profile { Name = "Editor" };
            editorP.Users.Add(new User("arara","ara123"));

            editorP.GrantPrivilege("TablaY", Privilege.Select);
            editorP.GrantPrivilege("TablaY", Privilege.Insert);
            editorP.GrantPrivilege("TablaY", Privilege.Update);

            manager.Profiles.Add(editorP);
            manager.Save(dbName);

            Manager loadedManager= Manager.Load(dbName, "Admin");
            


            Assert.True(loadedManager.IsGrantedPrivilege("arara", "TablaY", Privilege.Select));
            Assert.True(loadedManager.IsGrantedPrivilege("arara", "TablaY", Privilege.Insert));
            Assert.True(loadedManager.IsGrantedPrivilege("arara", "TablaY", Privilege.Update));
            Assert.False(loadedManager.IsGrantedPrivilege("arara", "TablaY", Privilege.Delete));
        }

        [Fact]

        public void testLoadNonExistenFile()
        {
            Manager load= Manager.Load("BDInexistente", "admin");

            Assert.NotNull(load);
            Assert.Equal(0, load.Profiles.Count);
        }
        [Fact]
        public void testCheckPermissionsNotBeingAdminForRegularQueries()
        {
            string dbName = "SecurityManualTest";
            Database db = Database.CreateTestDatabase(); 
    
            Profile p = new Profile { Name = "UserRole" };
            p.Users.Add(new User("ara", "1234"));
            db.SecurityManager.AddProfile(p);
            db.Save(dbName);

            Database dbAra = Database.Load(dbName, "ara", "1234");

            Assert.NotNull(dbAra.SecurityManager);
            Assert.NotNull(dbAra.TableByName(Table.TestTableName)); 
            
            string query = "DROP SECURITY PROFILE GuestRole"; 

            string result = dbAra.ExecuteMiniSQLQuery(query);

            Assert.Equal(Constants.UsersProfileIsNotGrantedRequiredPrivilege, result);

            File.Delete(dbName);
            File.Delete(dbName + "_Security.json");
        }

        [Fact]
        public void testCheckPermissionsNotBeingAdmin2()
        {

        string dbName = "SecurityTestDB";
        Database db = new Database(Database.AdminUsername, Database.AdminPassword);

        Profile profile = new Profile { Name = "userProfile" };
        User user = new User("user", "1234");
        profile.Users.Add(user);

        db.SecurityManager.Profiles.Add(profile);

        db.Save(dbName);

        Database dbUser = Database.Load(dbName, "user", "1234");
    
        db.SecurityManager.GrantPrivilege("userProfile", "Table", Privilege.Update);

        bool privilege = dbUser.SecurityManager.IsGrantedPrivilege("user", "Table", Privilege.Update);
        Assert.False(privilege);
        }

        [Fact]
        public void testCheckPermissionsnotBeingAdminGrantRevoke()
        {
            string dbName= "SecurityGrantRevokeTest";  
            Database db= new Database("admin", "admin123");

            Profile userProfile= new Profile { Name = "UserRole" };
            userProfile.Users.Add(new User("lupe", "lupe123"));
            db.SecurityManager.Profiles.Add(userProfile);

            Profile p= new Profile { Name = "TestRole" };
            db.SecurityManager.Profiles.Add(p);

            db.Save(dbName);

            Database dbLupe = Database.Load(dbName, "lupe", "lupe123");

            string queryGrant= "GRANT SELECT ON TestTable TO TestRole";
            string resultGrant= dbLupe.ExecuteMiniSQLQuery(queryGrant);

            Assert.Equal(Constants.UsersProfileIsNotGrantedRequiredPrivilege, resultGrant);

            string queryRevoke= "REVOKE SELECT ON TestTable TO TestRole";
            string resultRevoke= dbLupe.ExecuteMiniSQLQuery(queryRevoke);

            Assert.Equal(Constants.UsersProfileIsNotGrantedRequiredPrivilege, resultRevoke);

        }

        [Fact]

        public void testAdminGrantRevoke()
        {
            Manager manager = new Manager("admin");
            Profile admin= new Profile { Name = Profile.AdminProfileName };
            admin.Users.Add(new User("admin", "1234"));
            manager.Profiles.Add(admin);

            Profile p1 = new Profile { Name = "wi" };
            p1.Users.Add(new User("wiwi", "123"));
            manager.Profiles.Add(p1);
            
            manager.GrantPrivilege("wi", "Tabla1", Privilege.Select);
            Assert.True(manager.IsGrantedPrivilege("wiwi", "Tabla1", Privilege.Select));

            manager.RevokePrivilege("wi", "Tabla1", Privilege.Select);
            Assert.False(manager.IsGrantedPrivilege("wiwi", "Tabla1", Privilege.Select));
        }
    }
}
