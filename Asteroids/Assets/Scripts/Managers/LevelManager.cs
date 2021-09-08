using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility;

public class LevelManager : Singleton<LevelManager>
{
    public BasePool PlayerBulletsPool => playerBulletsPool;
    public BasePool AsteroidsPool => asteroidsPool;
    public BasePool UfoBulletsPool => ufoBulletsPool;
    public PlayerController Player => player;

    [SerializeField] private BasePool playerBulletsPool;
    [SerializeField] private BasePool ufoBulletsPool;
    [SerializeField] private BasePool asteroidsPool;
    [SerializeField] private PlayerController player;
    [SerializeField] private ObstaclesSpawner obstaclesSpawner;

    private int _score;

    private void Start()
    {
        InterfaceManager.Instance.UpdateScoreText(_score);
    }

    public void OnRoundEnd()
    {
        obstaclesSpawner.NewRoundSpawnAsteroids();
    }
    public void AddScorePoints(int value)
    {
        _score += value;
        InterfaceManager.Instance.UpdateScoreText(_score);
    }

    public void Loose()
    {
        GameManager.Instance.PauseGame();
        InterfaceManager.Instance.SetIsInteractableContinueButton(false);
    }

}
