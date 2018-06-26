#include "Question.h"
#include <time.h>

Question::Question(int id, string question, string correctAnswer, string answer2, string answer3, string answer4) {
	std::array<string, 4> answers { correctAnswer, answer2, answer3, answer4 };
	this->_id = id;
	this->_question = question;

	// Randomize the answers
	unsigned seed = std::chrono::system_clock::now().time_since_epoch().count();
	shuffle(answers.begin(), answers.end(), std::default_random_engine(seed));
	this->_answers[0] = answers[0];
	this->_answers[1] = answers[1];
	this->_answers[2] = answers[2];
	this->_answers[3] = answers[3];
	
	for (int i = 0; i < 4; i++) {
		if (answers[i] == correctAnswer) {
			this->_correctAnswersIndex = i;
		}
	}
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
