using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class NamePasserInput : MonoBehaviour {
    //public InputField Input;
    public Dropdown Input;
    public Text LoadMessages;
    private NamePasser Passer;
	// Use this for initialization
	void Start () {
        Passer = GameObject.FindObjectOfType<NamePasser>();
        LoadMessages.text = "No load messages yet";
	}
    public void AttemptLoad()
    {
        Input = GameObject.FindObjectOfType<Dropdown>();
        if (Input.options.Count > 0)
        {
            string retText = "";
            List<string> messages = Passer.CheckMap(Input.options[Input.value].text);
            if (Passer.AttemptSceneChange(messages) == false)
            {
                foreach (string line in messages)
                {
                    retText = retText + line + "\n";
                }
                LoadMessages.text = retText;
            }
            
        }
    }
    public void Back()
    {
        Destroy(Passer);
        SceneManager.LoadScene("NumberOfPlayersCustom");
    }
	// Update is called once per frame
	void Update () {
		
	}
}
