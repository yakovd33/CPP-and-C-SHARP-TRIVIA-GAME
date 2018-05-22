#pragma once
#include <iostream>
#include <WinSock2.h>

using std::string;

class User {
public:
	User(string username, SOCKET sock);
	~User();
	void send(string message);
	//void setGame(Game* gm);
	void clearGame();
	bool createRoom(int troomId, string roomName, int maxUsers, int questionsNo, int questionTime);
	//bool joinRoom(Room* newRoom);
	void leaveRoom();
	int closeRoom();
	bool leaveGame();
	void clearRoom();
private:
	string _username;
	//Room* _currRoom;
	//Game* _cirrGame;
	SOCKET _sock;
};