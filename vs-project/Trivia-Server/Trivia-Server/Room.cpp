#include "Room.h"

Room::Room(int maxUsers, int questionTime, int questionsNo, string name, int id) {
	this->_maxUsers = maxUsers;
	this->_questionTime = questionTime;
	this->_questionsNo = questionsNo;
	this->_name = name;
	this->_id = id;
}

Room::~Room() {
}

bool Room::joinRoom(User * user) {
	if (this->getJoinedUsersCount() < this->getMaxUsers()) {
		if (user->joinRoom()) {
			// Can join
			this->_users.push_back(user);
			std::cout << "current users count" << this->getJoinedUsersCount() << std::endl;

			// TO:DO
			// Send message to users

			return true;
		}
	}

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

int Room::getJoinedUsersCount() {
	return this->getUsers().size();
}

int Room::getMaxUsers() {
	return this->_maxUsers;
}

int Room::getQuestionTime() {
	return this->_questionTime;
}

string Room::getUsersAsString(vector<User*> users, User * user) {
	return string();
}

void Room::sendMessage(string msg) {
}

void Room::sendMessage(User * user, string msg) {
}