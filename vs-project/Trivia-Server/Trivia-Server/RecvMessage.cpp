#include "RecievedMessage.h"

RecievedMessage::RecievedMessage(SOCKET sock, string messageCode)
{
	_sock = sock;
	_messageCode = messageCode;
}

RecievedMessage::RecievedMessage(SOCKET sock, string messageCode, map<string, string> values) : RecievedMessage(sock, messageCode)
{
	_values = values;
}

SOCKET RecievedMessage::getSock()
{
	return _sock;
}


string RecievedMessage::getMessageCode()
{
	return _messageCode;
}


map<string, string> RecievedMessage::getValues()
{
	return _values;
}