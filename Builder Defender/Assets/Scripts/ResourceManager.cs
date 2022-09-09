using System;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour {

    public static ResourceManager Instance { get; private set; }

    public event EventHandler OnResourceAmountChanged;

    [SerializeField] private List<ResourceAmount> startingResourceAmountList;
    
    private Dictionary<ResourceTypeSO, int> _resourceAmountDictionary;

    private void Awake() {
        Instance = this;
        _resourceAmountDictionary = new Dictionary<ResourceTypeSO, int>();

        var resourceTypeList = Resources.Load<ResourceTypeListSO>(nameof(ResourceTypeListSO));

        foreach (var resourceType in resourceTypeList.list) {
            _resourceAmountDictionary[resourceType] = 0;
        }

        foreach (var resourceAmount in startingResourceAmountList) {
            AddResource(resourceAmount.resourceType, resourceAmount.amount);
        }
    }

    public void AddResource(ResourceTypeSO resourceType, int amount) {
        _resourceAmountDictionary[resourceType] += amount;
        OnResourceAmountChanged?.Invoke(this, EventArgs.Empty);
    }

    public int GetResourceAmount(ResourceTypeSO resourceType) {
        return _resourceAmountDictionary[resourceType];
    }

    public bool CanAfford(ResourceAmount[] resourceAmountArray) {
        foreach (var resourceAmount in resourceAmountArray) {
            if (GetResourceAmount(resourceAmount.resourceType) >= resourceAmount.amount) {

            }
            else {
                return false;
            }
        }

        return true;
    }

    public void SpendResources(ResourceAmount[] resourceAmountArray) {
        foreach (var resourceAmount in resourceAmountArray) {
            _resourceAmountDictionary[resourceAmount.resourceType] -= resourceAmount.amount;

        }
    }
}
