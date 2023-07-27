using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    private bool _isActive;
    private Vector3 _ballPosition;
    private Vector2 _ballInitialForce;
    private Rigidbody2D _rb;

    public GameObject player;
    private float _playerSizeHalf; //���һ�볤���������㷴���Ƕ�
    public GameObject gameControl;

    private float _currentSpeed;
    private int _currentSpeedIndex;
    [SerializeField] private float[] _speeds;//װ��������λ���ٶ�

    private float _maxAngle;
    private Quaternion _horizontalPreventionQ;

    [SerializeField] private int _damage;
    private SpriteRenderer _colorRenderer;

    void Start()
    {
        _ballInitialForce = new Vector2(1f, 2f).normalized; 
        _isActive = false;
        _ballPosition = transform.position;
        _rb = GetComponent<Rigidbody2D>();
        _maxAngle = 60f;
        _horizontalPreventionQ = Quaternion.AngleAxis(10, Vector3.forward);//������ˮƽС��ת��
        _colorRenderer = GetComponent<SpriteRenderer>();
        _speeds = new float[] { 5f, 8f, 11f };//��Ҫ�ɴ�С���кã���������index++������index--
        BallReset();
    }

    public void Fire()
    {
        if (!_isActive)
        {
            _rb.AddForce(_ballInitialForce * _currentSpeed, ForceMode2D.Impulse);
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

        if (_isActive && transform.position.y < -5)
        {
            player.SendMessage("LengthReset");
            BallReset();
            gameControl.SendMessage("TakeLife");
        }
    }

    private void FixedUpdate()
    {
        if (_isActive)
        {
            _rb.velocity = _rb.velocity.normalized * _currentSpeed;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player" && _isActive)
        {
            //���ݼ��е㵽���ĵľ������С���ת��clamp������ֹ����ת�򣬿����ڡ�-60��60����֮��
            float xDistanceToMiddle = (player.transform.position.x - transform.position.x) / _playerSizeHalf;
            float bounceAngle = xDistanceToMiddle * _maxAngle;
            bounceAngle = Mathf.Clamp(bounceAngle, -_maxAngle, _maxAngle);
            Vector2 reflectDirection = Quaternion.AngleAxis(bounceAngle, Vector3.forward) * Vector2.up;
            _rb.velocity = _rb.velocity.magnitude * reflectDirection;

        } else if (collision.gameObject.tag == "Block")
        {

            collision.gameObject.SendMessage("GetHit", _damage);

        } else if (collision.gameObject.tag == "Wall")
        {
            //�ڼ�������£�С���������ǽ֮��ˮƽ������ӽ�������Ϸ�޷��������л���ڻ�����
         //��������⵱��ײ��ǽ��ʱ�Ƿ�y��velocity�Ƿ�ǳ��ӽ�Ϊ0
            if (_isActive && Mathf.Abs(_rb.velocity.y) <= 0.5f)
            {
                Debug.Log("Ball is moving horizontally");
                Vector2 ballCurDirection = _rb.velocity.normalized; //�����������ʱ�ֶ���С��ת��
                ballCurDirection = _horizontalPreventionQ * ballCurDirection;
                _rb.velocity = ballCurDirection * _rb.velocity.magnitude;
            }

        }
    }

    private void OnEnable()//ͬʱ����ƽ̨���ȱ仯��ʹ�����Ƕȸ���Ȼ����
    {
        Powerup.fast += PowerUpFasterReceived;
        Powerup.slow += PowerUpSlowerReceived;
        Powerup.damageUp += PowerUpDamageReceived;
        Powerup.shrink += PlayerSizeUpdate;
        Powerup.extend += PlayerSizeUpdate;
    }

    private void OnDisable()
    {
        Powerup.fast -= PowerUpFasterReceived;
        Powerup.slow -= PowerUpSlowerReceived;
        Powerup.damageUp -= PowerUpDamageReceived;
        Powerup.shrink -= PlayerSizeUpdate;
        Powerup.extend -= PlayerSizeUpdate;
    }

    private void PowerUpSlowerReceived()
    {
        if (_currentSpeedIndex > 0)//��index��0���������ٶ�ʱ��������
        {
            _currentSpeedIndex--;
            SpeedRefresh();
            Debug.Log("ball is slower!");
        }
    }

    private void PowerUpFasterReceived()
    {
        if (_currentSpeedIndex < 2)//��index��2��������ٶ�ʱ��������
        {
            _currentSpeedIndex++;
            SpeedRefresh();
            Debug.Log("ball is faster!");
        }
    }

    private void PowerUpDamageReceived()
    {
        _damage = 2;
        _colorRenderer.color = Color.red;//С���й�����buffʱ��ɫ�仯
    }


    private void BallReset()
    {
        _damage = 1;
        _colorRenderer.color = Color.white; //С��Ĭ����ɫ�������Ź������仯

        _currentSpeedIndex = 1;
        SpeedRefresh();

        _isActive = false;
        _ballPosition.y = -4.29f;
        _rb.velocity = Vector2.zero;
        PlayerSizeUpdate();
    }

    private void PlayerSizeUpdate()
    {
        _playerSizeHalf = player.transform.localScale.x;
    }

    private void SpeedRefresh()
    {
        _currentSpeed = _speeds[_currentSpeedIndex];
    }
}
