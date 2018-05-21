#include "TriviaServer.h"
#include <exception>
#include <iostream>
#include <string>
#include "Header.h"

// using static const instead of macros 
static const unsigned short PORT = 8826;
static const unsigned int IFACE = 0;


TriviaServer::TriviaServer()
{
	// notice that we step out to the global namespace
	// for the resolution of the function socket
	_socket = ::socket(AF_INET, SOCK_STREAM, IPPROTO_TCP);
	if (_socket == INVALID_SOCKET)
		throw std::exception(__FUNCTION__ " - socket");
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
	RecvMessage* currRcvMsg = nullptr;
	try {
		// get the first message code
		string msgCode = "";
		string msg = "";

		//cout << "Message: " << msgCode << endl;

		while (msgCode != "exit") {
			msgCode = Helper::getMessageTypeCode(client_socket);
			msg += msgCode;
			
			currRcvMsg = buildRecieveMessage(client_socket, msg);
			addRecievedMessage(currRcvMsg);
		}

		currRcvMsg = buildRecieveMessage(client_socket, "299");
		addRecievedMessage(currRcvMsg);

	} catch (const std::exception& e) {
		std::cout << "Exception was catch in function clientHandler. socket=" << client_socket << ", what=" << e.what() << std::endl;
		//currRcvMsg = buildRecieveMessage(client_socket, MT_CLIENT_EXIT);
		addRecievedMessage(currRcvMsg);
	}

	closesocket(client_socket);
}

void TriviaServer::addRecievedMessage(RecvMessage* msg)
{
	unique_lock<mutex> lck(_mtxRecievedMessages);

	_messageHandler.push(msg);
	lck.unlock();
	_msgQueueCondition.notify_all();

}

RecvMessage* TriviaServer::buildRecieveMessage(SOCKET client_socket, string msgCode)
{
	RecvMessage* msg = nullptr;
	map<string, string> values;
	/*if (msgCode == MT_CLIENT_LOG_IN) {
		int userSize = ;
		string userName = Helper::getStringPartFromSocket(client_socket, userSize);
		values.push_back(userName);
	} else if (msgCode == MT_CLIENT_FINISH || msgCode == MT_CLIENT_UPDATE) {
		int fileSize = Helper::getIntPartFromSocket(client_socket, 5);
		string fileContent = Helper::getStringPartFromSocket(client_socket, fileSize);
		values.push_back(fileContent);
	}*/

	if (msgCode == "200") {
		// Login
		cout << "Building login message" << endl;
		int usernameSize = atoi(Helper::getStringPartFromSocket(client_socket, 2).c_str());
		string username = Helper::getStringPartFromSocket(client_socket, usernameSize);
		int passwordSize = atoi(Helper::getStringPartFromSocket(client_socket, 2).c_str());
		string password = Helper::getStringPartFromSocket(client_socket, passwordSize);
		values.insert(make_pair("username", username));
		values.insert(make_pair("password", password));
	}

	msg = new RecvMessage(client_socket, msgCode, values);
	return msg;
}


// remove the user from queue
void TriviaServer::safeDeleteUser(SOCKET id)
{
	try {
		for (unsigned int i = 0; i < _clients.size(); i++) {
			if (_clients[i].first == id) {
				TRACE("REMOVED %d, %s from clients list", id, _clients[i].second.c_str());
				_clients.erase(_clients.begin() + i);

			}
		}
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

			RecvMessage* currMessage = _messageHandler.front();
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
			}

			delete currMessage;
		} catch (...) {

		}
	}
}


// get current user name (the writer)
std::string TriviaServer::getCurrentUser()
{

	if (_clients.size() < 1)
		return "";

	return _clients[0].second;
}

// get next user in queue
std::string TriviaServer::getNextUser()
{
	if (_clients.size() < 2)
		return "";

	return _clients[1].second;
}

// get the position in queue of that wanted socket
unsigned int TriviaServer::getSocketPosition(SOCKET id)
{

	for (unsigned int i = 0; i < _clients.size(); i++) {
		if (_clients[i].first == id)
			return i + 1;
	}
	return 0;
}

// get the socket of the current updater
SOCKET TriviaServer::getCurrentThreadSocket()
{
	return _clients[0].first;
}
