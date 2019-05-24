using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SetterObject")]
public class SetterScript : ScriptableObject
{
    public bool Loaded = false;
    public bool GameLoadAvailable = false;
    public bool NewGame = false;
}