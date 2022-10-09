using System;
using UnityEngine;

public class Building : MonoBehaviour {

    private BuildingTypeSO _buildingType;
    private HealthSystem _healthSystem;
    private Transform _buildingDemolishBtn;
    private Transform _buildingRepairBtn;

    private void Awake() {
        _buildingDemolishBtn = transform.Find("pfBuildingDemolishBtn");
        _buildingRepairBtn = transform.Find("pfBuildingRepairBtn");
        HideBuildingDemolishBtn();
        HideBuildingRepairBtn();
        
        _buildingType = GetComponent<BuildingTypeHolder>().buildingType;
        _healthSystem = GetComponent<HealthSystem>();

        _healthSystem.SetHealthAmountMax(_buildingType.healthAmountMax, true);
    }

    private void Start() {
        _healthSystem.OnDamaged += HealthSystem_OnDamaged;
        _healthSystem.OnHealed += HealthSystem_OnHealed;
        _healthSystem.OnDied += HealthSystem_OnDied;
    }

    private void HealthSystem_OnHealed(object sender, EventArgs e) {
        if (_healthSystem.IsFullHealth()) {
            HideBuildingRepairBtn();
        }
    }

    private void HealthSystem_OnDamaged(object sender, EventArgs e) {
        ShowBuildingRepairBtn();
        SoundManager.Instance.PlaySound(SoundManager.Sound.BuildingDamaged);
        CinemachineShake.Instance.ShakeCamera(5f, .15f);
        ChromaticAberrationEffect.Instance.SetWeight(0.5f);
    }

    private void HealthSystem_OnDied(object sender, EventArgs e) {
        SoundManager.Instance.PlaySound(SoundManager.Sound.BuildingDestroyed);
        CinemachineShake.Instance.ShakeCamera(10f, .2f);
        Instantiate(GameAssets.Instance.pfBuildingDestroyedParticles, transform.position, Quaternion.identity);
        ChromaticAberrationEffect.Instance.SetWeight(1f);
        Destroy(gameObject);
    }

    private void OnMouseEnter() {
        ShowBuildingDemolishBtn();
    }

    private void OnMouseExit() {
        HideBuildingDemolishBtn();
    }

    private void ShowBuildingDemolishBtn() {
        if (_buildingDemolishBtn != null) {
            _buildingDemolishBtn.gameObject.SetActive(true);
        }
    }
    
    private void HideBuildingDemolishBtn() {
        if (_buildingDemolishBtn != null) {
            _buildingDemolishBtn.gameObject.SetActive(false);
        }
    }
    
    private void ShowBuildingRepairBtn() {
        if (_buildingRepairBtn != null) {
            _buildingRepairBtn.gameObject.SetActive(true);
        }
    }
    
    private void HideBuildingRepairBtn() {
        if (_buildingRepairBtn != null) {
            _buildingRepairBtn.gameObject.SetActive(false);
        }
    }
}
