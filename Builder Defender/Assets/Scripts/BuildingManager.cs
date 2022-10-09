using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class BuildingManager : MonoBehaviour {
    
    public static BuildingManager Instance { get; private set; }

    public event EventHandler<OnActiveBuildingTypeChangedEventArgs> OnActiveBuildingTypeChanged;

    public class OnActiveBuildingTypeChangedEventArgs : EventArgs {
        public BuildingTypeSO ActiveBuildingType;
    }

    [SerializeField] private Building hqBuilding;
    
    private Camera _mainCamera;
    private BuildingTypeListSO _buildingTypeList;
    private BuildingTypeSO _activeBuildingType;

    private void Awake() {
        Instance = this;
        _buildingTypeList = Resources.Load<BuildingTypeListSO>(nameof(BuildingTypeListSO));
    }

    private void Start() {
        _mainCamera = Camera.main;

        hqBuilding.GetComponent<HealthSystem>().OnDied += HQ_OnDied;
    }

    private void HQ_OnDied(object sender, EventArgs e) {
        SoundManager.Instance.PlaySound(SoundManager.Sound.GameOver);
        GameOverUI.Instance.Show();
    }

    private void Update() {
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject()) {
            if (_activeBuildingType != null) {
                if (CanSpawnBuilding(_activeBuildingType, UtilsClass.GetMouseWorldPosition(), out var errorMessage)) {
                    if (ResourceManager.Instance.CanAfford(_activeBuildingType.constructionResourceCostArray)) {
                        ResourceManager.Instance.SpendResources(_activeBuildingType.constructionResourceCostArray);
                        //Instantiate(_activeBuildingType.prefab, UtilsClass.GetMouseWorldPosition(), Quaternion.identity);
                        BuildingConstruction.Create(UtilsClass.GetMouseWorldPosition(), _activeBuildingType);
                        SoundManager.Instance.PlaySound(SoundManager.Sound.BuildingPlaced);
                    }
                    else {
                        TooltipUI.Instance.Show("Cannot afford " + _activeBuildingType.GetConstructionResourceCostString(), new TooltipUI.TooltipTimer { Timer = 2f });
                    }
                }
                else {
                    TooltipUI.Instance.Show(errorMessage, new TooltipUI.TooltipTimer { Timer = 2f });
                }
            }
        }
    }

    public void SetActiveBuildingType(BuildingTypeSO buildingType) {
        _activeBuildingType = buildingType;
        OnActiveBuildingTypeChanged?.Invoke(this,
            new OnActiveBuildingTypeChangedEventArgs{ActiveBuildingType = _activeBuildingType});
    }

    public BuildingTypeSO GetActiveBuildingType() {
        return _activeBuildingType;
    }

    private bool CanSpawnBuilding(BuildingTypeSO buildingType, Vector3 position, out string errorMessage) {
        var boxCollider2D = buildingType.prefab.GetComponent<BoxCollider2D>();

        var collider2DArray = Physics2D.OverlapBoxAll(position + (Vector3)boxCollider2D.offset, boxCollider2D.size, 0);

        var isAreaClear = collider2DArray.Length == 0;
        if (!isAreaClear) {
            errorMessage = "Area is not clear!";
            return false;
        }

        collider2DArray = Physics2D.OverlapCircleAll(position, buildingType.minConstructionRadius);
        
        // ReSharper disable once LocalVariableHidesMember
        foreach (var collider2D in collider2DArray) {
            var buildingTypeHolder = collider2D.GetComponent<BuildingTypeHolder>();
            if (buildingTypeHolder != null) {
                if (buildingTypeHolder.buildingType == buildingType) {
                    errorMessage = "Too close to another building of the same type!";
                    return false;
                }
            }
        }

        /*if (buildingType.hasResourceGeneratorData) {
            var resourceGeneratorData = buildingType.resourceGeneratorData;
            var nearbyResourceAmount = ResourceGenerator.GetNearbyResourceAmount(resourceGeneratorData, transform.position);

            if (nearbyResourceAmount <= 0) {
                errorMessage = "There are no nearby Resource Nodes!";
                return false;
            }
        }*/

        const float maxConstructionRadius = 25f;
        collider2DArray = Physics2D.OverlapCircleAll(position, maxConstructionRadius);        
        
        // ReSharper disable once LocalVariableHidesMember
        foreach (var collider2D in collider2DArray) {
            var buildingTypeHolder = collider2D.GetComponent<BuildingTypeHolder>();
            if (buildingTypeHolder != null) {
                errorMessage = "";
                return true;
            }
        }

        errorMessage = "Too far from any other building!";
        return false;
    }

    public Building GetHQBuilding() {
        return hqBuilding;
    }
}
