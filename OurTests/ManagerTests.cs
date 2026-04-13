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
                new User("nombre", Encryption.Encrypt("contrasena")),
                new User("Igor", Encryption.Encrypt("1234")),
                new User("Eustakia", Encryption.Encrypt("admin")),
                new User("Josefina", Encryption.Encrypt("password")),
                new User("Joritz", Encryption.Encrypt("pasi")),
            ];
            return lista;
        }
        private List<User> createUserTestList2()
        {
            List<User> lista =
            [
                new User(Profile.AdminProfileName, Encryption.Encrypt("lobo")),
                new User("Txitx", Encryption.Encrypt("rip")),
                new User("Jere", Encryption.Encrypt("BDLover1")),
                new User("Fabian", Encryption.Encrypt("qwerty")),
                new User("Ainhoa", Encryption.Encrypt("BDLover1")),
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
    }
}
