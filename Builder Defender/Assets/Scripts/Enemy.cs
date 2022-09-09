using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class Enemy : MonoBehaviour {

    public static Enemy Create(Vector3 position) {
        var pfEnemy = Resources.Load<Transform>("pfEnemy");
        var enemyTransform = Instantiate(pfEnemy, position, Quaternion.identity);

        var enemy = enemyTransform.GetComponent<Enemy>();
        return enemy;
    }

    private HealthSystem _healthSystem;
    private Transform _targetTransform;
    private Rigidbody2D _rigidbody2D;
    private float _lookForTargetTimer;
    private const float LookForTargetTimerMax = .2f;

    private void Start() {
        if (BuildingManager.Instance.GetHQBuilding() != null) {
            _targetTransform = BuildingManager.Instance.GetHQBuilding().transform;
        }
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _healthSystem = GetComponent<HealthSystem>();
        _healthSystem.OnDamaged += HealthSystem_OnDamaged;
        _healthSystem.OnDied += HealthSystem_OnDied;

        _lookForTargetTimer = Random.Range(0f, LookForTargetTimerMax);
    }

    private void HealthSystem_OnDamaged(object sender, EventArgs e) {
        SoundManager.Instance.PlaySound(SoundManager.Sound.EnemyHit);
    }

    private void HealthSystem_OnDied(object sender, EventArgs e) {
        SoundManager.Instance.PlaySound(SoundManager.Sound.EnemyDie);
        Destroy(gameObject);
    }

    private void Update() {
        HandleMovement();
        HandleTargeting();
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        var building = collision.gameObject.GetComponent<Building>();
        if (building != null) {
            var healthSystem = building.GetComponent<HealthSystem>();
            healthSystem.Damage(10);
            Destroy(gameObject);
        }
    }

    private void HandleMovement() {
        if (_targetTransform != null) {
            var moveDir = (_targetTransform.position - transform.position).normalized;

            const float moveSpeed = 6f;
            _rigidbody2D.velocity = moveDir * moveSpeed;
        }
        else {
            _rigidbody2D.velocity = Vector2.zero;
        }
    }

    private void HandleTargeting() {
        _lookForTargetTimer -= Time.deltaTime;
        if (_lookForTargetTimer < 0f) {
            _lookForTargetTimer += LookForTargetTimerMax;
            LookForTargets();
        }
    }

    private void LookForTargets() {
        const float targetMaxRadius = 10f;
        var collider2DArray = Physics2D.OverlapCircleAll(transform.position, targetMaxRadius);

        // ReSharper disable once LocalVariableHidesMember
        foreach (var collider2D in collider2DArray) {
            var building = collider2D.GetComponent<Building>();
            if (building != null) {
                if (_targetTransform == null) {
                    _targetTransform = building.transform;
                }
                else {
                    if (Vector3.Distance(transform.position, building.transform.position) <
                        Vector3.Distance(transform.position, _targetTransform.position)) {
                        _targetTransform = building.transform;
                    }
                }
            }

            if (_targetTransform == null) {
                if (BuildingManager.Instance.GetHQBuilding() != null) {
                    _targetTransform = BuildingManager.Instance.GetHQBuilding().transform;
                }
            }
        }
    }
}