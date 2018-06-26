#pragma once
#ifndef DATABASE_H
#define DATABASE_H

#include <iostream>
#include <vector>
#include <sstream>
#include <string>
#include "sqlite3.h"
#include "User.h"
#include "Question.h"

#define DB_NAME "trivia.db"

using std::cout;
using std::endl;
using std::string;
using std::vector;
using std::stoi;
using std::atoi;

class Game;
class Question;
class DataBase {
public:
	DataBase();
	~DataBase();

	// Functions
	bool isUserExists(string name);
	bool isEmailExists(string email);
	bool isUserAndPasswordMatch(string username, string password);
	bool addNewUser(string username, string password, string email);
	int insertNewGame();
	vector<Question*> initQuestions(int questionsNo);
	bool addAnswerToPlayer(int gameId, string username, int questionId, string answer, bool isCorrect, int answerTime);
	bool updateGameStatus(int gameId);
	string getBestScores();
	string getPersonalStatus(string username);
	void insertUserGameResult(string username, int gameId, int result);
	string getUserProfilePicUrlByUsername(string username);
	void updateUserProfilePicByUsername(string username, string url);
	string getUserColByUsername(string username, string col);
	void updateProfileInfoByUsername(string username, string newEmail, string newPassword);
	void insertMessageToDB(string username, string message);
private:
	sqlite3 *db;
	char *zErrMsg = 0;
	int rc;
};

#endif // DATABASE_H