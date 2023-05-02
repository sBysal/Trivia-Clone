using System.Collections.Generic;

public interface ILeaderboardObserver       //The interface which brings inherited classes to observe leaderboard model feature. 
{
    void OnLeaderboardUpdate(List<Score> scoreList);
}