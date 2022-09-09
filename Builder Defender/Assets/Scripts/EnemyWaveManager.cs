using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyWaveManager : MonoBehaviour {
    
    public static EnemyWaveManager Instance { get; private set; }
    
    public event EventHandler OnWaveNumberChanged;
    
    private enum State {
        WaitingToSpawnNextWave,
        SpawningWave,
    }

    [SerializeField] private List<Transform> spawnPositionTransformList;
    [SerializeField] private Transform nextWaveSpawnPosition;
        
    private State _state;
    private int _waveNumber;
    private float _nextWaveSpawnTimer;
    private float _nextEnemySpawnTimer;
    private int _remainingEnemySpawnAmount;

    private Vector3 _spawnPosition;

    private void Awake() {
        Instance = this;
    }

    private void Start() {
        _state = State.WaitingToSpawnNextWave;
        _spawnPosition = spawnPositionTransformList[Random.Range(0, spawnPositionTransformList.Count)].position;
        nextWaveSpawnPosition.position = _spawnPosition;
        _nextWaveSpawnTimer = 3f;
    }

    private void Update() {
        switch (_state) {
            case State.WaitingToSpawnNextWave:
                _nextWaveSpawnTimer -= Time.deltaTime;
                if (_nextWaveSpawnTimer < 0f) {
                    SpawnWave();
                }
                break;
            case State.SpawningWave:
                if (_remainingEnemySpawnAmount > 0) {
                    _nextEnemySpawnTimer -= Time.deltaTime;
                    if (_nextEnemySpawnTimer < 0f) {
                        _nextEnemySpawnTimer = Random.Range(0f, .2f);
                        Enemy.Create(_spawnPosition + UtilsClass.GetRandomDir() * Random.Range(0f, 10f));
                        _remainingEnemySpawnAmount--;

                        if (_remainingEnemySpawnAmount <= 0f) {
                            _state = State.WaitingToSpawnNextWave;
                            _spawnPosition = spawnPositionTransformList[Random.Range(0, spawnPositionTransformList.Count)].position;
                            nextWaveSpawnPosition.position = _spawnPosition;
                            _nextWaveSpawnTimer = 10f;
                        }
                    }
                }
                break;
        }
    }

    private void SpawnWave() {
        _remainingEnemySpawnAmount = 5 + 3 * _waveNumber;
        _state = State.SpawningWave;
        _waveNumber++;
        OnWaveNumberChanged?.Invoke(this, EventArgs.Empty);
    }

    public int GetWaveNumber() {
        return _waveNumber;
    }

    public float GetNextWaveSpawnTimer() {
        return _nextWaveSpawnTimer;
    }

    public Vector3 GetSpawnPosition() {
        return _spawnPosition;
    }
}
