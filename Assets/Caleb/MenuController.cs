using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour {

    public void startGame()
    {
        SceneManager.LoadScene("NumberOfPlayers");
    }

    public void endGame()
    {
        Application.Quit();
    }

    public void controls()
    {
        SceneManager.LoadScene("");
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("Menu");
    }
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
