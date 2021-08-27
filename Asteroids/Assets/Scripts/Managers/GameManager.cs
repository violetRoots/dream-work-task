using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Utility;
using static PlayerController;

public class GameManager : Singleton<GameManager>
{
    public bool IsGamePaused => _isGamePaused;

    [SerializeField] private LevelManager levelPrefab;
    [Space]
    [SerializeField] private MovementMode playerMovementMode;

    private bool _isGamePaused;
    private LevelManager _level;

    private void Start()
    {
        InterfaceManager.Instance.SetIsInteractableContinueButton(false);
        InterfaceManager.Instance.UpdatePlayerMovementText(playerMovementMode);
        PauseGame();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!_isGamePaused)
            {
                PauseGame();
            }
            else
            {
                ResumeGame();
            }
        }
    }

    public void PauseGame()
    {
        _isGamePaused = true;

        Time.timeScale = 0;
        InterfaceManager.Instance.SetIsActivePuseMenu(true);
    }

    public void ResumeGame()
    {
        if (!_level) return;

        _isGamePaused = false;

        Time.timeScale = 1;
        InterfaceManager.Instance.SetIsActivePuseMenu(false);
    }

    public void StartNewGame()
    {
        if (_level)
        {
            DestroyImmediate(_level.gameObject);
        }
        else
        {
            InterfaceManager.Instance.SetIsInteractableContinueButton(true);
        }

        _level = Instantiate(levelPrefab);
        _level.Player.SetMovementMode(playerMovementMode);
        ResumeGame();
    }

    public void ChangePlayerMovementMode()
    {
        playerMovementMode = playerMovementMode == MovementMode.KeyboardAndMouse ? MovementMode.KeyboardOnly : MovementMode.KeyboardAndMouse;

        InterfaceManager.Instance.UpdatePlayerMovementText(playerMovementMode);

        if (!_level) return;

        _level.Player.SetMovementMode(playerMovementMode);
    }

    public void Exit()
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
    }
}
