using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LootLocker.Requests;
using TMPro;
using Mono.Cecil.Cil;
using Unity.VisualScripting;
using UnityEngine.SocialPlatforms.Impl;

public class ScoreManager : MonoBehaviour, IScoreObserver
{
    int leaderboardId = 7915;
    public const int retrievedLeaderboardSize = 999;

    public LootLockerLeaderboardMember[] scoreboard;

    public static ScoreManager _instance;

    private int latestScore;

    public int LatestScore { get => latestScore; }

    private List<IScoreListener> listeners = new List<IScoreListener>();

    private void Awake()
    {
        if (_instance != null) Destroy(this);
        else _instance = this;

        DontDestroyOnLoad(transform.gameObject);
    }

    // Start is called before the first frame update
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            FinalizeScore(Random.Range(0, 99999));
        }
    }

    public void FinalizeScore(int score)
    {
        StartCoroutine(SubmitScore(score));
    }

    private IEnumerator SubmitScore(int score)
    {
        bool done = false;
        string playerId = PlayerManager._instance.Player_id;
        latestScore = score;

        LootLockerSDKManager.SubmitScore(playerId, score, leaderboardId, (response) =>
        {
            if (response.success)
            {
                Debug.LogError(string.Format("Uploaded score: {0}", score));
                done = true;
                NotifyListeners();
                StartCoroutine(FetchHighscores());
            }
            else
            {
                Debug.LogError("Uploading score failed" + response.Error);
                done = true;
            }
        });
        yield return new WaitWhile(() => done == false);
    }

    public IEnumerator FetchHighscores()
    {
        if (scoreboard == null) scoreboard = new LootLockerLeaderboardMember[retrievedLeaderboardSize];

        bool done = false;
        LootLockerSDKManager.GetScoreList(leaderboardId, retrievedLeaderboardSize, 0, (response) =>
        {
            if (response.success)
            {
                Debug.LogError("Fetched leaderboards");
                scoreboard = response.items;
                done = true;
            }
            else
            {
                Debug.LogError("Failed to fetch leaderboards" + response.Error);
                done = true;
            }
        });

        yield return new WaitWhile(() => done == false);
    }

    public bool CheckIfNameExists(string name)
    {
        bool exists = false;

        for (int i = 0; i < scoreboard.Length; i++)
        {
            if (scoreboard[i].player.name.Equals(name))
            {
                if (!scoreboard[i].player.id.ToString().Equals(PlayerManager._instance.Player_id)) 
                    exists = true;
            }
        }

        return exists;
    }

    public void AddListener(IScoreListener listener)
    {
        listeners.Add(listener);
    }

    public void NotifyListeners()
    {
        foreach (IScoreListener listener in listeners)
        {
            listener.ChangeInLeaderboard();
        }
    }
}
