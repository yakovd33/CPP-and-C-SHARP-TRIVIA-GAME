﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
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

        //System.IO.Stream Swinner = Trivia_Client.Properties.Resources.winnerTheme;
        //System.IO.Stream Sloser = Trivia_Client.Properties.Resources.loserTheme;
        //System.IO.Stream SgameEnd = Trivia_Client.Properties.Resources.endgame;
        System.IO.Stream SbuttonsTheme = Trivia_Client.Properties.Resources.click;

        bool isMute;
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


        public Scores(TcpClient client, IPEndPoint serverEndPoint, NetworkStream clientStream, bool isMute, string myUser)
        {
            InitializeComponent();

            //System.Media.SoundPlayer winner = new System.Media.SoundPlayer(Swinner);
            //System.Media.SoundPlayer loser = new System.Media.SoundPlayer(Sloser);
            //System.Media.SoundPlayer endgame = new System.Media.SoundPlayer(SgameEnd);

            this.Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 6, 6));
            this.isMute = isMute;

            this.client = client;
            this.serverEndPoint = serverEndPoint;
            this.clientStream = clientStream;
            if (getResultFromServer(3) == "121")
            {
                int userNumber = Int32.Parse(getResultFromServer(1));

                Dictionary<string, int> users = new Dictionary<string, int>();

                for (int i = 0; i < userNumber; i++)
                {
                    int usernameLength = Int32.Parse(getResultFromServer(2));
                    string username = getResultFromServer(usernameLength);
                    int score = Int32.Parse(getResultFromServer(2));
                    users.Add(username, score);
                    Console.WriteLine("Username: " + username + " Score: " + score);
                }

                var usersSorted = users.ToList();
                usersSorted.Sort((pair1, pair2) => pair1.Value.CompareTo(pair2.Value));

                usersSorted.Reverse();

                int currYPos = 21;
                for (int i = 0; i < userNumber; i++)
                {
                    PictureBox profile = new PictureBox();
                    profile.Height = 40;
                    profile.Width = 40;
                    profile.Load(getUserProfilePicByUsername(usersSorted[i].Key));
                    profile.Location = new Point(17, currYPos);
                    profile.Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, profile.Width, profile.Height, 40, 40));
                    profile.SizeMode = PictureBoxSizeMode.StretchImage;
                    scoresList.Controls.Add(profile);

                    ///////

                    Label usernameLabel = new Label();
                    usernameLabel.Text = usersSorted[i].Key;
                    usernameLabel.Font = new Font("Arial", 18);
                    usernameLabel.ForeColor = Color.FromArgb(173, 190, 202);
                    usernameLabel.Location = new Point(110, currYPos);
                    scoresList.Controls.Add(usernameLabel);

                    ///////

                    Label scoreLabel = new Label();
                    scoreLabel.Text = usersSorted[i].Value.ToString();
                    scoreLabel.Font = new Font("Arial", 18);
                    scoreLabel.ForeColor = Color.FromArgb(173, 190, 202);
                    scoreLabel.Location = new Point(443, currYPos);
                    scoresList.Controls.Add(scoreLabel);

                    currYPos += 48;
                }

                if (userNumber == 1)
                {
                    // User is playing alone
                    stamp.Image = Trivia_Client.Properties.Resources.winser;
                }
                else
                {
                    if (usersSorted[0].Key == myUser)
                    {
                        // Current user is the winner
                        if (!isMute)
                        {
                            //winner.Play();
                        }

                        stamp.Image = Trivia_Client.Properties.Resources.winner;
                    }
                    else
                    {
                        // Current user lost
                        if (!isMute)
                        {
                            //loser.Play();
                        }
                        stamp.Image = Trivia_Client.Properties.Resources.loser;
                    }
                }

                currYPos += 45;
            }
        }
        private string getResultFromServer(int bytes)
        {
            byte[] bufferIn = new byte[bytes];
            int bytesRead = clientStream.Read(bufferIn, 0, bytes);
            string result = new ASCIIEncoding().GetString(bufferIn);

            return result;
        }

        string getUserProfilePicByUsername(string username)
        {
            string picture_url = "https://i.imgur.com/oeKRbhC.png";

            sendMessageToServer("419" + username.Length.ToString("D2") + username);

            if (getResultFromServer(3) == "189")
            {
                int pictureLength = Int32.Parse(getResultFromServer(3));
                picture_url = getResultFromServer(pictureLength);
            }

            return picture_url;
        }

        private void exitBtn_Click(object sender, EventArgs e)
        {
            try
            {
                if (!isMute)
                {
                    System.Media.SoundPlayer buttonsTheme = new System.Media.SoundPlayer(SbuttonsTheme);
                    buttonsTheme.Play();
                }
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
