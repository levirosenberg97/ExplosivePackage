﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;
public class NamePasser : MonoBehaviour {
    public string FileName = "";
    public static NamePasser instace = null;
    public bool CreateMapFolder = true;
    public bool CreateMaps = true;
    // Use this for initialization

    void Awake()
    {
        if (instace == null)
        {
            instace = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

	void Start () {
        DontDestroyOnLoad(gameObject.transform);
        string mapFolder = Application.persistentDataPath + @"/Maps";
        if (CreateMapFolder && Directory.Exists(mapFolder) == false)
        {
            //creates map folder
            Directory.CreateDirectory(mapFolder);
        }
        string dir = Application.persistentDataPath + @"/Maps";
        foreach (string file in Directory.GetFiles(Application.streamingAssetsPath, "*.txt"))
        {
            string filename = Path.GetFileNameWithoutExtension(file);
            if (File.Exists(mapFolder + @"/" + filename + ".txt") == false && CreateMaps == true)
            {
                //creates example map file
                Debug.Log(Application.streamingAssetsPath);
                File.Copy(Application.streamingAssetsPath + @"/" + filename + ".txt", mapFolder + @"/" + filename + ".txt");
            }
        }
    }
	
	// Update is called once per frame
	void Update () {
        // Create a temporary reference to the current scene.
        Scene currentScene = SceneManager.GetActiveScene();

        // Retrieve the name of this scene.
        string sceneName = currentScene.name;

        if (sceneName != "CustomMap" && sceneName != "CustomMapSelection" && sceneName != "NumberOfPlayersCustom")
        {
            SelfDestruct();
        }

    }
    public void OpenMapFolder()
    {
        System.Diagnostics.Process.Start(Application.persistentDataPath + @"/Maps");
    }
    void SelfDestruct()
    {
        Destroy(gameObject);
    }
    public string MapFolder()
    {
        return Application.persistentDataPath + @"/Maps";
    }
    public List<string> CheckMap(string mapFile)
    {
        string path = Application.persistentDataPath + @"/Maps/" + mapFile + ".txt";
        CustomMapLoader.MapData retval = new CustomMapLoader.MapData()
        {
            MapSize = new Vector2(0, 0),
            LoadMessages = new List<string>()
        };
        retval.LoadMessages.Add("LOADING MAP FROM\n" + path);
        if (File.Exists(path))
        {
            bool setPos = false;
            bool setRot = false;
            bool setSize = false;
            bool setTiles = false;
            bool setP1Spawn = false;
            bool setP2Spawn = false;
            bool setP3Spawn = false;
            bool setP4Spawn = false;
            var lines = File.ReadAllLines(path);
            List<string> tileInfo = new List<string>();
            foreach (var line in lines)
            {
                if (line[0] != '#')
                {
                    if (setPos == false && (line.Contains("position:") || line.Contains("Position:")))
                    {
                        int start = 0;
                        for (int i = 0; i < line.Length; i++)
                        {
                            //checks for starting char
                            if (line[i] == ':')
                            {
                                start = i + 1;
                                i = line.Length;
                            }
                        }
                        //create string of everything past the starting char
                        string positionStr = "";
                        for (int i = start; i < line.Length; i++)
                        {
                            positionStr = positionStr + line[i];
                        }
                        //creates float list after spliting the string 
                        List<float> floatlist = new List<float>();
                        foreach (string number in positionStr.Split(','))
                        {
                            floatlist.Add(float.Parse(number));
                        }
                        //checks to make sure there are 3 values
                        if (floatlist.Count == 3)
                        {
                            retval.CamPosition.x = floatlist[0];
                            retval.CamPosition.y = floatlist[1];
                            retval.CamPosition.z = floatlist[2];
                            setPos = true;
                        }
                        else
                        {
                            Debug.Log("map file has wrong number of position values");
                        }
                    }
                    else if (setRot == false && (line.Contains("rotation:") || line.Contains("Rotation:")))
                    {
                        int start = 0;
                        for (int i = 0; i < line.Length; i++)
                        {
                            //checks for starting char
                            if (line[i] == ':')
                            {
                                start = i + 1;
                                i = line.Length;
                            }
                        }
                        //create string of everything past the starting char
                        string positionStr = "";
                        for (int i = start; i < line.Length; i++)
                        {
                            positionStr = positionStr + line[i];
                        }
                        //creates float list after spliting the string 
                        List<float> floatlist = new List<float>();
                        foreach (string number in positionStr.Split(','))
                        {
                            floatlist.Add(float.Parse(number));
                        }
                        //checks to make sure there are 3 values
                        if (floatlist.Count == 3)
                        {
                            retval.CamRotation.x = floatlist[0];
                            retval.CamRotation.y = floatlist[1];
                            retval.CamRotation.z = floatlist[2];
                            setRot = true;
                        }
                        else
                        {
                            Debug.Log("map file has wrong number of rotation values");
                        }
                    }
                    else if (setSize == false && (line.Contains("size:") || line.Contains("Size:")))
                    {
                        int start = 0;
                        for (int i = 0; i < line.Length; i++)
                        {
                            //checks for starting char
                            if (line[i] == ':')
                            {
                                start = i + 1;
                                i = line.Length;
                            }
                        }
                        //create string of everything past the starting char
                        string positionStr = "";
                        for (int i = start; i < line.Length; i++)
                        {
                            positionStr = positionStr + line[i];
                        }
                        float f;
                        if (float.TryParse(positionStr, out f))
                        {
                            setSize = true;
                            retval.CamSize = f;
                        }
                        else
                        {
                            Debug.Log("map file has a bad size value");
                        }
                    }
                    else if (setPos && setRot && setSize)
                    {
                        setTiles = true;
                        //keep track of number of lines
                        retval.MapSize.y++;
                        //set line length
                        if (retval.MapSize.x == 0)
                        {
                            retval.MapSize.x = line.Length;
                        }
                        //check line length
                        if (retval.MapSize.x != line.Length)
                        {
                            Debug.Log("map tile info is not a rectangle\nMake sure lines are the same length");
                            retval.LoadMessages.Add("ERROR tile info rows are not the same length");
                            break;
                        }
                        //map tile time!
                        Debug.Log(line);
                        string retline = "";
                        foreach (char c in line)
                        {
                            if (c == ' ' || c == 'B' || c == 'U')
                            {
                                retline = retline + c;
                            }
                            else if  (c == '1')
                            {
                                if (setP1Spawn)
                                {
                                    retval.LoadMessages.Add("ERROR found an extra spawn for Player 1");
                                }
                                else
                                {
                                    setP1Spawn = true;
                                }
                            }
                            else if (c == '2')
                            {
                                if (setP2Spawn)
                                {
                                    retval.LoadMessages.Add("ERROR found an extra spawn for Player 2");
                                }
                                else
                                {
                                    setP2Spawn = true;
                                }
                            }
                            else if (c == '3')
                            {
                                if (setP3Spawn)
                                {
                                    retval.LoadMessages.Add("ERROR found an extra spawn for Player 3");
                                }
                                else
                                {
                                    setP3Spawn = true;
                                }
                            }
                            else if (c == '4')
                            {
                                if (setP4Spawn)
                                {
                                    retval.LoadMessages.Add("ERROR found an extra spawn for Player 4");
                                }
                                else
                                {
                                    setP4Spawn = true;
                                }
                            }
                            else if (c == 'b')
                            {
                                retline = retline + 'B';
                            }
                            else if (c == 'u')
                            {
                                retline = retline + 'U';
                            }
                            else if (c == 'R' || c == 'r')
                            {
                                char randomized;
                                int rand = Random.Range(1, 4);
                                if (rand == 1)
                                {
                                    randomized = 'B';
                                }
                                else if (rand == 2)
                                {
                                    randomized = 'U';
                                }
                                else
                                {
                                    randomized = ' ';
                                }
                                retline = retline + randomized;
                            }
                            else
                            {
                                //UNRECOGNIZED CHAR
                                char randomized;
                                int rand = Random.Range(1, 4);
                                if (rand == 1)
                                {
                                    randomized = 'B';
                                }
                                else if (rand == 2)
                                {
                                    randomized = 'U';
                                }
                                else
                                {
                                    randomized = ' ';
                                }
                                retline = retline + randomized;
                                Debug.Log("found an unsupported char in tile info " + c);
                                retval.LoadMessages.Add("WARNING unrecognized char " + c + " this will be replaced with a random value if there are no map loading errors");
                            }
                        }
                        tileInfo.Add(retline);
                    }

                }
                //ignore line if starts with #
            }
            retval.Tiles = tileInfo.ToArray();
            bool LoadDefault = false;
            if (setP1Spawn && setP2Spawn && setP3Spawn && setP4Spawn)
            {
                retval.LoadMessages.Add("ALL SPAWNS FOUND");
            }
            else
            {
                if (setP1Spawn)
                {
                    retval.LoadMessages.Add("Player 1 spawn set");
                }
                else
                {
                    retval.LoadMessages.Add("ERROR Player 1 spawn not set");
                }
                if (setP2Spawn)
                {
                    retval.LoadMessages.Add("Player 2 spawn set");
                }
                else
                {
                    retval.LoadMessages.Add("ERROR Player 2 spawn not set");
                }
                if (setP3Spawn)
                {
                    retval.LoadMessages.Add("Player 3 spawn set");
                }
                else
                {
                    retval.LoadMessages.Add("ERROR Player 3 spawn not set");
                }
                if (setP4Spawn)
                {
                    retval.LoadMessages.Add("Player 4 spawn set");
                }
                else
                {
                    retval.LoadMessages.Add("ERROR Player 4 spawn not set");
                }
            }
            if (setTiles == false)
            {
                retval.LoadMessages.Add("ERROR failed to set Tiles");
            }
            if (setPos == false)
            {
                retval.LoadMessages.Add("ERROR failed to set Camera Position");
            }
            if (setRot == false)
            {
                retval.LoadMessages.Add("ERROR failed to set Camera Rotation");
            }
            if (setSize == false)
            {
                retval.LoadMessages.Add("ERROR failed to set Camera Size");
            }
            foreach (string line in retval.LoadMessages)
            {
                if (line.Contains("ERROR"))
                {
                    LoadDefault = true;
                }
            }
            if (LoadDefault == false)
            {
                retval.LoadMessages.Add("SUCCESS");
                FileName = mapFile;
                return retval.LoadMessages;
            }
        }
        else
        {
            Debug.Log("Custom map file not found or loaded improperly");
            retval.LoadMessages.Add("ERROR unable to access map file");
        }
        retval.LoadMessages.Add("FAILURE to load desired map");
        return retval.LoadMessages;
    }
    //note will only return false ifmap didn't load successfully
    public bool AttemptSceneChange(List<string> loadMessages)
    {
        if (loadMessages.Contains("SUCCESS"))
        {
            //load # of players scene 
            SceneManager.LoadScene("CustomMap");
            return true;
        }
        else
        {
            return false;
        }
    }
}
