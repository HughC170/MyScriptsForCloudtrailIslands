using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class heartUI : MonoBehaviour {

    public CharacterList Characters;
    public MenuHandler menu;
    public Transform[] heartparents;
    // Use this for initialization
    void Start () {
        menu = this.gameObject.GetComponentInParent<MenuHandler>();
        heartparents = this.gameObject.GetComponentsInChildren<Transform>();
	}
	
	// Update is called once per frame
	void Update () {
        uiupdate();
		//menu.currentpage
	}

    void uiupdate()
    {
        if(menu.currentpage == 4)
        {
            for(int x = 1; x < 7; x++)
            {
                heartparents[x].gameObject.SetActive(true);
            }
            for (int y = 7; y < 11; y++)
            {
                heartparents[y].gameObject.SetActive(false);
            }
        }
        else if(menu.currentpage == 5)
        {
            for (int y = 1; y < 7; y++)
            {
                heartparents[y].gameObject.SetActive(false);
            }
            for (int x = 7; x < 11; x++)
            {
                heartparents[x].gameObject.SetActive(true);
            }
        }
        else
        {
            for(int y = 1; y < 11; y++)
            {
                heartparents[y].gameObject.SetActive(false);
            }
        }
        for(int x = 0; x < heartparents.Length; x++)//going through each gameobject
        {//lowest value of heart value is 1
            for (int z = 0; z < Characters.characters.Count; z++)//going through each of the characters per element of the gameobjects
            {
                if (heartparents[x].gameObject.name == Characters.characters[z].CharacterName)
                {
                    if (Characters.characters[z].HeartValue > -1)
                    {
                        for (int y = Characters.characters[z].HeartValue - 1; y > -1; y--)
                        {
                            heartparents[x].GetChild(y).gameObject.SetActive(true);
                        }
                    }
                }
            }
        }
    }
}
