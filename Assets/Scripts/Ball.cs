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
    private Vector2 horizontalPreventionForce;
    // Start is called before the first frame update
    void Start()
    {
        ballInitialForce = new Vector2(200f, 450f); 
        isActive = false;
        ballPosition = transform.position;
        rb = GetComponent<Rigidbody2D>();   //当小球碰到玩家操控的平台时，小球一定会向上，
        reflectForce = new Vector2(0f, 450f); //所以先创建出来一个已经设置好y的Vector，等碰撞时再计算x
        horizontalPreventionForce = new Vector2(0f, 50f);    
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
            rb.velocity = Vector2.zero; //重置时将velocity清零以防叠加
            gameControl.SendMessage("TakeLife");
        }     
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player" && isActive)
        {
            rb.velocity = Vector2.zero;//清零以防叠加
            float xDistanceToMiddle = (transform.position.x - player.transform.position.x) / 1.5f;
            //这里是根据碰撞时与中心的距离，再除以两边到中间的距离的百分比来决定反弹力的x方向
            //平台一半的距离大概是1.25，这里用的1.5是因为有的时候可能因为小球碰撞在两边，小球的半径将成为离中心距离的一部分，
            //但是这种情况毕竟是少数，所以取一个大概值1.5，防止x方向的力计算出大于预期，导致球移动过快
            reflectForce.x = xDistanceToMiddle * 300;
            rb.AddForce(reflectForce);
        } else if (collision.gameObject.tag == "Block")
        {
            collision.gameObject.SendMessage("getHit");
        } else if (collision.gameObject.tag == "Wall")
        {//在极端情况下，小球会在两边墙之间水平弹跳导致游戏无法继续进行，因此这里检测当球撞击墙壁时是否y的velocity是0
            if (isActive && rb.velocity.y == 0)
            {
                Debug.Log("Ball is moving horizontally");
                rb.AddForce(horizontalPreventionForce); //若y方向速度为0则手动添加一股力，防止游戏卡住
            }
        }
    }
}
