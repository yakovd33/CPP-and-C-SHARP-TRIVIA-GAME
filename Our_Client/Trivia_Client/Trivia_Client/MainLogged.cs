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
                try {
                    Thread.Sleep(1000);
                    mainProfilePicture.Load(getUserProfilePic());
                    profilePanelPic.Image = mainProfilePicture.Image;
                } catch (Exception e) {

                }
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
            roomsItem.Click += new EventHandler(SidebarItemClick);
            sidebarIcon1.Click += new EventHandler(SidebarItemClick);
            roomsIcon.Click += new EventHandler(SidebarItemClick);            
        }

        private void SidebarItemClick(object sender, EventArgs e)
        {
            Control ctrl = sender as Control;

            Control[] sidebarItems = { sidebarItem1, createRoomItem, roomsItem };
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
                creRoomPanel.BringToFront();
            } else if (ctrl.Name == "roomsItem" || ctrl.Name == "roomsIcon") {
                roomsItem.BackColor = System.Drawing.Color.FromArgb(54, 62, 71);
                newY = roomsItem.Location.Y;
                roomsPanel.BringToFront();
                listRooms();
            } else if (ctrl.Name == "sidebarItem3" || ctrl.Name == "sidebarIcon3") {
                roomsItem.BackColor = System.Drawing.Color.FromArgb(54, 62, 71);
                newY = roomsItem.Location.Y;
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

        private void listRooms () {
            roomsList.Controls.Clear();
            sendMessageToServer("205");

            if (getResultFromServer(3) == "106") {
                int numRooms = Int32.Parse(getResultFromServer(4));
                Panel[] roomItemsWraps = new Panel[numRooms];
                Point[] originalLocations = new Point[numRooms];

                int currentYPos = 0;

                for (int i = 0; i < numRooms; i++) {
                    int roomId = Int32.Parse(getResultFromServer(4));
                    int roomNameLength = Int32.Parse(getResultFromServer(2));
                    string roomName = getResultFromServer(roomNameLength);

                    Panel roomItem = new Panel();
                    roomItem.Cursor = Cursors.Hand;
                    roomItem.Height = 66;
                    roomItem.Width = 805;
                    roomItem.BackColor = Color.FromArgb(48, 56, 65);
                    roomItem.Location = new Point(0, currentYPos);
                    originalLocations[i] = roomItem.Location;

                    Label roomItemName = new Label();
                    roomItemName.Font = new Font("Open Sans Light", 12);
                    roomItemName.ForeColor = Color.FromArgb(173, 190, 202);
                    roomItemName.Location = new Point(10, 21);
                    roomItemName.Text = roomName;
                    roomItem.Controls.Add(roomItemName);

                    PictureBox joinBtn = new PictureBox();
                    joinBtn.Height = 34;
                    joinBtn.Width = 145;
                    joinBtn.Location = new Point(645, 16);
                    joinBtn.Image = Trivia_Client.Properties.Resources.join_btn;
                    joinBtn.SizeMode = PictureBoxSizeMode.StretchImage;
                    joinBtn.Cursor = Cursors.Hand;
                    roomItem.Controls.Add(joinBtn);

                    Label roomIdLabel = new Label();
                    roomIdLabel.Name = "roomIdLabel";
                    roomIdLabel.Text = roomId.ToString();
                    roomIdLabel.Hide();
                    roomItem.Controls.Add(roomIdLabel);

                    Label roomIsUsersListOpen = new Label();
                    roomIsUsersListOpen.Name = "roomIsUsersListOpen";
                    roomIsUsersListOpen.Text = "false";
                    roomIsUsersListOpen.Hide();
                    roomItem.Controls.Add(roomIsUsersListOpen);

                    roomItem.Click += delegate (object sender, EventArgs e) {
                        if (((Panel) sender).Controls["roomIsUsersListOpen"].Text == "true") {
                            // Panel is already open

                            bool beenAlready = false;
                            for (int j = 0; j < roomItemsWraps.Length; j++) {
                                Label secondaryRoomItemIdLabel = roomItemsWraps[j].Controls["roomIdLabel"] as Label;
                                roomItemsWraps[j].Location = originalLocations[j];
                                if (roomId.ToString() != secondaryRoomItemIdLabel.Text) {
                                    if (beenAlready) {
                                        roomItemsWraps[j].Location = originalLocations[j];
                                    }
                                } else {
                                    beenAlready = true;
                                }
                            }

                            ((Panel)sender).Controls["roomIsUsersListOpen"].Text = "false";
                        }
                        else {
                            // Add room users list
                            sendMessageToServer("207" + ((Panel)sender).Controls["roomIdLabel"].Text);

                            if (getResultFromServer(3) == "108") {
                                int numPlayers = Int32.Parse(getResultFromServer(1));

                                int usersCurYPos = ((Panel)sender).Location.Y + ((Panel)sender).Height + 10;
                                Console.WriteLine(usersCurYPos);
                                for (int q = 0; q < numPlayers; q++) {
                                    int usernameLength = Int32.Parse(getResultFromServer(2));
                                    string username = getResultFromServer(usernameLength);
                                    Console.WriteLine(username);
                                    Label roomUserNameLabel = new Label();
                                    roomUserNameLabel.Font = new Font("Open Sans Light", 10);
                                    roomUserNameLabel.Text = username;
                                    roomUserNameLabel.Location = new Point(0, usersCurYPos);
                                    roomsList.Controls.Add(roomUserNameLabel);
                                    usersCurYPos += 40;
                                }

                                bool beenAlready = false;
                                for (int j = 0; j < roomItemsWraps.Length; j++) {
                                    Label secondaryRoomItemIdLabel = roomItemsWraps[j].Controls["roomIdLabel"] as Label;
                                    roomItemsWraps[j].Location = originalLocations[j];
                                    if (roomId.ToString() != secondaryRoomItemIdLabel.Text) {
                                        if (beenAlready) {
                                            roomItemsWraps[j].Location = new Point(0, originalLocations[j].Y + (35 * numPlayers));
                                        }
                                    } else {
                                        beenAlready = true;
                                    }
                                }

                            ((Panel)sender).Controls["roomIsUsersListOpen"].Text = "true";
                            }
                        }
                    };

                    roomsList.Controls.Add(roomItem);
                    roomItemsWraps[i] = roomItem;

                    currentYPos += 76;
                }
            }
        }

        private void tabs_Paint(object sender, PaintEventArgs e)
        {

        }

        private void roomsListRefreshBtn_Click(object sender, EventArgs e) {
            listRooms();
        }
    }
}
 