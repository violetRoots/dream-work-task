using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ufo : MonoBehaviour
{
    [SerializeField] private float speed = 100.0f;
    [SerializeField] private float minShootInterval = 2, maxShootInterval = 5;
    [SerializeField] private int cost = 200;

    private BasePool _bulletsPool;
    private PlayerController _player;
    private Rigidbody _rigidbody;
    private bool _isShooting = true;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        _bulletsPool = LevelManager.Instance.UfoBulletsPool;
        _player = FindObjectOfType<PlayerController>();
        StartCoroutine(ShootPorcess());
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out PLayerBullet bullet))
        {
            bullet.HideBullet();

            Die();
        }
    }

    public void StartMovement(int direction)
    {
        var force = new Vector3(direction * speed, 0, 0);
        _rigidbody.AddForce(force);
    }

    public void Die()
    {
        LevelManager.Instance.AddScorePoints(cost);
        Destroy(gameObject);
    }

    private IEnumerator ShootPorcess()
    {
        while (_isShooting)
        {
            yield return new WaitForSeconds(Random.Range(minShootInterval, maxShootInterval));

            Shoot();
        }
    }

    private void Shoot()
    {
        if (!_player) return;

        var bullet = _bulletsPool.PopObject<Bullet>();
        bullet.gameObject.SetActive(true);
        bullet.transform.position = transform.position;

        var velocityDirection = (_player.transform.position - transform.position).normalized;
        bullet.StartMovement(velocityDirection);

        AudioManager.Instance.PlaySound(AudioManager.Sounds.Fire);
    }
}
