#include "Game.h"

Game::Game(const vector<User*>& players, int questions_no, DataBase &db) : _db(db) {

	this->_db = db;

	try {
		this->_id = _db.insertNewGame();
		this->_players = players;
		this->_questions_no = questions_no;
		this->_questions = _db.initQuestions(_questions_no);
	} catch (int e) {
		// Insertion didnt succeed
	}
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
		message += string(string(3 - to_string(question->getQuestion().length()).length(), '0') + to_string(question->getQuestion().length()));
		message += question->getQuestion();

		for (int i = 0; i < 4; i++) {
			message += string(3 - to_string(question->getAnswers()[i].length()).length(), '0') + to_string(question->getAnswers()[i].length());
			message += question->getAnswers()[i];
		}

		user->send(message);
	}
}