using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbManager.Network
{
    public static class XmlSerializer
    {
        public static string OpenDatabase(string database, string username, string password)
        {
            
            return $"<Open Database=\"{database}\" User=\"{username}\" Password=\"{password}\"/>";
        }
        

        public static string OpenCreateSuccess = "<Success/>";
        public static string OpenCreateError(string error)
        {
            return $"<Error>{error}</Error>";
        }

        public static string CreateDatabase(string database, string username, string password)
        {
            return $"<Create Database=\"{database}\" User=\"{username}\" Password=\"{password}\"/>";
        }

        public static string CreateSuccess = "<Success/>";
        public static string CreateError(string error)
        {
            return $"<Error>{error}</Error>";
        }

        public static string Query(string query)
        {
            return $"<Query>{query}</Query>";
        }

        public static string SucessfulAnswer(string answer)
        {
            return $"<Answer>{answer}</Answer>";
        }

        public static string ErrorAnswer(string error)
        {
            return $"<Answer><Error>{error}</Error></Answer>";
        }

        public static string CloseConnection = "<Close/>";
    }
}
