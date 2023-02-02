using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IScoreListener
{
    public void ChangeInLeaderboard();
}

public interface IScoreObserver
{
    public void AddListener(IScoreListener listener);
    public void NotifyListeners();
}
