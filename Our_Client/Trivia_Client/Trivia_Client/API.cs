using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Threading;
using System.Net.Sockets;
using System.Net;

namespace Trivia_Client {
    static class API {
        const int port = 8820;
        const string ip = "127.0.0.1";

        public static void ServerHandler() {
            try {
                TcpClient client = new TcpClient();
                IPEndPoint serverEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8820);
                client.Connect(serverEndPoint);
                NetworkStream clientStream = client.GetStream();


                //Thread t = new Thread(new ThreadStart(() => RecieveCards(client, serverEndPoint)));
                //t.Start();
            } catch (Exception e) {
                Console.WriteLine(e);
            }
        }
    }
}
