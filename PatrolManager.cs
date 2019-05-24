using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolManager : MonoBehaviour {

    public DayNightCycle timeregister;
    public NPCWalkPatrol[] walkers;

	public void patrol()
    {
        for (int x = 0; x < walkers.Length; x++)
        {
            walkers[x].Walkpointscollected(timeregister.CurrentPatrolPoint);
        }
    }
}
