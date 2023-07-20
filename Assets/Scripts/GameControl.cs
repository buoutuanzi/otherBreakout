using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameControl : MonoBehaviour
{
    public int playerLives;
    public int playerPoints;
    // Start is called before the first frame update
    void Start()
    {
        playerLives = 3;
        playerPoints = 0;
    }

    void TakeLife()
    {
        playerLives--;
        if (playerLives <= 0)
        {
            SceneManager.LoadScene("Main");
        }
    }

    void addPoints(int points)
    {
        playerPoints += points;
    }
}
