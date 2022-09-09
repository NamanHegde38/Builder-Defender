using System;
using TMPro;
using UnityEngine;

public class EnemyWaveUI : MonoBehaviour {

    [SerializeField] private EnemyWaveManager enemyWaveManager;
    
    private Camera _mainCamera;
    private TextMeshProUGUI _waveNumberText;
    private TextMeshProUGUI _waveMessageText;
    private RectTransform _enemyWaveSpawnPositionIndicator;
    private RectTransform _enemyClosestPositionIndicator;

    private void Awake() {
        _waveNumberText = transform.Find("waveNumberText").GetComponent<TextMeshProUGUI>();
        _waveMessageText = transform.Find("waveMessageText").GetComponent<TextMeshProUGUI>();
        _enemyWaveSpawnPositionIndicator = transform.Find("enemyWaveSpawnPositionIndicator").GetComponent<RectTransform>();
        _enemyClosestPositionIndicator = transform.Find("enemyClosestPositionIndicator").GetComponent<RectTransform>();
    }

    private void Start() {
        _mainCamera = Camera.main; 
        enemyWaveManager.OnWaveNumberChanged += EnemyWaveManager_OnWaveNumberChanged;
        SetWaveNumberText("Wave " + enemyWaveManager.GetWaveNumber());
    }

    private void EnemyWaveManager_OnWaveNumberChanged(object sender, EventArgs e) {
        SetWaveNumberText("Wave " + enemyWaveManager.GetWaveNumber());
    }

    private void Update() {
        HandleNextWaveMessage();
        HandleEnemyWaveSpawnPositionIndicator();
        HandleEnemyClosestPositionIndicator();
    }

    private void HandleNextWaveMessage() {
        var nextWaveSpawnTimer = enemyWaveManager.GetNextWaveSpawnTimer();
        if (nextWaveSpawnTimer <= 0f) {
            SetMessageText("");
        }
        else {
            SetMessageText("Next Wave in " + nextWaveSpawnTimer.ToString("F1") + "s");
        }
    }
    
    private void HandleEnemyWaveSpawnPositionIndicator() {
        var dirToNextSpawnPosition = (enemyWaveManager.GetSpawnPosition() - _mainCamera.transform.position).normalized;
        _enemyWaveSpawnPositionIndicator.anchoredPosition = dirToNextSpawnPosition * 300f;
        _enemyWaveSpawnPositionIndicator.eulerAngles = new Vector3(0, 0, UtilsClass.GetAngleFromVector(dirToNextSpawnPosition));
        
        var distanceToNextSpawnPosition = Vector3.Distance(enemyWaveManager.GetSpawnPosition(), _mainCamera.transform.position);
        _enemyWaveSpawnPositionIndicator.gameObject.SetActive(distanceToNextSpawnPosition > _mainCamera.orthographicSize * 1.5f);
    }

    private void HandleEnemyClosestPositionIndicator() {
        const float targetMaxRadius = 999f;
        var collider2DArray = Physics2D.OverlapCircleAll(_mainCamera.transform.position, targetMaxRadius);

        Enemy targetEnemy = null;
        
        // ReSharper disable once LocalVariableHidesMember
        foreach (var collider2D in collider2DArray) {
            var enemy = collider2D.GetComponent<Enemy>();
            if (enemy != null) {
                if (targetEnemy == null) {
                    targetEnemy = enemy;
                }
                else {
                    if (Vector3.Distance(transform.position, enemy.transform.position) <
                        Vector3.Distance(transform.position, targetEnemy.transform.position)) {
                        targetEnemy = enemy;
                    }
                }
            }
        }

        if (targetEnemy != null) {
            var dirToClosestEnemy = (targetEnemy.transform.position - _mainCamera.transform.position).normalized;
            _enemyClosestPositionIndicator.anchoredPosition = dirToClosestEnemy * 250f;
            _enemyClosestPositionIndicator.eulerAngles = new Vector3(0, 0, UtilsClass.GetAngleFromVector(dirToClosestEnemy));
        
            var distanceToClosestEnemy = Vector3.Distance(targetEnemy.transform.position, _mainCamera.transform.position);
            _enemyClosestPositionIndicator.gameObject.SetActive(distanceToClosestEnemy > _mainCamera.orthographicSize * 1.5f);
        }
        else {
            _enemyClosestPositionIndicator.gameObject.SetActive(false);
        }
    }

    private void SetMessageText(string message) {
        _waveMessageText.SetText(message);
    }

    private void SetWaveNumberText(string text) {
        _waveNumberText.SetText(text);
    }
}
