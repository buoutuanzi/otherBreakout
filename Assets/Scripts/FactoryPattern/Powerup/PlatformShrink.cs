using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformShrink : Powerup, IProduct
{
    [SerializeField] private string _productName = "PlatformShrink";

    public override string productName { get => _productName; set => _productName = value; }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("platform shrink debuff!");
            Destroy(this.gameObject);
        }
    }
}
