using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameControl;

public class PlatformShrink : Powerup, IProduct
{
    public delegate void Shrink();
    public static event Shrink shrink;

    [SerializeField] private string _productName = "PlatformShrink";

    public override string productName { get => _productName; set => _productName = value; }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            shrink?.Invoke();
            Debug.Log("platform shrink debuff!");
            Destroy(this.gameObject);
        }
    }
}
