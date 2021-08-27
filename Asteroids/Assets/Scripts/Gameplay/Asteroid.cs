using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    private const int MaxStagesCount = 3;

    [SerializeField] private float startMinVelocity = 0.5f, startMaxVelocity = 3.0f;
    [SerializeField] private int[] stagesCosts = new int[MaxStagesCount];

    private BasePool _asteroidsPool;
    private Rigidbody _rigidbody;
    private int _stage = 1;

    private void Start()
    {
        _asteroidsPool = GetComponentInParent<BasePool>();
        _rigidbody = GetComponent<Rigidbody>();

        var randomForce = new Vector3(Random.Range(startMinVelocity, startMaxVelocity), 0, Random.Range(startMinVelocity, startMaxVelocity));
        _rigidbody.AddForce(randomForce, ForceMode.Acceleration);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out Bullet bullet))
        {
            if(_stage < MaxStagesCount)
            {
                var velocityMultiplier = Random.Range(0.5f, 1.5f);
                ShowNewAsteroid(45, velocityMultiplier);
                ShowNewAsteroid(-45, velocityMultiplier);
            }

            HideAsteroid();

            LevelManager.Instance.AddScorePoints(stagesCosts[_stage - 1]);

            bullet.HideBullet();
        }
        if(other.TryGetComponent(out PlayerController player) && !player.IsVulnerable)
        {
            player.Die();
            HideAsteroid();
        }
        if (other.TryGetComponent(out Ufo ufo))
        {
            ufo.Die();
            HideAsteroid();
        }
    }

    public void SetStage(int stage)
    {
        _stage = stage;
        transform.localScale = Vector3.one * 1 / (1.1f * _stage);
    }

    private void ShowNewAsteroid(float newAsteroidAngle, float velocityMultiplier)
    {
        var asteroid = _asteroidsPool.PopObject<Asteroid>();
        asteroid.gameObject.SetActive(true);
        asteroid.transform.position = transform.position;
        asteroid.SetStage(_stage + 1);

        var rotation = Quaternion.Euler(0, newAsteroidAngle, 0);
        var asteroidRigidbody = asteroid.GetComponent<Rigidbody>();
        asteroidRigidbody.velocity = rotation * _rigidbody.velocity * velocityMultiplier;
    }

    private void HideAsteroid()
    {
        switch (_stage)
        {
            case 1:
                AudioManager.Instance.PlaySound(AudioManager.Sounds.LargeExplosion);
                break;
            case 2:
                AudioManager.Instance.PlaySound(AudioManager.Sounds.MediumExplosion);
                break;
            case 3:
                AudioManager.Instance.PlaySound(AudioManager.Sounds.SmallExplosion);
                break;
        }

        gameObject.SetActive(false);
        _asteroidsPool.PushObject(gameObject);

        if (_asteroidsPool.IsAllObjectsInactive())
        {
            LevelManager.Instance.OnRoundEnd();
        }
    }
}
