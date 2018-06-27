#include "TriviaServer.h"
#include <exception>
#include <iostream>
#include <string>
#include "Header.h"

//#include "stdafx.h"

#include "easendmailobj.tlh"
using namespace EASendMailObjLib;

// using static const instead of macros 
static const unsigned short PORT = 8820;
static const unsigned int IFACE = 0;

int TriviaServer::_roomIdSequence = 1;

TriviaServer::TriviaServer()
{
	// notice that we step out to the global namespace
	// for the resolution of the function socket
	_socket = ::socket(AF_INET, SOCK_STREAM, IPPROTO_TCP);
	if (_socket == INVALID_SOCKET)
		throw std::exception(__FUNCTION__ " - socket");

	// Create DataBase Instance
	_db = new DataBase();

	_roomsList.insert(make_pair(203, new Room(5, 20, 20, "room1", 203)));
	_roomsList.insert(make_pair(1235, new Room(5, 20, 20, "room2", 1235)));
	_roomsList.insert(make_pair(123, new Room(5, 20, 20, "room3", 1235)));
	_roomsList.insert(make_pair(124, new Room(5, 20, 20, "room4", 124)));
	_roomsList.insert(make_pair(315, new Room(5, 20, 20, "room5", 315)));
	_roomsList.insert(make_pair(316, new Room(5, 20, 20, "room6", 316)));
	_roomsList.insert(make_pair(317, new Room(5, 20, 20, "room7", 317)));
	_roomsList.insert(make_pair(318, new Room(5, 20, 20, "room8", 318)));
	_roomsList.insert(make_pair(319, new Room(5, 20, 20, "room9", 319)));
	_roomsList.insert(make_pair(320, new Room(5, 20, 20, "room10", 320)));
}

TriviaServer::~TriviaServer()
{
	TRACE(__FUNCTION__ " closing accepting socket");
	// why is this try necessarily ?
	try {
		// the only use of the destructor should be for freeing 
		// resources that was allocated in the constructor
		::closesocket(_socket);
	}
	catch (...) {}
}

void TriviaServer::serve()
{
	bindAndListen();

	// create new thread for handling message
	std::thread tr(&TriviaServer::handleRecievedMessages, this);
	tr.detach();

	while (true) {
		// the main thread is only accepting clients 
		// and add then to the list of handlers
		TRACE("accepting client...");
		acceptClient();
	}
}

// listen to connecting requests from clients
// accept them, and create thread for each client
void TriviaServer::bindAndListen()
{
	struct sockaddr_in sa = { 0 };
	sa.sin_port = htons(PORT);
	sa.sin_family = AF_INET;
	sa.sin_addr.s_addr = IFACE;
	// again stepping out to the global namespace
	if (::bind(_socket, (struct sockaddr*)&sa, sizeof(sa)) == SOCKET_ERROR)
		throw std::exception(__FUNCTION__ " - bind");
	TRACE("binded");

	if (::listen(_socket, SOMAXCONN) == SOCKET_ERROR)
		throw std::exception(__FUNCTION__ " - listen");
	TRACE("listening...");
}

void TriviaServer::acceptClient()
{
	SOCKET client_socket = accept(_socket, NULL, NULL);
	if (client_socket == INVALID_SOCKET)
		throw std::exception(__FUNCTION__);

	TRACE("Client accepted !");
	// create new thread for client	and detach from it
	std::thread tr(&TriviaServer::clientHandler, this, client_socket);
	tr.detach();

}

