using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Ball : MonoBehaviour
{
    private bool isActive;
    private Vector3 ballPosition;
    private Vector2 ballInitialForce;
    private Rigidbody2D rb;
    public GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        ballInitialForce = new Vector2(200f, 450f);
        isActive = false;
        ballPosition = transform.position;
        rb = GetComponent<Rigidbody2D>();
    }

    public void fire()
    {
        if (!isActive)
        {
            rb.isKinematic = false;
            Debug.Log("ss");
            rb.AddForce(ballInitialForce);
            isActive = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!isActive & player)
        {
            ballPosition.x = player.transform.position.x;
            transform.position = ballPosition;
        }

        if (isActive & transform.position.y < -5.5)
        {
            isActive = false;
            ballPosition.x = player.transform.position.x;
            ballPosition.y = -4.258f;
            transform.position = ballPosition;
            rb.isKinematic = true;
            player.SendMessage("TakeLife");
        }
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player" & isActive)
        {
            rb.velocity = Vector2.zero;
            float xDistancePercent = (transform.position.x - player.transform.position.x) / 1.5f;
            Vector2 ballReflectForce = new Vector2(xDistancePercent * 300, 450);
            rb.AddForce(ballReflectForce);
        }
    }
}
