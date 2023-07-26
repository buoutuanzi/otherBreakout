using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallDamageUp : Powerup, IProduct
{
    public delegate void DamageUp();
    public static event DamageUp damageUp;

    [SerializeField] private string _productName = "BallDamageUp";

    public override string productName { get => _productName; set => _productName = value; }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            damageUp?.Invoke();
            Destroy(this.gameObject);
        }
    }
}
