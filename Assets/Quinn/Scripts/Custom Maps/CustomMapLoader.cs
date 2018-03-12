using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class CustomMapLoader : MonoBehaviour {
    [Tooltip("Map settings to be loaded if the custom map isn't loaded correctly")]
    [Header("Default map settings")]
    public Vector3 CameraPosition = new Vector3 (0,22.31f,-0.12f);
    public Vector3 CameraRotation = new Vector3(90, 0, 0);
    public string[] TileInfo = new string[11] {
        "1  BBBBBBB  4",
        " UBUBUBUBUBU ",
        " BBBBBBBBBBB ",
        "BUBUBUBUBUBUB",
        "BBBBBBBBBBBBB",
        "BUBUBUBUBUBUB",
        "BBBBBBBBBBBBB",
        "BUBUBUBUBUBUB",
        " BBBBBBBBBBB ",
        " UBUBUBUBUBU ",
        "3  BBBBBBB  2" };
    public MapData LoadedMap = new MapData();
    //prefabs to be used while loading a custom map note (the commented char above is what will be used to load them

    //U
    public GameObject UnbreakableWall;
    //B
    public GameObject BreakableWall;
    // 
    public GameObject BombDropZone;
    //1
    public GameObject P1Spawn;
    //2
    public GameObject P2Spawn;
    //3
    public GameObject P3Spawn;
    //4
    public GameObject P4Spawn;
    //floor
    [Tooltip("This should be an invisible plane for the players to stand on that will be sized according to map size")]
    public GameObject Floor;
    [Tooltip("If enabled will check for a map folder, if one is not found it will create one")]
    public bool CreateMapFolder = true;
    [Tooltip("Create Map Folder must be enabled for this to work\nWill create a map file of the default map as an example with a map key included")]
    public bool CreateExampleMap = true;
    // Use this for initialization
    void Start () {
        Debug.Log(Application.persistentDataPath);
        string mapFolder = Application.persistentDataPath + @"/Maps";
        if (CreateMapFolder)
        {
            if (Directory.Exists(mapFolder) == false)
            {
                //creates map folder
                Directory.CreateDirectory(mapFolder);
            }
            if (File.Exists(mapFolder + @"/ExampleMap.txt") == false)
            {
                //creates example map file
                Debug.Log(Application.streamingAssetsPath);
                File.Copy(Application.streamingAssetsPath + @"/ExampleMap.txt", mapFolder + @"/ExampleMap.txt");
            }
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}
    //takes map data and creates a map
    public void ConstructMap(MapData map)
    {
        //camera setting
        Camera cam = GameObject.FindObjectOfType<Camera>();
        cam.transform.SetPositionAndRotation(map.CamPosition, Quaternion.Euler(map.CamRotation));
        //map setting
        //tiles
        for (int i = 0; i < map.Tiles.Length; i++)
        {
            string line = map.Tiles[i];
            for (int j = 0; j < line.Length; j++)
            {
                Vector3 position = new Vector3(0 + i, 0.5f, 0 - j);
                //top and bottom walls
                if (i == 0 || i == map.Tiles.Length - 1)
                {
                    Vector3 positionAdjustment = position;
                    if (i == 0)
                    {
                        positionAdjustment.x--;
                    }
                    else
                    {
                        positionAdjustment.x++;
                    }
                    //place unbreakable wall
                    Instantiate(UnbreakableWall, positionAdjustment, UnbreakableWall.transform.rotation);
                }
                //right and left walls as well as corners
                if (j == 0 || j == line.Length - 1)
                {
                    Vector3 positionAdjustment = position;
                    if (j == 0)
                    {
                        positionAdjustment.z--;
                    }
                    else
                    {
                        positionAdjustment.z++;
                    }
                    //place unbreakable wall
                    Instantiate(UnbreakableWall, positionAdjustment, UnbreakableWall.transform.rotation);
                    //corners
                    if (i == 0)
                    {
                        positionAdjustment.x--;
                        Instantiate(UnbreakableWall, positionAdjustment, UnbreakableWall.transform.rotation);
                    }
                    else if (i == map.Tiles.Length - 1)
                    {
                        positionAdjustment.x--;
                        Instantiate(UnbreakableWall, positionAdjustment, UnbreakableWall.transform.rotation);
                    }
                }
                char c = line[j];
                //place tiles
                if (c == 'U')
                {
                    Instantiate(UnbreakableWall, position, UnbreakableWall.transform.rotation);
                }
                else if (c == 'B')
                {
                    Instantiate(BreakableWall, position, BreakableWall.transform.rotation);
                }
                else if (c == ' ')
                {
                    Instantiate(BombDropZone, position, BombDropZone.transform.rotation);
                }
                else if (c == '1')
                {
                    Instantiate(P1Spawn, position, P1Spawn.transform.rotation);
                }
                else if (c == '2')
                {
                    Instantiate(P2Spawn, position, P2Spawn.transform.rotation);
                }
                else if (c == '3')
                {
                    Instantiate(P3Spawn, position, P3Spawn.transform.rotation);
                }
                else if (c == '4')
                {
                    Instantiate(P4Spawn, position, P4Spawn.transform.rotation);
                }
            }
        }
        //floor? (textures might cause issues so the renderer will be turned off)
        GameObject floor = Instantiate(Floor, new Vector3(map.MapSize.x / 2, 0, -map.MapSize.y / 2), new Quaternion());
        Vector3 scale = floor.transform.localScale;
        scale.x = (map.MapSize.x / 10);
        scale.z = (map.MapSize.y / 10);
        floor.transform.localScale = scale;
        floor.GetComponent<Renderer>().enabled = false;
    }
    //reads info from text file and returns map data
    public MapData ReadMap(string mapFile)
    {
        string path = Application.persistentDataPath + @"\Maps\" + mapFile + ".txt";
        MapData retval = new MapData()
        {
            MapSize = new Vector2Int(0, 0),
            LoadMessages = new List<string>()
        };
        if (File.Exists(path))
        {
            bool setPos = false;
            bool setRot = false;
            var lines = File.ReadAllLines(path);
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
                            retval.LoadMessages.Add("ERROR Wrong number of position values");
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
                            setPos = true;
                        }
                        else
                        {
                            retval.LoadMessages.Add("ERROR Wrong number of rotation values");
                            Debug.Log("map file has wrong number of rotation values");
                        }
                    }
                    if (setPos && setRot)
                    {
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
                            if (c == ' ' || c == 'B' || c == 'U' || c == '1' || c == '2' || c == '3' || c == '4')
                            {
                                retline = retline + c;
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
                                int rand = Random.Range(1, 3);
                                if (rand == 1)
                                {
                                    randomized = 'B';
                                }
                                if (rand == 2)
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
                                int rand = Random.Range(1, 3);
                                if (rand == 1)
                                {
                                    randomized = 'B';
                                }
                                if (rand == 2)
                                {
                                    randomized = 'U';
                                }
                                else
                                {
                                    randomized = ' ';
                                }
                                retline = retline + randomized;
                                Debug.Log("found an unsupported char in tile info " + c);
                                retval.LoadMessages.Add("unrecognized char " + c + " this will be replaced with a random value if there are no map loading errors");
                            }
                        }

                    }
                    
                }
                //ignore line if starts with #
            }
            bool LoadDefault = false;
            foreach (string line in retval.LoadMessages)
            {
                if (line.Contains("ERROR"))
                {
                    LoadDefault = true;
                }
            }
            if (LoadDefault == false)
            {
                return retval;
            }
        }
        else
        {
            Debug.Log("Custom map file not found or loaded improperly");
            retval.LoadMessages.Add("ERROR unable to access map file");
        }
        retval.CamPosition = CameraPosition;
        retval.CamRotation = CameraRotation;
        retval.MapSize = new Vector2Int(TileInfo[0].Length, TileInfo.Length);
        retval.Tiles = TileInfo;
        retval.LoadMessages.Add("loaded default map");
        return retval;

    }
    //used to map data from map file 
    public struct MapData
    {
        //holds camera info
        public Vector3 CamPosition;
        public Vector3 CamRotation;
        //holds map info
        public Vector2Int MapSize;
        public string[] Tiles;
        //holds info that was generated on read/load
        public List<string> LoadMessages;
    }
}