void TriviaServer::clientHandler(SOCKET client_socket)
{
	RecievedMessage* currRcvMsg = nullptr;
	try {
		// get the first message code
		string msgCode = "";

		//cout << "Message: " << msgCode << endl;

		while (true) {
			msgCode = Helper::getMessageTypeCode(client_socket);
			if (msgCode != "") {
				cout << msgCode << endl;
				currRcvMsg = buildRecieveMessage(client_socket, msgCode);
				addRecievedMessage(currRcvMsg);
				msgCode = "";
			}
		}

		currRcvMsg = buildRecieveMessage(client_socket, msgCode);
		addRecievedMessage(currRcvMsg);

	}
	catch (const std::exception& e) {
		// Exit
		//std::cout << "Exception was catch in function clientHandler. socket=" << client_socket << ", what=" << e.what() << std::endl;
		currRcvMsg = buildRecieveMessage(client_socket, "299"); // Handle a socket disconnection
		addRecievedMessage(currRcvMsg);
	}

	closesocket(client_socket);
}

void TriviaServer::addRecievedMessage(RecievedMessage* msg)
{
	unique_lock<mutex> lck(_mtxRecievedMessages);

	_messageHandler.push(msg);
	lck.unlock();
	_msgQueueCondition.notify_all();

}

RecievedMessage* TriviaServer::buildRecieveMessage(SOCKET client_socket, string msgCode) {
	RecievedMessage* msg = nullptr;
	map<string, string> values;

	if (msgCode == "200") {
		// Login
		int usernameSize = atoi(Helper::getStringPartFromSocket(client_socket, 2).c_str());
		string username = Helper::getStringPartFromSocket(client_socket, usernameSize);
		int passwordSize = atoi(Helper::getStringPartFromSocket(client_socket, 2).c_str());
		string password = Helper::getStringPartFromSocket(client_socket, passwordSize);
		values.insert(make_pair("username", username));
		values.insert(make_pair("password", password));
	}

	if (msgCode == "203") {
		// Signup
		int usernameSize = atoi(Helper::getStringPartFromSocket(client_socket, 2).c_str());
		string username = Helper::getStringPartFromSocket(client_socket, usernameSize);
		int passwordSize = atoi(Helper::getStringPartFromSocket(client_socket, 2).c_str());
		string password = Helper::getStringPartFromSocket(client_socket, passwordSize);
		int emailSize = atoi(Helper::getStringPartFromSocket(client_socket, 2).c_str());
		string email = Helper::getStringPartFromSocket(client_socket, emailSize);
		values.insert(make_pair("username", username));
		values.insert(make_pair("password", password));
		values.insert(make_pair("email", email));
	}

	if (msgCode == "207") {
		// Get users in room
		string roomId = Helper::getStringPartFromSocket(client_socket, 4).c_str();
		values.insert(make_pair("room_id", roomId));
	}

	if (msgCode == "209") {
		// Join room
		string roomId = Helper::getStringPartFromSocket(client_socket, 4).c_str();
		values.insert(make_pair("room_id", roomId));
	}

	if (msgCode == "213") {
		int nameSize = atoi(Helper::getStringPartFromSocket(client_socket, 2).c_str());
		string name = Helper::getStringPartFromSocket(client_socket, nameSize);
		string playersNumber = Helper::getStringPartFromSocket(client_socket, 1);
		string questionsNumber = Helper::getStringPartFromSocket(client_socket, 2);
		string questionTime = Helper::getStringPartFromSocket(client_socket, 2);

		values.insert(make_pair("name", name));
		values.insert(make_pair("max_users", playersNumber));
		values.insert(make_pair("questions_number", questionsNumber));
		values.insert(make_pair("question_time", questionTime));
	}

	if (msgCode == "219") {
		values.insert(make_pair("answer_number", Helper::getStringPartFromSocket(client_socket, 1).c_str()));
		values.insert(make_pair("answer_time", Helper::getStringPartFromSocket(client_socket, 20).c_str()));
	}

	if (msgCode == "666")//forgot password
	{
		int emailLength = atoi(Helper::getStringPartFromSocket(client_socket, 2).c_str());
		values.insert(make_pair("email", Helper::getStringPartFromSocket(client_socket, emailLength).c_str()));
	}

	if (msgCode == "419") {
		// Get user profile picture by username
		int usernameLength = atoi(Helper::getStringPartFromSocket(client_socket, 2).c_str());
		values.insert(make_pair("username", Helper::getStringPartFromSocket(client_socket, usernameLength).c_str()));
	}

	if (msgCode == "381") {
		// Profile picture update
		int pictureLength = atoi(Helper::getStringPartFromSocket(client_socket, 3).c_str());
		values.insert(make_pair("url", Helper::getStringPartFromSocket(client_socket, pictureLength).c_str()));
	}

	if (msgCode == "395") {
		// Get user database column
		int colLength = atoi(Helper::getStringPartFromSocket(client_socket, 2).c_str());
		values.insert(make_pair("col", Helper::getStringPartFromSocket(client_socket, colLength)));
	}

	if (msgCode == "714") {
		// Get user database column
		int emailLength = atoi(Helper::getStringPartFromSocket(client_socket, 2).c_str());
		values.insert(make_pair("email", Helper::getStringPartFromSocket(client_socket, emailLength)));
		int passwordLength = atoi(Helper::getStringPartFromSocket(client_socket, 2).c_str());
		values.insert(make_pair("password", Helper::getStringPartFromSocket(client_socket, passwordLength)));
	}

	if (msgCode == "673") {
		// Insert chat message
		int messageLength = atoi(Helper::getStringPartFromSocket(client_socket, 4).c_str());
		values.insert(make_pair("message", Helper::getStringPartFromSocket(client_socket, messageLength)));
	}

	if (msgCode == "984") {
		// Set client version
		values.insert(make_pair("version", Helper::getStringPartFromSocket(client_socket, 3)));
	}

	if (msgCode == "529") {
		// Insert new question
		int questionLength = atoi(Helper::getStringPartFromSocket(client_socket, 4).c_str());
		values.insert(make_pair("question", Helper::getStringPartFromSocket(client_socket, questionLength)));
		int correctAnsLength = atoi(Helper::getStringPartFromSocket(client_socket, 4).c_str());
		values.insert(make_pair("correct_ans", Helper::getStringPartFromSocket(client_socket, correctAnsLength)));
		int secAnsLength = atoi(Helper::getStringPartFromSocket(client_socket, 4).c_str());
		values.insert(make_pair("sec_ans", Helper::getStringPartFromSocket(client_socket, secAnsLength)));
		int thirdAnsLength = atoi(Helper::getStringPartFromSocket(client_socket, 4).c_str());
		values.insert(make_pair("third_ans", Helper::getStringPartFromSocket(client_socket, thirdAnsLength)));
		int fourthAnsLength = atoi(Helper::getStringPartFromSocket(client_socket, 4).c_str());
		values.insert(make_pair("fourth_ans", Helper::getStringPartFromSocket(client_socket, fourthAnsLength)));
		int hintLength = atoi(Helper::getStringPartFromSocket(client_socket, 4).c_str());
		values.insert(make_pair("hint", Helper::getStringPartFromSocket(client_socket, hintLength)));
	}

	if (msgCode == "803") {
		// Get question hint
		int questionLength = atoi(Helper::getStringPartFromSocket(client_socket, 4).c_str());
		values.insert(make_pair("question", Helper::getStringPartFromSocket(client_socket, questionLength)));
	}

	msg = new RecievedMessage(client_socket, msgCode, values);
	return msg;
}

