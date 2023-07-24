using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FactoryPlatformExtend : FactoryPowerup
{
    [SerializeField] private Powerup _platformExtendPrefab;
    protected override Powerup _powerupPrefab
    {
        get { return _platformExtendPrefab; }
        set { }
    }
}
