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

    private float _speed;
    private float _maxAngle;
    private Quaternion _horizontalPreventionQ;

    private enum Speed {Slower, Regular, Faster};//表示球的三个状态，以便之后区分和编程
    private Speed _currentSpeed;

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
        BallReset();
    }

    public void Fire()
    {
        if (!_isActive)
        {
            _rb.AddForce(_ballInitialForce * _speed, ForceMode2D.Impulse);
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
            player.SendMessage("LengthReset");
            BallReset();
            gameControl.SendMessage("TakeLife");
        }
    }

    private void FixedUpdate()
    {
        if (_isActive)
        {
            _rb.velocity = _rb.velocity.normalized * _speed;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
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
        BallFast.fast += PowerUpFasterReceived;
        BallSlow.slow += PowerUpSlowerReceived;
        BallDamageUp.damageUp += PowerUpDamageReceived;
        PlatformShrink.shrink += PlayerSizeUpdate;
        PlatformExtend.extend += PlayerSizeUpdate;
    }

    private void OnDisable()
    {
        BallFast.fast -= PowerUpFasterReceived;
        BallSlow.slow -= PowerUpSlowerReceived;
        BallDamageUp.damageUp -= PowerUpDamageReceived;
        PlatformShrink.shrink -= PlayerSizeUpdate;
        PlatformExtend.extend -= PlayerSizeUpdate;
    }

    private void PowerUpSlowerReceived()
    {   
        switch (_currentSpeed)
        {
            case Speed.Slower:
                break;
            case Speed.Regular:
                _currentSpeed = Speed.Slower;
                break;
            case Speed.Faster:
                _currentSpeed = Speed.Regular;
                break;
        }
        SpeedRefresh();
    }

    private void PowerUpFasterReceived()
    {
        switch (_currentSpeed)
        {
            case Speed.Slower:
                _currentSpeed = Speed.Regular;
                break;
            case Speed.Regular:
                _currentSpeed = Speed.Faster;
                break;
            case Speed.Faster:
                break;
        }
        SpeedRefresh();
    }

    private void PowerUpDamageReceived()
    {
        _damage = 2;
        _colorRenderer.color = Color.red;//小球有攻击力buff时颜色变化
    }

    private void SpeedRefresh()
    {
        Debug.Log("Speed change, ball speed status is" + _currentSpeed);
        switch (_currentSpeed)
        {
            case Speed.Slower:
                _speed = 5;
                break;
            case Speed.Regular:
                _speed = 8;
                break;
            case Speed.Faster:
                _speed = 11;
                break;
        }
    }

    private void BallReset()
    {
        _damage = 1;
        _colorRenderer.color = Color.white; //小球默认颜色，会随着攻击力变化

        _isActive = false;
        _ballPosition.y = -4.258f;
        _rb.velocity = Vector2.zero;
        _currentSpeed = Speed.Regular;
        PlayerSizeUpdate();
        SpeedRefresh();
    }

    private void PlayerSizeUpdate()
    {
        _playerSizeHalf = player.transform.localScale.x;
    }
}
