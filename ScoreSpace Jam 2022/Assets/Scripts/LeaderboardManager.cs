using LootLocker.Requests;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;

public class LeaderboardManager : MonoBehaviour, IScoreListener
{
    [SerializeField] private List<TextMeshProUGUI> scoresText = new List<TextMeshProUGUI>(), namesText = new List<TextMeshProUGUI>();
    [SerializeField] private Color highlighted, regular;

    public void ChangeInLeaderboard()
    {
        DrawScores(ScoreManager._instance.LatestScore);
    }

    private void Start()
    {
        ScoreManager._instance.AddListener(this);
    }

    public void DrawScores(int score)
    {
        LootLockerLeaderboardMember[] scoreboard = ScoreManager._instance.scoreboard;

        int leadboardLength = Mathf.Min(namesText.Count - 1, scoreboard.Length);

        for (int i = 0; i < leadboardLength; i++)
        {
            string scoreString = scoreboard[i].rank + ". ";

            if (scoreboard[i].player.name != "")
            {
                namesText[i].text = scoreboard[i].player.name;
            }
            else
            {
                namesText[i].text = scoreboard[i].player.id.ToString();
            }

            namesText[i].color = regular;
            namesText[i].fontStyle = FontStyles.Bold;

            scoresText[i].color = regular;
            scoresText[i].fontStyle = FontStyles.Bold;

            // Highlight player score in top 10
            if (PlayerManager._instance.Player_id.Equals(scoreboard[i].player.id.ToString()))
            {
                namesText[i].color = highlighted;
                namesText[i].fontStyle = FontStyles.Bold | FontStyles.Italic;

                scoresText[i].color = highlighted;
                scoresText[i].fontStyle = FontStyles.Bold | FontStyles.Italic;
            }

            scoresText[i].text = scoreString + scoreboard[i].score.ToString();
        }

        leadboardLength = scoreboard.Length;
        int rank = leadboardLength + 1;

        // Show player score outside of top 10
        for (int i = 0; i < leadboardLength; i++)
        {
            if (score < scoreboard[i].score) continue;

            rank = i + 1;
            break;
        }

        string scoreCombined = rank + ". ";

        if (PlayerManager._instance.Player_name != "")
        {
            namesText[namesText.Count - 1].text = PlayerManager._instance.Player_name;
        }
        else
        {
            namesText[namesText.Count - 1].text = PlayerManager._instance.Player_id;
        }
        scoresText[scoresText.Count - 1].text = scoreCombined + score.ToString();
    }
}
