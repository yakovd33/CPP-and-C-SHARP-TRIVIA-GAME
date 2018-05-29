#pragma once
#include "Question.h"
#include <vector>
#include <map>
#include "User.h"
#include "DataBase.h"

using std::vector;
using std::map;

class Game {
public:
	Game(const vector<User*>& players, int questions_no, DataBase& db);
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
	//vector<User*> _players;
	int _questions_no;
	int _currQuestionIndex;
	DataBase& _db;
	map<string, int> _results;
	int _currentTurnAnswers;
};