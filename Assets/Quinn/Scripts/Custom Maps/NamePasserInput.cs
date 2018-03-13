using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NamePasserInput : MonoBehaviour {
    public InputField Input;
    public Text LoadMessages;
    private NamePasser Passer;
	// Use this for initialization
	void Start () {
        Passer = GameObject.FindObjectOfType<NamePasser>();
        LoadMessages.text = "No load messages yet";
	}
    public void AttemptLoad()
    {
        if (Input.text != "")
        {
            string retText = "";
            List<string> messages = Passer.CheckMap(Input.text);
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
	// Update is called once per frame
	void Update () {
		
	}
}
