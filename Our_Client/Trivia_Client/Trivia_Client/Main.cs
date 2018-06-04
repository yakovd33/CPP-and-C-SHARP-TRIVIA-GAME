﻿using System;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Drawing;

namespace Trivia_Client
{
    public partial class LogInScreen : Form
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

        public LogInScreen()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.None;
            Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 6, 6));

            usernameWrap.Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, usernameWrap.Width, usernameWrap.Height, 5, 5));
            passwordWrap.Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, passwordWrap.Width, passwordWrap.Height, 5, 5));
            loginBtn.Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, loginBtn.Width, loginBtn.Height, 5, 5));
            SignupBtn.Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, SignupBtn.Width + 1, SignupBtn.Height + 1, 5, 5));

            try {
                client = new TcpClient();
                serverEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8820);
                client.Connect(serverEndPoint);
                clientStream = client.GetStream();
            } catch (Exception e) {
                Console.WriteLine(e);
            }

            Bitmap myBitmap = new Bitmap(logo.Image);
            original = (Image)myBitmap.Clone();

            timer1.Tick += tmrFadeOut_Tick;
            timer1.Interval = 2;
            timer1.Start();
        }

        private void tmrFadeOut_Tick (object sender, EventArgs e) {
            Bitmap myBitmap = new Bitmap(logoCopy.Image);
            Image img = (Image)myBitmap.Clone();

            if (opacity > 0) {
                using (Graphics g = Graphics.FromImage(img)) {
                    Pen pen = new Pen(Color.FromArgb(opacity, logo.Parent.BackColor), img.Width);
                    g.DrawLine(pen, -1, -1, img.Width, img.Height);
                    g.Save();
                }

                logo.Image = img;
                opacity -= 3;
            } else {
                timer1.Stop();
            }
        }

        private void usernameBox_TextChanged(object sender, EventArgs e)
        {
        }        

        private void passwordBox_TextChanged(object sender, EventArgs e)
        {
            passwordBox.PasswordChar = '•';
        }

        private void passwordBox_Enter(object sender, EventArgs e)
        {
            if (passwordBox.Text == "Password")
            {
                passwordBox.Text = "";
                passwordBox.TabStop = true;
                usernameBox.TabStop = true;
            }
        }

        private void usernameBox_Enter(object sender, EventArgs e)
        {
            if (usernameBox.Text == "Username")
            {
                usernameBox.Text = "";
                passwordBox.TabStop = true;
                usernameBox.TabStop = true;
            }
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

        private void passwordBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 13) // Enter
            {
                login();
            }
        }

        protected void login() {
            string username = usernameBox.Text;
            string password = passwordBox.Text;

            if (username != "" && username != "Username" && password != "" && password != "Password") {
                try {
                    string message = "200" + username.Length.ToString("D2") + username + password.Length.ToString("D2") + password;
                    Console.WriteLine(message);

                    byte[] buffer = new ASCIIEncoding().GetBytes(message);
                    clientStream.Write(buffer, 0, message.Length);
                    clientStream.Flush();

                    // Get login response
                    byte[] bufferIn = new byte[4];
                    int bytesRead = clientStream.Read(bufferIn, 0, 4);
                    string resuLtCode = new ASCIIEncoding().GetString(bufferIn);
                    string errorMsg = protocol.getCodeErrorMsg(resuLtCode);

                    if (errorMsg == "success") {
                        // Login
                        loginFeedbackLabel.Hide();
                        MainLogged main = new MainLogged(client, serverEndPoint, clientStream);
                        this.Hide();
                        main.ShowDialog();
                        this.Close();
                    }
                    else {
                        loginFeedbackLabel.Visible = Visible;
                        loginFeedbackLabel.Text = errorMsg;
                    }
                } catch (Exception e) {
                    Console.WriteLine(e);
                }
            } else {
                loginFeedbackLabel.Visible = Visible;
                loginFeedbackLabel.Text = "Fields must not be empty.";
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
        }

        private void SignupBtn_Click(object sender, EventArgs e) {
            this.Hide();
            Signup signup = new Signup(client, serverEndPoint, clientStream);
            signup.ShowDialog();
            this.Show();
            //this.Close();
        }
    }
}
