using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityChasing : MonoBehaviour
{
    public Rigidbody2D rb;
    public float speed = 10;
    float xMove;
    float yMove;
    public GameObject GameOverUI;
    public GameObject diedCam;
    public AudioListener diedCamAudio;
    void Start()
    {

    }

    void Update()
    {
        xMove = 1;
        yMove = 0;

    }

    private void FixedUpdate()
    {
        Vector2 moveTotal = new Vector2(xMove, yMove);
        rb.velocity = moveTotal * speed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            
            collision.gameObject.SetActive(false);
            diedCam.gameObject.SetActive(true);
            
            TriggerGameOver();
        }
    }

    void TriggerGameOver()
    {
        GameOverUI.gameObject.SetActive(true);
    }
}
