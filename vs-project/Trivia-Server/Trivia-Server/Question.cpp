#include "Question.h"

Question::Question(int id, string question, string correctAnswer, string answer2, string answer3, string answer4) {
	this->_id = id;
	this->_question = question;
	this->_correctAnswersIndex = 0;
	this->_answers[0] = correctAnswer;
	this->_answers[1] = answer2;
	this->_answers[2] = answer3;
	this->_answers[3] = answer4;
}

Question::~Question() {
}

string Question::getQuestion() {
	return this->_question;
}

string * Question::getAnswers() {
	return this->_answers;
}

int Question::getCorrectAnswersIndex() {
	return this->_correctAnswersIndex;
}

int Question::getId() {
	return this->_id;
}
