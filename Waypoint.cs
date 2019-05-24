using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoint : MonoBehaviour
{

    public float DrawRadius = 1.0f;
    public float WaitingTime = 0f;
    public bool setpatrol = false;
    public float characterspeed = 3f;
    [Space(10)]
    public bool sitDown = false;
    // Use this for initialization
    public virtual void OnDrawGizmos()//virtual methods are used for overriding, look into this more later
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, DrawRadius);
    }
}
