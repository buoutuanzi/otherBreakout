using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockMenu : MonoBehaviour
{
    private FactoryBlock[] blockFactories;
    // Start is called before the first frame update
    void Start()
    {
        blockFactories = GetComponentsInChildren<FactoryBlock>();
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
                blockFactories[(i + j) % 3].GetProduct(relativePos);
                x += 2.5f;
            }
            y -= 2f;
        }
    }
}
