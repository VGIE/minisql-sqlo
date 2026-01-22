using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbManager.Parser
{
    public static class PrivilegeUtils
    {
        public static Security.Privilege FromPrivilegeName(string name)
        {
            switch(name)
            {
                case "DELETE": return Security.Privilege.Delete;
                case "INSERT": return Security.Privilege.Insert;
                case "UPDATE": return Security.Privilege.Update;
                case "SELECT": return Security.Privilege.Select;
                default:
                    throw new Exception("Unknown Privilege name in PrivilegeUtils.FromPrivilegeName()");
            }
        }
    }
}
