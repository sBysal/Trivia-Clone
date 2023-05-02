using TMPro;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;
using UnityEngine.UI;

public class QuizView : MonoBehaviour       //Controls quiz panel. 
{
    [SerializeField] private TMP_Text _timerText;
    [SerializeField] private TMP_Text _scoreHeadlineText;
    [SerializeField] private TMP_Text _scoreText;
    [SerializeField] private QuestionItem _questionItem;


    public void SetQuestionText(string text)        //Sets text of the question.
    {
        _questionItem.questionText.text = text;
    }

    public void SetQuestionChoices(Question data, UnityAction<int> onAnswerSelected)    //Sets question choices.
    {
        for (int i = 0; i < _questionItem.choiceButtons.Length; i++)
        {
            if (i < data.choices.Count)
            {
                TMP_Text answerButtonText = _questionItem.choiceButtons[i].GetComponentInChildren<TMP_Text>();
                answerButtonText.text = data.choices[i];
                int choiceIndex = i;
                _questionItem.choiceButtons[i].onClick.RemoveAllListeners();
                _questionItem.choiceButtons[i].onClick.AddListener(() => onAnswerSelected?.Invoke(choiceIndex));     //Cant use i variable instead of choiceIndex because i changes until the method invokes.
                _questionItem.choiceButtons[i].gameObject.SetActive(true);
            }
            else
            {
                _questionItem.choiceButtons[i].gameObject.SetActive(false);
            }
        }
    }

    public void UpdateTimerText(int seconds)    //Updates timer text.
    {
        _timerText.text = $"{seconds}";
        DOTween.Sequence().Append(_timerText.transform.DOScale(1.25f, 0.25f)).Append(_timerText.transform.DOScale(1f, 0.25f));
    }

    public void UpdateScoreText(int score, bool isIncrease, float animationTime)    //Updates score text.
    {
        Color startColor = _scoreText.color;
        _scoreText.text = $"{score}";
        if(isIncrease )
        {
            DOTween.Sequence().Append(_scoreText.transform.DOScale(1.25f, animationTime / 2)).Append(_scoreText.transform.DOScale(1f, animationTime / 2));
            DOTween.Sequence().Append(_scoreText.DOColor(Color.green, animationTime / 2)).Append(_scoreText.DOColor(startColor, animationTime / 2));
            DOTween.Sequence().Append(_scoreHeadlineText.DOColor(Color.green, animationTime / 2)).Append(_scoreHeadlineText.DOColor(startColor, animationTime / 2));
        }
        else
        {
            DOTween.Sequence().Append(_scoreText.transform.DOScale(0.75f, animationTime / 2)).Append(_scoreText.transform.DOScale(1f, animationTime / 2));
            DOTween.Sequence().Append(_scoreText.DOColor(Color.red, animationTime / 2)).Append(_scoreText.DOColor(startColor, animationTime / 2));
            DOTween.Sequence().Append(_scoreHeadlineText.DOColor(Color.red, animationTime / 2)).Append(_scoreHeadlineText.DOColor(startColor, animationTime / 2));
        }
    }

    public void SetColorChoices(int correctChoice, int choice = -1) //Sets button colors according to player choice and correct choice.
    {
        var correctButton = _questionItem.choiceButtons[correctChoice];
        correctButton.image.color = choice == -1 ? Color.yellow : Color.green;
        if(choice != correctChoice && choice != -1)
        {
            var selectedButton = _questionItem.choiceButtons[choice];
            selectedButton.image.color = Color.red;
        }
    }

    public void DisableButtons()    //Disables the buttons.
    {
        foreach(Button button in _questionItem.choiceButtons)
        {
            button.enabled = false;
        }
    }


    public void EnableButtons()     //Enables the buttons.
    {
        foreach (Button button in _questionItem.choiceButtons)
        {
            button.enabled = true;
        }
    }


    public void ResetColorChoices() //Resets button colors.
    {
        foreach(var button in _questionItem.choiceButtons)
        {
            button.image.color = Color.white;
        }
    }
    
    public void ClearQuestion()     //Clears question item.
    {
        foreach (var button in _questionItem.choiceButtons)
        {
            button.onClick.RemoveAllListeners();
            button.gameObject.SetActive(false);
        }
    }

    public void CloseQuizPanel()    //Closes the quiz panel.
    {
        gameObject.SetActive(false);
    }
}