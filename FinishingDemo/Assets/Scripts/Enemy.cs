using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public bool IsDead { get; private set; }

    [SerializeField] private float spawnDelay = 5.0f;
    [SerializeField] private Vector2 spawnBounds = new Vector2(10.0f, 10.0f);

    private Animator _animator;
    private Rigidbody[] _ragdollRigidbodies;

    private void Start()
    {
        _animator = GetComponentInChildren<Animator>();
        _ragdollRigidbodies = GetComponentsInChildren<Rigidbody>();

        SetIsRagdoll(false);
    }

    public void Die()
    {
        StartCoroutine(DieProcess());
    }

    private IEnumerator DieProcess()
    {
        IsDead = true;
        SetIsRagdoll(true);

        yield return new WaitForSeconds(spawnDelay);

        IsDead = false;
        SetIsRagdoll(false);
        transform.position = new Vector3(Random.Range(-spawnBounds.x, spawnBounds.x), 0, Random.Range(-spawnBounds.y, spawnBounds.y));
    }

    private void SetIsRagdoll(bool isRagdoll)
    {
        _animator.enabled = !isRagdoll;

        foreach(var rigidbody in _ragdollRigidbodies)
        {
            rigidbody.isKinematic = !isRagdoll;
            rigidbody.detectCollisions = isRagdoll;
        }
    }
}
