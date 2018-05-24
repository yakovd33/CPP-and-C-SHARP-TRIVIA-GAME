#pragma once

#include <thread>
#include <deque>
#include <queue>
#include <map>
#include <mutex>
#include <condition_variable>
#include <WinSock2.h>
#include <Windows.h>
#include <algorithm>
#include "RecievedMessage.h"
#include "Helper.h"
#include "DataBase.h"
#include "User.h"
#include "Room.h";
#include "Validator.h"

// Q: why do we need this class ?
// A: this is the main class which holds all the resources,
// accept new clients and handle them.
class TriviaServer {
public:
	TriviaServer();
	~TriviaServer();
	void serve();

private:
	static int _roomIdSequence;

	void bindAndListen();
	void acceptClient();
	void clientHandler(SOCKET client_socket);
	void safeDeleteUser(SOCKET id);

	void handleRecievedMessages();
	std::string getCurrentUser();
	std::string getNextUser();
	unsigned int getSocketPosition(SOCKET id);
	SOCKET getCurrentThreadSocket();
	void addRecievedMessage(RecievedMessage*);
	RecievedMessage* buildRecieveMessage(SOCKET userSock, string msgCode);

	SOCKET _socket;
	DataBase* _db;
	map<int, Room*> _roomsList;
	std::map<SOCKET, User*> _connectedUsers;
	//std::map<int, Room*> _roomsList;
	Validator _validator;

	// Queue for all clients. This way we will know who's the current writer.
	// SOCKET: client socket
	// string: userName
	std::deque<pair<SOCKET, string>> _clients;

	// Queue for messages - Will hold the mssage code and the file data. To add messages use std::ref<const ClientSocket>
	// SOCKET: client socket
	// string: message
	std::queue<RecievedMessage*> _messageHandler;

	std::mutex _mtxRecievedMessages;
	std::condition_variable _msgQueueCondition;

	// Wake up when an action has been finished.
	std::condition_variable _edited;

	// Socket functions
	void sendMessageToSocket(SOCKET sock, string msg);

	// Message handlers
	User* handleSignin(RecievedMessage* msg);
	bool TriviaServer::handleSignup(RecievedMessage * msg);
	void handleGetRooms(RecievedMessage * msg);
	bool handleJoinRoom(RecievedMessage * msg);
	void handleGetUsersInRoom(RecievedMessage * msg);
	bool handleLeaveRoom(RecievedMessage * msg);
	bool handleCreateRoom(RecievedMessage * msg);

	Room* getRoomById(int id);
	User* getUserBySocket(SOCKET sock);
};