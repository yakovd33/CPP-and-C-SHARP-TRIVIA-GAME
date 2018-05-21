#include "RecvMessage.h"

RecvMessage::RecvMessage(SOCKET sock, string messageCode)
{
	_sock = sock;
	_messageCode = messageCode;
}

RecvMessage::RecvMessage(SOCKET sock, string messageCode, map<string, string> values) : RecvMessage(sock, messageCode)
{
	_values = values;
}

SOCKET RecvMessage::getSock()
{
	return _sock;
}


string RecvMessage::getMessageCode()
{
	return _messageCode;
}


map<string, string>& RecvMessage::getValues()
{
	return _values;
}