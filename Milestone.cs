using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Milestone
{
    public string MilestoneName;
    public int[] MilestoneRequirements = new int[10];
    public int[] CP = new int[10];
    public int currentrank = 0;
}
