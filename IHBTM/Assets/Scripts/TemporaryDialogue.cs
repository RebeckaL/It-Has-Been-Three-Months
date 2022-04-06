using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
[System.Serializable]
public class TemporaryDialogue : ScriptableObject
{
    [SerializeField] private List<DialogueBlock> listOfBlocks = new List<DialogueBlock>();

    public List<DialogueBlock> ListOfBlocks { get { return listOfBlocks; } set { listOfBlocks = value; } }
}
