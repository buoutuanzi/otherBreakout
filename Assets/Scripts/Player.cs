using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    private float movementX;
    public float speed;
    public float bound;
    private Rigidbody2D rb;
    public Ball ball;

    private Vector3 leftBound;
    private Vector3 rightBound;

    Vector3 moveDistance;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        leftBound = new Vector3(-bound, transform.position.y, transform.position.z);
        rightBound = new Vector3(bound, transform.position.y, transform.position.z);
        moveDistance = new Vector3(0, 0, 0);
    }

    private void OnMove(InputValue moveVal)
    {
        Vector2 movementVector = moveVal.Get<Vector2>();
        movementX = movementVector.x;
    }

    private void OnFire(InputValue fire)
    {
        ball.fire();
    }

    private void FixedUpdate()
    {
        moveDistance.x = movementX * speed;
        float newPositionX = transform.position.x + moveDistance.x;
        if (newPositionX < -bound)
        {
            transform.position = leftBound;
        } else if (newPositionX > bound)
        {
            transform.position = rightBound;
        } else
        {
            transform.position += moveDistance;
        }
    }
}
