using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FactoryBlock2 : FactoryBlock
{
    [SerializeField] private Block _block2Prefab;
    protected override Block _blockPrefab
    {
        get { return _block2Prefab; }
        set { }
    }
}
