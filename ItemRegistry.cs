using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item Registry")]
public class ItemRegistry : ScriptableObject
{
    public List<Item> items;
}
