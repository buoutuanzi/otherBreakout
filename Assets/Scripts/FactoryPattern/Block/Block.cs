using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    public int hitsToKill;
    private int _score;
    private int _numberOfHits;
    private GameObject _gameControl;

    private FactoryPowerup _factoryPowerup;
    private int _powerupIndex;

    public void Initialize()
    {
        _gameControl = GameObject.FindGameObjectsWithTag("GameControl")[0];
        _numberOfHits = 0;
        _score = hitsToKill * 10;
        float chance = hitsToKill * 10; //根据方块血量决定掉落道具几率，分别为10%，20%，40%
        if (Random.Range(0, 100) < chance) //此处为应用几率chance，100个数若随机到得数小于chance则表示将生成道具
        {
            _factoryPowerup = _gameControl.GetComponentInChildren<FactoryPowerup>();
            _powerupIndex = Random.Range(0, _factoryPowerup.GetPowerupListLength());
        }
    }

    private void GetHit(int damage)
    {
        _numberOfHits += damage;
        if (_numberOfHits >= hitsToKill)
        {
            _gameControl.SendMessage("AddPoints", _score);
            if (_factoryPowerup != null) //如果有道具则生成掉落
            {
                _factoryPowerup.GetProduct(transform.position, _powerupIndex);
            }
            Destroy(this.gameObject);
        }
    }
}
