using System.Collections;
using System.Collections.Generic;
using UnityEditor.UI;
using UnityEngine;

public class UI : MonoBehaviour
{
    private UnityEngine.UI.Text _lifeText;
    private UnityEngine.UI.Text _scoreText;

    void Start()
    {
        _lifeText = transform.Find("Life").GetComponent<UnityEngine.UI.Text>();
        _scoreText = transform.Find("Score").GetComponent<UnityEngine.UI.Text>();
        
    }

    private void OnEnable()
    {
        GameControl.takelife += LifeRefresh;
        GameControl.addscore += ScoreRefresh;
    }

    private void OnDisable()
    {
        GameControl.takelife -= LifeRefresh;
        GameControl.addscore -= ScoreRefresh;
    }


    private void LifeRefresh(int playerLives)
    {
        _lifeText.text = "Lives: " + playerLives.ToString();
    }

    private void ScoreRefresh(int playerPoints)
    {
        _scoreText.text = "Score: " + playerPoints.ToString();
    }
}
