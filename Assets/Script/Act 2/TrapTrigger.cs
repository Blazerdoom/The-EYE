using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapTrigger : MonoBehaviour
{
    public Rigidbody2D spikeRb;
    public GameObject drops;
    public Transform spawn;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            for(int i = 40; i>0; i--)
            {
                float randomX = Random.Range(spawn.position.x - 20f, spawn.position.x + 20f);
                float randomY = Random.Range(spawn.position.y - 5f, spawn.position.y + 5f);
                Vector2 dropSpawn = new Vector2(randomX, randomY);
                Instantiate(drops, dropSpawn, Quaternion.identity);
                
            }
        }
    }
    
   
}
