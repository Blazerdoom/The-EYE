using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource source;
    // Start is called before the first frame update
    void Awake()
    {
        source.time = 26f;
        source.Play();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
