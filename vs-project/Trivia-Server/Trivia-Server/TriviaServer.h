#pragma once

#include <thread>
#include <deque>
#include <queue>
#include <map>
#include <mutex>
#include <condition_variable>
#include <WinSock2.h>
#include <Windows.h>
#include "RecvMessage.h"
#include "Helper.h"

// Q: why do we need this class ?
// A: this is the main class which holds all the resources,
// accept new clients and handle them.
class TriviaServer {
public:
	TriviaServer();
	~TriviaServer();
	void serve();


private:

	void bindAndListen();
	void acceptClient();
	void clientHandler(SOCKET client_socket);
	void safeDeleteUser(SOCKET id);

	void handleRecievedMessages();
	std::string getCurrentUser();
	std::string getNextUser();
	unsigned int getSocketPosition(SOCKET id);
	SOCKET getCurrentThreadSocket();
	void addRecievedMessage(RecvMessage*);
	RecvMessage* buildRecieveMessage(SOCKET userSock, string msgCode);

	SOCKET _socket;
	
	// Queue for all clients. This way we will know who's the current writer.
	// SOCKET: client socket
	// string: userName
	std::deque<pair<SOCKET, string>> _clients;


	// Queue for messages - Will hold the mssage code and the file data. To add messages use std::ref<const ClientSocket>
	// SOCKET: client socket
	// string: message
	std::queue<RecvMessage*> _messageHandler;

	std::mutex _mtxRecievedMessages;
	std::condition_variable _msgQueueCondition;

	// Wake up when an action has been finished.
	std::condition_variable _edited;

};

