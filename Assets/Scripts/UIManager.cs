using DG.Tweening;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour      //Controls gameplay scene UI's except Quiz.
{
    [SerializeField] private QuizController _quizController;
    [SerializeField] private GameObject _endPopup;
    [SerializeField] private TMP_Text _scoreText;


    private void Start()
    {
        _endPopup.SetActive(false);
        _quizController.SetQuestions();
        _quizController.ShowNextQuestion();
    }

    public void EndGame(int score)      //Shows endPopup.
    {
        _scoreText.text = score.ToString();
        _endPopup.transform.localScale = Vector3.zero;
        _endPopup.SetActive(true);
        _endPopup.transform.DOScale(Vector3.one, 0.25f).SetEase(Ease.Flash);
    }

    
}
