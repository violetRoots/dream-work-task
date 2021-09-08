using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float speed = 1.0f;

    private SpaceObject _spaceObject;
    private Rigidbody _rigidbody;
    private BasePool _bulletsPool;
    private Vector3 _previousPosition;
    private float _screenWidth;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _spaceObject = GetComponent<SpaceObject>();
    }

    protected virtual void Start()
    {
        _screenWidth = ScreenController.Width;
        _bulletsPool = GetComponentInParent<BasePool>();
    }

    private void Update()
    {
        if (_spaceObject.PathLength <= _screenWidth) return;

        HideBullet();
    }

    public void HideBullet()
    {
        _spaceObject.ResetPath();
        gameObject.SetActive(false);
        _bulletsPool.PushObject(gameObject);
    }

    public void StartMovement(Vector3 velocityDirection)
    {
        _rigidbody.velocity = velocityDirection * speed;
        _previousPosition = transform.position;
    }
}
