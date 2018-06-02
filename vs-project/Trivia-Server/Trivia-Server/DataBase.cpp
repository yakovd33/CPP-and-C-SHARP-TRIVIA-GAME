#include "DataBase.h"

DataBase::DataBase() {
	rc = sqlite3_open(DB_NAME, &db);

	if (rc) {
		cout << "Can't open database: " << sqlite3_errmsg(db) << endl;
	} else {
		cout << "Opened database successfully" << endl;
	}
}

DataBase::~DataBase() {
	sqlite3_close(db);
}

bool DataBase::isUserExists(string name) {
	string query = "SELECT * FROM `t_users` WHERE `username` = '" + name + "'";

	int rowCount = 0;
	sqlite3_stmt *stmt;
	sqlite3_prepare_v2(db, query.c_str(), -1, &stmt, NULL);
	rc = sqlite3_step(stmt);

	if (rc != SQLITE_DONE && rc != SQLITE_OK) {
		return true;
	}

	return false;
}

bool DataBase::isUserAndPasswordMatch(string username, string password) {
	string query = "SELECT * FROM `t_users` WHERE `username` = '" + username + "' AND `password` = '" + password + "'";

	int rowCount = 0;
	sqlite3_stmt *stmt;
	sqlite3_prepare_v2(db, query.c_str(), -1, &stmt, NULL);
	rc = sqlite3_step(stmt);

	if (rc != SQLITE_DONE && rc != SQLITE_OK) {
		return true;
	}

	return false;
}

bool DataBase::addNewUser(string username, string password, string email) {
	string query = "INSERT INTO `t_users` (`username`, `password`, `email`) VALUES ('" + username + "', '" + password + "', '" + email + "')";

	rc = sqlite3_exec(db, query.c_str(), NULL, 0, &zErrMsg);

	if (rc != SQLITE_OK) {
		sqlite3_free(zErrMsg);
		return false;
	}

	return true;
}

int DataBase::insertNewGame() {
	string query = "INSERT INTO `t_games` (`status`, `start_time`, `end_time`) VALUES ('0', datetime('now'), NULL)";

	rc = sqlite3_exec(db, query.c_str(), NULL, 0, &zErrMsg);

	if (rc != SQLITE_OK) {
		sqlite3_free(zErrMsg);
		return false;
	}

	return sqlite3_last_insert_rowid(db);
}

vector<Question*> DataBase::initQuestions(int questionsNo) {
	vector<Question*> questions;
	string query = "SELECT * FROM `t_questions` WHERE `question_id` IN (SELECT `question_id` FROM `t_questions` ORDER BY RANDOM() LIMIT " + std::to_string(questionsNo) + ")";

	sqlite3_stmt *stmt;
	sqlite3_prepare_v2(db, query.c_str(), -1, &stmt, NULL);

	while (sqlite3_step(stmt) == SQLITE_ROW) {
		const unsigned char* id = sqlite3_column_text(stmt, 0);
		string id_str = std::string(reinterpret_cast<const char*>(id));
		Question* question = new Question(
			stoi(id_str),
			std::string(reinterpret_cast<const char*>(sqlite3_column_text(stmt, 1))),
			std::string(reinterpret_cast<const char*>(sqlite3_column_text(stmt, 2))),
			std::string(reinterpret_cast<const char*>(sqlite3_column_text(stmt, 3))),
			std::string(reinterpret_cast<const char*>(sqlite3_column_text(stmt, 4))),
			std::string(reinterpret_cast<const char*>(sqlite3_column_text(stmt, 5))));

		questions.push_back(question);
	}
	
	return questions;
}

bool DataBase::addAnswerToPlayer(int gameId, string username, int questionId, string answer, bool isCorrect, int answerTime) {
	string query = "INSERT INTO `t_players_answers` (`game_id`, `username`, `question_id`, `player_answer`, `is_correct`, `answer_time`) VALUES(" + to_string(gameId) + ", '" + username + "', " + to_string(questionId) + ", '" + answer + "', " + to_string(isCorrect) + ", " + to_string(answerTime) + ")";
	rc = sqlite3_exec(db, query.c_str(), NULL, 0, &zErrMsg);

	if (rc != SQLITE_OK) {
		sqlite3_free(zErrMsg);
		return false;
	}

	return true;
}

bool DataBase::updateGameStatus(int gameId) {
	string query = "UPDATE `t_games` SET `status` = 1 AND `end_time` = datetime('now') WHERE `game_id` = " + to_string(gameId);

	rc = sqlite3_exec(db, query.c_str(), NULL, 0, &zErrMsg);

	if (rc != SQLITE_OK) {
		sqlite3_free(zErrMsg);
		return false;
	}

	return true;
}

void DataBase::insertUserGameResult(string username, int gameId, int result) {
	string query = "INSERT INTO `t_games_results` (`game_id`, `username`, `result`) VALUES (" + to_string(gameId) + ", '" + username + "', " + to_string(result) + ")";
	cout << "inserting result" << endl;
	rc = sqlite3_exec(db, query.c_str(), NULL, 0, &zErrMsg);

	if (rc != SQLITE_OK) {
		sqlite3_free(zErrMsg);
	}
}

string DataBase::getBestScores() {
	string message = "124";

	string query = "SELECT * FROM `t_games_results` GROUP BY `username` ORDER BY `result` DESC LIMIT 3";

	sqlite3_stmt *stmt;
	sqlite3_prepare_v2(db, query.c_str(), -1, &stmt, NULL);
	
	int users = 0;

	while (sqlite3_step(stmt) == SQLITE_ROW) {
		users++;
		message += string(string(2 - to_string(std::string(reinterpret_cast<const char*>(sqlite3_column_text(stmt, 2))).length()).length(), '0') + to_string(std::string(reinterpret_cast<const char*>(sqlite3_column_text(stmt, 2))).length()));
		message += std::string(reinterpret_cast<const char*>(sqlite3_column_text(stmt, 2)));
		message += string(string(6 - std::string(reinterpret_cast<const char*>(sqlite3_column_text(stmt, 3))).length(), '0') + std::string(reinterpret_cast<const char*>(sqlite3_column_text(stmt, 3))));
	}

	for (int i = 0; i < 3 - users; i++) {
		message += "00000000";
	}

	cout << message << endl;

	return message;
}
