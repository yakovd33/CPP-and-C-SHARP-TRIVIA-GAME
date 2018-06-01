#include "User.h"

User::User(string username, SOCKET sock) {
	this->_username = username;
	this->_sock = sock;
	//this->_currRoom = NULL;
	isInRoom = false;
}

User::~User() {
}

void User::send(string message) {
	::send(_sock, message.c_str(), message.length(), 0);
}

void User::setGame(Game * gm) {
	this->_currGame = gm;
}

void User::clearGame() {
	this->_currGame = NULL;
}

bool User::createRoom(int troomId, string roomName, int maxUsers, int questionsNo, int questionTime) {
	return false;
}

bool User::joinRoom(int roomId) {
	if (!this->isInRoom) {
		this->isInRoom = true;
		this->roomId = roomId;
		return true;
	}

	return false;
}

int User::getRoomId() {
	return this->roomId;
}

void User::leaveRoom() {
	this->isInRoom = false;
	this->roomId = NULL;
}

int User::closeRoom() {
	return 0;
}

bool User::leaveGame() {
	return false;
}

void User::clearRoom() {
}

Game * User::getGame() {
	return this->_currGame;
}

string User::getUsername()
{
	//return "-------";
	return this->_username;
}

SOCKET User::getSock() {
	return this->_sock;
}
