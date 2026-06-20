using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeDespawn : MonoBehaviour
{
    public GameObject spike;
    public Rigidbody2D rbSpike;
    private void Awake()
    {
        float randomFreezeTime = Random.Range(0f, 3f);
        Invoke("rbRemover", randomFreezeTime);
        Invoke("despawn", 4f);
    }

    void despawn()
    {
        Destroy(spike);
    }
    void rbRemover()
    {
        Destroy(rbSpike);
    }
}
