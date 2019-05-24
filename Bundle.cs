using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;

[System.Serializable]
public class Bundle
{
    public string CharacterName;
    public string[] ItemName;
    [Tooltip("Make sure these match the the item names according to order")]
    public int[] ItemRequirements;
}
