using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    private float _movementX;
    private Vector3 _moveDistance;
    public float speed;

    public float bound;
    private Vector3 _leftBound;
    private Vector3 _rightBound;
    public Ball ball;

    private enum State {Shrinking, RegularSize, Extending};//表示玩家平台的三个状态，以便之后区分和编程
    private State _currentState;
    [SerializeField] private Vector3 _shrinkSize;
    [SerializeField] private Vector3 _regularSize;
    [SerializeField] private Vector3 _extendSize;

    void Start()
    {
        _leftBound = new Vector3(-bound, transform.position.y, transform.position.z); //提前设置好玩家可移动的边缘坐标，所以玩家
        _rightBound = new Vector3(bound, transform.position.y, transform.position.z);//所以当玩家尝试向外移动时可以被直接置入在这个坐标中
        _moveDistance = new Vector3(0, 0, 0);

        _shrinkSize = new Vector3(2f, 0.3f, 1);
        _regularSize = new Vector3(2.5f, 0.3f, 1);
        _extendSize = new Vector3(3.5f, 0.3f, 1);
        LengthReset();
    }

    private void OnEnable()
    {
        PlatformShrink.shrink += PowerUpShrinkReceived;
        PlatformExtend.extend += PowerUpExtendReceived;
    }

    private void OnDisable()
    {
        PlatformShrink.shrink -= PowerUpShrinkReceived;
        PlatformExtend.extend -= PowerUpExtendReceived;
    }

    private void OnMove(InputValue moveVal)
    {
        Vector2 movementVector = moveVal.Get<Vector2>();
        _movementX = movementVector.x;
    }

    private void OnFire(InputValue fire)
    {
        ball.Fire();
    }

    private void FixedUpdate()
    {
        _moveDistance.x = _movementX * speed;
        float newPositionX = transform.position.x + _moveDistance.x;
        if (newPositionX < -bound)
        {
            transform.position = _leftBound;
        } else if (newPositionX > bound)
        {
            transform.position = _rightBound;
        } else
        {
            transform.position += _moveDistance;
        }
    }

    private void PowerUpShrinkReceived()
    {
        switch (_currentState)
        {
            case State.Shrinking:
                break;
            case State.RegularSize:
                _currentState = State.Shrinking;
                break;
            case State.Extending:
                _currentState = State.RegularSize;
                break;
        }
        LengthRefresh();
    }

    private void PowerUpExtendReceived()
    {
        switch (_currentState)
        {
            case State.Shrinking:
                _currentState = State.RegularSize;
                break;
            case State.RegularSize:
                _currentState = State.Extending;
                break;
            case State.Extending:
                break;
        }
        LengthRefresh();
    }

    private void LengthRefresh()
    {
        switch (_currentState)
        {
            case State.Shrinking:
                transform.localScale = _shrinkSize;
                break;
            case State.RegularSize:
                transform.localScale = _regularSize;
                break;
            case State.Extending:
                transform.localScale = _extendSize;
                break;
        }
    }

    private void LengthReset()
    {
        _currentState = State.RegularSize;
        LengthRefresh();
    }
}
