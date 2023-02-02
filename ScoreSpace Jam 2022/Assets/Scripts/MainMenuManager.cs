using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private TMP_InputField nameField;

    public void NameUpdate()
    {
        PlayerManager._instance.SetPlayerName(nameField.text);
    }

    public void CheckNameInput()
    {
        bool exists = ScoreManager._instance.CheckIfNameExists(nameField.text);
        Debug.LogWarning(exists);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            SceneManager.LoadScene("Leaderboard");
        }
    }
}
