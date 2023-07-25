using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static BallSlow;

public class BallFast : Powerup, IProduct
{
    public delegate void Fast();
    public static event Fast fast;
    [SerializeField] private string _productName = "BallFast";

    public override string productName { get => _productName; set => _productName = value; }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            fast?.Invoke();
            Destroy(this.gameObject);
        }
    }
}
