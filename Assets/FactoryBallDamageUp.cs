using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FactoryDamageUp : FactoryPowerup
{
    [SerializeField] private Powerup _ballDamageUpPrefab;
    protected override Powerup _powerupPrefab
    {
        get { return _ballDamageUpPrefab; }
        set { }
    }
}
