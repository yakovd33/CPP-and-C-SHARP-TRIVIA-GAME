#include "TriviaServer.h"
#include <exception>
#include <iostream>
#include <string>
#include "Header.h"

// using static const instead of macros 
static const unsigned short PORT = 8820;
static const unsigned int IFACE = 0;

TriviaServer::TriviaServer()
{
	// notice that we step out to the global namespace
	// for the resolution of the function socket
	_socket = ::socket(AF_INET, SOCK_STREAM, IPPROTO_TCP);
	if (_socket == INVALID_SOCKET)
		throw std::exception(__FUNCTION__ " - socket");

	// Create DataBase Instance
	_db = new DataBase();

	_roomsList.insert(make_pair(1, new Room(5, 20, 20, "room1", 1)));
	_roomsList.insert(make_pair(1235, new Room(5, 20, 20, "room2", 1235)));
}

TriviaServer::~TriviaServer()
{
	TRACE(__FUNCTION__ " closing accepting socket");
	// why is this try necessarily ?
	try {
		// the only use of the destructor should be for freeing 
		// resources that was allocated in the constructor
		::closesocket(_socket);
	} catch (...) {}
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

		while (msgCode != "299") {
			msgCode = Helper::getMessageTypeCode(client_socket);
			
			if (msgCode != "") {
				currRcvMsg = buildRecieveMessage(client_socket, msgCode);
				addRecievedMessage(currRcvMsg);
				msgCode = "";
			}
		}

		currRcvMsg = buildRecieveMessage(client_socket, msgCode);
		addRecievedMessage(currRcvMsg);

	} catch (const std::exception& e) {
		std::cout << "Exception was catch in function clientHandler. socket=" << client_socket << ", what=" << e.what() << std::endl;
		//currRcvMsg = buildRecieveMessage(client_socket, MT_CLIENT_EXIT);
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

	msg = new RecievedMessage(client_socket, msgCode, values);
	return msg;
}

void TriviaServer::sendMessageToSocket(SOCKET sock, string msg) {
	send(sock, msg.c_str(), msg.length(), 0);
}

User* TriviaServer::handleSignin(RecievedMessage * msg) {
	cout << "Handling signin" << endl;

	if (!_db->isUserExists(msg->getValues().find("username")->second)) {
		sendMessageToSocket(msg->getSock(), "1021");
	} else {
		if (_db->isUserAndPasswordMatch(msg->getValues().find("username")->second, msg->getValues().find("password")->second)) {
			// Username and password are matching

			if (_connectedUsers.find(msg->getSock()) == _connectedUsers.end()) {
				_connectedUsers.insert(make_pair(msg->getSock(), new User(msg->getValues().find("username")->second, msg->getSock())));
				sendMessageToSocket(msg->getSock(), "1020");
			} else {
				sendMessageToSocket(msg->getSock(), "1022"); // User is already connected
			}
		} else {
			sendMessageToSocket(msg->getSock(), "1021"); // Wrong details
		}
	}

	return nullptr;
}

bool TriviaServer::handleSignup(RecievedMessage * msg) {
	cout << "Handling signup" << endl;

	string username = msg->getValues().find("username")->second;
	string password = msg->getValues().find("password")->second;
	string email = msg->getValues().find("email")->second;

	if (_validator.isPasswordValid(password)) {
		if (_validator.isUsernameValid(username)) {
			if (!_db->isUserExists(username)) {
				if (_db->addNewUser(username, password, email)) {
					// Success
					cout << "User added." << endl;
					sendMessageToSocket(msg->getSock(), "1040");
				} else {
					cout << "Error.";
					sendMessageToSocket(msg->getSock(), "1044");
				}
			} else {
				cout << "User exists" << endl;
				sendMessageToSocket(msg->getSock(), "1042");
			}
		} else {
			cout << "Invalid username" << endl;
			sendMessageToSocket(msg->getSock(), "1043");
		}
	} else {
		sendMessageToSocket(msg->getSock(), "1041");
		cout << "Invalid password" << endl;
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

	if (room) {
		if (room->joinRoom(user)) {
			// Success
			string message = "1100";
			message += string(2 - to_string(room->getQuestionsNo()).length(), '0') + to_string(room->getQuestionsNo());
			message += string(2 - to_string(room->getQuestionTime()).length(), '0') + to_string(room->getQuestionTime());
			sendMessageToSocket(msg->getSock(), message);
		} else {
			sendMessageToSocket(msg->getSock(), "1102"); // Unsuccessful join
		}
	} else {
		sendMessageToSocket(msg->getSock(), "1102"); // Room does not exist
	}

	return false;
}

void TriviaServer::handleGetUsersInRoom(RecievedMessage * msg) {
	cout << "Getting room users" << endl;
	int roomId = stoi(msg->getValues().find("room_id")->second);
	Room* room = getRoomById(roomId);
	User* user = getUserBySocket(msg->getSock());

	if (room) {
		string message = "108";
		message += to_string(room->getJoinedUsersCount()); // Room users count
		vector<User*>* users = room->getUsers();

		for (auto curUser : *users) {
			message += string(2 - to_string(curUser->getUsername().length()).length(), '0') + to_string(curUser->getUsername().length());
			message += curUser->getUsername();
		}

		sendMessageToSocket(msg->getSock(), message);
	} else {
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
			cout << "current room users: " << room->getJoinedUsersCount() << endl;
			/*for (int i = 0; i < room->getUsers()->size(); i++) {
				cout << "user found" << endl;
				if (room->getUsers()->at(i) == user) {
					room->getUsers()->erase(room->getUsers()->begin() + i);
				}
			}*/

			room->getUsers()->erase(std::remove(room->getUsers()->begin(), room->getUsers()->end(), user), room->getUsers()->end());
			sendMessageToSocket(msg->getSock(), "1120");
		} else {
			cout << "room not found" << endl;
		}
	}
	
	cout << user->getUsername() << " is trying to leave room" << endl;

	return false;
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

// remove the user from map
void TriviaServer::safeDeleteUser(SOCKET id)
{
	try {
		_connectedUsers.erase(id);
	} catch (...) {}

}

void TriviaServer::handleRecievedMessages()
{
	string msgCode = "";
	SOCKET clientSock = 0;
	string userName;
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
			clientSock = currMessage->getSock();
			msgCode = currMessage->getMessageCode();

			if (msgCode == "299") {
				// Client exit
				cout << "Client exit." << endl;
			}

			if (msgCode == "200") {
				cout << "Handling login message" << endl;
				cout << "Username: " << currMessage->getValues().find("username")->second << endl;
				cout << "Password: " << currMessage->getValues().find("password")->second << endl;
				handleSignin(currMessage);
			}

			if (msgCode == "203") {
				cout << "Handling login message" << endl;
				cout << "Username: " << currMessage->getValues().find("username")->second << endl;
				cout << "Password: " << currMessage->getValues().find("password")->second << endl;
				cout << "Email: " << currMessage->getValues().find("email")->second << endl;
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

			delete currMessage;
		} catch (...) {

		}
	}
}