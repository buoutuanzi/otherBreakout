using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FactoryBlock : MonoBehaviour
{
    [SerializeField] private Block[] blockPrefabs;

    public Block GetProduct(Vector3 position, int index)
    {
        if (index < 0 || index >= GetBlockListLength())
        {
            throw new System.IndexOutOfRangeException();
        }
        Block instance = Instantiate(blockPrefabs[index], position, Quaternion.identity);
        instance.Initialize();
        return instance;
    }

    public int GetBlockListLength()
    {
        return blockPrefabs.Length;
    }
}
