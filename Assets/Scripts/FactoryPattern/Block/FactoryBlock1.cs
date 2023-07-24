using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FactoryBlock1 : FactoryBlock
{
    [SerializeField] private Block _block1Prefab;
    protected override Block _blockPrefab
    {
        get { return _block1Prefab; }
        set { }
    }
}
