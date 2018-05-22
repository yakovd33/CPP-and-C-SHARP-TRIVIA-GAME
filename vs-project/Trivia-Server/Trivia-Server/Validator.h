#pragma once
#include <iostream>
#include <string>
#include <algorithm>

using std::string;

static class Validator {
public:
	static bool isPasswordValid(string password);
	static bool isUsernameValid(string username);
};