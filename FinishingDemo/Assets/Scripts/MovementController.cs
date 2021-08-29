using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    private const string VelocityAnimatorParameter = "Velocity";

    public bool isRotateAfterMouse = true;
    public bool isCanMove = true;

    [SerializeField] private float speed = 10.0f;
    [SerializeField] private Transform spine;

    private Rigidbody _rigidbody;
    private Camera _camera;
    private Vector3 _velocity;
    private Animator _animator;
    private Transform _playerModel;

    private void Start()
    {
        _camera = Camera.main;
        _rigidbody = GetComponent<Rigidbody>();
        _animator = GetComponentInChildren<Animator>();
        _playerModel = _animator.transform;
    }

    private void FixedUpdate()
    {
        if (!isCanMove) return;

        var direction = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        _velocity = Vector3.ClampMagnitude(Quaternion.Euler(0, _camera.transform.rotation.eulerAngles.y, 0) * direction * speed, speed);
        _rigidbody.velocity = _velocity;
        _animator.SetFloat(VelocityAnimatorParameter, _velocity.magnitude);

        if (_velocity == Vector3.zero) return;
        _playerModel.rotation = Quaternion.LookRotation(Quaternion.Euler(0, -90, 0) * _velocity);
    }

    private void LateUpdate()
    {
        if (!isRotateAfterMouse) return;

        var mousePos = Input.mousePosition;
        mousePos.z = _camera.transform.position.y;
        var worldMousePos = _camera.ScreenToWorldPoint(mousePos);
        var mouseRotation = Quaternion.LookRotation(worldMousePos - transform.position);
        var newRotation = spine.rotation.eulerAngles;
        newRotation.y = mouseRotation.eulerAngles.y - 90;
        spine.rotation = Quaternion.Euler(newRotation);
    }
}
