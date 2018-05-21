#pragma once

#include <string>
#include <vector>
#include <Windows.h>

using namespace std;

class RecvMessage
{
public:
	RecvMessage(SOCKET sock, int messageCode);

	RecvMessage(SOCKET sock, int messageCode, vector<string> values);

	SOCKET getSock();
	int getMessageCode();

	vector<string>& getValues();

private:
	SOCKET _sock;
	int _messageCode;
	vector<string> _values;
};

