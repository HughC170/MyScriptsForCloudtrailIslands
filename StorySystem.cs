using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;

public class StorySystem : MonoBehaviour {

    public DayNightCycle daynight;
    public manageTime timer;
    public CharacterList characterlist;
    public string BlockToExecute;
    public string currentcharacter;
    public int TempHeart;
    public Flowchart charflowchart;

    void Start () {
    }
	
	void Update () {
        heartsystem();
	}

    public void heartsystem()
    {
        for(int x = 0; x < characterlist.characters.Count; x++)
        {
            if (characterlist.characters[x].HeartEXP > 399)
            {//progress bar
                characterlist.characters[x].HeartValue++;
                characterlist.characters[x].HeartEXP = characterlist.characters[x].HeartEXP - 400;
            }
        }
    }

    void Patrolling()
    {
        switch (daynight.currentTime)
        {
            case DayNightCycle.TimePeriod.Morning:
                
                break;
            case DayNightCycle.TimePeriod.Afternoon:
                
                break;
            case DayNightCycle.TimePeriod.Evening:
                
                break;
            case DayNightCycle.TimePeriod.Night:
                
                break;
        }
    }
   
    public void Dialogue(string character)//character tag name being passed through here
    {
        for(int x = 0; x < characterlist.characters.Count; x ++)
        {
            if (characterlist.characters[x].CharacterName == character)
            {
                TempHeart = characterlist.characters[x].HeartValue;
                BlockToExecute = character + "_" + timer.CurrentDay + "_" + daynight.currentTime;// + "_" + TempHeart;//this is setting up the flowchart block to call. Gathers all intel that dialogue is dependent on.
                //characterlist.characters[x].characterflowchart.ExecuteBlock(BlockToExecute);
                charflowchart = GameObject.Find(characterlist.characters[x].characterflowchart).GetComponent<Flowchart>();//grabbing the flowchart of the character class form characterlist
                charflowchart.SetIntegerVariable("FungusHeartValue", TempHeart);
                if (characterlist.characters[x].greeting == true)
                {
                    charflowchart.ExecuteBlock(character + "_intro");
                }
                else
                {
                    charflowchart.ExecuteBlock(BlockToExecute);
                }
                currentcharacter = character;
            }
            else
            {
                //Debug.LogError("Can't match character name");
            }
        }
    }
}
