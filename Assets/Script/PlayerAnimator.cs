using UnityEngine;

/// Swaps the player's sprite each frame to animate walking.
/// Down (S) uses frontFrames, Up (W) uses backFrames (falls back to front if empty),
/// Left/Right (A/D) use sideFrames, mirrored horizontally.
[RequireComponent(typeof(SpriteRenderer))]
public class PlayerAnimator : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public Rigidbody2D rb;

    [Header("Drag the frames in order 1..5")]
    public Sprite[] frontFrames;   // walking DOWN / toward camera (S)
    public Sprite[] backFrames;    // walking UP / away (W) - optional, falls back to front
    public Sprite[] sideFrames;    // walking sideways (A / D)

    [Header("Animation speed (frames per second)")]
    public float frameRate = 10f;

    [Header("Which frame to show when standing still (0-based: frame 2 = 1)")]
    public int idleFrame = 1;

    [Header("Tick this if the side walk faces the WRONG way")]
    public bool mirrorSideways = false;

    float _timer;
    int _frame;
    Sprite[] _current;

    void Reset()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        Vector2 v = rb.velocity;

        // Standing still: hold the idle frame of whatever we last faced.
        if (v.sqrMagnitude < 0.01f)
        {
            if (_current != null && _current.Length > idleFrame)
                spriteRenderer.sprite = _current[idleFrame];
            _frame = 0;
            _timer = 0f;
            return;
        }

        bool sideways = Mathf.Abs(v.x) > Mathf.Abs(v.y);

        if (sideways)
        {
            _current = sideFrames;
            // Flip so the character faces the way it's moving.
            bool movingRight = v.x > 0f;
            spriteRenderer.flipX = (movingRight == mirrorSideways);
        }
        else
        {
            spriteRenderer.flipX = false;
            bool movingUp = v.y > 0f;
            // Use back frames when walking up, if they exist; otherwise front.
            _current = (movingUp && backFrames != null && backFrames.Length > 0)
                ? backFrames
                : frontFrames;
        }

        if (_current == null || _current.Length == 0)
            return;

        // Advance the frame on a timer.
        _timer += Time.deltaTime;
        if (_timer >= 1f / frameRate)
        {
            _timer -= 1f / frameRate;
            _frame = (_frame + 1) % _current.Length;
        }

        if (_frame >= _current.Length) _frame = 0;
        spriteRenderer.sprite = _current[_frame];
    }
}
