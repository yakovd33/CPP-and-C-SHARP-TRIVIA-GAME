#include "RecvMessage.h"

RecvMessage::RecvMessage(SOCKET sock, int messageCode)
{
	_sock = sock;
	_messageCode = messageCode;
}

RecvMessage::RecvMessage(SOCKET sock, int messageCode, vector<string> values) : RecvMessage(sock, messageCode)
{
	_values = values;
}

SOCKET RecvMessage::getSock()
{
	return _sock;
}


int RecvMessage::getMessageCode()
{
	return _messageCode;
}


vector<string>& RecvMessage::getValues()
{
	return _values;
}