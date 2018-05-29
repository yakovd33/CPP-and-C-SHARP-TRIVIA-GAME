using System;
using System.Windows.Forms;
using System.Runtime.InteropServices;

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

        public LogInScreen()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.None;
            Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 6, 6));

            usernameWrap.Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, usernameWrap.Width, usernameWrap.Height, 5, 5));
            passwordWrap.Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, passwordWrap.Width, passwordWrap.Height, 5, 5));
            loginBtn.Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, loginBtn.Width, loginBtn.Height, 5, 5));
            SignupBtn.Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, SignupBtn.Width + 1, SignupBtn.Height + 1, 5, 5));
            exitBtn.Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, exitBtn.Width, exitBtn.Height, exitBtn.Height, exitBtn.Width));
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
                Application.Exit(); // In the future, it might be for log in.
            }
        }

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
