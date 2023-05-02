using DG.Tweening;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class LeaderboardController : MonoBehaviour      //Leaderboard's controller. Provides connection between the leaderboard view and leaderboard model.
{
    [SerializeField] private LeaderboardView _leaderboardView;

    private GameObject _leaderboardPanel;
    private Leaderboard _leaderboard = new();
    private LeaderboardAPI _leaderboardAPI;
    private bool _isPanelOpen;


    private void Awake()
    {
        Application.targetFrameRate = 60;       //Prevents ui breakings.
        _leaderboardAPI = gameObject.GetComponent<LeaderboardAPI>();
        _leaderboard.RegisterObserver(_leaderboardView);        //Register view to model for changes.
        _leaderboardPanel = _leaderboardView.gameObject;
    }

    private void Start()
    {
        _leaderboardView.gameObject.SetActive(false);
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
            _leaderboardPanel.transform.localScale = Vector3.zero;
            _leaderboardPanel.SetActive(true);
            _leaderboardPanel.transform.DOScale(Vector3.one, 0.3f).SetEase(Ease.OutBack);            
            await UpdateLeaderBoardModel();
            _leaderboardView.OnLeaderboardUpdate(_leaderboard.data); 
        }
    }

    public void CloseLeaderboardPanel()     //Closes leaderboard panel.
    {
        _isPanelOpen = false;
        _leaderboardView.DestroyScoreItems();
        _leaderboardView.gameObject.SetActive(false);
    }


}
