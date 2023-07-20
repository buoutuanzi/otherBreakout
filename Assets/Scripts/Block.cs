using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    public int hitsToKill;
    private int score;
    private int numberOfHits;
    private GameObject gameControl;

    // Start is called before the first frame update
    void Start()
    {
        gameControl = GameObject.FindGameObjectsWithTag("GameControl")[0];
        numberOfHits = 0;
        score = hitsToKill * 10;
    }

    private void getHit()
    {
        numberOfHits++;
        if (numberOfHits == hitsToKill)
        {
            gameControl.SendMessage("addPoints", score);
            Destroy(this.gameObject);
        }
    }
}
