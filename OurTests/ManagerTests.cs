using DbManager;
using DbManager.Security;
using Xunit;

namespace OurTests
{
    public class ManagerTests
    {

        private List<User> createUserTestList()
        {
            List<User> lista =
            [
                new User("nombre","contrasena"),
                new User("Igor", "1234"),
                new User("Eustakia", "admin"),
                new User("Josefina", "password"),
                new User("Joritz", "pasi"),
            ];
            return lista;
        }
        private List<User> createUserTestList2()
        {
            List<User> lista =
            [
                new User(Profile.AdminProfileName, "lobo"),
                new User("Txitx", "rip"),
                new User("Jere", "BDLover1"),
                new User("Fabian", "qwerty"),
                new User("Ainhoa", "BDLover1"),
            ];
            return lista;
        }
        [Fact]
        public void IsUserAdminTest()
        {
            Profile pTest1 = new Profile
            {
                Name = Profile.AdminProfileName,
                Users = createUserTestList()
            };
            Profile pTest2 = new Profile
            {
                Name = "noname",
                Users = createUserTestList2()
            };
            Manager m = new Manager("Joritz");

            m.Profiles.Add(pTest2);
            m.Profiles.Add(pTest1);
            

            Manager m2 = new Manager("Jere");
            m2.Profiles.Add(pTest2);

            Assert.True(m.IsUserAdmin());
            Assert.False(m2.IsUserAdmin());
        }
        [Fact]
        public void IsPasswordCorrectTest()
        {
            Profile pTest1 = new Profile
            {
                Name = Profile.AdminProfileName,
                Users = createUserTestList()
            };
            Profile pTest2 = new Profile
            {
                Name = "josefran",
                Users = createUserTestList2()
            };
            Manager m = new Manager("Hector");
            m.Profiles.Add(pTest1);
            m.Profiles.Add(pTest2);

            Assert.True(m.IsPasswordCorrect("Igor", "1234"));
            Assert.True(m.IsPasswordCorrect("Fabian", "qwerty"));
            Assert.False(m.IsPasswordCorrect("Ainhoa", "BDlover1")); //pwd is wrong on purpose
        }
        [Fact]
        public void UserByNameTest()
        {
            Profile pTest1 = new Profile
            {
                Name = Profile.AdminProfileName,
                Users = createUserTestList()
            };
            Profile pTest2 = new Profile
            {
                Name = "josefran",
                Users = createUserTestList2()
            };
            Manager m = new Manager("Borja");
            m.Profiles.Add(pTest1);
            m.Profiles.Add(pTest2);

            Assert.Equal("Fabian",m.UserByName("Fabian").Username);
            Assert.Equal(Encryption.Encrypt("qwerty"), m.UserByName("Fabian").EncryptedPassword);

            Assert.Equal(Profile.AdminProfileName, m.UserByName(Profile.AdminProfileName).Username);
            Assert.Equal(Encryption.Encrypt("lobo"), m.UserByName(Profile.AdminProfileName).EncryptedPassword);

            Assert.Equal("Joritz", m.UserByName("Joritz").Username);
            Assert.Equal(Encryption.Encrypt("pasi"), m.UserByName("Joritz").EncryptedPassword);

            Assert.NotEqual("Administrator", m.UserByName(Profile.AdminProfileName).Username);
            Assert.NotEqual(Encryption.Encrypt(" "), m.UserByName(Profile.AdminProfileName).EncryptedPassword);

            Assert.NotEqual(Profile.AdminProfileName, m.UserByName("Joritz").Username);
            Assert.NotEqual(Encryption.Encrypt("1234"), m.UserByName("Joritz").EncryptedPassword);
        }
        /*
        [Fact]
        public void ProfileByName()
        {

        }
        */
        
    }
}
