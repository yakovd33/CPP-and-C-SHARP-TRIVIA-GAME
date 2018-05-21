#pragma once

#pragma once
#include "Trivia_Server.h"
#include <exception>
#include <iostream>
#include <string>
#include <thread>
#include <WinSock2.h>
#include <Windows.h>
#include <fstream>
#include <iomanip>
#include <sstream>
#include <deque>
#include <queue>
#include <map>
#include <mutex>
#include <condition_variable>

#include "RecvMessage.h"

class Trivia_Server
{
public:
	Trivia_Server();
	~Trivia_Server();
	void server();
	void bindAndListen();
	
	void acceptClient();
	void clientHandler(SOCKET client_socket);
	void handleRecievedMessages();

	void safeDeleteUser(SOCKET id);

	void handleRecievedMessages();
	std::string getCurrentUser();
	std::string getNextUser();
	unsigned int getSocketPosition(SOCKET id);
	SOCKET getCurrentThreadSocket();
	void addRecievedMessage(RecvMessage*);
	RecvMessage* buildRecieveMessage(SOCKET userSock, int msgCode);
	void sendUpdateMessageToAllClients(string fileContent);

	SOCKET _socket;
	MagshDocument _doc;

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

