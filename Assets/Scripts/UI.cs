using System.Collections;
using System.Collections.Generic;
using UnityEditor.UI;
using UnityEngine;

public class UI : MonoBehaviour
{
    public GameControl gameControl;
    private UnityEngine.UI.Text life;
    private UnityEngine.UI.Text score;
    // Start is called before the first frame update
    void Start()
    {
        life = transform.Find("Life").GetComponent<UnityEngine.UI.Text>();
        score = transform.Find("Score").GetComponent<UnityEngine.UI.Text>();
    }

    // Update is called once per frame
    void Update()
    {
        life.text = "Lives: " + gameControl.playerLives.ToString();
        score.text = "Score: " + gameControl.playerPoints.ToString();
    }
}
