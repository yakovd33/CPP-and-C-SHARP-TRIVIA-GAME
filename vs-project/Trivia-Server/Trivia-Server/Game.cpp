#include "Game.h"

Game::Game(const vector<User*>& players, int questions_no, DataBase &db) : _db(db) {
	//this->_players = players;
	this->_questions_no = questions_no;
	this->_db = db;
}

Game::~Game()
{
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
	return 0;
}

bool Game::insertGameToDB() {
	return false;
}

void Game::initQuestionsFromDB() {
}

void Game::sendQuestionToAllUsers() {
}
