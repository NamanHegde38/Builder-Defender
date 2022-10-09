using System;
using TMPro;
using UnityEngine;

public class ResourceNearbyOverlay : MonoBehaviour {

    private ResourceGeneratorData _resourceGeneratorData;
    private TextMeshPro _text;

    private void Awake() {
        _text = transform.Find("text").GetComponent<TextMeshPro>();
        Hide();
    }

    private void Update() {
        var nearbyResourceAmount = ResourceGenerator.GetNearbyResourceAmount(_resourceGeneratorData, transform.position - transform.localPosition);
        var percent = Mathf.RoundToInt((float)nearbyResourceAmount / _resourceGeneratorData.maxResourceAmount * 100f);
        _text.SetText(percent + "%");
    }

    public void Show(ResourceGeneratorData resourceGeneratorData) {
        _resourceGeneratorData = resourceGeneratorData;
        gameObject.SetActive(true);

        transform.Find("icon").GetComponent<SpriteRenderer>().sprite = resourceGeneratorData.resourceType.sprite;
    }

    public void Hide() {
        gameObject.SetActive(false);
    }
}