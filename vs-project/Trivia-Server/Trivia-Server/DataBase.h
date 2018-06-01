#pragma once
#ifndef DATABASE_H
#define DATABASE_H

#include <iostream>
#include <vector>
#include "sqlite3.h"
#include "User.h"
#include "Question.h"

#define DB_NAME "trivia.db"

using std::cout;
using std::endl;
using std::string;
using std::vector;
using std::stoi;

class Game;
class Question;
class DataBase {
public:
	DataBase();
	~DataBase();

	// Functions
	bool isUserExists(string name);
	bool isUserAndPasswordMatch(string username, string password);
	bool addNewUser(string username, string password, string email);
	int insertNewGame();
	vector<Question*> initQuestions(int questionsNo);
private:
	sqlite3 *db;
	char *zErrMsg = 0;
	int rc;
};

#endif // DATABASE_H