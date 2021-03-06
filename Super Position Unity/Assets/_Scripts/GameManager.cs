﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private Player player;
    private bool gamePaused = false;
    private bool gameEnded = false;
    private float levelTimer = 0;

    [Header("UI")]
    [SerializeField] private Text UI_timer = null;

    private void Awake()
    {
        player = Component.FindObjectOfType<Player>();

        if (player == null)
        {
            Debug.Log("Player Not Found!!!");
            return;
        }

        player.SetGameManger(this);
    }

    private void Update()
    {
        if (!gamePaused && !gameEnded)
        {
            levelTimer += Time.deltaTime;
        }

        UI_timer.text = ((int)levelTimer).ToString();

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!gamePaused)
            {
                PauseGame();
            }
            else
            {
                if (!gameEnded)
                    ResumeGame();
                else
                {
                    //Add return to Main Menu
                }
            }
        }
    }

    public void PlayerWon()
    {
        EndGame();
    }

    public void PlayerLost()
    {
        EndGame();
    }

    public void PauseGame()
    {
        player.Pause();
        gamePaused = true;
    }

    public void ResumeGame()
    {
        player.Unpause();
        gamePaused = false;
    }

    public void EndGame()
    {
        gameEnded = true;
    }
}
