using DbManager;
using DbManager.Parser;
using DbManager.Security;
using Xunit;
namespace OurTests
{
    public class ProfileTests
    {
        public List<User> Users4Tests()
        {
            List<User> users = new List<User>();
            users.Add(new User("LamelaBalls","enviei"));
            users.Add(new User("Ioris","bomboclat"));
            users.Add(new User("rizzan","rizzlez"));
            users.Add(new User("Geisgor", "cabezabuque23"));
            return users;
        }
        public Profile AdminProfile4Tests() {
            Profile profile = new Profile()
            {
                Name = Profile.AdminProfileName,
                Users = Users4Tests()
            };
            return profile;
        }
        public Profile Profile4Tests()
        {
            Profile profile = new Profile()
            {
                Name = "AndresitoDonNadie",
                Users = Users4Tests()
            };
            return profile;
        }
        public Dictionary<string, List<Privilege>> FakeTables4Tests()
        {
            Dictionary<string, List<Privilege>> fakeTables =new Dictionary<string, List<Privilege>>();
            fakeTables.Add("Users", new List<Privilege>
            {
                Privilege.Select,
                Privilege.Insert
            });
            fakeTables.Add("Clients", new List<Privilege>
            {
                Privilege.Insert,
                Privilege.Select,
                Privilege.Delete,
                Privilege.Update
            });
            return fakeTables ;
        }
        [Fact]
        public void ProfileMethodTests()
        {
            Profile AdminProfile = AdminProfile4Tests();
            Profile NormalProfile = Profile4Tests();
            AdminProfile.setListOfPrivileges4Testing(FakeTables4Tests());
            NormalProfile.setListOfPrivileges4Testing(FakeTables4Tests());
            //admintesting
            Assert.True(AdminProfile.IsGrantedPrivilege("Users", Privilege.Insert));
            Assert.True(AdminProfile.IsGrantedPrivilege("Users", Privilege.Update));
            Assert.True(AdminProfile.IsGrantedPrivilege("Users", Privilege.Delete));
            Assert.True(AdminProfile.IsGrantedPrivilege("Users", Privilege.Select));
            //normalusertesting
            Assert.True(NormalProfile.IsGrantedPrivilege("Users", Privilege.Select));
            Assert.True(NormalProfile.IsGrantedPrivilege("Users", Privilege.Insert));
            Assert.False(NormalProfile.IsGrantedPrivilege("Users", Privilege.Update));
            Assert.False(NormalProfile.IsGrantedPrivilege("Users", Privilege.Delete));
            //adding the privileges for testing
            NormalProfile.GrantPrivilege("Users", Privilege.Update);
            NormalProfile.GrantPrivilege("Users", Privilege.Delete);
            //checking if privilege was added
            Assert.True(NormalProfile.IsGrantedPrivilege("Users", Privilege.Update));
            Assert.True(NormalProfile.IsGrantedPrivilege("Users", Privilege.Delete));
            //Revoking privileges
            NormalProfile.RevokePrivilege("Users", Privilege.Update);
            NormalProfile.RevokePrivilege("Users", Privilege.Delete);
            //checking if privilege was succesfully revoked
            Assert.False(NormalProfile.IsGrantedPrivilege("Users", Privilege.Update));
            Assert.False(NormalProfile.IsGrantedPrivilege("Users", Privilege.Delete));
        }
        [Fact]
        public void equalsTest()
        {
            Profile profile1 = Profile4Tests();
            profile1.setListOfPrivileges4Testing(FakeTables4Tests());

            Profile profile2 = Profile4Tests();
            profile2.setListOfPrivileges4Testing(FakeTables4Tests());

            Assert.True(profile1.Equals(profile2));

            Assert.False(profile1.Equals(null));

            Assert.False(profile1.Equals(new object()));

            profile2.Name = "OtroNombre";
            Assert.False(profile1.Equals(profile2));

            profile2 = Profile4Tests();
            profile2.setListOfPrivileges4Testing(FakeTables4Tests());
            profile2.RevokePrivilege("Users", Privilege.Insert);
            Assert.False(profile1.Equals(profile2));

            profile2 = Profile4Tests();
            profile2.setListOfPrivileges4Testing(FakeTables4Tests());
            profile2.Users.RemoveAt(0);
            Assert.False(profile1.Equals(profile2));
        }
    }
}
