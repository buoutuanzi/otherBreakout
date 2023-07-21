using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    private float _movementX;
    public float speed;
    public float bound;
    public Ball ball;

    private Vector3 _leftBound;
    private Vector3 _rightBound;

    private Vector3 _moveDistance;

    void Start()
    {
        _leftBound = new Vector3(-bound, transform.position.y, transform.position.z); //提前设置好玩家可移动的边缘坐标，所以玩家
        _rightBound = new Vector3(bound, transform.position.y, transform.position.z);//所以当玩家尝试向外移动时可以被直接置入在这个坐标中
        _moveDistance = new Vector3(0, 0, 0);
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
}
