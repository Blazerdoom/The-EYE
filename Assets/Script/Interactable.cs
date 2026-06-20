using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public GameObject eButton;
    public GameObject bubbleChat;
    int clickCounter;

    bool isInArea = false;


    private void Update()
    {
        interactable();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            eButton.gameObject.SetActive(true);
            isInArea = true;
            interactable();
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            eButton.gameObject.SetActive(false);
            isInArea = false;
        }
    }
    private void interactable()
    {
        clickCounter = 1;
        if (Input.GetKey(KeyCode.E) && isInArea == true)
        {
            Debug.Log("e is pressed");
            bubbleChat.gameObject.SetActive(true);
        }
        if(Input.GetKey(KeyCode.Mouse1) && clickCounter > 0)
        {
            clickCounter--;
            bubbleChat.gameObject.SetActive(false);
        }
    }
}