void TriviaServer::handleForgotPassword(RecievedMessage * msg)
{
	/*if (!exit) {
	sendMessageToSocket(msg->getSock(), "667");
	}
	*/
	::CoInitialize(NULL);

	IMailPtr oSmtp = NULL;
	oSmtp.CreateInstance("EASendMailObj.Mail");
	oSmtp->LicenseCode = ("TryIt");

	cout << " ***** " << msg->getMessageCode() << " ****** " << endl;
	// user gmail email address
	oSmtp->FromAddr = ("lolpirt2@gmail.com");//meeeeeeee
	oSmtp->AddRecipientEx(_T(msg->getValues().find("email")->second.c_str()), 0);//meeeee
																				 //email subject
	oSmtp->Subject = ("Trivia++ notification: pass reset");
	// Set email body
	oSmtp->BodyText = ("this is a test email sent from Visual C++ project with Gmail");
	// Gmail SMTP server address
	oSmtp->ServerAddr = ("smtp.gmail.com");

	// If you want to use direct SSL 465 port,
	// Please add this line, otherwise TLS will be used.
	// oSmtp->ServerPort = 465;

	// Set 25 or 587 SMTP port
	oSmtp->ServerPort = 587;

	// detect SSL/TLS automatically
	oSmtp->SSL_init();

	// Gmail user authentication should use your
	// Gmail email address as the user name.
	// For example: your email is "gmailid@gmail.com", then the user should be "gmailid@gmail.com"
	oSmtp->UserName = ("lolpirt2@gmail.com");
	oSmtp->Password = ("lolpirt.3692");

	cout << ("Start to send email via gmail account ...\r\n") << endl;

	if (oSmtp->SendMail() == 0)
	{
		cout << "email was sent successfully!\r\n" << endl;
	}
	else
	{
		cout << "failed to send email with the following error: %s\r\n" <<
			(const TCHAR*)oSmtp->GetLastErrDescription() << endl;
	}

	if (oSmtp != NULL)
		oSmtp.Release();
	//system("PAUSE"); 
}

