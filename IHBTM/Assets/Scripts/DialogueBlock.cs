using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu]
[System.Serializable]
public class DialogueBlock : ScriptableObject
{
    public List<SingleLine> lines = new List<SingleLine>();
    public BlockType type;
    public bool openWithPic = false;
    public int conditionIndex;

    public enum BlockType
    {
        basic, branch, gate, parallel
    }
}

[System.Serializable]
public class SingleLine
{
    public string line;
    public MessageTypes type;
    public Sprite sprite;
}
