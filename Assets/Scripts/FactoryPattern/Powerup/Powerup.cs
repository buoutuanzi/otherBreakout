using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    public delegate void Fast();
    public static event Fast fast;
    public delegate void Slow();
    public static event Slow slow;
    public delegate void Extend();
    public static event Extend extend;
    public delegate void Shrink();
    public static event Shrink shrink;
    public delegate void DamageUp();
    public static event DamageUp damageUp;

    private string _powerupName;//装着prefab的名称好唤起对应event，直接用gameObject.name会不稳定（后面会有clone）

    public void Initialize(string name)
    {//这里选择传进来他的prefab名字，用来区分这个powerup是哪个
        gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector3(0f, -200f, 0f));
        _powerupName = name;
    }


    private void Update()
    {
        if (gameObject.transform.position.y < -5.5)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            switch (_powerupName)
            {
                case "BallDamageUp":
                    damageUp?.Invoke();
                    break;
                case "BallFast":
                    fast?.Invoke();
                    break;
                case "BallSlow":
                    slow?.Invoke();
                    break;
                case "PlatformExtend":
                    extend?.Invoke();
                    break;
                case "PlatformShrink":
                    shrink?.Invoke();
                    break;
            }
            Destroy(this.gameObject);
        }
    }
}