void TriviaServer::sendMessageToSocket(SOCKET sock, string msg) {
	send(sock, msg.c_str(), msg.length(), 0);
}

User* TriviaServer::handleSignin(RecievedMessage * msg) {
	if (!_db->isUserExists(msg->getValues().find("username")->second)) {
		sendMessageToSocket(msg->getSock(), "1021");
	}
	else {
		if (_db->isUserAndPasswordMatch(msg->getValues().find("username")->second, msg->getValues().find("password")->second)) {
			// Username and password are matching

			if (!checkIfConnected(msg->getValues().find("username")->second)) {
				// Successful Login
				_connectedUsers.insert(make_pair(msg->getSock(), new User(msg->getValues().find("username")->second, msg->getSock())));
				sendMessageToSocket(msg->getSock(), "1020");
				cout << "successsss" << endl;
			}
			else {
				sendMessageToSocket(msg->getSock(), "1022"); // User is already connected
			}
		}
		else {
			sendMessageToSocket(msg->getSock(), "1021"); // Wrong details
		}
	}

	return nullptr;
}

void TriviaServer::handleSignout(RecievedMessage * msg) {
	User* user = getUserBySocket(msg->getSock());

	if (user) {
		Room* room = getRoomById(user->getRoomId());

		if (room) {
			room->closeRoom(user);
			//room->leaveRoom(user);

			// Remove user from room list
			room->leaveRoom(user);
		}
	}

	// TODO Close game

	safeDeleteUser(msg->getSock()); // Remove users from connected users
}

bool TriviaServer::handleSignup(RecievedMessage * msg) {
	string username = msg->getValues().find("username")->second;
	string password = msg->getValues().find("password")->second;
	string email = msg->getValues().find("email")->second;

	if (_validator.isPasswordValid(password)) {
		if (_validator.isUsernameValid(username)) {
			if (!_db->isUserExists(username)) {
				if (_db->addNewUser(username, password, email)) {
					// Success
					sendMessageToSocket(msg->getSock(), "1040");
				}
				else {
					sendMessageToSocket(msg->getSock(), "1044"); // Unsuccessful insertion
				}
			}
			else {
				sendMessageToSocket(msg->getSock(), "1042"); // User exists
			}
		}
		else {
			sendMessageToSocket(msg->getSock(), "1043"); // Invalid username
		}
	}
	else {
		sendMessageToSocket(msg->getSock(), "1041"); // Invalid password
	}

	return false;
}

