using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PuzzleManager : MonoBehaviour
{
    // this is for deleteing the tilepuzzle when done
    public GameObject tilepuzzleUI;

    // this is for getting checkmarkUI to confirm they did the puzzle right
    public GameObject checkMarkUI;
    public GameObject XmarkUI;


    // this is for list of what tile that is correct
    public List<GameObject> correctTiles = new List<GameObject>(); //holding the right one
    public int correctCounter; // indexing for correcetTile list

    public AudioClip correctSound;
    public AudioClip wrongSound;



    //pressedTile is a parameter where the tile that i press will be check within the list of correct tiles
    //clickedTile is a function that tilebutton.cs will handle
    public void clickedTile(GameObject pressedTile)
    {
        // this condition check the player click the tile then compare it to the correctTile list element in unity public data

        if (pressedTile == correctTiles[correctCounter])
        {
            correctCounter++;
            Juice.Pop(pressedTile.transform);
            GameAudio.Instance.PlaySFX(correctSound);

            Debug.Log("This is the correct one");

            if (correctCounter >= correctTiles.Count)
            {
                Debug.Log("puzzle completed");
                PuzzleGate.Instance.PuzzleSolved();
                CameraShake.Instance.Shake(0.3f, 0.2f);
                Destroy(tilepuzzleUI.gameObject);
                checkMarkUI.gameObject.SetActive(true);
                //ui goes out and 
                Invoke("checkMarkUIRemover", 1f );
                // pop a ui wth an icon checkmark
                // disable the pad
            }
        }
        else // if somehow the player click the wrong one it will reset to element 0
        {
            Debug.Log("wrong one");
            CameraShake.Instance.Shake(0.2f, 0.25f);
            GameAudio.Instance.PlaySFX(wrongSound);
            XmarkUI.gameObject.SetActive(true);
            Invoke("Xmark", 0.5f);
            correctCounter = 0;
        }
    }
    // Update is called once per frame
    void checkMarkUIRemover()
    {
        Destroy(checkMarkUI);
    }
    void Xmark()
    {
        XmarkUI.gameObject.SetActive(false);
    }
}
