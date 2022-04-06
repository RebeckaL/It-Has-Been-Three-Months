using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChatScript : MonoBehaviour
{
    [SerializeField] private DialogueManager dm;

    private void OnEnable()
    {
        dm.OnChatEnable();
    }

    private void OnDisable()
    {
        dm.OnChatDisable();
    }
}
