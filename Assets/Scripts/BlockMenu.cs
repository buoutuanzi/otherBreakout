using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockMenu : MonoBehaviour
{
    public List<GameObject> Blocks;
    // Start is called before the first frame update
    void Start()
    {
        float Y = 4f;
        
        for (int i = 0; i < 3; i++)
        {
            float X = -7.5f;
            for (int j = 0; j < 7; j++)
            {
                Vector3 relativePos = new Vector3(X, Y, 0);
                GameObject instance = Instantiate(Blocks[(i + j)%3], transform);
                instance.transform.position = relativePos;
                X += 2.5f;
            }
            Y -= 2f;
        }
    }
}
