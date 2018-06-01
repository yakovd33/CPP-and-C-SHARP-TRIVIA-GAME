#pragma once
#ifndef GAME_H
#define GAME_H

#include <iostream>
#include <vector>
#include <string>
#include <map>
#include <exception>
#include "User.h"
#include "Question.h"
#include "DataBase.h"

using std::vector;
using std::map;
using std::to_string;

class DataBase;
class User;
class Question;
class Game {
public:
	Game(const vector<User*> &players, int questions_no, DataBase &db);
	~Game();
	void sendFirstQuestion();
	void handleFinishGame();
	bool handleNextTurn();
	bool handleAnswerFromUser(User* user, int answerNo, int time);
	bool leaveGame(User* user);
	int getID();

	bool insertGameToDB();
	void initQuestionsFromDB();
	void sendQuestionToAllUsers();

private:
	vector<Question*> _questions;
	vector<User*> _players;
	int _questions_no;
	int _currQuestionIndex;
	DataBase& _db;
	map<std::string, int> _results;
	int _currentTurnAnswers;
	int _id;
};

#endif