using UnityEngine;

/// Loops through a list of sprites on a timer. Drop it on any object that
/// has a SpriteRenderer (e.g. the eye or the entity) and drag the frames in.
/// Turn on Ping Pong for a back-and-forth scan (e.g. eye: left, middle, right, middle...).
[RequireComponent(typeof(SpriteRenderer))]
public class SpriteLoop : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;

    [Header("Drag the frames in order")]
    public Sprite[] frames;

    [Header("Frames per second")]
    public float frameRate = 4f;

    [Header("Bounce back and forth instead of looping (good for the eye)")]
    public bool pingPong = false;

    float _timer;
    int _frame;
    int _dir = 1;

    void Reset()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        // Show the first frame right away instead of whatever placeholder is set.
        if (spriteRenderer != null && frames != null && frames.Length > 0)
            spriteRenderer.sprite = frames[0];
    }

    void Update()
    {
        if (frames == null || frames.Length == 0)
            return;

        _timer += Time.deltaTime;
        if (_timer < 1f / frameRate)
            return;

        _timer -= 1f / frameRate;

        if (pingPong)
        {
            _frame += _dir;
            if (_frame >= frames.Length - 1) { _frame = frames.Length - 1; _dir = -1; }
            else if (_frame <= 0) { _frame = 0; _dir = 1; }
        }
        else
        {
            _frame = (_frame + 1) % frames.Length;
        }

        spriteRenderer.sprite = frames[_frame];
    }
}
