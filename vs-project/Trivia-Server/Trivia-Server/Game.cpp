#include "Game.h"

Game::Game(const vector<User*>& players, int questions_no, DataBase &db) : _db(db) {
	this->_players = players;
	this->_questions_no = questions_no;
	this->_db = db;
	this->_id = _db.insertNewGame();
}

Game::~Game() {
}

void Game::sendFirstQuestion() {
}

void Game::handleFinishGame() {
}

bool Game::handleNextTurn() {
	return false;
}

bool Game::handleAnswerFromUser(User * user, int answerNo, int time) {
	return false;
}

bool Game::leaveGame(User * user) {
	return false;
}

int Game::getID() {
	return this->_id;
}

bool Game::insertGameToDB() {
	return false;
}

void Game::initQuestionsFromDB() {
}

void Game::sendQuestionToAllUsers() {
	for (auto user : _players) {
		Question* question = _questions.front();
		_questions.pop_back();

		string message = "118";
		message += string(3 - question->getQuestion().length(), '0') + question->getQuestion();
		


		user->send(message);
	}
}
