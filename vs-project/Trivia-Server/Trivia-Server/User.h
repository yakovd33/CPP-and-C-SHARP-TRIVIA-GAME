#pragma once
#include <iostream>
#include <WinSock2.h>
//#include "Room.h";

using std::string;

class User {
public:
	User(string username, SOCKET sock);
	~User();
	void send(string message);
	//void setGame(Game* gm);
	void clearGame();
	bool createRoom(int troomId, string roomName, int maxUsers, int questionsNo, int questionTime);
	bool User::joinRoom();
	//bool joinRoom(Room* newRoom);
	void leaveRoom();
	int closeRoom();
	bool leaveGame();
	void clearRoom();
private:
	string _username;
	bool isInRoom;
	//Room* _currRoom;
	//Game* _cirrGame;
	SOCKET _sock;
};