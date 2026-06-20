using UnityEngine;

/// <summary>
/// Universal audio manager. Handles background music (loops) and sound effects (one-shots).
///
/// SETUP (per scene):
///  1. Create an empty GameObject called "GameAudio".
///  2. Add this script to it.
///  3. It will auto-create its own AudioSources, OR you can drag your own into the slots.
///  4. Drag your music clip into "sceneMusic" to auto-play on scene start.
///
/// USE FROM CODE:
///  GameAudio.Instance.PlaySFX(myClip);            // play a one-shot
///  GameAudio.Instance.PlayFootstep(stepClip);     // footstep with pitch variation
///  GameAudio.Instance.PlayMusic(clip);            // swap the looping music
/// </summary>
public class GameAudio : MonoBehaviour
{
    public static GameAudio Instance;

    [Header("Music that plays automatically when this scene starts (optional)")]
    public AudioClip sceneMusic;

    [Header("Volumes (0 to 1)")]
    [Range(0f, 1f)] public float musicVolume = 0.5f;
    [Range(0f, 1f)] public float sfxVolume = 1f;

    private AudioSource _musicSource;
    private AudioSource _sfxSource;

    void Awake()
    {
        Instance = this;

        // Create two dedicated audio sources: one for looping music, one for one-shot SFX.
        _musicSource = gameObject.AddComponent<AudioSource>();
        _musicSource.loop = true;
        _musicSource.playOnAwake = false;
        _musicSource.volume = musicVolume;

        _sfxSource = gameObject.AddComponent<AudioSource>();
        _sfxSource.loop = false;
        _sfxSource.playOnAwake = false;
        _sfxSource.volume = sfxVolume;
    }

    void Start()
    {
        // Auto-play the scene's music if one is assigned.
        if (sceneMusic != null)
            PlayMusic(sceneMusic);
    }

    /// <summary>Swap and play looping background music.</summary>
    public void PlayMusic(AudioClip clip)
    {
        if (clip == null) return;
        _musicSource.clip = clip;
        _musicSource.volume = musicVolume;
        _musicSource.Play();
    }

    /// <summary>Stop the music.</summary>
    public void StopMusic()
    {
        _musicSource.Stop();
    }

    /// <summary>Play a one-shot sound effect (puzzle ding, buzz, etc.).</summary>
    public void PlaySFX(AudioClip clip)
    {
        if (clip == null) return;
        _sfxSource.pitch = 1f; // reset pitch in case a footstep changed it
        _sfxSource.PlayOneShot(clip, sfxVolume);
    }

    /// <summary>Play a footstep with slight random pitch so repeated steps don't sound robotic.</summary>
    public void PlayFootstep(AudioClip clip)
    {
        if (clip == null) return;
        _sfxSource.pitch = Random.Range(0.9f, 1.1f);
        _sfxSource.PlayOneShot(clip, sfxVolume);
    }
}
