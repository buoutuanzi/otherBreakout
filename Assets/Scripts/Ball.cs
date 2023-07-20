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
        reflectForce = new Vector2(0, 450); //��С��������Ҳٿص�ƽ̨ʱ��С��һ�������ϣ�
                                            //�����ȴ�������һ���Ѿ����ú�y��Vector������ײʱ�ټ���x
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
            //�����Ǹ�����ײʱ�����ĵľ��룬�ٳ������ߵ��м�ľ���İٷֱ���������������x����
            //ƽ̨һ��ľ�������1.25�������õ�1.5����Ϊ�е�ʱ�������ΪС����ײ�����ߣ�С��İ뾶����Ϊ�������ľ����һ���֣�
            //������������Ͼ�������������ȡһ��ֵ1.5����ֹx����������������Ԥ�ڣ��������ƶ�����
            reflectForce.x = xDistanceToMiddle * 300;
            rb.AddForce(reflectForce);
        } else if (collision.gameObject.tag == "Block")
        {
            collision.gameObject.SendMessage("getHit");
        }
    }
}
