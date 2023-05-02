using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class LeaderboardAPI : MonoBehaviour    //Controls leaderboard api calls.
{    
    private IDataReader _dataReader;
    private IDataFormatter _dataFormatter;
    private readonly string _path = "https://magegamessite.web.app/case1/leaderboard_page_";    //I divide page part for creates leaderboard according to page.
    private readonly string _path2 = ".json";

    void Awake()
    {
        _dataReader = new UnityWebDataReader();
        _dataFormatter = new JsonToObjectFormatter();
    }

    private void Start()
    {
        Task t = Task.Run(ReadLeaderboardData);
    }

    public async Task<List<Score>> ReadLeaderboardData()        //Reads and returns leaderboard data.
    {
        var leaderboard = new Leaderboard();
        Leaderboard leaderBoardPage;
        bool isLastPage;
        int pageIndex = 0;
        string jsonData;
        do
        {
            jsonData = await _dataReader.GetData(_path + pageIndex + _path2);
            leaderBoardPage = _dataFormatter.FormatData<Leaderboard>(jsonData);
            leaderboard.data.AddRange(leaderBoardPage.data);
            isLastPage = leaderBoardPage.is_last;
            pageIndex++;
        }
        while (!isLastPage);
        return leaderboard.data;        
    }
}
