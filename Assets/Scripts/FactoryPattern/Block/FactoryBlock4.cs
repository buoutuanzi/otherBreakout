using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FactoryBlock4 : FactoryBlock
{
    [SerializeField] private Block _block4Prefab;
    protected override Block _blockPrefab
    {
        get { return _block4Prefab; }
        set { }
    }
}
