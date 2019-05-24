using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Item
{
    public string ItemName;
    public enum rarity {one, two, three};
    public rarity stars;
    public int statamount = 0;
    public int currentamount = 0;
}