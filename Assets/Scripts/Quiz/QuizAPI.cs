using System.Collections.Generic;
using UnityEngine;

public class QuizAPI : MonoBehaviour
{
    private IDataFormatter _dataFormatter;

    private void Awake()
    {
        _dataFormatter = new JsonToObjectFormatter();
    }

    public Quiz ReadQuestionsData()        //Reads and returns leaderboard data.
    {
        string jsonData = Resources.Load<TextAsset>("Questions").text;
        var quiz = _dataFormatter.FormatData<Quiz>(jsonData);
        return quiz;
    }
}
