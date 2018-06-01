#pragma once
#ifndef QUESTION_H
#define QUESTION_H

#include <iostream>
#include <random>

using std::string;

class Game;
class DataBase;
class Question {
public:
	Question(int id, string question, string correctAnswer, string answer2, string answer3, string answer4);
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


#endif