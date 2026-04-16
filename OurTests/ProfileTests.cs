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
        public Profile Profile4Tests() {
            Profile profile = new Profile()
            {
                Name = Profile.AdminProfileName,
                Users = Users4Tests()
            };
            return profile;
        }
    [Fact]
    public void GrantAndIsGrantedPrivilegeTests()
        {
            

        
        }
    public void RevokeAndIsGrantedPrivilegeTests()
        {

        }
    }
}
