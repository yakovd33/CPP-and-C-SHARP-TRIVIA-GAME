using System;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading;
using System.Drawing;
using System.Windows.Forms;
using System.IO;

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
        int currentRoomId = 0;
        bool inRoom = false;
        bool inGame = false;
        bool exit = false;
        bool isLeaderBoardLoaded = false;

        int roomQuestionTime;
        int roomQuestionsNumber;
        int currentScore = 0;
        int currentQuestionsCounter = 0;
        Thread questionsThread;
        Thread answerTimerThread;

        int answerSeconds = 0;

        public MainLogged(TcpClient client, IPEndPoint serverEndPoint, NetworkStream clientStream)
        {
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
            saveSettingsBtn.Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, saveSettingsBtn.Width, saveSettingsBtn.Height, 5, 5));

            mainProfilePicture.Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, mainProfilePicture.Width, mainProfilePicture.Height, 49, 49));
            Sidebar();

            ipBox.Text = getSettingValue("server_ip");
            portBox.Text = getSettingValue("port");
            volumeBar.Value = Int32.Parse(getSettingValue("volume"));

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

        private void MainLogged_Load(object sender, EventArgs e) {
            answerTimerThread = new Thread(answerTimer);
            answerTimerThread.IsBackground = true;
            answerTimerThread.Start();
        }

        private void answerTimer () {
            while (!exit) {
                answerSeconds++;

                if (inGame)
                {
                    // Update game counter
                    gameCountdown.BeginInvoke((MethodInvoker)delegate {
                        gameCountdown.Text = (roomQuestionTime - answerSeconds).ToString();
                    });

                    // Update game counter bar
                    gameTimeProgressBar.BeginInvoke((MethodInvoker)delegate {
                        gameTimeProgressBar.Width = (670 / roomQuestionTime) * (roomQuestionTime - answerSeconds);
                    });

                    if (answerSeconds >= roomQuestionTime) //if time is up
                    {
                        answerSeconds = 0;
                        sendMessageToServer("219" + "5" + answerSeconds.ToString("D2")); // Question timeout
                        getAnswerResponse();
                        nextTour();
                    }
                }

                Thread.Sleep(1000);
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

        private void exitBtn_Click(object sender, EventArgs e) {
            try
            {
                sendMessageToServer("299");
                questionsThread.IsBackground = true;
                answerTimerThread.IsBackground = true;
            } catch (Exception ex) {
                Console.WriteLine(ex);
            } finally {
                Application.Exit();

            }
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
            sendMessageToServer("201");
        }

        private void Sidebar()
        {
            sidebarItem1.Click += new EventHandler(SidebarItemClick);
            createRoomItem.Click += new EventHandler(SidebarItemClick);
            createRoomIcon.Click += new EventHandler(SidebarItemClick);
            roomsItem.Click += new EventHandler(SidebarItemClick);
            sidebarIcon1.Click += new EventHandler(SidebarItemClick);
            roomsIcon.Click += new EventHandler(SidebarItemClick);
            leadeboardItem.Click += new EventHandler(SidebarItemClick);
            leadeboardItemIcon.Click += new EventHandler(SidebarItemClick);
            settingsItem.Click += new EventHandler(SidebarItemClick);
            settingsItemIcon.Click += new EventHandler(SidebarItemClick);
        }

        private void SidebarItemClick(object sender, EventArgs e) {
            Control ctrl = sender as Control;

            Control[] sidebarItems = { sidebarItem1, createRoomItem, roomsItem, leadeboardItem };
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
            }
            else if (ctrl.Name == "roomsItem" || ctrl.Name == "roomsIcon") {
                roomsItem.BackColor = System.Drawing.Color.FromArgb(54, 62, 71);
                newY = roomsItem.Location.Y;
                roomsPanel.BringToFront();
                listRooms();
            } else if (ctrl.Name == "sidebarItem3" || ctrl.Name == "sidebarIcon3") {
                roomsItem.BackColor = System.Drawing.Color.FromArgb(54, 62, 71);
                newY = roomsItem.Location.Y;
            } else if (ctrl.Name == "leadeboardItem" || ctrl.Name == "leadeboardItemIcon") {
                leadeboardItem.BackColor = System.Drawing.Color.FromArgb(54, 62, 71);
                newY = leadeboardItem.Location.Y;

                if (!isLeaderBoardLoaded) {
                    Thread getLeaderboardThread = new Thread(getLeaderboard);
                    getLeaderboardThread.IsBackground = true;
                    getLeaderboardThread.Start();
                }

                leadboardPanel.BringToFront();
            } else if (ctrl.Name == "settingsItem" || ctrl.Name == "settingsItemIcon") {
                settingsItem.BackColor = System.Drawing.Color.FromArgb(54, 62, 71);
                newY = settingsItem.Location.Y;
                settingsPanel.BringToFront();
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

        string getUserProfilePicByUsername(string username) {
            string picture_url = "https://i.imgur.com/oeKRbhC.png";

            sendMessageToServer("419" + username.Length.ToString("D2") + username);

            if (getResultFromServer(3) == "189") {
                int pictureLength = Int32.Parse(getResultFromServer(3));
                picture_url = getResultFromServer(pictureLength);
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
            profilePanel.BringToFront();
            profilePanelPic.Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, profilePanelPic.Width, profilePanelPic.Height, 100, 100));
            
            // Get personal status
            sendMessageToServer("225");

            if (getResultFromServer(3) == "126") {
                string numGames = Int32.Parse(getResultFromServer(4)).ToString();
                string R_ans = Int32.Parse(getResultFromServer(6)).ToString();
                string W_ans = Int32.Parse(getResultFromServer(6)).ToString();
                string AVG_t_f = Int32.Parse(getResultFromServer(2)).ToString();
                string AVG_t_s = Int32.Parse(getResultFromServer(2)).ToString();
                label1.Text = "Number of games: " + numGames + "\nRight answers: " + R_ans + "\nWrong answers: " + W_ans + "\nAVG time to answer: " + AVG_t_f + "." + AVG_t_s;
            }
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

            if (Int32.Parse(numPlayers) < 10 && Int32.Parse(numPlayers) > 0 && roomName != "" && numPlayers != "" && numQuest != "" && questionsTime != "" &&
                roomName != "Room Name" && numPlayers != "No. Players" && numQuest != "No. Questions" && questionsTime != "Time Per Question")
            {
                try
                {
                    int questionsNum = Convert.ToInt32(numQuest);
                    int timeQuestions = Convert.ToInt32(questionsTime);
                    roomQuestionsNumber = questionsNum;
                    roomQuestionTime = timeQuestions;

                    string message = "213" + roomName.Length.ToString("D2") + roomName + numPlayers
                        + questionsNum.ToString("D2") + timeQuestions.ToString("D2"); //D2 for 2 decimal digits format.
                    sendMessageToServer(message);
                    string resultCode = getResultFromServer(4);
                    string errorMsg = protocol.getCodeErrorMsg(resultCode);
                    if (errorMsg == "success")
                    {
                        CreateRoomFeedbackLabel.Hide();
                        roomPanel.BringToFront();

                        inRoom = true;
                        // Get new room id
                        sendMessageToServer("517");
                        currentRoomId = Int32.Parse(getResultFromServer(4));
                        roomNameLabel.Text = roomName;
                        roomNumQuestionsLabel.Text = "Number of questions: " + numQuest;
                        roomTimePerQuestion.Text = "Time per question: " + questionsTime;
                        listRoomUsers();

                        // Show start game and room close btns
                        closeRoomBtn.Visible = true;
                        startGameBtn.Visible = true;
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
                
                if (Int32.Parse(numPlayers) >= 10 || Int32.Parse(numPlayers) < 0) {
                    CreateRoomFeedbackLabel.Text = "Num players needs to be between 1-9.";
                } else {
                    CreateRoomFeedbackLabel.Text = "Fields must not be empty.";
                }
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

                    // Make panel rounder
                    roomItem.Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, roomItem.Width, roomItem.Height, 6, 6));

                    Label roomItemName = new Label();
                    roomItemName.Name = "roomItemName";
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

                    joinBtn.Click += delegate (object sender, EventArgs e) {
                        currentRoomId = Int32.Parse(((PictureBox) sender).Parent.Controls["roomIdLabel"].Text);

                        sendMessageToServer("209" + currentRoomId.ToString("D2"));

                        string roomJoinRequestResponse = getResultFromServer(4);
                        string roomJoinRequestResponseMsg = protocol.getCodeErrorMsg(roomJoinRequestResponse);
                        
                        if (roomJoinRequestResponseMsg == "success") {
                            // Server joined user to room
                            inRoom = true;

                            // Disable sidebar tabs
                            sidebarItem1.Enabled = false;
                            sidebarIcon1.Enabled = false;
                            createRoomItem.Enabled = false;
                            createRoomIcon.Enabled = false;
                            roomsItem.Enabled = false;
                            roomsIcon.Enabled = false;

                            // Hide start game and room close btns
                            closeRoomBtn.Visible = false;
                            startGameBtn.Visible = false;

                            roomPanel.BringToFront();
                            roomNameLabel.Text = ((PictureBox)sender).Parent.Controls["roomItemName"].Text;
                            int questionNumber = Int32.Parse(getResultFromServer(2));
                            int questionTime = Int32.Parse(getResultFromServer(2));
                            roomQuestionTime = questionTime;
                            roomQuestionsNumber = questionNumber;

                            roomNumQuestionsLabel.Text = "Number of questions: " + questionNumber.ToString();
                            roomTimePerQuestion.Text = "Time per question: " + questionTime.ToString();

                            listRoomUsers();

                            // Listen to game start while in room Lobby
                            Thread gameStartListen = new Thread(listenToGameStart);
                            gameStartListen.IsBackground = true;
                            gameStartListen.Start();
                        } else {
                            currentRoomId = 0;
                            System.Windows.Forms.MessageBox.Show(roomJoinRequestResponseMsg);
                        }
                    };

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

        private void roomExitBtn_Click(object sender, EventArgs e) {
            sendMessageToServer("211");

            if (getResultFromServer(4) == "1120") {
                // Room leave successful
                currentRoomId = 0;
                inRoom = false;

                mainPanel.BringToFront();
                sidebarItem1.Enabled = true;
                sidebarIcon1.Enabled = true;
                createRoomItem.Enabled = true;
                createRoomIcon.Enabled = true;
                roomsItem.Enabled = true;
                roomsIcon.Enabled = true;

                // Go back to main tab
                Control[] sidebarItems = { sidebarItem1, createRoomItem, roomsItem };
                for (int i = 0; i < sidebarItems.Length; i++) {
                    sidebarItems[i].BackColor = System.Drawing.Color.FromArgb(1, 48, 56, 65);
                }

                int newY = sidebarActivePanelIndicator.Location.Y;
                sidebarItem1.BackColor = System.Drawing.Color.FromArgb(54, 62, 71);
                newY = sidebarItem1.Location.Y;

                Thread animate = new Thread(new ParameterizedThreadStart(animateSlidebarSelectionBar));
                animate.Start(newY);
            }
        }

        void listRoomUsers () {
            // Clear current room list
            currentRoomUsersList.Controls.Clear();

            // Get room players
            sendMessageToServer("207" + currentRoomId.ToString());

            if (getResultFromServer(3) == "108")
            {
                int curRoomNumPlayers = Int32.Parse(getResultFromServer(1));
                string[] usernames = new string[curRoomNumPlayers];

                int curRoomUsersCurYPos = 2;
                for (int q = 0; q < curRoomNumPlayers; q++)
                {
                    string usernameLengthStr = getResultFromServer(2);
                    int roomCurUsernameLength = Int32.Parse(usernameLengthStr);
                    string username = getResultFromServer(roomCurUsernameLength);
                    usernames[q] = username;

                    Label roomUserNameLabel = new Label();
                    roomUserNameLabel.Font = new Font("Open Sans Light", 10);
                    roomUserNameLabel.ForeColor = Color.FromArgb(173, 190, 202);
                    roomUserNameLabel.Text = username;
                    roomUserNameLabel.Location = new Point(40, curRoomUsersCurYPos);
                    currentRoomUsersList.Controls.Add(roomUserNameLabel);
                    curRoomUsersCurYPos += 43;
                }

                int curUserPicYPos = 0;
                new Thread(() => {
                    for (int q = 0; q < curRoomNumPlayers; q++)
                    {
                        sendMessageToServer("419" + usernames[q].Length.ToString("D2") + usernames[q]);

                        if (getResultFromServer(3) == "189")
                        {
                            int pictureLength = Int32.Parse(getResultFromServer(3));
                            string profilePicUrl = getResultFromServer(pictureLength);

                            PictureBox currentUserListUserPic = new PictureBox();
                            currentUserListUserPic.Load(profilePicUrl);
                            currentUserListUserPic.Height = 30;
                            currentUserListUserPic.Width = 30;
                            currentUserListUserPic.Location = new Point(0, curUserPicYPos);
                            currentUserListUserPic.SizeMode = PictureBoxSizeMode.StretchImage;
                            currentRoomUsersList.Invoke((MethodInvoker)delegate {
                                currentRoomUsersList.Controls.Add(currentUserListUserPic);
                                //currentRoomUsersList.Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, 30, 30, 30, 30));
                            });

                            curUserPicYPos += 40;
                        }
                    }
                }).Start();
            }
        }

        void getLeaderboard () {
            firstPlacePic.Invoke((MethodInvoker)delegate {
                firstPlacePic.Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, firstPlacePic.Width, firstPlacePic.Height, firstPlacePic.Height, firstPlacePic.Width));
                secPlacePic.Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, secPlacePic.Width, secPlacePic.Height, secPlacePic.Height, secPlacePic.Width));
                thirdPlacePic.Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, thirdPlacePic.Width, thirdPlacePic.Height, thirdPlacePic.Height, thirdPlacePic.Width));
            });

            sendMessageToServer("223");

            bool isFirst = false, isSec = false, isThird = false;
            string secUsername = "";
            string firstUsername = "";
            string thirdUsername = "";

            if (getResultFromServer(3) == "124") {
                isLeaderBoardLoaded = true;
                int firstUsernameLength = Int32.Parse(getResultFromServer(2));
                if (firstUsernameLength > 0) {
                    // First exists
                    isFirst = true;
                    firstUsername = getResultFromServer(firstUsernameLength);
                    int firstUserScoreCount = Int32.Parse(getResultFromServer(6));

                    firstPlaceUsername.Invoke((MethodInvoker)delegate {
                        firstPlaceUsername.Text = firstUsername;
                        firstPlaceScore.Text = firstUserScoreCount.ToString();
                    });

                    int secUsernameLength = Int32.Parse(getResultFromServer(2));
                    if (secUsernameLength > 0) {
                        // Second exists
                        isSec = true;
                        secUsername = getResultFromServer(secUsernameLength);
                        int secUserScoreCount = Int32.Parse(getResultFromServer(6));
                        double secProgress = ((double)secUserScoreCount / (double)firstUserScoreCount) * (double)100;

                        secPlaceUsername.Invoke((MethodInvoker)delegate {
                            secPlaceUsername.Text = secUsername;
                            secPlaceScore.Text = secUserScoreCount.ToString();
                            secPlaceProgress.Width = (int)((double)firstPlaceProgress.Width * ((double)secProgress / (double)100));
                        });

                        int thirdUsernameLength = Int32.Parse(getResultFromServer(2));
                        if (thirdUsernameLength > 0) {
                            // Third exists
                            isThird = true;
                            thirdUsername = getResultFromServer(thirdUsernameLength);
                            int thirdUserScoreCount = Int32.Parse(getResultFromServer(6));
                            double thirdProgress = ((double)secUserScoreCount / (double)firstUserScoreCount) * (double)100;

                            lastPlaceUsername.Invoke((MethodInvoker)delegate {
                                lastPlaceUsername.Text = thirdUsername;
                                lastPlaceScore.Text = thirdUserScoreCount.ToString();
                                thirdPlaceProgress.Width = (int)((double)firstPlaceProgress.Width * ((double)thirdProgress / (double)100));
                            });
                        } else {
                            thirdPlaceProgress.Width = 0;
                            getResultFromServer(8);
                        }
                    } else {
                        secPlaceProgress.Invoke((MethodInvoker)delegate {
                            secPlaceProgress.Width = 0;
                        });

                        getResultFromServer(16);
                    }
                } else {
                    getResultFromServer(24);
                }

                new Thread(() => {
                    if (isFirst) {
                        firstPlacePic.Invoke((MethodInvoker)delegate {
                            firstPlacePic.Load(getUserProfilePicByUsername(firstUsername));
                        });
                    }

                    if (isSec) {
                        secPlacePic.Invoke((MethodInvoker)delegate {
                            secPlacePic.Load(getUserProfilePicByUsername(secUsername));
                        });
                    }

                    if (isThird) {
                        thirdPlacePic.Invoke((MethodInvoker)delegate {
                            thirdPlacePic.Load(getUserProfilePicByUsername(thirdUsername));
                        });
                    }
                }).Start();
            }
        }

        private void startGameBtn_Click(object sender, EventArgs e)
        {
            sendMessageToServer("217");//start game msg
            gamePanel.BringToFront();
            startGame();
        }

        private void startGame() {
            inGame = true;
            currentScore = 0;
            currentQuestionsCounter = 0;

            getQuestions();
        }

        private void getQuestions ()
        {
            firstAnswerBtn.BackColor = Color.FromArgb(48, 56, 65);
            secondAnswerBtn.BackColor = Color.FromArgb(48, 56, 65);
            thirdAnswerBtn.BackColor = Color.FromArgb(48, 56, 65);
            fourthAnswerBtn.BackColor = Color.FromArgb(48, 56, 65);

            if (getResultFromServer(3) == "118")
            {
                Console.WriteLine("getting questions");

                inGame = true;
                currentQuestionsCounter++;
                answerSeconds = 0;
                int questionLength = Int32.Parse(getResultFromServer(3));
                string question = getResultFromServer(questionLength);
                string firstAnswer = getResultFromServer(Int32.Parse(getResultFromServer(3)));
                string secondAnswer = getResultFromServer(Int32.Parse(getResultFromServer(3)));
                string thirdAnswer = getResultFromServer(Int32.Parse(getResultFromServer(3)));
                string fourthAnswer = getResultFromServer(Int32.Parse(getResultFromServer(3)));

                // Update answer buttons
                questionLabel.Invoke((MethodInvoker)delegate
                {
                    questionLabel.Text = question;
                });

                    
                firstAnswerBtn.Invoke((MethodInvoker)delegate
                {
                    firstAnswerBtn.Text = firstAnswer;
                });

                secondAnswerBtn.Invoke((MethodInvoker)delegate
                {
                    secondAnswerBtn.Text = secondAnswer;
                });

                thirdAnswerBtn.Invoke((MethodInvoker)delegate
                {
                    thirdAnswerBtn.Text = thirdAnswer;
                });

                fourthAnswerBtn.Invoke((MethodInvoker)delegate
                {
                    fourthAnswerBtn.Text = fourthAnswer;
                });

                // Update question counter
                questionInformer.Invoke((MethodInvoker)delegate
                {
                    questionInformer.Text = "Question " + currentQuestionsCounter.ToString() + "/" + roomQuestionsNumber.ToString();
                });

                // Update score counter
                gameScore.Invoke((MethodInvoker)delegate
                {
                    gameScore.Text = "Score: " + currentScore.ToString();
                });
            }
        }

        private void disableAllAnswerBtns () {
            firstAnswerBtn.Enabled = false;
            secondAnswerBtn.Enabled = false;
            thirdAnswerBtn.Enabled = false;
            fourthAnswerBtn.Enabled = false;
        }

        private void enableAllAnswerBtns() {
            new Thread(() => {
                firstAnswerBtn.Invoke((MethodInvoker) delegate {
                    firstAnswerBtn.Enabled = true;
                    secondAnswerBtn.Enabled = true;
                    thirdAnswerBtn.Enabled = true;
                    fourthAnswerBtn.Enabled = true;
                });
            }).Start();
        }

        private void firstAnswerBtn_Click(object sender, EventArgs e) {
            disableAllAnswerBtns();

            sendMessageToServer("219" + "1" + answerSeconds.ToString("D2"));
            if (getAnswerResponse()) {
                firstAnswerBtn.BackColor = Color.Green;
            } else {
                firstAnswerBtn.BackColor = Color.Red;
            }

            nextTour();
        }

        private void secondAnswerBtn_Click(object sender, EventArgs e) {
            disableAllAnswerBtns();
            sendMessageToServer("219" + "2" + answerSeconds.ToString("D2"));
            if (getAnswerResponse()) {
                secondAnswerBtn.BackColor = Color.Green;
            } else {
                secondAnswerBtn.BackColor = Color.Red;
            }

            nextTour();
        }

        private void thirdAnswerBtn_Click(object sender, EventArgs e) {
            disableAllAnswerBtns();
            sendMessageToServer("219" + "3" + answerSeconds.ToString("D2"));
            if (getAnswerResponse()) {
                thirdAnswerBtn.BackColor = Color.Green;
            } else {
                thirdAnswerBtn.BackColor = Color.Red;
            }

            nextTour();
        }

        private void fourthAnswerBtn_Click(object sender, EventArgs e) {
            disableAllAnswerBtns();
            sendMessageToServer("219" + "4" + answerSeconds.ToString("D2"));
            if (getAnswerResponse()) {
                fourthAnswerBtn.BackColor = Color.Green;
            } else {
                fourthAnswerBtn.BackColor = Color.Red;
            }

            nextTour();
        }

        private void nextTour () {
            if (currentQuestionsCounter < roomQuestionsNumber) {
                // Get next question
                new Thread(() => {
                    Thread.Sleep(1500);
                    getQuestions();
                    enableAllAnswerBtns();
                }).Start();
            } else {
                // Game finished
                answerTimerThread.Abort(); // Stop timer thread

                Scores scores = new Scores(client, serverEndPoint, clientStream);
                scores.ShowDialog();
               
            }
        }

        private bool getAnswerResponse () {
            if (getResultFromServer(4) == "1201") {
                // Correct answer
                currentScore++;
                return true;
            }

            return false;
        }

        private void listenToGameStart () {
            while (!inGame) {
                Byte[] peekBuffer = new byte[3];
                client.Client.Receive(peekBuffer, SocketFlags.Peek);

                if (new ASCIIEncoding().GetString(peekBuffer).Substring(0, 3) == "118") {
                    Console.WriteLine("starting game");
                    // Start game
                    gamePanel.Invoke((MethodInvoker)delegate {
                        gamePanel.BringToFront();
                    });

                    startGame();
                }
            }
        }

        [DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();

        private void dragContainerMouseMove(object sender, MouseEventArgs e) {
            // Fixing the window dragging problem
            if (e.Button == MouseButtons.Left) {
                ReleaseCapture();
                SendMessage(Handle, 0x00A1, 2, 0);
            }
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private string getSettingValue (string key) {
            System.IO.StreamReader file = new System.IO.StreamReader(@"config.triv");
            string line = "";

            while ((line = file.ReadLine()) != null) {
                if (line.Split('=')[0] == key) {
                    file.Close();
                    return line.Split('=')[1];
                }
            }

            file.Close();
            return "";
        }

        // Save settings
        private void saveSettingsBtn_Click(object sender, EventArgs e) {
            using (StreamWriter writer = new StreamWriter("config.triv")) {
                writer.WriteLine("server_ip=" + ipBox.Text + "\nport=" + portBox.Text + "\nvolume=" + volumeBar.Value);
                settingsFeedbackLabel.Text = "Changes saved.";
                writer.Close();
            }
        }
    }
}
