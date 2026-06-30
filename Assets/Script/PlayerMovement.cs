using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody2D rb;
    public float speed = 10;
    float xMove;
    float yMove;
    public AudioClip footstepClip;
    private float _stepTimer;

    void Start()
    {
        
    }

    void Update()
    {

        xMove = Input.GetAxisRaw("Horizontal");
        yMove = Input.GetAxisRaw("Vertical");
        running();

        
    }

    private void FixedUpdate()
    {
        Vector2 moveTotal = new Vector2(xMove, yMove);
        rb.velocity = moveTotal * speed;
    }

    private void running()
    {
        if ((xMove != 0 || yMove != 0))
        {
            _stepTimer -= Time.deltaTime;
            if (_stepTimer <= 0f)
            {
                if (GameAudio.Instance != null) GameAudio.Instance.PlayFootstep(footstepClip);
                _stepTimer = 0.4f;
            }
        }
        if (Input.GetKey(KeyCode.LeftShift))
        {
            speed = 20;

        }
        else
        {
            speed = 10;
        }
    }
}
