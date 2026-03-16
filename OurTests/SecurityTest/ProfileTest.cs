using DbManager;
using DbManager.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace OurTests.SecurityTest
{
    public class ProfileTest
    {
        [Fact]
        public void GrantPrivilegeTest()
        {
            Profile p = new Profile();

            Assert.True(p.GrantPrivilege("t", Privilege.Update));
            Assert.True(p.GrantPrivilege("t", Privilege.Delete));
            Assert.False(p.GrantPrivilege("t", Privilege.Delete));

        }

        [Fact]
        public void RevokePrivilegeTest()
        {
            Profile p = new Profile();

            Assert.False(p.RevokePrivilege("t", Privilege.Select));
            p.GrantPrivilege("t", Privilege.Select);
            Assert.True(p.RevokePrivilege("t", Privilege.Select));
            Assert.False(p.RevokePrivilege("t", Privilege.Select));
        }

        [Fact]
        public void IsGrantedPrivilegeTest()
        {
            Profile p = new Profile();

            Assert.False(p.IsGrantedPrivilege("t", Privilege.Select));
            p.GrantPrivilege("t", Privilege.Select);
            Assert.True(p.IsGrantedPrivilege("t", Privilege.Select));
            p.RevokePrivilege("t", Privilege.Select);
            Assert.False(p.IsGrantedPrivilege("t", Privilege.Select));
        }
    }
}
