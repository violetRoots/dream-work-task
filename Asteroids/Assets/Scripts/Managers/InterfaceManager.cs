using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Utility;

public class InterfaceManager : Singleton<InterfaceManager>
{
    [SerializeField] private Text lifesText;
    [SerializeField] private Text scoreText;
    [Space]
    [SerializeField] private Image pauseMenu;
    [SerializeField] private Button continueButton;
    [SerializeField] private Text playerMovementModeText;

    public void SetIsActivePuseMenu(bool isActive)
    {
        pauseMenu.gameObject.SetActive(isActive);
    }

    public void SetIsInteractableContinueButton(bool interactable)
    {
        continueButton.interactable = interactable;
    }

    public void UpdatePlayerMovementText(PlayerController.MovementMode movementMode)
    {
        if (movementMode == PlayerController.MovementMode.KeyboardOnly)
        {
            playerMovementModeText.text = "”правление: клавиатура";
        }
        else
        {
            playerMovementModeText.text = "”правление: клавиатура + мышь";
        }
    }

    public void UpdateLifesText(int lifes)
    {
        lifesText.text = $"Lifes : {lifes}";
    }

    public void UpdateScoreText(int score)
    {
        scoreText.text = $"Score : {score}";
    }
}
