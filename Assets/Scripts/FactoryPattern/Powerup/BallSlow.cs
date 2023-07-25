using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallSlow : Powerup, IProduct
{
    public delegate void Slow();
    public static event Slow slow;

    [SerializeField] private string _productName = "BallSlow";

    public override string productName { get => _productName; set => _productName = value; }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            slow?.Invoke();
            Destroy(this.gameObject);
        }
    }
}