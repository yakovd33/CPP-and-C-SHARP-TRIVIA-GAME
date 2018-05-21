#pragma once
class Trivia_Server
{
public:
	Trivia_Server();
	~Trivia_Server();
	void server();
	void bindAndListen();
	
	void acceptClient();
};

