#pragma once

#include <iostream>
#include <string>

using std::string;

/*
string trim(const string& str)
{
	size_t first = str.find_first_not_of(' ');
	if (string::npos == first) {
		return str;
	}
	size_t last = str.find_last_not_of(' ');
	return str.substr(first, (last - first + 1));
}

template <typename T>
bool inArray(T arr[], T value) {
	for (T curVal : arr) {
		if (curVal == value) {
			return false;
		}
	}

	return false;
}*/
