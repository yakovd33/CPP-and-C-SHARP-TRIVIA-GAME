


// using static const instead of macros 
static const unsigned short PORT = 8826;
static const unsigned int IFACE = 0;

Trivia_Server::Trivia_Server()
{
}


Trivia_Server::~Trivia_Server()
{
}


void Trivia_Server::server()
{
	bindAndListen();

	// create new thread for handling message
	std::thread tr(&Trivia_Server::handleRecievedMessages, this);
	tr.detach();

	while (true)
	{
		// the main thread is only accepting clients 
		// and add then to the list of handlers
		TRACE("accepting client...");
		acceptClient();
	}
}



// listen to connecting requests from clients
// accept them, and create thread for each client
void Trivia_Server::bindAndListen()
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


void Trivia_Server::acceptClient()
{
	SOCKET client_socket = accept(_socket, NULL, NULL);
	if (client_socket == INVALID_SOCKET)
		throw std::exception(__FUNCTION__);

	TRACE("Client accepted !");
	// create new thread for client	and detach from it
	std::thread tr(&MagshDocsServer::clientHandler, this, client_socket);
	tr.detach();
}


void Trivia_Server::clientHandler(SOCKET client_socket)
{
	RecvMessage* currRcvMsg = nullptr;
	try
	{
		// get the first message code
		int msgCode = Helper::getMessageTypeCode(client_socket);

		while (msgCode != 0 && (msgCode == MT_CLIENT_LOG_IN || msgCode == MT_CLIENT_UPDATE || msgCode == MT_CLIENT_FINISH))
		{
			currRcvMsg = buildRecieveMessage(client_socket, msgCode);
			addRecievedMessage(currRcvMsg);

			msgCode = Helper::getMessageTypeCode(client_socket);
		}

		currRcvMsg = buildRecieveMessage(client_socket, MT_CLIENT_EXIT);
		addRecievedMessage(currRcvMsg);

	}
	catch (const std::exception& e)
	{
		std::cout << "Exception was catch in function clientHandler. socket=" << client_socket << ", what=" << e.what() << std::endl;
		currRcvMsg = buildRecieveMessage(client_socket, MT_CLIENT_EXIT);
		addRecievedMessage(currRcvMsg);
	}
	closesocket(client_socket);
}


void Trivia_Server::handleRecievedMessages()
{
	int msgCode = 0;
	SOCKET clientSock = 0;
	string userName;
	while (true)
	{
		try
		{
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

			if (msgCode == MT_CLIENT_LOG_IN)
			{
				userName = currMessage->getValues()[0];
				_clients.push_back(make_pair(clientSock, userName));

				TRACE("ADDED new client %d, %s to clients list", clientSock, userName.c_str());

				std::string docData = _doc.read();
				Helper::sendUpdateMessageToClient(clientSock, docData, getCurrentUser(), getNextUser(), getSocketPosition(clientSock));
			}

			else if (msgCode == MT_CLIENT_UPDATE)
			{
				TRACE("Recieved update message from current client");
				string fileData = currMessage->getValues()[0];
				_doc.write(fileData);
				sendUpdateMessageToAllClients(fileData);
			}
			else if (msgCode == MT_CLIENT_FINISH)
			{
				TRACE("Recieved finish message from current client");
				// move to end of the queue
				_clients.push_back(_clients.front());
				_clients.pop_front();

				string fileData = currMessage->getValues()[0];
				_doc.write(fileData);

				sendUpdateMessageToAllClients(fileData);
			}
			else if (msgCode == MT_CLIENT_EXIT)
			{
				TRACE("Recieved exit message from client");
				safeDeleteUser(clientSock);

				std::string docData = _doc.read();
				sendUpdateMessageToAllClients(docData);
			}

			delete currMessage;
		}
		catch (...)
		{

		}
	}
}


void Trivia_Server::addRecievedMessage(RecvMessage* msg)
{
	unique_lock<mutex> lck(_mtxRecievedMessages);

	_messageHandler.push(msg);
	lck.unlock();
	_msgQueueCondition.notify_all();

}

