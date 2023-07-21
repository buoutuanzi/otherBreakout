using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameControl : MonoBehaviour
{
    private int playerLives;
    private int playerPoints;
    public delegate void takeLife(int playerLives);
    public static event takeLife takelife;
    public delegate void addScore(int playerPoints);
    public static event addScore addscore;

    void Start()
    {
        playerLives = 4;
        takelife?.Invoke(playerLives);
        playerPoints = 0;
        addscore?.Invoke(playerLives);
    }

    private void TakeLife()
    {
        playerLives--;
        takelife?.Invoke(playerLives);
        if (playerLives <= 0)
        {
            SceneManager.LoadScene("Main");
        }
    }

    private void addPoints(int points)
    {
        playerPoints += points;
        addscore?.Invoke(playerPoints);
    }
}
