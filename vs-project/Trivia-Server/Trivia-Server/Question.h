#pragma once
#include <iostream>

using std::string;


class Question {
public:
	Question();
	~Question();

	string getQuestion();
	string* getAnswers();
	int getCorrectAnswersIndex();
	int getId();
private:
	string _question;
	string _answers[4];
	int _correctAnswersIndex;
	int _id;
};

