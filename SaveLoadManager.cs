using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;//serializable attribute in system
using System.Runtime.Serialization.Formatters.Binary;//our binary formatter
using System.IO;//for opening files, open and save

public static class SaveLoadManager{
    public static void SavePlayer(saving Saving)//Parameters set up for taking player class, this passes on to playerdata where it can call player class values
    {
        BinaryFormatter bf = new BinaryFormatter();//our binary formatter
        FileStream Stream = new FileStream(Application.persistentDataPath + "/player.sav", FileMode.Create);// Opening a file to save to. In parameters you put where its being saved to (things in apostrophe is name of file) and how its being saved. Persistantdatapath saves to program files, its more permanent 
        PlayerData data = new PlayerData(Saving);//calls PlayerData method with saving script as parameter. PlayerData script will get variables of saving script and set them to stats values
        bf.Serialize(Stream, data);//this serializes our data (values) into a binary file. Literal version : binaryformattername.Serialize(location of file, your values)
        Stream.Close();//close your stream or else errors will occur
    }

    public static int[] LoadPlayer()
    {
        if (File.Exists(Application.persistentDataPath + "/player.sav"))
        {
            BinaryFormatter bf = new BinaryFormatter();//our binary formatter
            FileStream Stream = new FileStream(Application.persistentDataPath + "/player.sav", FileMode.Open);//Same as before, opening binary file but this time taking info from file with FileMode.Open

            //PlayerData data = bf.Deserialize(Stream) as PlayerData;
            PlayerData data = bf.Deserialize(Stream) as PlayerData;//need to cast playerdata, im pretty sure this brings values from binary files to PlayerData

            Stream.Close();//close stream
            return data.stats;//returns saved stats in PlayerData class to load method in playerclass to load in values
        }
        else//need this else as something must return
        {
            Debug.LogError("File does not exist");
            return new int[4];
        }
    }

    public static int[] NewPlayer()
    {
        if (File.Exists(Application.persistentDataPath + "/player.sav"))
        {
            BinaryFormatter bf = new BinaryFormatter();//our binary formatter
            FileStream Stream = new FileStream(Application.persistentDataPath + "/player.sav", FileMode.Open);//Same as before, opening binary file but this time taking info from file with FileMode.Open

            //PlayerData data = bf.Deserialize(Stream) as PlayerData;
            PlayerDataReset data2 = bf.Deserialize(Stream) as PlayerDataReset;//need to cast playerdata, im pretty sure this brings values from binary files to PlayerData

            Stream.Close();//close stream
            Debug.LogError("Accesesing newplayer method");
            return data2.stats2;//returns saved stats in PlayerData class to load method in playerclass to load in values
        }
        else//need this else as something must return
        {
            Debug.LogError("File does not exist");
            return new int[4];
        }
    }
}

[Serializable]
public class PlayerData//What holds our saved variables
{
    public int[] stats;
    //public bool[] boolstats;

    public PlayerData(saving Saving)
    {
        stats = new int[180];
        //boolstats = new bool[1];
        
        stats[0] = Saving.RankStats.TotalCP;
        stats[1] = Saving.RankStats.milestonelist.milestones[0].currentrank;//GlidingTime
        stats[2] = Saving.RankStats.secondscount;
        stats[3] = Saving.RankStats.milestonelist.milestones[1].currentrank;//ItemsCollected
        stats[4] = Saving.RankStats.itemscollected;
        stats[5] = Saving.RankStats.milestonelist.milestones[2].currentrank;//TimePlayed
        stats[6] = Saving.RankStats.secondscount2;
        for(int x = 7; x < 60; x++)//for current amount for each item
        {
            stats[x] = Saving.RankStats.itemstats.items[x - 7].currentamount;
        }
        for(int y = 61; y < 114; y++)
        {
            stats[y] = Saving.RankStats.itemstats.items[y - 61].statamount;
        }
        for(int z = 115; z < 168; z++)
        {
            stats[z] = Saving.RankStats.itemmilestonelist.milestones[z - 115].currentrank;
        }
        for (int a = 169; a < 179; a++)
        {
            stats[a] = Saving.RankStats.menuhandler.bundlesoff[a - 169];
        }
        //boolstats[0] = Saving.Setter.GameLoadAvailable;/*
        //stats[2] = Saving.Magic;*/
    }
}

[Serializable]
public class PlayerDataReset//What holds our saved variables
{
    public int[] stats2;
    //public bool[] boolstats;

    public PlayerDataReset(saving Saving)
    {
        stats2 = new int[176];
        //boolstats = new bool[1];
        Debug.LogError("Well this was called, the setting of new vars");
        stats2[0] = 0;
        stats2[1] = 0;//GlidingTime
        stats2[2] = 0;
        stats2[3] = 0;//ItemsCollected
        stats2[4] = 0;
        stats2[5] = 0;//TimePlayed
        stats2[6] = 0;
        Debug.LogError("Well this was called, the setting of new vars2");
        //boolstats[0] = Saving.Setter.GameLoadAvailable;/*
        //stats[2] = Saving.Magic;*/
    }
}
