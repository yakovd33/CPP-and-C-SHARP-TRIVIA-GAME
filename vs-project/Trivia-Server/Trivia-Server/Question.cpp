#include "Question.h"
#include <time.h>

Question::Question(int id, string question, string correctAnswer, string answer2, string answer3, string answer4) {
	srand(time(NULL));
	this->_id = id;
	this->_question = question;
	this->_correctAnswersIndex = rand() % 4;
	this->_answers[0] = correctAnswer;
	this->_answers[1] = answer2;
	this->_answers[2] = answer3;
	this->_answers[3] = answer4;

	this->_answers[0] = this->_answers[_correctAnswersIndex];
	this->_answers[_correctAnswersIndex] = correctAnswer;
	std::cout << "Answer #1" << this->_answers[0] << std::endl;
	std::cout << "Answer #2" << this->_answers[1] << std::endl;
	std::cout << "Answer #3" << this->_answers[2] << std::endl;
	std::cout << "Answer #4" << this->_answers[3] << std::endl;
	std::cout << "correct" << this->_correctAnswersIndex << std::endl;
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
