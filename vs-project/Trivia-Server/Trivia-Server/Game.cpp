#include "Game.h"

Game::Game(const vector<User*>& players, int questions_no, DataBase &db) : _db(db) {

	this->_db = db;

	try {
		this->_id = _db.insertNewGame();
		this->_players = players;
		this->_questions_no = questions_no;
		this->_questions = _db.initQuestions(_questions_no);
		_currQuestionIndex = 0;
		_currentTurnAnswers = 0;

		for (auto user : this->_players) {
			this->_results.insert(make_pair(user->getUsername(), 0));
		}
	} catch (int e) {
		// Insertion didnt succeed
	}
}

Game::~Game() {
}

void Game::sendFirstQuestion() {
	sendQuestionToAllUsers();
}

void Game::handleFinishGame() {
	cout << "game finished" << endl;
	string message = "121";
	message += to_string(this->_players.size());
	
	for (auto user : this->_players) {
		message += string(2 - to_string(user->getUsername().length()).length(), '0') + to_string(user->getUsername().length());
		message += user->getUsername();
		message += string(2 - to_string(this->_results.find(user->getUsername())->second).length(), '0') + to_string(this->_results.find(user->getUsername())->second);
	}

	cout << "message: " << message << endl;

	for (auto user : this->_players) {
		try {
			user->clearGame();
			user->leaveGame();
			user->send(message);
		} catch (std::exception e) {

		}
	}

	// Update game status
	_db.updateGameStatus(this->getID());
}

bool Game::handleNextTurn() {
	if (this->_players.size() == 0) {
		handleFinishGame();
	} else {
		cout << "_currentTurnAnswers: " << _currentTurnAnswers << endl;
		cout << "_players.size(): " << _players.size() << endl;
		cout << "_currQuestionIndex: " << _currQuestionIndex << endl;

		_currentTurnAnswers++;
		if (_currentTurnAnswers >= _players.size()) {
			cout << "next turn" << endl;

			// Every player has answered current question
			_currentTurnAnswers = 0;

			cout << "question index: " << _currQuestionIndex << endl;
			if (_currQuestionIndex >= _questions_no) {
				// Last question
				handleFinishGame();
			} else {
				sendQuestionToAllUsers();
			}
		}
	}

	return false;
}

bool Game::handleAnswerFromUser(User * user, int answerNo, int time) {
	int questionIndex = _currQuestionIndex - 1;
	bool isCorrect = false;
	string msg = "1200";

	if (answerNo - 1 == this->_questions.at(questionIndex)->getCorrectAnswersIndex()) {
		// Correct answer
		cout << "correct answer" << endl;
		isCorrect = true;
		this->_results.find(user->getUsername())->second++;
		msg = "1201";
	}

	user->send(msg);

	string answer = "";
	if (answerNo != 5) {
		answer = this->_questions.at(questionIndex)->getAnswers()[this->_questions.at(questionIndex)->getCorrectAnswersIndex()];
	}

	_db.addAnswerToPlayer(this->getID(), user->getUsername(), this->_questions.at(questionIndex)->getId(), answer, isCorrect, time);

	handleNextTurn();
	//_currentTurnAnswers++;

	return true;
}

bool Game::leaveGame(User * user) {
	return false;
}

int Game::getID() {
	return this->_id;
}

vector<User*> Game::getPlayers() {
	return this->_players;
}

bool Game::insertGameToDB() {
	return false;
}

void Game::initQuestionsFromDB() {
}

void Game::sendQuestionToAllUsers() {
	cout << "sending question to all users" << endl;
	Question* question = _questions.at(_currQuestionIndex);
	_currQuestionIndex++;

	for (auto user : _players) {
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