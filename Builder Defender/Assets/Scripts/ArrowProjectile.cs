using System;
using UnityEngine;

public class ArrowProjectile : MonoBehaviour {
    
    public static ArrowProjectile Create(Vector3 position, Enemy enemy) {
        var pfArrowProjectile = GameAssets.Instance.pfArrowProjectile;
        var arrowTransform = Instantiate(pfArrowProjectile, position, Quaternion.identity);

        var arrowProjectile = arrowTransform.GetComponent<ArrowProjectile>();
        arrowProjectile.SetTarget(enemy);
        return arrowProjectile;
    }
    
    private Enemy _targetEnemy;
    private Vector3 _lastMoveDir;
    private float _timeToDie = 2f;

    private void Update() {
        Vector3 moveDir;
        if (_targetEnemy != null) {
            moveDir = (_targetEnemy.transform.position - transform.position).normalized;
            _lastMoveDir = moveDir;
        }
        else {
            moveDir = _lastMoveDir;
        }

        const float moveSpeed = 20f;
        transform.position += moveDir * moveSpeed * Time.deltaTime;
        
        transform.eulerAngles = new Vector3(0, 0, UtilsClass.GetAngleFromVector(moveDir));

        _timeToDie -= Time.deltaTime;
        if (_timeToDie < 0f) {
            Destroy(gameObject);
        }
    }

    private void SetTarget(Enemy targetEnemy) {
        _targetEnemy = targetEnemy;
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        var enemy = collision.GetComponent<Enemy>();
        if (enemy != null) {
            const int damageAmount = 10;
            enemy.GetComponent<HealthSystem>().Damage(damageAmount);
            Destroy(gameObject);
        }
    }
}
