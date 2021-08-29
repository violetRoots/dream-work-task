using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FinishingController : MonoBehaviour
{
    private const string FinishingAnimatorTrigger = "Finishing";

    [Range(0.0f, 1.0f)]
    [SerializeField] private float enemyDieTimeMultiplier = 0.5f;
    [SerializeField] private float rotationSpeed = 1.0f;
    [SerializeField] private float movementSpeed = 1.0f;
    [SerializeField] private float enemyDistance = 1.0f;
    [Space]
    [SerializeField] private GameObject sword;
    [SerializeField] private GameObject automatic;
    [Space]
    [SerializeField] private Text finishingText;

    private Animator _animator;
    private Transform _playerModel;
    private Enemy _finishingEnemy;
    private MovementController _movementController;

    private void Start()
    {
        _animator = GetComponentInChildren<Animator>();
        _movementController = GetComponent<MovementController>();
        _playerModel = _animator.transform;
    }

    private void Update()
    {
        if (!_finishingEnemy) return;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(FinishEnemyProcess());
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.TryGetComponent(out FinishingArea finishingArea) && !finishingArea.Enemy.IsDead)
        {
            _finishingEnemy = finishingArea.Enemy;
            finishingText.gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out FinishingArea finishingArea) && !finishingArea.Enemy.IsDead)
        {
            _finishingEnemy = null;
            finishingText.gameObject.SetActive(false);
        }
    }

    private IEnumerator FinishEnemyProcess()
    {
        _animator.SetTrigger(FinishingAnimatorTrigger);
        var finishingLength = _animator.GetCurrentAnimatorClipInfo(0).Length;

        automatic.SetActive(false);
        sword.SetActive(true);
        _movementController.isRotateAfterMouse = false;
        _movementController.isCanMove = false;

        var lerpCount = 0.0f;
        var startRotation = _playerModel.rotation;
        var endRotation = Quaternion.LookRotation(_finishingEnemy.transform.position - transform.position);
        while (lerpCount < 1)
        {
            lerpCount += rotationSpeed * Time.deltaTime;
            _playerModel.rotation = Quaternion.Lerp(startRotation, endRotation, lerpCount);

            yield return null;
        }

        lerpCount = 0;
        var startPosition = transform.position;
        var endPosition = _finishingEnemy.transform.position;
        endPosition += (transform.position - endPosition).normalized * enemyDistance;

        while (lerpCount < 1)
        {
            lerpCount += movementSpeed * Time.deltaTime;
            transform.position = Vector3.Lerp(startPosition, endPosition, lerpCount);

            yield return null;
        }

        yield return new WaitForSeconds(enemyDieTimeMultiplier * finishingLength);

        _finishingEnemy.Die();

        yield return new WaitForSeconds((1-enemyDieTimeMultiplier) * finishingLength);

        automatic.SetActive(true);
        sword.SetActive(false);
        _movementController.isRotateAfterMouse = true;
        _movementController.isCanMove = true;
        finishingText.gameObject.SetActive(false);
    }
}
