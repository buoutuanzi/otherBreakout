using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FactoryBallSlow : FactoryPowerup
{
    [SerializeField] private Powerup _ballSlowPrefab;
    protected override Powerup _powerupPrefab
    {
        get { return _ballSlowPrefab; }
        set { }
    }
}
