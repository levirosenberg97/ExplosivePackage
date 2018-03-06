using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Buttons : MonoBehaviour
{
    public void LoadScene(string wool)
    {
        SceneManager.LoadScene(wool);
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quit Out");
    }
}
