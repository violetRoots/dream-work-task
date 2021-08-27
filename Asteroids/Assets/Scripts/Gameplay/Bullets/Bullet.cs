using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float speed = 1.0f;

    private Rigidbody _rigidbody;
    private BasePool _bulletsPool;
    private Vector3 _previousPosition;
    private float _pathLength;
    private float _screenWidth;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        _screenWidth = ScreenController.Width;
        _bulletsPool = GetComponentInParent<BasePool>();
    }

    private void Update()
    {
        _pathLength += Vector3.Distance(_previousPosition, transform.position);
        _previousPosition = transform.position;

        if (_pathLength <= _screenWidth) return;

        HideBullet();
    }

    public void HideBullet()
    {
        _pathLength = 0;
        gameObject.SetActive(false);
        _bulletsPool.PushObject(gameObject);
    }

    public void StartMovement(Vector3 velocityDirection)
    {
        _rigidbody.velocity = velocityDirection * speed;
        _previousPosition = transform.position;
    }
}
