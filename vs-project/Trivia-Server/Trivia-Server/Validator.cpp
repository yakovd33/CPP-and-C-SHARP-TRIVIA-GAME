#include "Validator.h"

bool Validator::isPasswordValid(string password)
{

	return (password.length() >= 4 && password.find(' ') == std::string::npos && std::any_of(password.begin(), password.end(), isdigit) && std::any_of(password.begin(), password.end(), islower) && std::any_of(password.begin(), password.end(), isupper));
}

bool Validator::isUsernameValid(string username)
{
	return (username.length() >= 1 && username.find(' ') == std::string::npos && isalpha(username[0]));
}