void TriviaServer::handleGetRooms(RecievedMessage * msg) {
	string roomsCount = to_string(_roomsList.size());
	roomsCount = string(4 - roomsCount.length(), '0') + roomsCount; // Leading zeros
	string message = "106" + roomsCount;

	for (auto const& room : _roomsList) {
		message += string(4 - to_string(room.first).length(), '0') + to_string(room.first); // Room id
		message += string(2 - to_string(room.second->getName().length()).length(), '0') + to_string(room.second->getName().length()); // Room name length
		message += room.second->getName();
	}

	sendMessageToSocket(msg->getSock(), message);
}

bool TriviaServer::handleJoinRoom(RecievedMessage * msg) {
	int roomId = stoi(msg->getValues().find("room_id")->second);
	Room* room = getRoomById(roomId);
	User* user = getUserBySocket(msg->getSock());

	if (!user->getRoomId()) {
		if (room) {
			if (room->joinRoom(user)) {
				// Success
				string message = "1100";
				message += string(2 - to_string(room->getQuestionsNo()).length(), '0') + to_string(room->getQuestionsNo());
				message += string(2 - to_string(room->getQuestionTime()).length(), '0') + to_string(room->getQuestionTime());
				sendMessageToSocket(msg->getSock(), message);

				// Send 108 message to other room players
				string roomListMessage = "108";
				roomListMessage += to_string(room->getJoinedUsersCount()); // Room users count
				vector<User*> usersList = room->getUsers();

				for (auto curUser : usersList) {
					roomListMessage += string(2 - to_string(curUser->getUsername().length()).length(), '0') + to_string(curUser->getUsername().length());
					roomListMessage += curUser->getUsername();
				}

				for (auto curUser : usersList) {
					if (curUser != user) {
						curUser->send(roomListMessage);
					}
				}
			}
			else {
				sendMessageToSocket(msg->getSock(), "1102"); // Unsuccessful join
			}
		}
		else {
			sendMessageToSocket(msg->getSock(), "1102"); // Room does not exist
		}
	}
	else {
		sendMessageToSocket(msg->getSock(), "1102"); // User is already in room
	}

	return false;
}

void TriviaServer::handleGetUsersInRoom(RecievedMessage * msg) {
	int roomId = stoi(msg->getValues().find("room_id")->second);
	Room* room = getRoomById(roomId);
	User* user = getUserBySocket(msg->getSock());

	if (room) {
		string message = "108";
		message += to_string(room->getJoinedUsersCount()); // Room users count
		vector<User*> users = room->getUsers();

		for (auto curUser : users) {
			message += string(2 - to_string(curUser->getUsername().length()).length(), '0') + to_string(curUser->getUsername().length());
			message += curUser->getUsername();
		}

		sendMessageToSocket(msg->getSock(), message);
	}
	else {
		// Room does not exist
		sendMessageToSocket(msg->getSock(), "1080");
	}
}

bool TriviaServer::handleLeaveRoom(RecievedMessage * msg) {
	User* user = getUserBySocket(msg->getSock());

	if (user) {
		Room* room = getRoomById(user->getRoomId());
		user->leaveRoom();

		if (room) {
			vector<User*> users = room->getUsers();
			users.erase(std::remove(users.begin(), users.end(), user), users.end());
			room->setUsers(users);
			sendMessageToSocket(msg->getSock(), "1120");
		}
		else {
			// Room not found
		}
	}

	return false;
}

bool TriviaServer::handleCreateRoom(RecievedMessage * msg) {
	string name = msg->getValues().find("name")->second;
	int maxUsers = atoi(msg->getValues().find("max_users")->second.c_str());
	int questionTime = atoi(msg->getValues().find("question_time")->second.c_str());
	int questionsNumber = atoi(msg->getValues().find("questions_number")->second.c_str());

	User* user = getUserBySocket(msg->getSock());
	cout << "user: " << user << endl;
	Room* room = new Room(maxUsers, questionTime, questionsNumber, name, _roomIdSequence);
	room->setAdmin(user);
	room->joinRoom(user);

	if (_roomsList.insert(make_pair(_roomIdSequence, room)).second == false) { // Insertion failed
		sendMessageToSocket(msg->getSock(), "1141"); // Fail
		return false;
	}

	_roomIdSequence++;
	sendMessageToSocket(msg->getSock(), "1140"); // Success
	return true;
}

