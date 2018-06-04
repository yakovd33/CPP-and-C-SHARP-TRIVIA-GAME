﻿using System;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading;

namespace Trivia_Client
{
    public partial class MainLogged : Form
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

        public MainLogged(TcpClient client, IPEndPoint serverEndPoint, NetworkStream clientStream) {
            InitializeComponent();

            this.client = client;
            this.serverEndPoint = serverEndPoint;
            this.clientStream = clientStream;

            Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 6, 6));

            Sidebar();
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

        private void pictureBox2_Click(object sender, EventArgs e) {
            // Logout
            logout();
            LogInScreen main = new LogInScreen();
            this.Hide();
            main.ShowDialog();
            this.Close();
        }

        private void logout () {
            byte[] buffer = new ASCIIEncoding().GetBytes("201");
            clientStream.Write(buffer, 0, 3);
            clientStream.Flush();
        }

        private void Sidebar () {
            sidebarItem1.Click += new EventHandler(SidebarItemClick);
            sidebarItem2.Click += new EventHandler(SidebarItemClick);
            sidebarItem3.Click += new EventHandler(SidebarItemClick);
            sidebarIcon1.Click += new EventHandler(SidebarItemClick);
            sidebarIcon2.Click += new EventHandler(SidebarItemClick);
            sidebarIcon3.Click += new EventHandler(SidebarItemClick);
        }

        private void SidebarItemClick(object sender, EventArgs e) {
            Control ctrl = sender as Control;

            Control[] sidebarItems = { sidebarItem1, sidebarItem2, sidebarItem3 };
            for (int i = 0; i < sidebarItems.Length; i++){
                sidebarItems[i].BackColor = System.Drawing.Color.FromArgb(1, 48, 56, 65);
            }


            int newY = sidebarActivePanelIndicator.Location.Y;
            if (ctrl.Name == "sidebarItem1" || ctrl.Name == "sidebarIcon1") {
                sidebarItem1.BackColor = System.Drawing.Color.FromArgb(54, 62, 71);
                newY = sidebarItem1.Location.Y;
            } else if (ctrl.Name == "sidebarItem2" || ctrl.Name == "sidebarIcon2") {
                sidebarItem2.BackColor = System.Drawing.Color.FromArgb(54, 62, 71);
                newY = sidebarItem2.Location.Y;
            } else if (ctrl.Name == "sidebarItem3" || ctrl.Name == "sidebarIcon3") {
                sidebarItem3.BackColor = System.Drawing.Color.FromArgb(54, 62, 71);
                newY = sidebarItem3.Location.Y;
            }

            Thread animate = new Thread(new ParameterizedThreadStart(animateSlidebarSelectionBar));
            animate.Start(newY);
        }

        protected void animateSlidebarSelectionBar (object Y) {
            bool isPos = (sidebarActivePanelIndicator.Location.Y < (int)Y);

            for (int i = sidebarActivePanelIndicator.Location.Y; sidebarActivePanelIndicator.Location.Y != (int)Y; i++) {
                if (isPos) {
                    sidebarActivePanelIndicator.Invoke((MethodInvoker)delegate {
                        sidebarActivePanelIndicator.Location = new System.Drawing.Point(sidebarActivePanelIndicator.Location.X, sidebarActivePanelIndicator.Location.Y + 1);
                    });
                } else {
                    sidebarActivePanelIndicator.Invoke((MethodInvoker)delegate {
                        sidebarActivePanelIndicator.Location = new System.Drawing.Point(sidebarActivePanelIndicator.Location.X, sidebarActivePanelIndicator.Location.Y - 1);
                    });
                }
            }
        }
    }
}
