using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileButton : MonoBehaviour
{
    public PuzzleManager puzzleManagerObject;

    public void onTileClicked ()
    {
        if (puzzleManagerObject != null)
        {
            // managetiles is the script on puzzlemanager that is on the gameobject
            //clickedTile is a function in puzzlemanager that has condition to check correct tiles are pressed
            //this.gameobject is basically object that has the script
            puzzleManagerObject.clickedTile(this.gameObject);
        }
    }
}
