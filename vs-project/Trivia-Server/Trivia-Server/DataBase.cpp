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

bool DataBase::isEmailExists(string email) {
	string query = "SELECT * FROM `t_users` WHERE `email` = '" + email + "'";

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

string DataBase::getUserProfilePicUrlByUsername(string username) {
	string query = "SELECT `picture_url` FROM `t_users` WHERE `username` = '" + username + "'";
	sqlite3_stmt *stmt;
	sqlite3_prepare_v2(db, query.c_str(), -1, &stmt, NULL);

	if (sqlite3_step(stmt) == SQLITE_ROW) {
		return std::string(reinterpret_cast<const char*>(sqlite3_column_text(stmt, 0)));
	} else {
		return "https://i.imgur.com/oeKRbhC.png";
	}
}

void DataBase::updateUserProfilePicByUsername(string username, string url) {
	cout << "profile" << url << endl;
	string query = "UPDATE `t_users` SET `picture_url` = '" + url + "' WHERE `username` = '" + username + "'";

	rc = sqlite3_exec(db, query.c_str(), NULL, 0, &zErrMsg);

	if (rc != SQLITE_OK) {
		cout << "error" << endl;
		sqlite3_free(zErrMsg);
	}
}

string DataBase::getUserColByUsername(string username, string col) {
	string query = "SELECT `" + col + "` FROM `t_users` WHERE `username` = '" + username + "'";
	sqlite3_stmt *stmt;
	sqlite3_prepare_v2(db, query.c_str(), -1, &stmt, NULL);

	if (sqlite3_step(stmt) == SQLITE_ROW) {
		return std::string(reinterpret_cast<const char*>(sqlite3_column_text(stmt, 0)));
	} else {
		return "";
	}
}

void DataBase::updateProfileInfoByUsername(string username, string newEmail, string newPassword) {
	string query = "UPDATE `t_users` SET `email` = '" + newEmail + "', `password` = '" + newPassword + "' WHERE `username` = '" + username + "'";

	rc = sqlite3_exec(db, query.c_str(), NULL, 0, &zErrMsg);

	if (rc != SQLITE_OK) {
		sqlite3_free(zErrMsg);
	}
}

void DataBase::insertNewQuestion(string question, string correctAns, string secAns, string thirdAns, string fourthAns, string hint) {
	string query = "INSERT INTO `t_questions` (`question`, `correct_ans`, `ans2`, `ans3`, `ans4`, `hint`) VALUES('" + question + "', '" + correctAns + "', '" + secAns + "', '" + thirdAns + "', '" + fourthAns + "', '" + hint + "')";

	rc = sqlite3_exec(db, query.c_str(), NULL, 0, &zErrMsg);

	if (rc != SQLITE_OK) {
		cout << "insertion error" << zErrMsg << endl;
		sqlite3_free(zErrMsg);
	}
}

string DataBase::getQuestionHint(string question) {
	string query = "SELECT `hint` FROM `t_questions` WHERE `question` = '" + question + "'";
	sqlite3_stmt *stmt;
	sqlite3_prepare_v2(db, query.c_str(), -1, &stmt, NULL);

	if (sqlite3_step(stmt) == SQLITE_ROW) {
		return std::string(reinterpret_cast<const char*>(sqlite3_column_text(stmt, 0)));
	} else {
		return "";
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

string DataBase::getPersonalStatus(string username) {
	string message = "126";

	// Num games
	string numGamesQuery = "SELECT COUNT(*) AS `num_games` FROM `t_games_results` WHERE `username` = '" + username + "'";
	sqlite3_stmt *numGamesStmt;
	sqlite3_prepare_v2(db, numGamesQuery.c_str(), -1, &numGamesStmt, NULL);

	if (sqlite3_step(numGamesStmt) == SQLITE_ROW) {
		string numGames = std::string(reinterpret_cast<const char*>(sqlite3_column_text(numGamesStmt, 0)));

		if (numGames != "0") {
			message += string(string(4 - std::string(reinterpret_cast<const char*>(sqlite3_column_text(numGamesStmt, 0))).length(), '0') + std::string(reinterpret_cast<const char*>(sqlite3_column_text(numGamesStmt, 0))));
		} else {
			message += "0000";
		}
	}

	// Num correct answers
	string numCorrectAnswersQuery = "SELECT COUNT(*) AS `num_correct_answers` FROM `t_players_answers` WHERE `username` = '" + username + "' AND `is_correct`";
	sqlite3_stmt *numCorrectAnswersStmt;
	sqlite3_prepare_v2(db, numCorrectAnswersQuery.c_str(), -1, &numCorrectAnswersStmt, NULL);

	if (sqlite3_step(numCorrectAnswersStmt) == SQLITE_ROW) {
		string numCorrectAnswers = std::string(reinterpret_cast<const char*>(sqlite3_column_text(numCorrectAnswersStmt, 0)));

		if (numCorrectAnswers != "0") {
			message += string(string(6 - std::string(reinterpret_cast<const char*>(sqlite3_column_text(numCorrectAnswersStmt, 0))).length(), '0') + std::string(reinterpret_cast<const char*>(sqlite3_column_text(numCorrectAnswersStmt, 0))));
		} else {
			message += "000000";
		}
	}

	// Num wrong answers
	string numWrongAnswersQuery = "SELECT COUNT(*) AS `num_correct_answers` FROM `t_players_answers` WHERE `username` = '" + username + "' AND NOT `is_correct`";
	sqlite3_stmt *numWrongAnswersStmt;
	sqlite3_prepare_v2(db, numWrongAnswersQuery.c_str(), -1, &numWrongAnswersStmt, NULL);

	if (sqlite3_step(numWrongAnswersStmt) == SQLITE_ROW) {
		string numWrongAnswers = std::string(reinterpret_cast<const char*>(sqlite3_column_text(numCorrectAnswersStmt, 0)));

		if (numWrongAnswers != "0") {
			message += string(string(6 - std::string(reinterpret_cast<const char*>(sqlite3_column_text(numWrongAnswersStmt, 0))).length(), '0') + std::string(reinterpret_cast<const char*>(sqlite3_column_text(numWrongAnswersStmt, 0))));
		} else {
			message += "000000";
		}
	}

	// Average answer time
	string avgAnswerTimeQuery = "SELECT IFNULL(AVG(`answer_time`), 0) AS `average_time` FROM `t_players_answers` WHERE `username` = '" + username + "'";
	sqlite3_stmt *avgAnswerTimeStmt;
	sqlite3_prepare_v2(db, avgAnswerTimeQuery.c_str(), -1, &avgAnswerTimeStmt, NULL);

	if (sqlite3_step(avgAnswerTimeStmt) == SQLITE_ROW) {
		string avg = std::string(reinterpret_cast<const char*>(sqlite3_column_text(avgAnswerTimeStmt, 0)));

		if (avg != "0") {
			vector<string> tokens;
			string token;
			std::istringstream ss(avg);

			// Spliting by '.' to tokens vector
			while (std::getline(ss, token, '.')) {
				tokens.push_back(token.substr(0, 2));
			}

			message += string(2 - tokens.at(0).length(), '0') + tokens.at(0);
			message += string(2 - tokens.at(1).length(), '0') + tokens.at(1);
		} else {
			message += "0";
		}
	}

	//message += "0550";
	return message;
}