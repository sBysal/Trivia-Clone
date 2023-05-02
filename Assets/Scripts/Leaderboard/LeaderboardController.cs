using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class LeaderboardController : MonoBehaviour      //Leaderboard's controller
{
    [SerializeField] private GameObject _leaderboardPanel;
    [SerializeField] private LeaderboardView _leaderboardView;


    private Leaderboard _leaderboard = new();
    private LeaderboardAPI _leaderboardAPI;
    private bool _isPanelOpen;


    private void Awake()
    {
        Application.targetFrameRate = 60;       //Prevents ui breakings.
        _leaderboardAPI = gameObject.GetComponent<LeaderboardAPI>();
        _leaderboard.RegisterObserver(_leaderboardView);
    }

    private void Start()
    {
        _leaderboardPanel.SetActive(false);
        _isPanelOpen = false;
    }

    private async Task UpdateLeaderBoardModel()     //Updates leaderboard model.
    {
        _leaderboard.ClearData();
        List<Score> temp = await _leaderboardAPI.ReadLeaderboardData();
        _leaderboard.AddData(temp);
    }

    public async void OpenLeaderboardPanel()        //Opens leaderboard panel.
    {
        if(!_isPanelOpen )
        {
            _isPanelOpen = true;
            _leaderboardPanel.SetActive(true);
            await UpdateLeaderBoardModel();
            _leaderboardView.OnLeaderboardUpdate(_leaderboard.data);
        }
    }

    public void CloseLeaderboardPanel()     //Closes leaderboard panel.
    {
        _isPanelOpen = false;
        _leaderboardPanel.SetActive(false);
    }


}