bool TriviaServer::handleCloseRoom(RecievedMessage * msg) {
	User* user = getUserBySocket(msg->getSock());
	Room* room = getRoomById(user->getRoomId());

	if (user && room) {
		if (room->closeRoom(user)) {
			// Remove room from list
			_roomsList.erase(room->getId());
			return true;
		}
		else {
			// Room or user does not exist
			return false;
		}
	}
}

void TriviaServer::handleStartGame(RecievedMessage * msg) {
	User* user = getUserBySocket(msg->getSock());
	cout << "starting gameeeee" << endl;

	if (user) {
		Room* room = getRoomById(user->getRoomId());

		if (room) {
			cout << "user and room found" << endl;

			// Close room
			if (user == room->getAdmin()) {
				Game* game = new Game(room->getUsers(), room->getQuestionsNo(), *_db);

				for (auto user : room->getUsers()) {
					user->setGame(game);
					user->leaveRoom();
				}

				/*if (room->closeRoom(user)) {
				// Remove room from list
				_roomsList.erase(room->getId());
				}*/
				_roomsList.erase(room->getId());

				game->sendFirstQuestion();

				delete room;
			}
		}
	}
}

void TriviaServer::handlePlayerAnswer(RecievedMessage * msg) {
	User* user = getUserBySocket(msg->getSock());

	if (user) {
		Game* game = user->getGame();

		if (game) {
			if (!game->handleAnswerFromUser(user, atoi(msg->getValues().find("answer_number")->second.c_str()), atoi(msg->getValues().find("answer_time")->second.c_str()))) {
				// Game over
				for (auto user : game->getPlayers()) {
					user->leaveGame();
					user->clearGame();
				}

				delete game;
			}
		}
	}
}

void TriviaServer::handleLeaveGame(RecievedMessage * msg) {
	User* user = getUserBySocket(msg->getSock());
	Game* game = user->getGame();

	if (user && game) {
		if (user->leaveGame()) {
			delete game;
		}
	}
}

void TriviaServer::handleGetBestScores(RecievedMessage * msg) {
	sendMessageToSocket(msg->getSock(), _db->getBestScores());
}

void TriviaServer::handleGetPersonalStatus(RecievedMessage * msg) {
	User* user = getUserBySocket(msg->getSock());

	if (user) {
		sendMessageToSocket(msg->getSock(), _db->getPersonalStatus(user->getUsername()));
	}
}

void TriviaServer::handleGetProfilePic(RecievedMessage * msg) {
	string picture_url = _db->getUserProfilePicUrlByUsername(getUserBySocket(msg->getSock())->getUsername());
	string picture_length = to_string(picture_url.length());
	picture_length = string(3 - picture_length.length(), '0') + picture_length;
	cout << string("189" + picture_length + picture_url) << endl;
	sendMessageToSocket(msg->getSock(), string("189" + picture_length + picture_url));
}

void TriviaServer::handleChnageProfilePic(RecievedMessage * msg) {
	cout << "url: " << msg->getValues().find("url")->second << endl;
	_db->updateUserProfilePicByUsername(getUserBySocket(msg->getSock())->getUsername(), msg->getValues().find("url")->second);

}

void TriviaServer::handleGetUserProfilePicByUsername(RecievedMessage * msg) {
	string picture_url = _db->getUserProfilePicUrlByUsername(msg->getValues().find("username")->second);
	string picture_length = to_string(picture_url.length());
	picture_length = string(3 - picture_length.length(), '0') + picture_length;
	string message = string("189" + picture_length + picture_url);
	cout << message << endl;
	sendMessageToSocket(msg->getSock(), string("189" + picture_length + picture_url));
}

void TriviaServer::handleGetCuruserRoomId(RecievedMessage * msg) {
	User* user = getUserBySocket(msg->getSock());
	user->send(string(4 - to_string(user->getRoomId()).length(), '0') + to_string(user->getRoomId()));
}

