#pragma once
#ifndef USER_H
#define USER_H

#include <iostream>
#include <WinSock2.h>
#include <string>
#include "Game.h"
//#include "Room.h";

using std::string;

class Game;
class DataBase;
class Question;
class User {
public:
	User(string username, SOCKET sock);
	~User();
	void send(string message);
	void setGame(Game* gm);
	void clearGame();
	bool createRoom(int troomId, string roomName, int maxUsers, int questionsNo, int questionTime);
	bool User::joinRoom(int roomId);
	int getRoomId();
	//bool joinRoom(Room* newRoom);
	void leaveRoom();
	int closeRoom();
	bool leaveGame();
	void clearRoom();
	Game* getGame();
	string getUsername();
	double getClientVersion();
	void setClientVersion(double version);
	SOCKET getSock();
private:
	string _username;
	bool isInRoom;
	int roomId;
	double clientVersion;
	//Room* _currRoom;
	Game* _currGame;
	SOCKET _sock;
};

#endif