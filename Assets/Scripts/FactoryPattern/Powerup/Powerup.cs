using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Powerup : MonoBehaviour, IProduct
{

    public abstract string productName { get; set; }

    public void Initialize()
    {
        gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector3(0f, -200f, 0f));
    }


    protected void Update()
    {
        if (gameObject.transform.position.y < -5.5)
        {
            Destroy(this.gameObject);
        }
    }

    protected abstract void OnTriggerEnter2D(Collider2D collision);
}
