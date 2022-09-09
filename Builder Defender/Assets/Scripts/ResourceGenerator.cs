using UnityEngine;

public class ResourceGenerator : MonoBehaviour {

    public static int GetNearbyResourceAmount(ResourceGeneratorData resourceGeneratorData, Vector3 position) {
        var collider2DArray = Physics2D.OverlapCircleAll(position, resourceGeneratorData.resourceDetectionRadius);

        var nearbyResourceAmount = 0;
        
        // ReSharper disable once LocalVariableHidesMember
        foreach (var collider2D in collider2DArray) {
            var resourceNode = collider2D.GetComponent<ResourceNode>();
            if (resourceNode != null) {
                if (resourceNode.resourceType == resourceGeneratorData.resourceType) {
                    nearbyResourceAmount++;
                }
            }
        }

        nearbyResourceAmount = Mathf.Clamp(nearbyResourceAmount, 0, resourceGeneratorData.maxResourceAmount);
        
        return nearbyResourceAmount;
    }
    
    private ResourceGeneratorData _resourceGeneratorData;
    private float _timer;
    private float _timerMax;

    private void Awake() {
        _resourceGeneratorData = GetComponent<BuildingTypeHolder>().buildingType.resourceGeneratorData;
        _timerMax = _resourceGeneratorData.timerMax;
    }

    private void Start() {
        var nearbyResourceAmount = GetNearbyResourceAmount(_resourceGeneratorData, transform.position);
        if (nearbyResourceAmount == 0) {
            enabled = false;
        }
        else {
            _timerMax = (_resourceGeneratorData.timerMax / 2f) + 
                        _resourceGeneratorData.timerMax * 
                        (1 - (float) nearbyResourceAmount / _resourceGeneratorData.maxResourceAmount);
        }

    }

    private void Update() {
        _timer -= Time.deltaTime;
        if (_timer <= 0f) {
            _timer += _timerMax;
            ResourceManager.Instance.AddResource(_resourceGeneratorData.resourceType, 1);
        }
    }

    public ResourceGeneratorData GetResourceGeneratorData() {
        return _resourceGeneratorData;
    }

    public float GetTimerNormalized() {
        return _timer / _timerMax;
    }

    public float GetAmountGeneratedPerSecond() {
        return 1 / _timerMax;
    }
}
