using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using XInputDotNetPure;

public class MenuController : MonoBehaviour {

    public ControllerUX buttons;

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
        SceneManager.LoadScene("Controls");
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("Menu");
    }
    // Use this for initialization
    void Start()
    {
        buttons.pIdx = PlayerIndex.One;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
