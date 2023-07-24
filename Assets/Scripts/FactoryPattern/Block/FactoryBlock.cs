using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class FactoryBlock : Factory
{
    abstract protected Block _blockPrefab { get; set; }

    public override IProduct GetProduct(Vector3 position)
    {
        if (_blockPrefab == null)
        {
            throw new System.NotImplementedException();
        }
        GameObject instance = Instantiate(_blockPrefab.gameObject, position, Quaternion.identity);
        Block newProduct = instance.GetComponent<Block>();
        newProduct.Initialize();
        return newProduct;
    }
}
