using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlatformShrink;

public class PlatformExtend : Powerup, IProduct
{
    public delegate void Extend();
    public static event Extend extend;

    [SerializeField] private string _productName = "PlatformExtend";

    public override string productName {get => _productName; set => _productName = value; }


    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            extend?.Invoke();
            Debug.Log("platform extend powerup!");
            Destroy(this.gameObject);
        }
    }

}
