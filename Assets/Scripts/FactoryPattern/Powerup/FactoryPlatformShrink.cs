using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FactoryPlatformShrink : FactoryPowerup
{
    [SerializeField] private Powerup _platformShrinkPrefab;
    protected override Powerup _powerupPrefab
    {
        get { return _platformShrinkPrefab; }
        set { }
    }
}
