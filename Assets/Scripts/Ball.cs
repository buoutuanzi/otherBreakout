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
    public GameObject gameControl;

    private Vector2 _reflectForce;
    private Vector2 _horizontalPreventionForce;

    private enum Speed {Slower, Regular, Faster};//表示球的三个状态，以便之后区分和编程
    private Speed _currentSpeed;

    // Start is called before the first frame update
    void Start()
    {
        _ballInitialForce = new Vector2(200f, 400f); 
        _isActive = false;
        _ballPosition = transform.position;
        _rb = GetComponent<Rigidbody2D>();
        _horizontalPreventionForce = new Vector2(0f, -50f);
        _reflectForce = new Vector2(0f, 0f); //先创建出来一个的Vector，等碰撞时再计算x，y由速度决定
    }

    public void Fire()
    {
        if (!_isActive)
        {
            ForceReset(); //由于有时玩家会在小球掉下去重置的同时吃到道具，所以在这里重置在这里以保证初始力和反弹力的统一
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
            _rb.velocity = Vector2.zero; //重置时将velocity清零以防叠加
            gameControl.SendMessage("TakeLife");
            player.SendMessage("LengthReset");
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player" && _isActive)
        {
            _rb.velocity = Vector2.zero;//清零以防叠加
            float xDistanceToMiddle = (transform.position.x - player.transform.position.x) / 1.6f;
            //这里是根据碰撞时与中心的距离，再除以两边到中间的距离的百分比来决定反弹力的x方向
            //平台一半的距离大概是1.25，这里用的1.6是因为有的时候可能因为小球碰撞在两边，小球的半径将成为离中心距离的一部分，
            //但是这种情况毕竟是少数，所以取一个大概值1.6，防止x方向的力计算出大于预期，导致球移动过快
            _reflectForce.x = xDistanceToMiddle * 250;
            _rb.AddForce(_reflectForce);
        } else if (collision.gameObject.tag == "Block")
        {
            collision.gameObject.SendMessage("GetHit");
        } else if (collision.gameObject.tag == "Wall")
        {//在极端情况下，小球会在两边墙之间水平弹跳导致游戏无法继续进行，因此这里检测当球撞击墙壁时是否y的velocity是0
            if (_isActive && Mathf.Abs(_rb.velocity.y) <= 1)
            {
                Debug.Log("Ball is moving horizontally");
                _rb.AddForce(_horizontalPreventionForce); //若y方向速度为0则手动添加一股力，防止游戏卡住
            }
        }
    }

    private void OnEnable()
    {
        BallFast.fast += PowerUpFasterReceived;
        BallSlow.slow += PowerUpSlowerReceived;
    }

    private void OnDisable()
    {
        BallFast.fast -= PowerUpFasterReceived;
        BallSlow.slow -= PowerUpSlowerReceived;
    }

    private void PowerUpSlowerReceived()
    {   //若玩家在小球飞行过程中吃到这个道具，小球也需要立刻变换速度，因此这里先把小球当前的速度按照设置好的比例乘上
        //由于每次撞到玩家平台都会清零，重新使力，所以不用担心会影响之后的速度
        Vector2 newVelocity = new Vector2(_rb.velocity.x, _rb.velocity.y) * VelocityMultiplierSlower();
        Debug.Log("原本y速度是" + _rb.velocity.y);
        _rb.velocity = Vector2.zero; //清空当前速度防止有时不生效
        _rb.velocity = newVelocity;
        Debug.Log("现在y速度是" + _rb.velocity.y);
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
        ForceRefresh();
    }

    private void PowerUpFasterReceived()
    {
        Vector2 newVelocity = new Vector2(_rb.velocity.x, _rb.velocity.y) * VelocityMultiplierFaster();
        Debug.Log("原本y速度是" + _rb.velocity.y);
        _rb.velocity = Vector2.zero;
        _rb.velocity = newVelocity;
        Debug.Log("现在y速度是" + _rb.velocity.y);
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
        ForceRefresh();
    }

    private void ForceRefresh()
    {//虽然说理论上我们要改速度，但是由于小球是用力推动，所以改变速度更倾向于是改变与平台接触时施加的力的大小
        //改速度只会在上面小球正在飞行的过程中会用到
        Debug.Log("Speed change, ball speed status is" + _currentSpeed);
        switch (_currentSpeed)
        {
            case Speed.Slower:
                _reflectForce.y = 250;
                break;
            case Speed.Regular:
                _reflectForce.y = 400;
                break;
            case Speed.Faster:
                _reflectForce.y = 550;
                break;
        }
    }

    private void ForceReset()
    {
        _currentSpeed = Speed.Regular;
        ForceRefresh();
    }

    private float VelocityMultiplierFaster() //这里设置好velocity需要乘的比例以匹配力的变化
    {
        switch (_currentSpeed)
        {
            case Speed.Slower:
                return 1.6f;
            case Speed.Regular:
                return 1.375f;
            case Speed.Faster:
                return 1f;
        }
        return float.NaN;
    }

    private float VelocityMultiplierSlower()
    {
        switch (_currentSpeed)
        {
            case Speed.Slower:
                return 1f;
            case Speed.Regular:
                return 0.625f;
            case Speed.Faster:
                return 0.728f;
        }
        return float.NaN;
    }
}
