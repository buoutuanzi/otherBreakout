using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    public int hitsToKill;
    private int numberOfHits;

    // Start is called before the first frame update
    void Start()
    {
        numberOfHits = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ball")
        {
            numberOfHits++;
            if (numberOfHits == hitsToKill)
            {
                Destroy(this.gameObject);
            }
        }
    }
}
