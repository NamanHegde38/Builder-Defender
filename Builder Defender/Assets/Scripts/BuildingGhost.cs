using System;
using UnityEngine;

public class BuildingGhost : MonoBehaviour {

    private GameObject spriteGameobject;
    private ResourceNearbyOverlay _resourceNearbyOverlay;
    
    private void Awake() {
        spriteGameobject = transform.Find("sprite").gameObject;
        _resourceNearbyOverlay = transform.Find("pfResourceNearbyOverlay").GetComponent<ResourceNearbyOverlay>();
        Hide();
    }

    private void Start() {
        BuildingManager.Instance.OnActiveBuildingTypeChanged += BuildingManager_OnActiveBuildingTypeChanged;
    }

    private void BuildingManager_OnActiveBuildingTypeChanged(object sender, BuildingManager.OnActiveBuildingTypeChangedEventArgs e) {
        if (e.ActiveBuildingType == null) {
            Hide();
            _resourceNearbyOverlay.Hide();
        }
        else {
            Show(e.ActiveBuildingType.sprite);
            if (e.ActiveBuildingType.hasResourceGeneratorData) {
                _resourceNearbyOverlay.Show(e.ActiveBuildingType.resourceGeneratorData);
            }
            else {
                _resourceNearbyOverlay.Hide();
            }
        }
    }

    private void Update() {
        transform.position = UtilsClass.GetMouseWorldPosition();
    }

    private void Show(Sprite ghostSprite) {
        spriteGameobject.SetActive(true);
        spriteGameobject.GetComponent<SpriteRenderer>().sprite = ghostSprite;
    }

    private void Hide() {
        spriteGameobject.SetActive(false);
    }
}
