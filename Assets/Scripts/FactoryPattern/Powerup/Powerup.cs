using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Powerup : MonoBehaviour, IProduct
{
    //由于不同种类的道具除了他们的效果有非常相近的逻辑，所以这里先用了一个abstract类继承product接口减少代码重复
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
