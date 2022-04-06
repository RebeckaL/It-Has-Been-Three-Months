using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
[System.Serializable]
public class GateCondition : ScriptableObject
{
    public string condition;
    public bool isTrue = false;

    private void OnEnable()
    {
        isTrue = false;
    }
}
