using System.Collections;
using System.Collections.Generic;
using UnityEditor.UI;
using UnityEngine;

public class UI : MonoBehaviour
{
    private UnityEngine.UI.Text life;
    private UnityEngine.UI.Text score;

    void Start()
    {
        life = transform.Find("Life").GetComponent<UnityEngine.UI.Text>();
        score = transform.Find("Score").GetComponent<UnityEngine.UI.Text>();
        
    }

    private void OnEnable()
    {
        GameControl.takelife += lifeRefresh;
        GameControl.addscore += scoreRefresh;
    }

    private void OnDisable()
    {
        GameControl.takelife -= lifeRefresh;
        GameControl.addscore -= scoreRefresh;
    }


    public void lifeRefresh(int playerLives)
    {
        life.text = "Lives: " + playerLives.ToString();
    }

    private void scoreRefresh(int playerPoints)
    {
        score.text = "Score: " + playerPoints.ToString();
    }
}
