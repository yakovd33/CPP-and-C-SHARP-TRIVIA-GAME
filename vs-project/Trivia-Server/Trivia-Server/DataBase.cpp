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
	sqlite3_prepare_v2(db, query.c_str(), -1, &stmt, NULL);//replcae callback's shiltot.
	rc = sqlite3_step(stmt);//read line in answear

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

bool DataBase::addNewUser(string username, string password, string email)
{
	string query = "INSERT INTO `t_users` (`username`, `password`, `email`) VALUES ('" + username + "', '" + password + "', '" + email + "')";

	rc = sqlite3_exec(db, query.c_str(), NULL, 0, &zErrMsg);

	if (rc != SQLITE_OK) {
		return false;
		sqlite3_free(zErrMsg);
	}

	return true;
}
