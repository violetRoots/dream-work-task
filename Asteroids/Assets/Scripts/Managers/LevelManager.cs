using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility;

public class LevelManager : Singleton<LevelManager>
{
    public BasePool PlayerBulletsPool => playerBulletsPool;
    public BasePool AsteroidsPool => asteroidsPool;
    public BasePool UfoBulletsPool => ufoBulletsPool;
    public PlayerController Player => _player;

    [SerializeField] private BasePool playerBulletsPool;
    [SerializeField] private BasePool ufoBulletsPool;
    [SerializeField] private BasePool asteroidsPool;

    private PlayerController _player;
    private ObstaclesSpawner _obstaclesSpawner;
    private int _score;

    public override void Awake()
    {
        base.Awake();
        _obstaclesSpawner = GetComponentInChildren<ObstaclesSpawner>();
        _player = GetComponentInChildren<PlayerController>();
    }

    private void Start()
    {
        InterfaceManager.Instance.UpdateScoreText(_score);
    }

    public void OnRoundEnd()
    {
        _obstaclesSpawner.NewRoundSpawnAsteroids();
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
