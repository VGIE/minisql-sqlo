using DbManager;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace DbManager.Network
{
    public class Client
    {
        TcpClient m_tcpClient;
        public Client()
        {
            m_tcpClient = new TcpClient();
        }
        public bool Connect(string ipAddress, int port)
        {
            //DEADLINE 6: Connect the tcp client to the given ip/port
            //Return false if something goes wrong, true otherwise (try/catch)
            
            return false;
            
        }

        private string SendString(string message)
        {
            //DEADLINE 6: Send a string to the server, read the answer and return it.
            //Here, we do not do any Xml formatting, we just send the string as it comes and return the string as it comes
            //This private method should be used from Open/SendQuery/Close
            //Have a look at the project ClientConsole to see how we can use the TcpClient class
            
            return null;
            
        }

        public bool Open(string database, string username, string password, out string error)
        {
            //DEADLINE 6: Send an Open command to the server using SendString
            
            error = null;
            return false;
            
        }

        public bool Create(string database, string username, string password, out string error)
        {
            //DEADLINE 6: Send a Create command to the server using SendString
            
            error = null;
            return false;
            
        }

        public string SendQuery(string query)
        {
            //DEADLINE 6: Send a Query command to the server using SendString
            
            return null;
            
        }

        public void Close()
        {
            //DEADLINE 6: Send a Close command to the server using SendString and close the connection to the server
            
        }
    }
}
