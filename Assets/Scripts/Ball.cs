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
    private float _playerSizeHalf; //玩家一半长度用来计算反弹角度
    public GameObject gameControl;

    private float _currentSpeed;
    private int _currentSpeedIndex;
    [SerializeField] private float[] _speeds;//装着三个档位的速度

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
        _horizontalPreventionQ = Quaternion.AngleAxis(10, Vector3.forward);//用来给水平小球转向
        _colorRenderer = GetComponent<SpriteRenderer>();
        _speeds = new float[] { 5f, 8f, 11f };//需要由大到小排列好，这样加速index++，减速index--
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
            //根据集中点到中心的距离决定小球的转向，clamp用来防止过度转向，控制在【-60，60】度之间
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
            //在极端情况下，小球会在两边墙之间水平弹跳或接近导致游戏无法继续进行或过于缓慢，
         //因此这里检测当球撞击墙壁时是否y的velocity是否非常接近为0
            if (_isActive && Mathf.Abs(_rb.velocity.y) <= 0.5f)
            {
                Debug.Log("Ball is moving horizontally");
                Vector2 ballCurDirection = _rb.velocity.normalized; //这种情况发生时手动将小球转向
                ballCurDirection = _horizontalPreventionQ * ballCurDirection;
                _rb.velocity = ballCurDirection * _rb.velocity.magnitude;
            }

        }
    }

    private void OnEnable()//同时监听平台长度变化，使反弹角度更自然过度
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
        if (_currentSpeedIndex > 0)//当index是0，即最慢速度时将会跳过
        {
            _currentSpeedIndex--;
            SpeedRefresh();
            Debug.Log("ball is slower!");
        }
    }

    private void PowerUpFasterReceived()
    {
        if (_currentSpeedIndex < 2)//当index是2，即最快速度时将会跳过
        {
            _currentSpeedIndex++;
            SpeedRefresh();
            Debug.Log("ball is faster!");
        }
    }

    private void PowerUpDamageReceived()
    {
        _damage = 2;
        _colorRenderer.color = Color.red;//小球有攻击力buff时颜色变化
    }


    private void BallReset()
    {
        _damage = 1;
        _colorRenderer.color = Color.white; //小球默认颜色，会随着攻击力变化

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
