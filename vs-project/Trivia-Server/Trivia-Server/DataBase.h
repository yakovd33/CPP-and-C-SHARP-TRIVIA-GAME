#pragma once

#include <iostream>
#include "sqlite3.h"
#include "User.h"

#define DB_NAME "trivia.db"

using std::cout;
using std::endl;
using std::string;

class Game;
class DataBase {
public:
	DataBase();
	~DataBase();

	// Functions
	bool isUserExists(string name);
	bool isUserAndPasswordMatch(string username, string password);
	bool addNewUser(string username, string password, string email);
	int insertNewGame();
private:
	sqlite3 *db;
	char *zErrMsg = 0;
	int rc;
};