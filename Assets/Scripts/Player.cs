using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    private float _movementX;
    private Vector3 _moveDistance;
    public float speed;

    public float bound;
    private Vector3 _leftBound;
    private Vector3 _rightBound;
    public Ball ball;

    private int _currentSizeIndex;
    [SerializeField] private Vector3[] _sizes;

    void Start()
    {
        _moveDistance = new Vector3(0, 0, 0);

        Vector3 _shrinkSize = new Vector3(2f, 0.3f, 1);
        Vector3 _regularSize = new Vector3(2.5f, 0.3f, 1);
        Vector3 _extendSize = new Vector3(3.5f, 0.3f, 1);
        _sizes = new Vector3[] {_shrinkSize, _regularSize, _extendSize};
        //依旧需要从小到大排列，变小index--，变大index++
        LengthReset();
    }

    private void OnEnable()
    {
        Powerup.shrink += PowerUpShrinkReceived;
        Powerup.extend += PowerUpExtendReceived;
    }

    private void OnDisable()
    {
        Powerup.shrink -= PowerUpShrinkReceived;
        Powerup.extend -= PowerUpExtendReceived;
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
        if (newPositionX < _leftBound.x)
        {
            transform.position = _leftBound;
        } else if (newPositionX > _rightBound.x)
        {
            transform.position = _rightBound;
        } else
        {
            transform.position += _moveDistance;
        }
    }

    private void PowerUpShrinkReceived()
    {
        if (_currentSizeIndex > 0)
        {
            _currentSizeIndex--;
            Debug.Log("platform is shorter!");
        }
        SetSizeBounds();
    }

    private void PowerUpExtendReceived()
    {
        if (_currentSizeIndex < 2)
        {
            _currentSizeIndex++;
            Debug.Log("platform is longer!");
        }
        SetSizeBounds();
    }

    private void LengthReset()
    {
        _currentSizeIndex = 1;
        SetSizeBounds();
    }

    private void SetSizeBounds()
    {//将设置平台大小和边缘放进一个方法中，因为他们之间关系接近，可减少代码重复
        transform.localScale = _sizes[_currentSizeIndex];
        float halfLength = transform.localScale.x / 2;
        _leftBound = new Vector3(-bound + halfLength, transform.position.y, transform.position.z);
        _rightBound = new Vector3(bound - halfLength, transform.position.y, transform.position.z);
    }
}
