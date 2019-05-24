using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GEAR;

public class AchievementHunter : MonoBehaviour {

    //if we're going to setup a system, for this we may need to categorise achievements

    public Itemslots dagear;
    public CharControl control;
    public StoryChecker story;
    #region MonikaQ1
    public bool flowerready = false;
    public bool flowerready2 = false;
    #endregion
    #region shinyrocks
    public GameObject[] shinyrocks;
    public int steppedon = 0;
    #endregion
    #region rings
    public GameObject[] rockrings;
    public int ringpoints = 0;
    #endregion
    #region gems
    int constantcheck = 0;
    public int currentcheck;
    #endregion

    #region booltickboxes
    public bool shinyrocksfound = false;
    public bool ringscompleted = false;
    public bool gemsobtained = false;
    public bool MonikaQ1 = false;
    #endregion
    #region tickboxes
    public GameObject shinytick;
    public GameObject ringtick;
    public GameObject gemtick;
    public GameObject monikatick;
    #endregion

    private void Awake()
    {
            shinyrocks = GameObject.FindGameObjectsWithTag("shiny rocks");
        rockrings = GameObject.FindGameObjectsWithTag("rings");
    }

    void OnCollisionStay(Collision collision)
    {
        if(collision.gameObject.tag == "shiny rocks")
        {
            for(int x = 0; x < shinyrocks.Length; x ++)
            {
                if(shinyrocks[x] == collision.gameObject && control.isGrounded == true)
                {
                    Debug.Log("Hit");
                    shinyrocks[x] = null;
                    steppedon++;
                }
            }
            if(steppedon == 3)
            {
                shinyrocksfound = true;
                shinytick.SetActive(true);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "rings")
        {
            Debug.Log("Hittest 2");
            for (int x = 0; x < rockrings.Length; x++)
            {
                if (rockrings[x] == other.gameObject)
                {
                    Debug.Log("Hit2");
                    rockrings[x] = null;
                    ringpoints++;
                }
            }
            if (ringpoints == 3)
            {
                ringscompleted = true;
                ringtick.SetActive(true);
            }
        }
    }
    public void Itemcheck()
    {
        GemChecker();
        FlowerChecker();
    }
    public void GemChecker()
    {
            for (int x = 0; x < dagear.Slots.Count; x++)
            {
                if (dagear.Slots[x].itemname == "Diamond")
                {
                    constantcheck++;
                }
                if (x == dagear.Slots.Count - 1)
                {
                    currentcheck = constantcheck;
                    Debug.Log("currentcheck : " + currentcheck);
                    constantcheck = 0;
                }
            }
        if (currentcheck == 3)
        {
            gemsobtained = true;
            gemtick.SetActive(true);
        }
    }
    public void FlowerChecker()
    {
            for (int x = 0; x < dagear.Slots.Count; x++)
            {
                if (dagear.Slots[x].itemname == "Flower")
                {
                    flowerready2 = true;
                story.questchecker();
                }
            }
    }

    public void QuestComplete()
    {
        MonikaQ1 = true;
        monikatick.SetActive(true);
    }
}
