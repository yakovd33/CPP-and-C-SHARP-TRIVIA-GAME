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
}

void User::clearGame() {
}

bool User::createRoom(int troomId, string roomName, int maxUsers, int questionsNo, int questionTime) {
	return false;
}

bool User::joinRoom(int roomId) {
	if (!this->isInRoom) {
		this->isInRoom = true;
		std::cout << "roooom id: " << roomId << std::endl;
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

string User::getUsername()
{
	//return "-------";
	return this->_username;
}
