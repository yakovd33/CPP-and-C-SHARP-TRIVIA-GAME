#pragma once

#include <string>
#include <map>
#include <Windows.h>

using namespace std;

class RecievedMessage
{
public:
	RecievedMessage(SOCKET sock, string messageCode);

	RecievedMessage(SOCKET sock, string messageCode, map<string, string> values);

	SOCKET getSock();
	string getMessageCode();

	map<string, string> getValues();

private:
	SOCKET _sock;
	string _messageCode;
	map<string, string> _values;
};

