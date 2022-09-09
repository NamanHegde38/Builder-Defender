using System;
using UnityEngine;
using UnityEngine.UI;

public class ConstructionTimerUI : MonoBehaviour {

    [SerializeField] private BuildingConstruction buildingConstruction;
    private Image _constructionProgressImage;
    
    private void Awake() {
        _constructionProgressImage = transform.Find("mask").Find("image").GetComponent<Image>();
    }

    private void Update() {
        _constructionProgressImage.fillAmount = buildingConstruction.GetConstructionTimerNormalized();
    }
}
