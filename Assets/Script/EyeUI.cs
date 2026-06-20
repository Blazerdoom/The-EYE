using UnityEngine;
using UnityEngine.UI;

/// A UI eye (top of the screen) that darts left<->right FAST while the player
/// moves, and rests when the player is still. Put it on the eye's Image object.
[RequireComponent(typeof(Image))]
public class EyeUI : MonoBehaviour
{
    public Image eye;

    [Header("Eye look frames (e.g. look-left, look-right)")]
    public Sprite[] frames;

    [Header("How fast the eye darts while the player moves")]
    public float dartRate = 14f;

    [Tooltip("Shown when the player is standing still (optional)")]
    public Sprite idleFrame;

    Rigidbody2D _player;
    float _timer;
    int _frame;

    void Reset() { eye = GetComponent<Image>(); }

    void Start()
    {
        if (eye == null) eye = GetComponent<Image>();
        var p = GameObject.FindWithTag("Player");
        if (p != null) _player = p.GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (eye == null || frames == null || frames.Length == 0)
            return;

        bool moving = _player != null && _player.velocity.sqrMagnitude > 0.01f;

        if (!moving)
        {
            eye.sprite = idleFrame != null ? idleFrame : frames[0];
            _timer = 0f;
            return;
        }

        // Player is moving -> flick between the look frames quickly.
        _timer += Time.deltaTime;
        if (_timer >= 1f / dartRate)
        {
            _timer -= 1f / dartRate;
            _frame = (_frame + 1) % frames.Length;
            eye.sprite = frames[_frame];
        }
    }
}
