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

bool User::joinRoom() {
	return true;

	///////////////////
	if (!this->isInRoom) {
		this->isInRoom = true;
		return true;
	}

	return false;
}

/*bool User::joinRoom(Room * newRoom) {
	if (this->_currRoom == NULL) {
		this->_currRoom = newRoom;
		return true;
	}

	return false;
}*/

void User::leaveRoom() {
}

int User::closeRoom() {
	return 0;
}

bool User::leaveGame() {
	return false;
}

void User::clearRoom() {
}
