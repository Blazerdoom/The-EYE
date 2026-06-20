using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TilePuzzleTrigger : MonoBehaviour
{
    public GameObject eButtonUI;
    public GameObject tilePuzzleUI;
    

    bool isInArea = false;


    private void Update()
    {
        checkInteraction();
    }
    // this for pad
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            eButtonUI.gameObject.SetActive(true);
            isInArea = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            eButtonUI.gameObject.SetActive(false);
            isInArea = false;
        }
    }
    // if player gets near they trigger the puzzle ui
    private void checkInteraction()
    {
        
        if (Input.GetKey(KeyCode.E) && isInArea == true)
        {
            Debug.Log("e is pressed");
            tilePuzzleUI.gameObject.SetActive(true);
        }

        
    }
}
