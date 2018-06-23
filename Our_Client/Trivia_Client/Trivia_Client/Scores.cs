using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Sockets;
using System.Net;
using System.Runtime.InteropServices;

namespace Trivia_Client
{
    public partial class Scores : Form
    {
        TcpClient client;
        IPEndPoint serverEndPoint;
        NetworkStream clientStream;

        //Round Corners settings
        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn
        (
            int nLeftRect,     // x-coordinate of upper-left corner
            int nTopRect,      // y-coordinate of upper-left corner
            int nRightRect,    // x-coordinate of lower-right corner
            int nBottomRect,   // y-coordinate of lower-right corner
            int nWidthEllipse, // height of ellipse
            int nHeightEllipse // width of ellipse
        );


        public Scores(TcpClient client, IPEndPoint serverEndPoint, NetworkStream clientStream)
        {
            this.Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 6, 6));
            this.client = client;
            this.serverEndPoint = serverEndPoint;
            this.clientStream = clientStream;
            if (getResultFromServer(3) == "121")
            {
                int userNumber = Int32.Parse(getResultFromServer(1));

                for (int i = 0; i < userNumber; i++)
                {
                    int usernameLength = Int32.Parse(getResultFromServer(2));
                    string username = getResultFromServer(usernameLength);
                    int score = Int32.Parse(getResultFromServer(2));
                    Console.WriteLine("Username: " + username + " Score: " + score);
                }
            }
            InitializeComponent();
        }
        private string getResultFromServer(int bytes)
        {
            byte[] bufferIn = new byte[bytes];
            int bytesRead = clientStream.Read(bufferIn, 0, bytes);
            string result = new ASCIIEncoding().GetString(bufferIn);

            return result;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                sendMessageToServer("299");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            finally
            {
                this.Close();
            }
        }
        private void sendMessageToServer(string message)
        {
            byte[] buffer = new ASCIIEncoding().GetBytes(message);
            clientStream.Write(buffer, 0, message.Length);
            clientStream.Flush();
        }

        // Window drag
        [DllImport("user32.dll")]
        public static extern IntPtr SendMessage(IntPtr hWnd, int msg, int wparam, int lparam);

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            if (e.Y <= 50)
            {
                Capture = false;
                SendMessage(Handle, 0x00A1, 2, 0);
            }
        }
    }
}