RecvMessage* Trivia_Server::buildRecieveMessage(SOCKET client_socket, int msgCode)
{
	RecvMessage* msg = nullptr;
	vector<string> values;
	if (msgCode == MT_CLIENT_LOG_IN)
	{
		int userSize = Helper::getIntPartFromSocket(client_socket, 2);
		string userName = Helper::getStringPartFromSocket(client_socket, userSize);
		values.push_back(userName);
	}
	else if (msgCode == MT_CLIENT_FINISH || msgCode == MT_CLIENT_UPDATE)
	{
		int fileSize = Helper::getIntPartFromSocket(client_socket, 5);
		string fileContent = Helper::getStringPartFromSocket(client_socket, fileSize);
		values.push_back(fileContent);

	}

	msg = new RecvMessage(client_socket, msgCode, values);
	return msg;

}


// remove the user from queue
void Trivia_Server::safeDeleteUser(SOCKET id)
{
	try
	{
		for (unsigned int i = 0; i < _clients.size(); i++)
		{
			if (_clients[i].first == id)
			{
				TRACE("REMOVED %d, %s from clients list", id, _clients[i].second.c_str());
				_clients.erase(_clients.begin() + i);

			}
		}
	}
	catch (...) {}

}

void Trivia_Server::handleRecievedMessages()
{
	int msgCode = 0;
	SOCKET clientSock = 0;
	string userName;
	while (true)
	{
		try
		{
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

			if (msgCode == MT_CLIENT_LOG_IN)
			{
				userName = currMessage->getValues()[0];
				_clients.push_back(make_pair(clientSock, userName));

				TRACE("ADDED new client %d, %s to clients list", clientSock, userName.c_str());

				std::string docData = _doc.read();
				Helper::sendUpdateMessageToClient(clientSock, docData, getCurrentUser(), getNextUser(), getSocketPosition(clientSock));
			}

			else if (msgCode == MT_CLIENT_UPDATE)
			{
				TRACE("Recieved update message from current client");
				string fileData = currMessage->getValues()[0];
				_doc.write(fileData);
				sendUpdateMessageToAllClients(fileData);
			}
			else if (msgCode == MT_CLIENT_FINISH)
			{
				TRACE("Recieved finish message from current client");
				// move to end of the queue
				_clients.push_back(_clients.front());
				_clients.pop_front();

				string fileData = currMessage->getValues()[0];
				_doc.write(fileData);

				sendUpdateMessageToAllClients(fileData);
			}
			else if (msgCode == MT_CLIENT_EXIT)
			{
				TRACE("Recieved exit message from client");
				safeDeleteUser(clientSock);

				std::string docData = _doc.read();
				sendUpdateMessageToAllClients(docData);
			}

			delete currMessage;
		}
		catch (...)
		{

		}
	}
}


void Trivia_Server::sendUpdateMessageToAllClients(string fileContent)
{
	SOCKET id;

	TRACE("Send update message to all clients");

	for (unsigned int i = 0; i < _clients.size(); i++)
	{
		try
		{
			id = _clients[i].first;
			Helper::sendUpdateMessageToClient(id, fileContent, getCurrentUser(), getNextUser(), getSocketPosition(id));
		}
		catch (...)
		{

		}
	}
}


// get current user name (the writer)
std::string Trivia_Server::getCurrentUser()
{

	if (_clients.size() < 1)
		return "";

	return _clients[0].second;
}

// get next user in queue
std::string Trivia_Server::getNextUser()
{
	if (_clients.size() < 2)
		return "";

	return _clients[1].second;
}

// get the position in queue of that wanted socket
unsigned int Trivia_Server::getSocketPosition(SOCKET id)
{

	for (unsigned int i = 0; i < _clients.size(); i++)
	{
		if (_clients[i].first == id)
			return i + 1;
	}
	return 0;
}

// get the socket of the current updater
SOCKET Trivia_Server::getCurrentThreadSocket()
{
	return _clients[0].first;
}
