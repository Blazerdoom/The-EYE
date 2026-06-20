using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitButtonTrigger : MonoBehaviour
{
    public GameObject tilePuzzleUI;
    public void ClosePuzzle()
    {
        //hover
        Debug.Log("this shit work");
        tilePuzzleUI.gameObject.SetActive(false);
    }
}
