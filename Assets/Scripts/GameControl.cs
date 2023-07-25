using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameControl : MonoBehaviour
{
    private int _playerLives;
    private int _playerPoints;
    public delegate void takeLife(int playerLives);
    public static event takeLife takelife;
    public delegate void addScore(int playerPoints);
    public static event addScore addscore;

    void Start()
    {
        _playerLives = 3;
        takelife?.Invoke(_playerLives);
        _playerPoints = 0;
        addscore?.Invoke(_playerPoints);
    }

    private void TakeLife()
    {
        _playerLives--;
        takelife?.Invoke(_playerLives);
        if (_playerLives <= 0)
        {
            SceneManager.LoadScene("Main");
        }
    }

    private void AddPoints(int points)
    {
        _playerPoints += points;
        addscore?.Invoke(_playerPoints);
    }
}
