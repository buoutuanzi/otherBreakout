using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SocialPlatforms.Impl;

public class Ball : MonoBehaviour
{
    private bool _isActive;
    private Vector3 _ballPosition;
    private Vector2 _ballInitialForce;
    private Rigidbody2D _rb;

    public GameObject player;
    public GameObject gameControl;

    private Vector2 _reflectForce;
    private Vector2 _horizontalPreventionForce;

    // Start is called before the first frame update
    void Start()
    {
        _ballInitialForce = new Vector2(200f, 400f); 
        _isActive = false;
        _ballPosition = transform.position;
        _rb = GetComponent<Rigidbody2D>();   //��С��������Ҳٿص�ƽ̨ʱ��С��һ�������ϣ�
        _reflectForce = new Vector2(0f, 400f); //�����ȴ�������һ���Ѿ����ú�y��Vector������ײʱ�ټ���x
        _horizontalPreventionForce = new Vector2(0f, -50f);    
    }

    public void Fire()
    {
        if (!_isActive)
        {
            _rb.AddForce(_ballInitialForce);
            _isActive = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!_isActive && player)
        {
            _ballPosition.x = player.transform.position.x;
            transform.position = _ballPosition;
        }

        if (_isActive && transform.position.y < -5.5)
        {
            _isActive = false;
            _ballPosition.x = player.transform.position.x;
            _ballPosition.y = -4.258f;
            transform.position = _ballPosition;
            _rb.velocity = Vector2.zero; //����ʱ��velocity�����Է�����
            gameControl.SendMessage("TakeLife");
        }     
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player" && _isActive)
        {
            _rb.velocity = Vector2.zero;//�����Է�����
            float xDistanceToMiddle = (transform.position.x - player.transform.position.x) / 1.5f;
            //�����Ǹ�����ײʱ�����ĵľ��룬�ٳ������ߵ��м�ľ���İٷֱ���������������x����
            //ƽ̨һ��ľ�������1.25�������õ�1.5����Ϊ�е�ʱ�������ΪС����ײ�����ߣ�С��İ뾶����Ϊ�����ľ����һ���֣�
            //������������Ͼ�������������ȡһ�����ֵ1.5����ֹx����������������Ԥ�ڣ��������ƶ�����
            _reflectForce.x = xDistanceToMiddle * 300;
            _rb.AddForce(_reflectForce);
        } else if (collision.gameObject.tag == "Block")
        {
            collision.gameObject.SendMessage("GetHit");
        } else if (collision.gameObject.tag == "Wall")
        {//�ڼ�������£�С���������ǽ֮��ˮƽ����������Ϸ�޷��������У���������⵱��ײ��ǽ��ʱ�Ƿ�y��velocity��0
            if (_isActive && Mathf.Abs(_rb.velocity.y) <= 1)
            {
                Debug.Log("Ball is moving horizontally");
                _rb.AddForce(_horizontalPreventionForce); //��y�����ٶ�Ϊ0���ֶ����һ��������ֹ��Ϸ��ס
            }
        }
    }
}
