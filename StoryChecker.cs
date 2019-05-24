using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Fungus;

public class StoryChecker : MonoBehaviour {

    public StorySystem story;

    public bool talking = false;
    public bool DemoMode = true;
    public bool panend = false;
    public Flowchart flowchart;
    public Flowchart Boneflowchart;
    public Text PressE;
    public Image PressEIcon;
    public int progression = 0;
    public int progressionv2 = 0;
    public int progressionBone = 0;
    public string currentcharacter;
    public Animator animController;
    public CharControl avatar;
    public AchievementHunter achievements;
    public CharacterList characters;
    public Camera playercam;
    public Camera camerapan;
    public Vector3 campoints;
    public bool cameraon = false;
    [Tooltip("if ticked gliding is enabled immediately")]
    public bool skipstart;

    DayNightCycle dayNightScript;
    CharControl charControlScript;
    public SfxControl sfxControlScript;

    private void Awake()
    {
        sfxControlScript = GameObject.Find("SFXControl").GetComponent<SfxControl>();
        charControlScript = GetComponent<CharControl>();

        dayNightScript = GetComponent<DayNightCycle>();
        animController = transform.GetChild(1).GetComponent<Animator>();
        if (skipstart == false)
        {
            avatar.jumpVelocity = 0f;
        }
        campoints = camerapan.transform.position;
        /*if(DemoMode == false)
        {
            camerapan = null;
        }*/
    }

    public void Update()
    {
        if(cameraon == true)
        {
            camerapan.transform.position = campoints;
            
                campoints.x -= 0.1f;
                if(campoints.x < -225 || panend == true)
            {
                cameraon = false;
                flowchart.ExecuteBlock("Quickcamchange");
                panend = false;
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Monika" || other.gameObject.tag == "Everett" || other.gameObject.tag == "Felicia" || other.gameObject.tag == "Bone" || other.gameObject.tag == "Fauna" || other.gameObject.tag == "Waylon" || other.gameObject.tag == "Adley" || other.gameObject.tag == "Ezra" || other.gameObject.tag == "Pippin" || other.gameObject.tag == "Lorelei")
        {
            //PressE.text = "Press E to talk with " + other.gameObject.tag;
            //PressE.enabled = true;//ui enabled
            if (charControlScript.isGrounded) //Kris' code - there were instances where the player would be floating mid air when talking
            {
                PressEIcon.enabled = true;
            }

        }
    }

    public void AllCharactersReady()
    {
        for(int x = 0; x < characters.characters.Count; x++)
        {
            characters.characters[x].hpready = true;
        }
    }

    public void Reenabletext()
    {
        talking = false;
        story.currentcharacter = "blank";
        //PressE.enabled = true;
        PressEIcon.enabled = true;
        avatar.jumpVelocity = 9f;
        avatar.enabled = true;
        questchecker();
        Cursor.visible = false;
        dayNightScript.isTalking = false;
        sfxControlScript.ButtonOut(); // play button SFX
        charControlScript.UnfreezeCam();
        heartpointincrease();
    }

    public void heartpointincrease()
    {
            for (int x = 0; x < characters.characters.Count; x++)
            {
                if (currentcharacter == characters.characters[x].CharacterName)
                {
                if (characters.characters[x].hpready == true)
                {
                    characters.characters[x].HeartEXP += 14;
                    characters.characters[x].hpready = false;
                    characters.characters[x].greeting = false;
                }
                }
            }
    }

    public void Norepeat()
    {
        progression++;
    }

    public void Bone()
    {
        progressionBone++;
}

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Monika" || other.gameObject.tag == "Everett" || other.gameObject.tag == "Felicia" || other.gameObject.tag == "Bone" || other.gameObject.tag == "Fauna" || other.gameObject.tag == "Waylon" || other.gameObject.tag == "Adley" || other.gameObject.tag == "Ezra" || other.gameObject.tag == "Pippin" || other.gameObject.tag == "Lorelei")
        {
            currentcharacter = other.gameObject.tag;  
                if (Input.GetButtonDown("talk")
                && charControlScript.isGrounded) //Kris' code - there were instances where the player would be floating mid air when talking
            {
                Cursor.visible = true;
                charControlScript.FreezeCam();
                sfxControlScript.ButtonIn(); //play button SFX
                animController.SetInteger("Movement", 0);
                talking = true;
                    avatar.enabled = false;
                    //avatar.jumpVelocity = 0f;//so that skipping through dialogue wont make character jump
                    //PressE.enabled = false;
                PressEIcon.enabled = false;
                if (DemoMode == true)//this may be temporary as we may have this dialogue at the start of the end product
                {
                    if (other.gameObject.tag == "Monika")
                    {
                        switch (progression)
                        {
                            case 0:
                                flowchart.ExecuteBlock("Monika Intro");//just put down the name of the fungus block 
                                break;
                            case 1:
                                flowchart.ExecuteBlock("Flower quest");
                                achievements.flowerready = true;
                                break;
                            case 2:
                                flowchart.ExecuteBlock("Find some items");
                                break;
                            case 3:
                                flowchart.ExecuteBlock("Monika Q&A");
                                break;
                        }
                    }
                    if (other.gameObject.tag == "Bone")
                    {
                        if (DemoMode == true)//this may be temporary as we may have this dialogue at the start of the end product
                        {
                            if (Input.GetButton("talk"))
                            {
                                avatar.enabled = false;
                                //PressE.enabled = false;
                                PressEIcon.enabled = false;
                                if (progressionBone == 10)
                                {
                                    progressionBone = 1;
                                }
                                Boneflowchart.SetIntegerVariable("Prog", progressionBone);
                                Boneflowchart.ExecuteBlock("Bone dialogue");
                            }
                        }
                    }
                }
                else
                {
                    //Debug.LogError("dis aint supposed to work");
                    story.Dialogue(other.gameObject.tag);
                    dayNightScript.isTalking = true;
                    
                }
            }
            
        }
        /*else
        {
            PressE.enabled = false;
        }*/
    }

    public void CampanOn()
    {
        playercam.enabled = false;
        camerapan.enabled = true;
        cameraon = true;
    }

    public void Transition()
    {
        panend = true;
    }

    public void CampanOff()
    {
        //flowchart.ExecuteBlock("Quickcamchange");
        playercam.enabled = true;
        camerapan.enabled = false;
    }

    public void questchecker()
    {
        switch (progressionv2)
        {
            case 0:
        if (achievements.flowerready == true && achievements.flowerready2 == true)
        {
            progression = 2;
                    progressionv2++;
        }
                break;
        } 
    }

    void OnTriggerExit(Collider other)
    {
        // PressE.enabled = false;
        PressEIcon.enabled = false;
    }
}
