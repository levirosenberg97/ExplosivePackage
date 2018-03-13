using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class CustomMapLoader : MonoBehaviour {
    [Tooltip("Map settings to be loaded if the custom map isn't loaded correctly")]
    [Header("Default map settings")]
    public Vector3 CameraPosition = new Vector3 (0,22.31f,-0.12f);
    public Vector3 CameraRotation = new Vector3(90, 0, 0);
    public float CameraSize = 6.64601f;
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
    public bool CreateMapFolder = false;
    [Tooltip("Create Map Folder must be enabled for this to work\nWill create a map file of the default map as an example with a map key included")]
    public bool CreateExampleMap = false;

    private RawImage player3Face;
    private RawImage player4Face;

    private Slider player3Slider;
    private Slider player4Slider;

    private GameObject player3Indicator;
    private GameObject player4Indicator;
    void HookUpPlayers(GameObject P1, GameObject P2, GameObject P3, GameObject P4)
    {
        WinScreen screen = GameObject.FindObjectOfType<WinScreen>();
        screen.player1 = P1.GetComponent<PlayerControl>();
        screen.player2 = P2.GetComponent<PlayerControl>();
        screen.player3 = P3.GetComponent<PlayerControl>();
        screen.player4 = P4.GetComponent<PlayerControl>();
        screen.playerCount = 4;
        foreach (Canvas canvas in GameObject.FindObjectsOfType<Canvas>())
        {
            if (canvas.name == "Overlay")
            {
                foreach (Transform sectiont in canvas.transform)
                {
                    GameObject section = sectiont.gameObject;
                    if (section.name == "Faces")
                    {
                        foreach (Transform childt in section.transform)
                        {
                            GameObject child = childt.gameObject;
                            if (child.name == "Player1")
                            {
                                child.GetComponent<DeathVibration>().player = P1.GetComponent<PlayerControl>();
                                child.GetComponent<DeathIndicator>().player = P1.GetComponent<PlayerControl>();
                            }
                            if (child.name == "Player2")
                            {
                                child.GetComponent<DeathVibration>().player = P2.GetComponent<PlayerControl>();
                                child.GetComponent<DeathIndicator>().player = P2.GetComponent<PlayerControl>();
                            }
                            if (child.name == "Player3")
                            {
                                player3Face = child.GetComponent<RawImage>();
                                child.GetComponent<DeathVibration>().player = P3.GetComponent<PlayerControl>();
                                child.GetComponent<DeathIndicator>().player = P3.GetComponent<PlayerControl>();
                            }
                            if (child.name == "Player4")
                            {
                                player4Face = child.GetComponent<RawImage>();
                                child.GetComponent<DeathVibration>().player = P4.GetComponent<PlayerControl>();
                                child.GetComponent<DeathIndicator>().player = P4.GetComponent<PlayerControl>();
                            }
                        }
                    }
                    else if (section.name == "BombIndiacators")
                    {
                        foreach (Transform childt in section.transform)
                        {
                            GameObject child = childt.gameObject;
                            if (child.name == "Player1Slider")
                            {
                                child.GetComponent<bombTimer>().player = P1.GetComponent<PlayerControl>();
                            }
                            if (child.name == "Player2Slider")
                            {
                                child.GetComponent<bombTimer>().player = P2.GetComponent<PlayerControl>();
                            }
                            if (child.name == "Player3Slider")
                            {
                                child.GetComponent<bombTimer>().player = P3.GetComponent<PlayerControl>();
                                player3Slider = child.GetComponent<Slider>();
                            }
                            if (child.name == "Player4Slider")
                            {
                                child.GetComponent<bombTimer>().player = P3.GetComponent<PlayerControl>();
                                player4Slider = child.GetComponent<Slider>();
                            }
                        }
                    }
                    else if (section.name == "PowerUpIndicators")
                    {
                        foreach (Transform childt in section.transform)
                        {
                            GameObject child = childt.gameObject;
                            if (child.name == "Player1Flames")
                            {
                                child.GetComponent<PowerUpIndicator>().player = P1.GetComponent<PlayerControl>();
                            }
                            if (child.name == "Player2Flames")
                            {
                                child.GetComponent<PowerUpIndicator>().player = P2.GetComponent<PlayerControl>();
                            }
                            if (child.name == "Player3Flames")
                            {
                                player3Indicator = child;
                                child.GetComponent<PowerUpIndicator>().player = P3.GetComponent<PlayerControl>();
                            }
                            if (child.name == "Player4Flames")
                            {
                                player4Indicator = child;
                                child.GetComponent<PowerUpIndicator>().player = P4.GetComponent<PlayerControl>();
                            }
                        }
                    }
                }
            }
            if (canvas.name == "PauseScreen")
            {
                List<GameObject> players = new List<GameObject>();
                players.Add(P1);
                players.Add(P2);
                players.Add(P3);
                players.Add(P4);
                foreach (GameObject p in players)
                {
                    PauseScript pause = p.GetComponent<PauseScript>();
                    pause.pauseScreen = canvas;
                    pause.pausedPlayer = canvas.GetComponent<ControllerUX>();
                    Button[] buttons = GameObject.FindObjectsOfType<Button>();
                    foreach (Button b in buttons)
                    {
                        if (b.name == "ResumeButton")
                        {
                            pause.resumeButton = b;
                        }
                        if (b.name == "MenuButton")
                        {
                            pause.menuButton = b;
                        }
                        if (b.name == "ExitButton")
                        {
                            pause.exitButton = b;
                        }
                    }
                }
            }
        }
    }
    void RemovePlayer(GameObject Player, bool P4 = true)
    {
        WinScreen screen = GameObject.FindObjectOfType<WinScreen>();
        Player.SetActive(false);
        if (P4)
        {
            player4Face.gameObject.SetActive(false);
            player4Slider.gameObject.SetActive(false);
            player4Indicator.SetActive(false);

            screen.playerCount--;
            screen.player4Died = true;
        }
        else
        {
            player3Face.gameObject.SetActive(false);
            player3Slider.gameObject.SetActive(false);
            player3Indicator.SetActive(false);

            screen.playerCount--;
            screen.player3Died = true;
        }
    }
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
        //pull filename from name passer and players from readybehavior
        ConstructMap(ReadMap(GameObject.FindObjectOfType<NamePasser>().FileName), ReadyBehavior.players);
    }
	
	// Update is called once per frame
	void Update () {
		
	}
    //takes map data and creates a map
    public void ConstructMap(MapData map, int numberOfPlayers = 2)
    {
        GameObject P1 = gameObject;
        GameObject P2 = gameObject;
        GameObject P3 = gameObject;
        GameObject P4 = gameObject;
        GameObject Parent = (new GameObject("Map"));
        //camera setting
        Camera cam = GameObject.FindObjectOfType<Camera>();
        cam.transform.SetPositionAndRotation(map.CamPosition, Quaternion.Euler(map.CamRotation));
        cam.orthographicSize = map.CamSize;
        //map setting
        //tiles
        //Vector3 TopLeft = new Vector3();
        //Vector3 BottomRight = new Vector3();
        for (int i = 0; i < map.Tiles.Length; i++)
        {
            string line = map.Tiles[i];
            for (int j = 0; j < line.Length; j++)
            {
                Vector3 position = new Vector3(0 + j, 0.5f, 0 - i);
                //top and bottom walls
                if (i == 0 || i == map.Tiles.Length - 1)
                {
                    Vector3 positionAdjustment = position;
                    if (i == 0)
                    {
                        //down
                        positionAdjustment.z++;
                    }
                    else
                    {
                        //up
                        positionAdjustment.z--;
                    }
                    //place unbreakable wall
                    Instantiate(UnbreakableWall, positionAdjustment, UnbreakableWall.transform.rotation).transform.SetParent(Parent.transform);
                }
                //right and left walls as well as corners
                if (j == 0 || j == line.Length - 1)
                {
                    Vector3 positionAdjustment = position;
                    if (j == 0)
                    {
                        //left
                        positionAdjustment.x--;
                    }
                    else
                    {
                        //right
                        positionAdjustment.x++;
                    }
                    //place unbreakable wall
                    Instantiate(UnbreakableWall, positionAdjustment, UnbreakableWall.transform.rotation).transform.SetParent(Parent.transform);
                    //corners
                    if (i == 0)
                    {
                        positionAdjustment.z++;
                        Instantiate(UnbreakableWall, positionAdjustment, UnbreakableWall.transform.rotation).transform.SetParent(Parent.transform);
                    }
                    else if (i == map.Tiles.Length - 1)
                    {
                        positionAdjustment.z--;
                        Instantiate(UnbreakableWall, positionAdjustment, UnbreakableWall.transform.rotation).transform.SetParent(Parent.transform);
                    }
                }
                char c = line[j];
                //place tiles
                if (c == 'U')
                {
                    Instantiate(UnbreakableWall, position, UnbreakableWall.transform.rotation).transform.SetParent(Parent.transform);
                }
                else if (c == 'B')
                {
                    Instantiate(BreakableWall, position, BreakableWall.transform.rotation).transform.SetParent(Parent.transform);
                }
                else if (c == ' ')
                {
                    Instantiate(BombDropZone, position, BombDropZone.transform.rotation).transform.SetParent(Parent.transform);
                }
                else if (c == '1')
                {
                    if (true)
                    {
                        GameObject spawnPoint = Instantiate(P1Spawn, position, P1Spawn.transform.rotation);
                        spawnPoint.transform.SetParent(Parent.transform);
                        PlayerControl Player = new PlayerControl();
                        foreach (Transform child in spawnPoint.transform)
                        {
                            PlayerControl PC = child.GetComponent<PlayerControl>();
                            if (PC != null)
                            {
                                Player = PC;
                            }
                        }
                        foreach (Text text in GameObject.FindObjectsOfType<Text>())
                        {
                            if (text.name == "Player1Text")
                            {
                                Player.powerUpText = text;
                            }
                        }
                        P1 = Player.gameObject;
                    }
                    else
                    {
                        Debug.Log("player count to low play");
                        Instantiate(BombDropZone, position, BombDropZone.transform.rotation).transform.SetParent(Parent.transform);
                    }
                }
                else if (c == '2')
                {
                    if (true)
                    {
                        GameObject spawnPoint = Instantiate(P2Spawn, position, P2Spawn.transform.rotation);
                        spawnPoint.transform.SetParent(Parent.transform);
                        PlayerControl Player = new PlayerControl();
                        foreach (Transform child in spawnPoint.transform)
                        {
                            PlayerControl PC = child.GetComponent<PlayerControl>();
                            if (PC != null)
                            {
                                Player = PC;
                            }
                        }
                        foreach (Text text in GameObject.FindObjectsOfType<Text>())
                        {
                            if (text.name == "Player2Text")
                            {
                                Player.powerUpText = text;
                            }
                        }
                        P2 = Player.gameObject;
                    }
                    else
                    {
                        Debug.Log("player count to low play");
                        Instantiate(BombDropZone, position, BombDropZone.transform.rotation).transform.SetParent(Parent.transform);
                    }
                }
                else if (c == '3')
                {
                    GameObject spawnPoint = Instantiate(P3Spawn, position, P3Spawn.transform.rotation);
                    spawnPoint.transform.SetParent(Parent.transform);
                    PlayerControl Player = new PlayerControl();
                    foreach (Transform child in spawnPoint.transform)
                    {
                        PlayerControl PC = child.GetComponent<PlayerControl>();
                        if (PC != null)
                        {
                            Player = PC;
                        }
                    }
                    foreach (Text text in GameObject.FindObjectsOfType<Text>())
                    {
                        if (text.name == "Player3Text")
                        {
                            Player.powerUpText = text;
                        }
                    }
                    P3 = Player.gameObject;
                }
                else if (c == '4')
                {
                    GameObject spawnPoint = Instantiate(P4Spawn, position, P4Spawn.transform.rotation);
                    spawnPoint.transform.SetParent(Parent.transform);
                    PlayerControl Player = new PlayerControl();
                    foreach (Transform child in spawnPoint.transform)
                    {
                        PlayerControl PC = child.GetComponent<PlayerControl>();
                        if (PC != null)
                        {
                            Player = PC;
                        }
                    }
                    foreach (Text text in GameObject.FindObjectsOfType<Text>())
                    {
                        if (text.name == "Player4Text")
                        {
                            Player.powerUpText = text;
                        }
                    }
                    P4 = Player.gameObject;
                }
            }
        }
        if (P1 != gameObject && P2 != gameObject && P3 != gameObject && P4 != gameObject)
        {
            HookUpPlayers(P1, P2, P3, P4);
            if (numberOfPlayers < 4)
            {
                RemovePlayer(P4, true);
            }
            if (numberOfPlayers < 3)
            {
                RemovePlayer(P3, false);
            }
        }
        
        //floor? (textures might cause issues so the renderer will be turned off)
        GameObject floor = Instantiate(Floor, new Vector3(-.5f + map.MapSize.x / 2, 0, .5f - map.MapSize.y / 2), Floor.transform.rotation);
        floor.transform.SetParent(Parent.transform);
        Vector3 scale = floor.transform.localScale;
        scale.z = (map.MapSize.x / 10);
        scale.x = (map.MapSize.y / 10);
        floor.transform.localScale = scale;
        Renderer rend = floor.GetComponent<Renderer>();
        rend.material.mainTextureScale = new Vector2(map.MapSize.y/5, map.MapSize.x / 5);
    }
    //reads info from text file and returns map data
    public MapData ReadMap(string mapFile)
    {
        string path = Application.persistentDataPath + @"/Maps/" + mapFile + ".txt";
        MapData retval = new MapData()
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
                            setRot = true;
                        }
                        else
                        {
                            retval.LoadMessages.Add("ERROR Wrong number of rotation values");
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
                            retval.LoadMessages.Add("ERROR size value not valid");
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
                                retval.LoadMessages.Add("unrecognized char " + c + " this will be replaced with a random value if there are no map loading errors");
                            }
                        }
                        tileInfo.Add(retline);
                    }
                    
                }
                //ignore line if starts with #
            }
            retval.Tiles = tileInfo.ToArray();
            bool LoadDefault = false;
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
        retval.MapSize = new Vector2(TileInfo[0].Length, TileInfo.Length);
        retval.CamSize = CameraSize;
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
        public float CamSize;
        //holds map info
        public Vector2 MapSize;
        public string[] Tiles;
        //holds info that was generated on read/load
        public List<string> LoadMessages;
    }
}
