using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuizController : MonoBehaviour     //Doing quiz jobs and controls and provides connection between the quizview and quiz model.
{
    [SerializeField] private QuizView _quizView;
    [SerializeField] private UIManager _uiManager;

    private List<Question> _questions;
    private QuizAPI _quizAPI;
    private int _currentQuestionIndex = 0;
    private Coroutine _timerCoroutine;
    private int _correctAnswerIndex;
    private int _score = 0;
    private bool _isAnswerable = true;

    private const int TimePerQuestion = 20;
    private const int CorrectAnswerScore = 10;
    private const int WrongAnswerScore = -5;
    private const int NoAnswerScore = -3;


    private void Awake()
    {
        _quizAPI = GetComponent<QuizAPI>();
    }

    public void SetQuestions()     //Sets the quiz question.
    {
        var quiz = _quizAPI.ReadQuestionsData();
        _questions = quiz.questions;
        _currentQuestionIndex = -1;
    }

    public void FinishQuestions()      //Finishes the questions and call UIManager end game method.
    {
        StopAllCoroutines();
        _quizView.CloseQuizPanel();
        _uiManager.EndGame(_score);
    }

    public void ShowNextQuestion()     //Shows the next question.
    {
        _currentQuestionIndex++;
        if (_currentQuestionIndex < _questions.Count)
        {
            _isAnswerable = true;
            _quizView.EnableButtons();
            var currentQuestion = _questions[_currentQuestionIndex];
            _correctAnswerIndex = char.ToUpper(currentQuestion.answer[0]) - 65;        //Integer index of a character in the alphabet (A = 0 , B = 1, C = 2, D = 3)
            _quizView.ResetColorChoices();
            _quizView.ClearQuestion();
            _quizView.SetQuestionText(currentQuestion.question);
            _quizView.SetQuestionChoices(currentQuestion, HandleAnswerSelected);
            _quizView.UpdateTimerText(TimePerQuestion);
            _timerCoroutine = StartCoroutine(WaitForAnswer());
        }
        else
        {
            FinishQuestions();
        }
    }

    private IEnumerator WaitForAnswer()     //Wait coroutine for answer.
    {
        for(int i = 0; i < TimePerQuestion; i++)        //Updates quiz view timer one time of each second.
        {
            _quizView.UpdateTimerText(TimePerQuestion - i);
            yield return new WaitForSeconds(1);
        }
        HandleAnswerSelected(-1);
    }

    private void HandleAnswerSelected(int answerIndex)      //Handles the player question answer. 
    {
        if(_isAnswerable)
        {
            _isAnswerable = false;
            _quizView.DisableButtons();
            StopCoroutine(_timerCoroutine);
            int answerScore;
            if (answerIndex == -1)
            {
                _quizView.UpdateTimerText(0);
                answerScore = NoAnswerScore;
            }
            else
            {
                answerScore = _correctAnswerIndex == answerIndex ? CorrectAnswerScore : WrongAnswerScore;
            }
            _score += answerScore;
            _score = _score < 0 ? 0 : _score;
            _quizView.UpdateScoreText(_score, answerScore >= 0, 0.5f);
            _quizView.SetColorChoices(_correctAnswerIndex, answerIndex);
            StartCoroutine(BeforeNextQuestionEnum());
        }
    }

    private IEnumerator BeforeNextQuestionEnum()        //Wait coroutine between the questions.
    {                
        yield return new WaitForSeconds(2f);
        ShowNextQuestion();
    }
}