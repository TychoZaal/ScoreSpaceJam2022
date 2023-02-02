using LootLocker.Requests;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager _instance;

    private string player_id = "", player_name = "TestSubject2";

    public string Player_id { get => player_id; }
    public string Player_name { get => player_name; }

    // Start is called before the first frame update
    void Awake()
    {
        if (_instance != null) Destroy(this);
        else _instance = this;

        DontDestroyOnLoad(transform.gameObject);

        StartCoroutine(LoginRoutine());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.N))
            if (player_name != "") SetPlayerName(player_name);
    }

    private IEnumerator LoginRoutine()
    {
        bool done = false;
        LootLockerSDKManager.StartGuestSession((response) =>
        {
            if (response.success)
            {
                PlayerPrefs.SetString("PlayerID", response.player_id.ToString());
                player_id = PlayerPrefs.GetString("PlayerID");

                Debug.LogError(string.Format("Connected to server using ID: {0}", player_id));
                done = true;

                StartCoroutine(ScoreManager._instance.FetchHighscores());
            }
            else
            {
                Debug.LogError("Could not connect to server" + response.Error);
                done = true;
            }
        });
        yield return new WaitWhile(() => done == false);
    }

    public bool SetPlayerName(string newName)
    {
        bool success = false;

        if (ScoreManager._instance.CheckIfNameExists(newName))
        {
            Debug.LogError("Name already exists");
            success = false;
            return success;
        }

        LootLockerSDKManager.SetPlayerName(newName, (response) =>
        {
            if (response.success)
            {
                Debug.LogError("Changed player name");
                player_name = newName;
                success = true;
            }
            else
            {
                Debug.LogError("Could not change name" + response.Error);
                success = false;
            }
        });

        return success;
    }
}
