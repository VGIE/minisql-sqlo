using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace ClientConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                TcpClient client = new TcpClient();
                Console.WriteLine("Client running. Waiting 2 seconds...");
                Task.Delay(2000).Wait();

                Console.Write("Connecting");
                client.Connect("127.0.0.1", 1200);
                Console.WriteLine("Connected");

                string msg = null;

                while(msg != "Exit")
                {
                    Console.WriteLine("Write a message or if you want to finish write 'Exit' :");

                    msg = Console.ReadLine();
                    NetworkStream stream = client.GetStream();

                    ASCIIEncoding encoding = new ASCIIEncoding();
                    byte[] bytes = encoding.GetBytes(msg);
                    Console.WriteLine("Sending message");

                    stream.Write(bytes, 0, bytes.Length);

                    byte[] buffer = new byte[100];
                    int numBytesRead = stream.Read(buffer, 0, 100);

                    string answer = encoding.GetString(buffer);
                    Console.WriteLine("Answer from server: " + answer);
                 }

               
            }
            catch (Exception e)
            {
                Console.WriteLine("Unhandled error:\n" + e);
            }
        }
    }
}
