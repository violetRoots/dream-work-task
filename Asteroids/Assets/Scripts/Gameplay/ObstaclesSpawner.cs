using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstaclesSpawner : MonoBehaviour
{
    private const float UfoSpawnConstraintMultiolier = 0.8f;

    [SerializeField] private int startAsteroidsCount = 2;
    [SerializeField] private float newRoundSpawnDelay = 2.0f;
    [SerializeField] private float minUfoSpawnInterval = 20.0f, maxUfoSpawnInterval = 40.0f;
    [Space]
    [SerializeField] private Ufo ufoPrefab;

    private float _height, _width;
    private BasePool _asteroidsPool;
    private int _roundAsteroidsCount;
    private bool _isSpawnUfo = true;

    private void Start()
    {
        _height = ScreenController.Height;
        _width = ScreenController.Width;
        _roundAsteroidsCount = startAsteroidsCount;

        _asteroidsPool = LevelManager.Instance.AsteroidsPool;
        SpawnAsteroids(0);

        StartCoroutine(SpawnUfoProcess());
    }

    public void NewRoundSpawnAsteroids()
    {
        _roundAsteroidsCount++;
        StartCoroutine(SpawnAsteroidsProcess(newRoundSpawnDelay, _roundAsteroidsCount));
    }

    public void SpawnAsteroids(float spawnDelay)
    {
        StartCoroutine(SpawnAsteroidsProcess(spawnDelay, _roundAsteroidsCount));
    }

    private IEnumerator SpawnAsteroidsProcess(float spawnDelay, int count)
    {
        yield return new WaitForSeconds(spawnDelay);

        for(var i = 0; i < count; i++)
        {
            SpawnAsteroid();
        }
    }

    private void SpawnAsteroid()
    {
        var asteroid = _asteroidsPool.PopObject<Asteroid>();
        asteroid.SetStage(1);
        asteroid.gameObject.SetActive(true);
        asteroid.transform.position = new Vector3(Random.Range(-_width / 2, _width / 2), 0, Random.Range(-_height / 2, _height / 2));
    }

    private IEnumerator SpawnUfoProcess()
    {
        while (_isSpawnUfo)
        {
            yield return new WaitForSeconds(Random.Range(minUfoSpawnInterval, maxUfoSpawnInterval));

            SpawnUfo();
        }
    }

    private void SpawnUfo()
    {
        var sideProp = Random.Range(0.0f, 1.0f);;
        var side = sideProp < 0.5f ? -1 : 1;

        var zConstraint = (ScreenController.Height / 2) * UfoSpawnConstraintMultiolier;
        var spawnPosition = new Vector3(side * ScreenController.Width / 2, 0, Random.Range(-zConstraint, zConstraint));
        var ufo = Instantiate(ufoPrefab, spawnPosition, Quaternion.identity);
        ufo.StartMovement(-side);
    }
}
