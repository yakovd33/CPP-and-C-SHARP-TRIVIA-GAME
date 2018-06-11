using System;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading;
using System.Drawing;

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
        string fileDialogPath;

        public MainLogged(TcpClient client, IPEndPoint serverEndPoint, NetworkStream clientStream) {
            InitializeComponent();

            this.client = client;
            this.serverEndPoint = serverEndPoint;
            this.clientStream = clientStream;

            Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 6, 6));

            roomNameWrap.Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, roomNameWrap.Width, roomNameWrap.Height, 5, 5));
            numPlayersWrap.Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, numPlayersWrap.Width, numPlayersWrap.Height, 5, 5));
            numQuestWrap.Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, numQuestWrap.Width, numQuestWrap.Height, 5, 5));
            questionsTimeWrap.Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, questionsTimeWrap.Width, questionsTimeWrap.Height, 5, 5));
            createRoomBtn.Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, createRoomBtn.Width, createRoomBtn.Height, 5, 5));

            mainProfilePicture.Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, mainProfilePicture.Width, mainProfilePicture.Height, 49, 49));
            Sidebar();

            new Thread(() =>
            {
                Thread.Sleep(1000);
                mainProfilePicture.Load(getUserProfilePic());
                profilePanelPic.Image = mainProfilePicture.Image;
            }).Start();
        }

        private void exitBtn_MouseHover(object sender, EventArgs e)
        {
            exitBtn.BackgroundImage = Trivia_Client.Properties.Resources.exitButtonHover;
        }

        private void exitBtn_MouseLeave(object sender, EventArgs e)
        {
            exitBtn.BackgroundImage = Trivia_Client.Properties.Resources.exitButton;
        }

        private void exitBtn_Click(object sender, EventArgs e) {
            sendMessageToServer("299");
            Application.Exit();
        }

        // Window drag
        [DllImport("user32.dll")]
        public static extern IntPtr SendMessage(IntPtr hWnd, int msg, int wparam, int lparam);

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            if (e.Y <= 50 && e.X >= 63)
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

        private void Sidebar()
        {
            sidebarItem1.Click += new EventHandler(SidebarItemClick);
            createRoomItem.Click += new EventHandler(SidebarItemClick);
            createRoomIcon.Click += new EventHandler(SidebarItemClick);
            creRoomPanel.Hide();
            createRoomItem.Enter += new EventHandler(createRoomItem_Enter);
            createRoomIcon.Click += new EventHandler(createRoomItem_Enter);
            

            sidebarItem3.Click += new EventHandler(SidebarItemClick);
            sidebarIcon1.Click += new EventHandler(SidebarItemClick);
            sidebarIcon3.Click += new EventHandler(SidebarItemClick);            
        }

        private void SidebarItemClick(object sender, EventArgs e)
        {
            Control ctrl = sender as Control;

            Control[] sidebarItems = { sidebarItem1, createRoomItem, sidebarItem3 };
            for (int i = 0; i < sidebarItems.Length; i++){
                sidebarItems[i].BackColor = System.Drawing.Color.FromArgb(1, 48, 56, 65);
            }


            int newY = sidebarActivePanelIndicator.Location.Y;
            if (ctrl.Name == "sidebarItem1" || ctrl.Name == "sidebarIcon1") {
                sidebarItem1.BackColor = System.Drawing.Color.FromArgb(54, 62, 71);
                newY = sidebarItem1.Location.Y;
                mainPanel.BringToFront();
            } else if (ctrl.Name == "createRoomItem" || ctrl.Name == "createRoomIcon") {
                createRoomItem.BackColor = System.Drawing.Color.FromArgb(54, 62, 71);
                newY = createRoomItem.Location.Y;
                creRoomPanel.Show();
                creRoomPanel.BringToFront();
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

        string getUserProfilePic () {
            string picture_url = "";

            sendMessageToServer("543");

            if (getResultFromServer(3) == "189")
            {
                int picUrlSize = Int32.Parse(getResultFromServer(3));
                picture_url = getResultFromServer(picUrlSize);
            }

            return picture_url;
        }

        string uploadImageToServer (string path) {
            string myFile = path;
            WebClient webClient = new WebClient();
            Byte[] response = webClient.UploadFile(@"https://triviaplusplus.000webhostapp.com/image_upload.php", "POST", myFile);

            string url = "https://triviaplusplus.000webhostapp.com/" + System.Text.Encoding.UTF8.GetString(response);
            webClient.Dispose();

            return url;
        }

        private void sendMessageToServer (string message) {
            byte[] buffer = new ASCIIEncoding().GetBytes(message);
            clientStream.Write(buffer, 0, message.Length);
            clientStream.Flush();
        }

        private string getResultFromServer (int bytes) {
            byte[] bufferIn = new byte[bytes];
            int bytesRead = clientStream.Read(bufferIn, 0, bytes);
            string result = new ASCIIEncoding().GetString(bufferIn);

            return result;
        }

        private void mainProfilePicture_Click(object sender, EventArgs e) {
            profilePanel.Visible = true;
            profilePanel.BringToFront();
            profilePanelPic.Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, profilePanelPic.Width, profilePanelPic.Height, 100, 100));
        }

        private void profilePanelPic_Click(object sender, EventArgs e) {
            using (OpenFileDialog dlg = new OpenFileDialog())
            {
                dlg.Title = "Open Image";
                dlg.Filter = "Image Files (*.bmp;*.jpg;*.jpeg,*.png)|*.BMP;*.JPG;*.JPEG;*.PNG";

                if (dlg.ShowDialog() == DialogResult.OK) {
                    fileDialogPath = dlg.FileName;
                    Thread t = new Thread(new ParameterizedThreadStart(updateProfilePicture));
                    t.Start(fileDialogPath);
                }
            }
        }

        private void updateProfilePicture (object path) {
            mainProfilePicture.Invoke((MethodInvoker)delegate {
                mainProfilePicture.Image = Image.FromFile((string)fileDialogPath);
            });

            profilePanelPic.Invoke((MethodInvoker)delegate {
                profilePanelPic.Image = mainProfilePicture.Image;
            });

            string url = uploadImageToServer((string)fileDialogPath);

            string message = "381" + url.Length.ToString("D3") + url;
            sendMessageToServer(message);
        }

        private void profilePanel_Paint(object sender, PaintEventArgs e)
        {

        }

        private void roomNameBox_Enter(object sender, EventArgs e)
        {
            if (roomNameBox.Text == "Room Name")
            {
                roomNameBox.Text = "";
                roomNameBox.TabStop = true;
                numPlayersBox.TabStop = true;
                numQuestBox.TabStop = true;
                questionsTimeBox.TabStop = true;
            }
        }

        private void numPlayersBox_Enter(object sender, EventArgs e)
        {
            if (numPlayersBox.Text == "No. Players")
            {
                numPlayersBox.Text = "";
                numPlayersBox.TabStop = true;
                roomNameBox.TabStop = true;
                numQuestBox.TabStop = true;
                questionsTimeBox.TabStop = true;
            }
        }

        private void numQuestBox_Enter(object sender, EventArgs e)
        {
            if (numQuestBox.Text == "No. Questions")
            {
                numQuestBox.Text = "";
                numQuestBox.TabStop = true;
                roomNameBox.TabStop = true;
                numPlayersBox.TabStop = true;
                questionsTimeBox.TabStop = true;
            }
        }

        private void questionsTimeBox_Enter(object sender, EventArgs e)
        {
            if (questionsTimeBox.Text == "Time Per Question")
            {
                questionsTimeBox.Text = "";
                questionsTimeBox.TabStop = true;
                roomNameBox.TabStop = true;
                numPlayersBox.TabStop = true;
                numQuestBox.TabStop = true;
            }
        }

        protected void createRoom()
        {
            string roomName = roomNameBox.Text;
            string numPlayers = numPlayersBox.Text;
            string numQuest = numQuestBox.Text;
            string questionsTime = questionsTimeBox.Text;

            if (roomName != "" && numPlayers != "" && numQuest != "" && questionsTime != "" &&
                roomName != "Room Name" && numPlayers != "No. Players" && numQuest != "No. Questions" && questionsTime != "Time Per Question")
            {
                try
                {
                    int questionsNum = Convert.ToInt32(numQuest);
                    int timeQuestions = Convert.ToInt32(questionsTime);
                    string message = "213" + roomName.Length.ToString("D2") + roomName + numPlayers
                        + questionsNum.ToString("D2") + timeQuestions.ToString("D2"); //D2 for 2 decimal digits format.
                    Console.WriteLine(message);
                    sendMessageToServer(message);
                    string resultCode = getResultFromServer(4);
                    string errorMsg = protocol.getCodeErrorMsg(resultCode);
                    if (errorMsg == "success")
                    {
                        // Login
                        CreateRoomFeedbackLabel.Hide();
                    }
                    else
                    {
                        CreateRoomFeedbackLabel.Visible = Visible;
                        CreateRoomFeedbackLabel.Text = errorMsg;
                    }



                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }
            else
            {
                CreateRoomFeedbackLabel.Visible = Visible;
                CreateRoomFeedbackLabel.Text = "Fields must not be empty.";
            }
        }

        private void createRoomBtn_Click(object sender, EventArgs e)
        {
            createRoom();
        }

        private void questionsTimeBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 13) // Enter
            {
                createRoom();
            }
        }

        private void createRoomItem_Enter(object sender, EventArgs e)
        {
            //creRoomPanel.Show();
            //creRoomPanel.BringToFront();
            //creRoomPanel.Visible = Visible;
        }

        private void createRoomItem_Leave(object sender, EventArgs e)
        {
            //creRoomPanel.Hide();
        }
    }
}
 