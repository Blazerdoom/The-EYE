using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource source;
    // Start is called before the first frame update
    void Awake()
    {
        if (source == null) return;

        // Start partway into the track, but never past the clip's length.
        if (source.clip != null)
            source.time = Mathf.Min(26f, source.clip.length - 0.1f);

        source.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
