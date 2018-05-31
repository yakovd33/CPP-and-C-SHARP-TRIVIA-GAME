#include "Question.h"



Question::Question()
{
}


Question::~Question()
{
}

string Question::getQuestion()
{
	return this->_question;
}

string * Question::getAnswers()
{
	return this->_answers;
}

int Question::getCorrectAnswersIndex()
{
	return this->_correctAnswersIndex;
}

int Question::getId()
{
	return this->_id;
}
