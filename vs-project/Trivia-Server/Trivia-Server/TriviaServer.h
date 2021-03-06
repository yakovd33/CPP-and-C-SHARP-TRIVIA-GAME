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
#include "Functions.h"
#include <tchar.h>

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
	void handleForgotPassword(RecievedMessage*);

	SOCKET _socket;
	DataBase* _db;
	map<int, Room*> _roomsList;
	std::map<SOCKET, User*> _connectedUsers;
	Validator _validator;

	// Queue for messages - Will hold the mssage code and the file data. To add messages use std::ref<const ClientSocket>
	// SOCKET: client socket
	// string: message
	std::queue<RecievedMessage*> _messageHandler;

	std::mutex _mtxRecievedMessages;
	std::condition_variable _msgQueueCondition;

	// Socket functions
	void sendMessageToSocket(SOCKET sock, string msg);

	// Message handlers
	User* handleSignin(RecievedMessage* msg);
	void handleSignout(RecievedMessage* msg);
	bool handleSignup(RecievedMessage * msg);

	void handleGetRooms(RecievedMessage * msg);
	bool handleJoinRoom(RecievedMessage * msg);
	void handleGetUsersInRoom(RecievedMessage * msg);
	bool handleLeaveRoom(RecievedMessage * msg);
	bool handleCreateRoom(RecievedMessage * msg);
	bool handleCloseRoom(RecievedMessage * msg);

	void handleStartGame(RecievedMessage * msg);
	void handlePlayerAnswer(RecievedMessage * msg);
	void handleLeaveGame(RecievedMessage * msg);

	void handleGetBestScores(RecievedMessage * msg);
	void handleGetPersonalStatus(RecievedMessage * msg);
	void handleGetProfilePic(RecievedMessage * msg);
	void handleChnageProfilePic(RecievedMessage * msg);
	void handleGetUserProfilePicByUsername(RecievedMessage * msg);
	void handleGetCuruserRoomId(RecievedMessage * msg);
	void handleGetCuruserDBCol(RecievedMessage * msg);
	void handleUpdateProfileInfo(RecievedMessage * msg);
	void handleGetConnectedUsersList(RecievedMessage * msg);
	void handleInsertMessageToChat(RecievedMessage * msg);
	void handleSetClientVersion(RecievedMessage * msg);
	void handleInsertNewQuestion(RecievedMessage * msg);
	void handleGetMessageHint(RecievedMessage * msg);

	Room* getRoomById(int id);
	User* getUserBySocket(SOCKET sock);

	bool checkIfConnected(string username);
};