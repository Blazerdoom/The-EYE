using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunningSequence : MonoBehaviour
{
    public Rigidbody2D rb;
    public float speed = 10 ;
    float xMove;
    float yMove;
    public AudioClip footstepClip;
    private float _stepTimer;


    void Start()
    {

    }

    void Update()
    {
        xMove = 1;
        yMove = Input.GetAxisRaw("Vertical");
        _stepTimer -= Time.deltaTime;
        if (_stepTimer <= 0f)
        {
            if (GameAudio.Instance != null) GameAudio.Instance.PlayFootstep(footstepClip);
            _stepTimer = 0.3f;
        }

    }

    private void FixedUpdate()
    {
        Vector2 moveTotal = new Vector2(xMove, yMove);
        rb.velocity = moveTotal * speed;
    }

    
}