void TriviaServer::handleGetCuruserDBCol(RecievedMessage * msg) {
	string col = msg->getValues().find("col")->second;
	User* user = getUserBySocket(msg->getSock());
	string ans = _db->getUserColByUsername(user->getUsername(), col);
	string message = string(3 - to_string(ans.length()).length(), '0') + to_string(ans.length()) + ans;
	cout << message << endl;
	sendMessageToSocket(msg->getSock(), message);
}

void TriviaServer::handleUpdateProfileInfo(RecievedMessage * msg) {
	string email = msg->getValues().find("email")->second;
	string password = msg->getValues().find("password")->second;

	User* user = getUserBySocket(msg->getSock());

	if (email != _db->getUserColByUsername(user->getUsername(), "email") && _db->isEmailExists(email)) {
		user->send("1091");
	}
	else {
		_db->updateProfileInfoByUsername(user->getUsername(), email, password);
		user->send("1090");
	}
}

void TriviaServer::handleGetConnectedUsersList(RecievedMessage * msg) {
	string usersCount = to_string(_connectedUsers.size());
	usersCount = string(4 - usersCount.length(), '0') + usersCount; // Leading zeros
	string message = "672" + usersCount;
	for (auto const& user : _connectedUsers) {
		cout << user.second->getUsername() << endl;
		message += string(3 - to_string(user.second->getUsername().length()).length(), '0') + to_string(user.second->getUsername().length()); // Username length
		message += user.second->getUsername(); // Username
	}

	sendMessageToSocket(msg->getSock(), message);
}

void TriviaServer::handleInsertMessageToChat(RecievedMessage * msg) {
	// 619
	cout << "inserting message: " << endl;

	User* user = getUserBySocket(msg->getSock());
	string username = user->getUsername();
	string usernameLength = string(2 - to_string(username.length()).length(), '0') + to_string(username.length());
	string message = msg->getValues().find("message")->second;
	string messageLength = string(3 - to_string(message.length()).length(), '0') + to_string(message.length());

	cout << "new message message: " << "619" + usernameLength + username + messageLength + message << endl;
	//_db->insertMessageToDB();

	// Send 619 message to every user
	for (auto const& curUser : _connectedUsers) {
		// Check if user client supports 619 message
		if (curUser.second->getClientVersion() > 1) {
			curUser.second->send("619" + usernameLength + username + messageLength + message);
		}
	}
}

void TriviaServer::handleSetClientVersion(RecievedMessage * msg) {
	string versionStr = msg->getValues().find("version")->second;
	double version = std::stod(versionStr);
	User* user = getUserBySocket(msg->getSock());
	user->setClientVersion(version);
}

void TriviaServer::handleInsertNewQuestion(RecievedMessage * msg) {
	string question = msg->getValues().find("question")->second;
	string correctAns = msg->getValues().find("correct_ans")->second;
	string secAns = msg->getValues().find("sec_ans")->second;
	string thirdAns = msg->getValues().find("third_ans")->second;
	string fourthAns = msg->getValues().find("fourth_ans")->second;
	string hint = msg->getValues().find("hint")->second;

	_db->insertNewQuestion(question, correctAns, secAns, thirdAns, fourthAns, hint);
}

void TriviaServer::handleGetMessageHint(RecievedMessage * msg) {
	cout << "question to hint: " << msg->getValues().find("question")->second << endl;
	string hint = _db->getQuestionHint(msg->getValues().find("question")->second);
	string hintLength = string(3 - to_string(hint.length()).length(), '0') + to_string(hint.length());
	sendMessageToSocket(msg->getSock(), hintLength + hint);
}

Room * TriviaServer::getRoomById(int id) {
	for (auto const& room : _roomsList) {
		if (room.first == id) {
			return room.second;
		}
	}

	return nullptr;
}

User * TriviaServer::getUserBySocket(SOCKET sock) {
	for (auto user : _connectedUsers) {
		if (user.first == sock) {
			return user.second;
		}
	}

	return nullptr;
}

