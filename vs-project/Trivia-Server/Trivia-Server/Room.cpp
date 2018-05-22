#include "Room.h"

Room::Room(int maxUsers, int questionTime, int questionsNo, string name, int id) {
	this->_maxUsers = maxUsers;
	this->_questionTime = questionTime;
	this->_name = name;
	this->_id = id;
}

Room::~Room() {
}

bool Room::joinRoom(User * user) {
	return false;
}

void Room::leaveRoom(User * user) {
}

vector<User*> Room::getUsers() {
	return this->_users;
}

string Room::getUsersListMessage() {
	return string();
}

int Room::getQuestionsNo() {
	return this->_questionsNo;
}

int Room::getId() {
	return this->_id;
}

string Room::getName() {
	return this->_name;
}

string Room::getUsersAsString(vector<User*> users, User * user) {
	return string();
}

void Room::sendMessage(string msg) {
}

void Room::sendMessage(User * user, string msg) {
}