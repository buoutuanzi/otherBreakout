using System.Collections;
using System.Collections.Generic;
using UnityEditor.UI;
using UnityEngine;

public class UI : MonoBehaviour
{
    public Player player;
    private Transform life;
    private Transform score;
    // Start is called before the first frame update
    void Start()
    {
        life = transform.Find("Life");
        score = transform.Find("Score");
    }

    // Update is called once per frame
    void Update()
    {
        life.GetComponent<UnityEngine.UI.Text>().text = "Lives: " + player.playerLives.ToString();
        score.GetComponent<UnityEngine.UI.Text>().text = "Score: " + player.playerPoints.ToString();
    }
}
