using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using XInputDotNetPure;

public class WinScreen : MonoBehaviour
{
    int playerCount;
    public PlayerControl player1;
    public PlayerControl player2;
    public PlayerControl player3;
    public PlayerControl player4;

    bool player1Died;
    bool player2Died;
    bool player3Died;
    bool player4Died;

    public Canvas overlay;
    public Canvas winScreen;
    public ControllerUX winButtons;

    public Image player1Wins;
    public Image player2Wins;
    public Image player3Wins;
    public Image player4Wins;

    public ColorBlock player1Colors;
    public ColorBlock player2Colors;
    public ColorBlock player3Colors;
    public ColorBlock player4Colors;

    public Button playAgain;
    public Button menuButton;
    public Button exitButton;


    private void Start()
    {
        playerCount = ReadyBehavior.players;

        player1Died = false;
        player2Died = false;
        player3Died = false;
        player4Died = false;
    }

    private void Update()
    {
        if (player1.isAlive == false && player1Died == false)
        {
            player1Died = true;
            playerCount--;
        }

        if (player2.isAlive == false && player2Died == false)
        {
            player2Died = true;
            playerCount--;
        }

        if (player3.isAlive == false && player3Died == false)
        {
            player3Died = true;
            playerCount--;
        }

        if (player4.isAlive == false && player4Died == false)
        {
            player4Died = true;
            playerCount--;
        }

        if (playerCount == 1)
        {
            overlay.gameObject.SetActive(false);
            winScreen.gameObject.SetActive(true);

            if (player1.isAlive == true)
            {
                player1Wins.gameObject.SetActive(true);
                playAgain.colors = player1Colors;
                menuButton.colors = player1Colors;
                exitButton.colors = player1Colors;

                winButtons.pIdx = PlayerIndex.One;
            }
            else if (player2.isAlive == true)
            {
                player2Wins.gameObject.SetActive(true);
                playAgain.colors = player2Colors;
                menuButton.colors = player2Colors;
                exitButton.colors = player2Colors;

                winButtons.pIdx = PlayerIndex.Two;
            }
            else if (player3.isAlive == true)
            {
                player3Wins.gameObject.SetActive(true);
                playAgain.colors = player3Colors;
                menuButton.colors = player3Colors;
                exitButton.colors = player3Colors;

                winButtons.pIdx = PlayerIndex.Three;
            }
            else if (player4.isAlive == true)
            {
                player4Wins.gameObject.SetActive(true);
                playAgain.colors = player4Colors;
                menuButton.colors = player4Colors;
                exitButton.colors = player4Colors;

                winButtons.pIdx = PlayerIndex.Four;
            }

            Time.timeScale = 0;
        }
    }



}
