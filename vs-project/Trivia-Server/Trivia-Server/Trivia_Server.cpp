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
	std::thread tr(&MagshDocsServer::handleRecievedMessages, this);
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
