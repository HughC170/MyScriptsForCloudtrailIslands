using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;

[System.Serializable]
public class Character
{
    public string CharacterName;
    public int HeartEXP;
    public int HeartValue;
    public bool hpready = true;
    public bool greeting = false;
    [Tooltip("Type in the name of the character flowchart as it is in the hierarchy. e.g Monika Flowchart")]
    public string characterflowchart;
}
