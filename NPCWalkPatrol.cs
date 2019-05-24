using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPCWalkPatrol : MonoBehaviour {

    public StorySystem story;
    public StoryChecker story2;
    public CharControl controller;
    Rigidbody rb;
    public float timer = 0f;
    public Waypoint timerscript;
    public int setpatrolint = 2;
    public bool once = true;
    public float speed = 3f;

    public GameObject[] WalkPoints;
    public Transform[] WalkPointskid;
    public int randomizer;
    public NavMeshAgent _navMeshAgent;
    public Vector3 targetpos;

    public NPC_AnimationManager NPCAnimScript;

    void Start () {
        rb = GameObject.Find("Avatar").GetComponent<Rigidbody>();
        story = GameObject.Find("StorySystemObject").GetComponent<StorySystem>();
        story2 = GameObject.Find("Avatar").GetComponent<StoryChecker>();
        WalkPoints = GameObject.FindGameObjectsWithTag(this.tag + " Waypoint");
        for (int x = 0; x < WalkPoints.Length; x++)
        {
            WalkPoints[x] = GameObject.Find(this.tag + " Waypoints " + (x + 1));
        }
        WalkPointskid = WalkPoints[0].GetComponentsInChildren<Transform>();//Not needed really tbh
        randomizer = Random.Range(1, WalkPointskid.Length);
        _navMeshAgent = this.GetComponent<NavMeshAgent>();
        NPCAnimScript = GetComponent<NPC_AnimationManager>();
    }
	
	void Update () {
        if (_navMeshAgent.remainingDistance <= 0.1f)
        {
            if (once == true)
            {
                if (setpatrolint < WalkPointskid.Length - 1)
                {
                    setpatrolint++;
                    once = false;
                }
                else
                {
                    setpatrolint = 2;
                    once = false;
                }
            }
                randomizer = Random.Range(2, WalkPointskid.Length);
            timer += Time.deltaTime;
            //timer = 0f;
        }
        if (story2.talking == true && story.currentcharacter == this.tag)
        {
            Debug.Log("not meant to be moving");
            _navMeshAgent.isStopped = true;
            _navMeshAgent.SetDestination(this.transform.position);
            rb.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezeRotationY;
        }
        else
        {
            timerscript = WalkPointskid[randomizer].GetComponent<Waypoint>();//Grabs script of current waypoint destination
            if (timer > timerscript.WaitingTime)
            {
                _navMeshAgent.isStopped = false;
                if (timerscript.setpatrol == false)
                {
                    targetpos = WalkPointskid[randomizer].transform.position;
                    speed = timerscript.characterspeed;
                    NPCAnimScript.sitting = timerscript.sitDown;
                    if (NPCAnimScript.sitting)
                    {
                        _navMeshAgent.updateRotation = false;
                        this.gameObject.transform.rotation = Quaternion.Euler(0, timerscript.gameObject.transform.eulerAngles.y, 0);
                    } else
                    {
                        _navMeshAgent.updateRotation = true;
                    }
                }
                else//for setpatrol
                {
                    timerscript = WalkPointskid[setpatrolint].GetComponent<Waypoint>();
                    speed = timerscript.characterspeed;
                    targetpos = WalkPointskid[setpatrolint].transform.position;
                    once = true;
                }
                _navMeshAgent.SetDestination(targetpos);
                _navMeshAgent.speed = speed;
                timer = 0f;
            }
        }
    }

    public void Walkpointscollected(int setter)
    {
        setpatrolint = 2;
        WalkPointskid = WalkPoints[setter].GetComponentsInChildren<Transform>();//if we get timer of this waypoint
        _navMeshAgent.Warp(WalkPointskid[2].transform.position);//teleports to first warppoint
        //schedules should restart after a week due to this being dependent on currentpatrolpoint which resets at the end of the week
    }
}
