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