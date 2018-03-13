using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
public class SelectMap : MonoBehaviour {
    public NamePasser passer;
    public Dropdown selection;
	// Use this for initialization
	void Start () {
        passer = GameObject.FindObjectOfType<NamePasser>();
        UpdateMaps();
	}
    public void UpdateMaps()
    {
        selection.options = new List<Dropdown.OptionData>();
        string dir = Application.persistentDataPath + @"/Maps";
        foreach (string file in Directory.GetFiles(dir, "*.txt"))
        {
            selection.options.Add(new Dropdown.OptionData(Path.GetFileNameWithoutExtension(file)));
        }
        selection.captionText.text = selection.options[0].text;
    }
	// Update is called once per frame
	void Update () {
		
	}
}
