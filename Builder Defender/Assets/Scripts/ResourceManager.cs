using System;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour {
    
    public static ResourceManager Instance { get; private set; }

    public event EventHandler OnResourceAmountChanged;
    
    
    private Dictionary<ResourceTypeSO, int> _resourceAmountDictionary;

    private void Awake() {
        Instance = this;
        _resourceAmountDictionary = new Dictionary<ResourceTypeSO, int>();

        var resourceTypeList = Resources.Load<ResourceTypeListSO>(nameof(ResourceTypeListSO));

        foreach (var resourceType in resourceTypeList.list) {
            _resourceAmountDictionary[resourceType] = 0;
        }
        
        TestLogResourceAmountDictionary();
    }

    private void TestLogResourceAmountDictionary() {
        foreach (var resourceType in _resourceAmountDictionary.Keys) {
            Debug.Log(resourceType.name + ": " + _resourceAmountDictionary[resourceType]);
        }
    }

    public void AddResource(ResourceTypeSO resourceType, int amount) {
        _resourceAmountDictionary[resourceType] += amount;
        OnResourceAmountChanged?.Invoke(this, EventArgs.Empty);
        TestLogResourceAmountDictionary();
    }

    public int GetResourceAmount(ResourceTypeSO resourceType) {
        return _resourceAmountDictionary[resourceType];
    }
}