bool TriviaServer::checkIfConnected(string username) {
	for (auto user : _connectedUsers) {
		if (user.second->getUsername() == username) {
			return true;
		}
	}

	return false;
}

// remove the user from map
void TriviaServer::safeDeleteUser(SOCKET id)
{
	try {
		_connectedUsers.erase(id);
	}
	catch (...) {}

}

void TriviaServer::handleRecievedMessages()
{
	string msgCode = "";
	SOCKET clientSock = 0;
	string userName;

	string messageCodes[] = { "200", "201", "203", "205", "207", "209", "211", "213", "215", "217", "219", "222", "223", "225", "299", "666", "543", "381", "419", "517", "395", "714", "185", "673", "984", "529", "803" };

	while (true) {
		try {
			unique_lock<mutex> lck(_mtxRecievedMessages);

			// Wait for clients to enter the queue.
			if (_messageHandler.empty())
				_msgQueueCondition.wait(lck);

			// in case the queue is empty.
			if (_messageHandler.empty())
				continue;

			RecievedMessage* currMessage = _messageHandler.front();
			_messageHandler.pop();
			lck.unlock();

			// Extract the data from the tuple.
			if (currMessage != NULL) {
				clientSock = currMessage->getSock();
				msgCode = currMessage->getMessageCode();

				if (std::find(std::begin(messageCodes), std::end(messageCodes), msgCode) != std::end(messageCodes)) { // Checks if message code exists in protocol
					if (msgCode == "200") {
						handleSignin(currMessage);
					}

					if (msgCode == "201") {
						handleSignout(currMessage);
					}

					if (msgCode == "203") {
						handleSignup(currMessage);
					}

					if (msgCode == "205") {
						handleGetRooms(currMessage);
					}

					if (msgCode == "209") {
						handleJoinRoom(currMessage);
					}

					if (msgCode == "207") {
						handleGetUsersInRoom(currMessage);
					}

					if (msgCode == "211") {
						handleLeaveRoom(currMessage);
					}

					if (msgCode == "213") {
						handleCreateRoom(currMessage);
					}

					if (msgCode == "215") {
						handleCloseRoom(currMessage);
					}

					if (msgCode == "217") {
						handleStartGame(currMessage);
					}

					if (msgCode == "219") {
						handlePlayerAnswer(currMessage);
					}

					if (msgCode == "222") {
						handleLeaveGame(currMessage);
					}

					if (msgCode == "223") {
						handleGetBestScores(currMessage);
					}

					if (msgCode == "225") {
						handleGetPersonalStatus(currMessage);
					}

					if (msgCode == "299") {
						handleSignout(currMessage);
					}

					if (msgCode == "666") {
						handleForgotPassword(currMessage);
					}

					if (msgCode == "419") {
						handleGetUserProfilePicByUsername(currMessage);
					}

					if (msgCode == "517") {
						handleGetCuruserRoomId(currMessage);
					}

					if (msgCode == "543") {
						handleGetProfilePic(currMessage);
					}

					if (msgCode == "381") {
						handleChnageProfilePic(currMessage);
					}

					if (msgCode == "395") {
						handleGetCuruserDBCol(currMessage);
					}

					if (msgCode == "714") {
						handleUpdateProfileInfo(currMessage);
					}

					if (msgCode == "185") {
						handleGetConnectedUsersList(currMessage);
					}

					if (msgCode == "673") {
						handleInsertMessageToChat(currMessage);
					}

					if (msgCode == "984") {
						handleSetClientVersion(currMessage);
					}

					if (msgCode == "529") {
						handleInsertNewQuestion(currMessage);
					}

					if (msgCode == "803") {
						handleGetMessageHint(currMessage);
					}
				}
				else if (msgCode != "") {
					// Unknown message code
					cout << "unknown message code: " << msgCode << endl;
					msgCode = "";
					//handleSignout(currMessage);
				}
			}

			delete currMessage;
		}
		catch (...) {

		}
	}
}