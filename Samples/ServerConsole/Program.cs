using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using DbManager;

namespace ServerConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                DbManager.Database serverDatabase = new Database("admin", "adminPassword");
                //Listen on port 1200. Accept connections from any IP address
                TcpListener server = new TcpListener(IPAddress.Parse("0.0.0.0"), 1200);

                server.Start();

                Console.WriteLine("Server running and listening on port 1200");

                Socket socket = server.AcceptSocket();

                Console.WriteLine("Connection accepted from " + socket.RemoteEndPoint);

                bool trueFalse = true;
                while(trueFalse == true)
                {
                    byte[] buffer = new byte[100];
                    int bytesRead = socket.Receive(buffer);
                    buffer[bytesRead] = 0;
                    ASCIIEncoding encoding = new ASCIIEncoding();
                    string clientMessage = encoding.GetString(buffer).Substring(0,bytesRead);
                    Console.WriteLine("Message received from client: " + clientMessage);
                    if (clientMessage == "Exit")
                    {
                        trueFalse = false;
                    }
                    else
                    {                      
                        string clientResult = serverDatabase.ExecuteMiniSQLQuery(clientMessage);
                        socket.Send(encoding.GetBytes(clientResult));
                    }                   
                }
                
                Task.Delay(2000).Wait();

                socket.Close();
                server.Stop();

                Console.WriteLine("Server closed. Press any key to finish...");
                Console.ReadKey();
            }
            catch (Exception e)
            {
                Console.WriteLine("Unhandled error: " + e);
            }
        }
    }
}
