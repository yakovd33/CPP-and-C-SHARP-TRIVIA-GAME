using System;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Drawing;

namespace Client_game
{
    public partial class Client_game : Form
    {
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

        TcpClient client;
        IPEndPoint serverEndPoint;
        NetworkStream clientStream;

        public Client_game(TcpClient client, IPEndPoint serverEndPoint, NetworkStream clientStream) {
            this.client = client;
            this.serverEndPoint = serverEndPoint;
            this.clientStream = clientStream;

            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.None;
            Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 6, 6));

            usernameWrap.Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, usernameWrap.Width, usernameWrap.Height, 5, 5));
            Ans1_Btn.Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, Ans1_Btn.Width + 1, Ans1_Btn.Height + 1, 5, 5));

            try {
                client = new TcpClient();
                serverEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8820);
                client.Connect(serverEndPoint);
                clientStream = client.GetStream();
            } catch (Exception e) {
                Console.WriteLine(e);
            }

            panel1.BringToFront();

        }

        private void InitializeComponent()
        {
            throw new NotImplementedException();
        }

        private void exitBtn_MouseHover(object sender, EventArgs e)
        {
            exitBtn.BackgroundImage = Trivia_Client.Properties.Resources.exitButtonHover;
        }

        private void exitBtn_MouseLeave(object sender, EventArgs e)
        {
            exitBtn.BackgroundImage = Trivia_Client.Properties.Resources.exitButton;
        }

        private void exitBtn_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void LogInScreen_Load(object sender, EventArgs e)
        {

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

        private void SignupBtn_Click(object sender, EventArgs e) {
            this.Close();
        }
    }
}
