using System.Collections.Generic;

public interface ILeaderboardObserver
{
    void OnLeaderboardUpdate(List<Score> scoreList);
}