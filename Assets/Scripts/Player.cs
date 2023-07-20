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
    private int playerLives;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerLives = 3;
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
        Vector3 moveDistance = new Vector3(movementX, 0, 0).normalized * speed;
        float newPositionX = transform.position.x + moveDistance.x;
        if (newPositionX < -bound)
        {
            transform.position = new Vector3(-bound, transform.position.y, transform.position.z);
        } else if (newPositionX > bound)
        {
            transform.position = new Vector3(bound, transform.position.y, transform.position.z);
        } else
        {
            transform.position += moveDistance;
        }
    }
    
    // Update is called once per frame
    void Update()
    {
        if (playerLives <= 0)
        {
            SceneManager.LoadScene("Main");
        }
    }
    void OnGUI()
    {
        GUI.Label(new Rect(5.0f, 3.0f, 200.0f, 200.0f), "Live's: " + playerLives);
    }

    void TakeLife()
    {
        playerLives--;
    }
}
