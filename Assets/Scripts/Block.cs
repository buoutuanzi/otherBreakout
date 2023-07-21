using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    public int hitsToKill;
    private int _score;
    private int _numberOfHits;
    private GameObject _gameControl;

    void Start()
    {
        _gameControl = GameObject.FindGameObjectsWithTag("GameControl")[0];
        _numberOfHits = 0;
        _score = hitsToKill * 10;
    }

    private void GetHit()
    {
        _numberOfHits++;
        if (_numberOfHits == hitsToKill)
        {
            _gameControl.SendMessage("AddPoints", _score);
            Destroy(this.gameObject);
        }
    }
}
