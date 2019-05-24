using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Itemslot//this is our vessel for each individual item
{
    public string itemname;
    public int order;
	
	public Itemslot(string newitemname, int placement)
    {
        itemname = newitemname;
        order = placement;
	}
}
