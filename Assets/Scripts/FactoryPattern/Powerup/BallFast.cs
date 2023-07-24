using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallFast : Powerup, IProduct
{
    [SerializeField] private string _productName = "BallFast";

    public override string productName { get => _productName; set => _productName = value; }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("ball is getting faster!");
            Destroy(this.gameObject);
        }
    }
}
