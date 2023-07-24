using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FactoryBallFast : FactoryPowerup
{
    [SerializeField] private Powerup _ballFastPrefab;
    protected override Powerup _powerupPrefab
    {
        get { return _ballFastPrefab; }
        set { }
    }
}
