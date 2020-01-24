using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerController))]
public class Player : MonoBehaviour
{

    private GameManager gameManager;
    private PlayerController playerController;

    private void Awake()
    {
        playerController = GetComponent<PlayerController>();
    }

    public void SetGameManger(GameManager gameManager)
    {
        this.gameManager = gameManager;
    }

    public void Pause()
    {
        playerController.FreezePlayer();
    }

    public void Unpause()
    {
        playerController.UnfreezePlayer();
    }

    private void DestroyPlayer()
    {
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        string otherTag = other.tag;

        if (otherTag.Equals("Goal"))
        {
            gameManager.PlayerWon();
            playerController.FreezePlayer();
        }
        else if (otherTag.Equals("Trap"))
        {
            gameManager.PlayerLost();
            DestroyPlayer();
        }
    }

}
