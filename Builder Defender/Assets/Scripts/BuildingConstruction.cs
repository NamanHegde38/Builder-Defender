using System;
using UnityEngine;

public class BuildingConstruction : MonoBehaviour {
    
    public static BuildingConstruction Create(Vector3 position, BuildingTypeSO buildingType) {
        var pfBuildingConstruction = Resources.Load<Transform>("pfBuildingConstruction");
        var buildingConstructionTransform = Instantiate(pfBuildingConstruction, position, Quaternion.identity);

        var buildingConstruction = buildingConstructionTransform.GetComponent<BuildingConstruction>();
        buildingConstruction.SetBuildingType(buildingType);
        return buildingConstruction;
    }

    private BuildingTypeSO _buildingType;
    private float _constructionTimer;
    private float _constructionTimerMax;
    private BoxCollider2D _boxCollider2D;
    private SpriteRenderer _spriteRenderer;
    private BuildingTypeHolder _buildingTypeHolder;
    private Material _constructionMaterial;

    private void Awake() {
        _boxCollider2D = GetComponent<BoxCollider2D>();
        _spriteRenderer = transform.Find("sprite").GetComponent<SpriteRenderer>();
        _buildingTypeHolder = GetComponent<BuildingTypeHolder>();
        _constructionMaterial = _spriteRenderer.material;
    }

    private void Update() {
        _constructionTimer -= Time.deltaTime;
        
        _constructionMaterial.SetFloat("_Progress", GetConstructionTimerNormalized());
        if (_constructionTimer <= 0f) {
            Instantiate(_buildingType.prefab, transform.position, Quaternion.identity);
            SoundManager.Instance.PlaySound(SoundManager.Sound.BuildingPlaced);
            Destroy(gameObject);
        }
    }

    private void SetBuildingType(BuildingTypeSO buildingType) {
        _buildingType = buildingType;
        
        _constructionTimerMax = buildingType.constructionTimerMax;
        _constructionTimer = _constructionTimerMax;

        _spriteRenderer.sprite = buildingType.sprite;
        _boxCollider2D.offset = buildingType.prefab.GetComponent<BoxCollider2D>().offset;
        _boxCollider2D.size = buildingType.prefab.GetComponent<BoxCollider2D>().size;

        _buildingTypeHolder.buildingType = buildingType;
    }

    public float GetConstructionTimerNormalized() {
        return 1 - _constructionTimer / _constructionTimerMax;
    }
}
