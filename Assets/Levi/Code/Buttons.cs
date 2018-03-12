using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Buttons : MonoBehaviour
{
    public void ResumeGame()
    {
        if (Time.timeScale == 0)
        {
            Time.timeScale = 1;
        }
    }

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
