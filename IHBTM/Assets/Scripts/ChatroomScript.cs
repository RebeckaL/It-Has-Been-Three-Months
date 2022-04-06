using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChatroomScript : MonoBehaviour
{
    [SerializeField] GameObject chatMenu;
    [SerializeField] DialogueManager dm;

    private void OnDisable()
    {
        foreach(Transform child in transform)
        {
            if(child != transform.GetChild(transform.childCount - 1))
            {
                child.gameObject.SetActive(false);
            }
        }
    }

    public void GoBackToMenu()
    {
        chatMenu.SetActive(true);
        foreach (Transform child in transform)
        {
            if (child != transform.GetChild(transform.childCount - 1))
            {
                child.gameObject.SetActive(false);
            }
        }
    }
}
