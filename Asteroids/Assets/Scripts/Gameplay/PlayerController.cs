using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private const float InvulnerabilityIntervalActive = 0.3f;
    private const float InvulnerabilityIntervalInactive = 0.2f;
    private const int InvulnerabilityBlinkCount = 6;

    public enum MovementMode
    {
        KeyboardOnly,
        KeyboardAndMouse
    }
    public bool IsVulnerable => _isInvulnerable;

    [SerializeField] private MovementMode movementMode;
    [SerializeField] private float maxVelocity = 10.0f;
    [SerializeField] private float acelerationForce = 1.0f;
    [SerializeField] private float keyboardRotationSpeed = 100.0f;
    [SerializeField] private float mouseRotationSpeed = 1.0f;
    [SerializeField] private int maxShotsPerSecond = 3;
    [SerializeField] private int startLifesCount = 3;
    [Space]
    [SerializeField] private Transform forwardObj;

    private Rigidbody _rigidbody;
    private Camera _camera;
    private BasePool _bulletsPool;
    private int _shotsPerSecond;
    private float _lastShotTime;
    private bool _isInvulnerable;
    private int _lifesCount;

    private void Start()
    {
        _bulletsPool = LevelManager.Instance.PlayerBulletsPool;
        _rigidbody = GetComponent<Rigidbody>();
        _camera = Camera.main;

        _lifesCount = startLifesCount;
        InterfaceManager.Instance.UpdateLifesText(_lifesCount);

        BecomeInvulnerable();
    }

    private void Update()
    {
        if(movementMode == MovementMode.KeyboardOnly)
        {
            if (Input.GetKey(KeyCode.W))
            {
                AddAceleretion();
            }
            if (Input.GetKey(KeyCode.A))
            {
                RotateToDirection(false);
            }
            if (Input.GetKey(KeyCode.D))
            {
                RotateToDirection(true);
            }
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Shoot();
            }
        }
        else
        {
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow) || Input.GetMouseButton(1))
            {
                AddAceleretion();
            }
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
            {
                Shoot();
            }

            RotateAfterMouse();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out UfoBullet bullet))
        {
            bullet.HideBullet();

            Die();
        }
    }

    private void AddAceleretion()
    {
        _rigidbody.AddForce(transform.forward * acelerationForce, ForceMode.Acceleration);

        _rigidbody.velocity = Vector3.ClampMagnitude(_rigidbody.velocity, maxVelocity);

        AudioManager.Instance.PlaySound(AudioManager.Sounds.Thrust);
    }

    private void RotateAfterMouse()
    {
        if (GameManager.Instance.IsGamePaused) return;

        var mousePos = Input.mousePosition;
        mousePos.z = _camera.transform.position.y;
        var worldMousePos = Camera.main.ScreenToWorldPoint(mousePos);
        transform.rotation = Quaternion.LookRotation(Vector3.RotateTowards(transform.forward, worldMousePos - transform.position, mouseRotationSpeed, 0));
    }

    private void RotateToDirection(bool isClockwise)
    {
        var rotation = new Vector3(0, keyboardRotationSpeed * Time.deltaTime, 0);
        rotation = isClockwise ? rotation : -rotation;
        transform.Rotate(rotation);
    }

    private void Shoot()
    {
        if (Time.realtimeSinceStartup - _lastShotTime > 1)
        {
            _shotsPerSecond = 0;
        }
        else
        {
            _shotsPerSecond++;
        }

        if (_shotsPerSecond >= maxShotsPerSecond) return;

        var bullet = _bulletsPool.PopObject<Bullet>();
        bullet.gameObject.SetActive(true);
        bullet.transform.position = forwardObj.position;
        bullet.transform.rotation = transform.rotation;
        bullet.StartMovement(bullet.transform.forward);

        _lastShotTime = Time.realtimeSinceStartup;

        AudioManager.Instance.PlaySound(AudioManager.Sounds.Fire);
    }

    private void BecomeInvulnerable()
    {
        StartCoroutine(InvulnerablilityProcess());
    }

    private IEnumerator InvulnerablilityProcess()
    {
        var meshRenderer = GetComponent<MeshRenderer>();
        var forwardObjRenderer = forwardObj.GetComponent<MeshRenderer>();

        _isInvulnerable = true;

        for(var i = 0; i < InvulnerabilityBlinkCount; i++)
        {
            meshRenderer.enabled = false;
            forwardObjRenderer.enabled = false;

            yield return new WaitForSeconds(InvulnerabilityIntervalInactive);

            meshRenderer.enabled = true;
            forwardObjRenderer.enabled = true;

            yield return new WaitForSeconds(InvulnerabilityIntervalActive);
        }

        _isInvulnerable = false;
    }

    public void SetMovementMode(MovementMode mode)
    {
        movementMode = mode;
    }

    public void Die()
    {
        _lifesCount--;
        InterfaceManager.Instance.UpdateLifesText(_lifesCount);

        if (_lifesCount > 0)
        {
            transform.position = Vector3.zero;
            _rigidbody.velocity = Vector3.zero;
            BecomeInvulnerable();
        }
        else
        {
            gameObject.SetActive(false);
            LevelManager.Instance.Loose();
        }
    }
}
