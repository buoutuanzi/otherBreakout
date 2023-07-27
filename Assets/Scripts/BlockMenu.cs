using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockMenu : MonoBehaviour
{
    private FactoryBlock _factoryBlock;
    // Start is called before the first frame update
    void Start()
    {
        _factoryBlock = GetComponentInChildren<FactoryBlock>();
        ArrangeLevel1Blocks();
    }

    private void ArrangeLevel1Blocks()
    {
        float y = 4f;
        for (int i = 0; i < 3; i++)
        {
            float x = -7.5f;
            for (int j = 0; j < 7; j++)
            {
                Vector3 relativePos = new Vector3(x, y, 0);//达到砖块排列效果
                if (_factoryBlock != null)
                {
                    _factoryBlock.GetProduct(relativePos, (i + j) % _factoryBlock.GetBlockListLength());
                }
                x += 2.5f;
            }
            y -= 2f;
        }
    }
}
