#pragma once

#include <iostream>
#include <vector>
#include "User.h"

using std::vector;

class Room {
public:
	Room(int maxUsers, int questionTime, int questionsNo, string name, int id);
	~Room();
	bool joinRoom(User* user);
	void leaveRoom(User* user);
	vector<User*>* getUsers();
	string getUsersListMessage();
	int getQuestionsNo();
	int getId();
	string getName();
	int getJoinedUsersCount();
	int getMaxUsers();
	int getQuestionTime();

private:
	vector<User*> _users;
	User* _admin;
	int _maxUsers;
	int _questionTime;
	int _questionsNo;
	string _name;
	int _id;

	string getUsersAsString(vector<User*> users, User* user);
	void sendMessage(string msg);
	void sendMessage(User* user, string msg);
};

