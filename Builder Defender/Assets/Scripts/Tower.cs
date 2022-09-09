using UnityEngine;

public class Tower : MonoBehaviour {

    [SerializeField] private float shootTimerMax = .3f;
    private float _shootTimer;
    private Enemy _targetEnemy;
    private float _lookForTargetTimer;
    private const float LookForTargetTimerMax = .2f;
    private Vector3 _projectileSpawnPosition;

    private void Awake() {
        _projectileSpawnPosition = transform.Find("projectileSpawnPosition").position;
    }

    private void Update() {
        HandleTargeting();
        HandleShooting();
    }

    private void HandleTargeting() {
        _lookForTargetTimer -= Time.deltaTime;
        if (_lookForTargetTimer < 0f) {
            _lookForTargetTimer += LookForTargetTimerMax;
            LookForTargets();
        }
    }
    
    private void HandleShooting() {
        _shootTimer -= Time.deltaTime;
        if (_shootTimer <= 0f) {
            _shootTimer += shootTimerMax;
            if (_targetEnemy != null) {
                ArrowProjectile.Create(_projectileSpawnPosition, _targetEnemy);
            }
        }
        
    }
    
    private void LookForTargets() {
        const float targetMaxRadius = 30f;
        var collider2DArray = Physics2D.OverlapCircleAll(transform.position, targetMaxRadius);

        // ReSharper disable once LocalVariableHidesMember
        foreach (var collider2D in collider2DArray) {
            var enemy = collider2D.GetComponent<Enemy>();
            if (enemy != null) {
                if (_targetEnemy == null) {
                    _targetEnemy = enemy;
                }
                else {
                    if (Vector3.Distance(transform.position, enemy.transform.position) <
                        Vector3.Distance(transform.position, _targetEnemy.transform.position)) {
                        _targetEnemy = enemy;
                    }
                }
            }
        }
    }
}
