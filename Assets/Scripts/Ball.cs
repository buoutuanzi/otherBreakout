using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SocialPlatforms.Impl;

public class Ball : MonoBehaviour
{
    private bool isActive;
    private Vector3 ballPosition;
    private Vector2 ballInitialForce;
    private Rigidbody2D rb;
    public GameObject player;
    public GameObject gameControl;
    private Vector2 reflectForce;
    // Start is called before the first frame update
    void Start()
    {
        ballInitialForce = new Vector2(200f, 450f);
        isActive = false;
        ballPosition = transform.position;
        rb = GetComponent<Rigidbody2D>();
        reflectForce = new Vector2(0, 450); //当小球碰到玩家操控的平台时，小球一定会向上，
                                            //所以先创建出来一个已经设置好y的Vector，等碰撞时再计算x
    }

    public void fire()
    {
        if (!isActive)
        {
            rb.AddForce(ballInitialForce);
            isActive = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!isActive & player)
        {
            ballPosition.x = player.transform.position.x;
            transform.position = ballPosition;
        }

        if (isActive & transform.position.y < -5.5)
        {
            isActive = false;
            ballPosition.x = player.transform.position.x;
            ballPosition.y = -4.258f;
            transform.position = ballPosition;
            rb.velocity = Vector2.zero;
            gameControl.SendMessage("TakeLife");
        }
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player" && isActive)
        {
            rb.velocity = Vector2.zero;
            float xDistanceToMiddle = (transform.position.x - player.transform.position.x) / 1.5f;
            //这里是根据碰撞时与中心的距离，再除以两边到中间的距离的百分比来决定反弹力的x方向
            //平台一半的距离大概是1.25，这里用的1.5是因为有的时候可能因为小球碰撞在两边，小球的半径将成为距离中心距离的一部分，
            //但是这种情况毕竟是少数，所以取一个值1.5，防止x方向的力计算出大于预期，导致球移动过快
            reflectForce.x = xDistanceToMiddle * 300;
            rb.AddForce(reflectForce);
        } else if (collision.gameObject.tag == "Block")
        {
            collision.gameObject.SendMessage("getHit");
        }
    }
}
