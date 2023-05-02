using System;
using System.Collections.Generic;

[Serializable]
public class Leaderboard    //Leaderboard Model
{
    public int page;
    public bool is_last;
    public List<Score> data = new();

    private readonly List<ILeaderboardObserver> _observers = new();

    public void AddData(List<Score> newScores)      //Adds new scores to scores data.
    {        
        data.AddRange(newScores);
        NotifyObservers();
    }

    public void ClearData()     //Clears scores data.
    {
        data.Clear();
    }

    public void RegisterObserver(ILeaderboardObserver observer)     //Registers an observer to this model observers.
    {
        _observers.Add(observer);
    }

    public void RemoveObserver(ILeaderboardObserver observer)       //Removes an observer from this model observers.
    {
        _observers.Remove(observer);
    }

    private void NotifyObservers()      //Notifies all of the models observers.
    {
        foreach (var observer in _observers)
        {
            observer.OnLeaderboardUpdate(data);
        }
    }
}
