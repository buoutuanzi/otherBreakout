using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformExtend : Powerup, IProduct
{
    [SerializeField] private string _productName = "PlatformExtend";

    public override string productName {get => _productName; set => _productName = value; }


    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("platform extend powerup!");
            Destroy(this.gameObject);
        }
    }

}
