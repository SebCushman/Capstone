using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Dialogue
{
    public string NPC_name;
    public NPC owner;

    [TextArea(1,5)]
    public string[] sentences;
}
