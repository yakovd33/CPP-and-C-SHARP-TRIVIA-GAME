using System;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Drawing;

namespace Trivia_Client
{
    public partial class Pass_Reset_Screen : Form
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
        Protocol protocol = new Protocol();

        Timer timer1 = new Timer();
        int opacity = 255;
        Image original;

        public Pass_Reset_Screen(TcpClient client, IPEndPoint serverEndPoint, NetworkStream clientStream)
        {
            this.client = client;
            this.serverEndPoint = serverEndPoint;
            this.clientStream = clientStream;

            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.None;
            Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 6, 6));

            usernameWrap.Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, usernameWrap.Width, usernameWrap.Height, 5, 5));
            loginBtn.Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, loginBtn.Width, loginBtn.Height, 5, 5));

            Bitmap myBitmap = new Bitmap(logo.Image);
            original = (Image)myBitmap.Clone();
            
            timer1.Interval = 2;
            timer1.Start();
        }


        private void usernameBox_Enter(object sender, EventArgs e)
        {
            if (emailBox.Text == "Username")
            {
                emailBox.Text = "";
                emailBox.TabStop = true;
            }
        }

        private void exitBtn_MouseHover(object sender, EventArgs e)
        {
            exitPanel.BackgroundImage = Trivia_Client.Properties.Resources.exitButtonHover;
        }

        private void exitBtn_MouseLeave(object sender, EventArgs e)
        {
            exitPanel.BackgroundImage = Trivia_Client.Properties.Resources.exitButton;
        }

        private void exitBtn_Click(object sender, EventArgs e)
        {
            //Application.Exit();
            this.Close();
        }

        private void passwordBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 13) // Enter
            {
                login();
            }
        }

        protected void login() {
            string email_addr = emailBox.Text;
                try {
                    string message = "666" + email_addr.Length.ToString("D2") + email_addr;
                    Console.WriteLine(message);

                    byte[] buffer = new ASCIIEncoding().GetBytes(message);
                    clientStream.Write(buffer, 0, message.Length);
                    clientStream.Flush();

                    // Get login response
                    byte[] bufferIn = new byte[4];
                    int bytesRead = clientStream.Read(bufferIn, 0, 4);
                    string resuLtCode = new ASCIIEncoding().GetString(bufferIn);
                    string errorMsg = protocol.getCodeErrorMsg(resuLtCode);

                } catch (Exception e) {
                    Console.WriteLine(e);
                }
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

        private void loginBtn_Click(object sender, EventArgs e) {
            login();
            //this.Hide();
            //this.Show();
        }
    }
}
