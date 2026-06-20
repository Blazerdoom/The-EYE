using UnityEngine;

public class PuzzleGate : MonoBehaviour
{
    public static PuzzleGate Instance;
    public int puzzlesNeeded = 1;
    public GameObject act2Zone;        // green zone (optional)
    public GameObject[] blockers;      // the yellow Blocker walls

    int done = 0;

    void Awake()
    {
        Instance = this;
        if (act2Zone != null) act2Zone.SetActive(false);
    }

    public void PuzzleSolved()
    {
        done++;
        Debug.Log("puzzles done: " + done);
        if (done >= puzzlesNeeded)
        {
            if (act2Zone != null) act2Zone.SetActive(true);
            foreach (GameObject b in blockers)
                if (b != null) b.SetActive(false);
            Debug.Log("exit unlocked");
        }
    }
}