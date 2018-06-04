using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Sockets;
using System.Net;
using System.Text;

namespace Trivia_Client
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new LogInScreen());
            /*
            TcpClient client;
            IPEndPoint serverEndPoint;
            NetworkStream clientStream;

            try
            {
                client = new TcpClient();
                serverEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8820);
                client.Connect(serverEndPoint);
                clientStream = client.GetStream();

                string message = "20005assaf06123456";
                byte[] buffer = new ASCIIEncoding().GetBytes(message);
                clientStream.Write(buffer, 0, message.Length);
                clientStream.Flush();

                Application.Run(new MainLogged(client, serverEndPoint, clientStream));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }*/
        }
    }
}
