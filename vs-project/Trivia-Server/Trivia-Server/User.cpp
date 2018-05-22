#include "User.h"

User::User(string username, SOCKET sock) {
	this->_username = username;
	this->_sock = sock;
}

User::~User()
{
}

void User::send(string message)
{
}

void User::clearGame()
{
}

bool User::createRoom(int troomId, string roomName, int maxUsers, int questionsNo, int questionTime)
{
	return false;
}

void User::leaveRoom()
{
}

int User::closeRoom()
{
	return 0;
}

bool User::leaveGame()
{
	return false;
}

void User::clearRoom()
{
}
