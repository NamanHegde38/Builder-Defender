using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingTypeSelectUI : MonoBehaviour {

    [SerializeField] private Sprite arrowSprite;
    [SerializeField] private List<BuildingTypeSO> ignoreBuildingTypeList;
    
    private Dictionary<BuildingTypeSO, Transform> btnTransformDictionary;
    private Transform _arrowBtn;
    
    private void Awake() {
        var btnTemplate = transform.Find("btnTemplate");
        btnTemplate.gameObject.SetActive(false);
        
        var buildingTypeList = Resources.Load<BuildingTypeListSO>(nameof(BuildingTypeListSO));

        btnTransformDictionary = new Dictionary<BuildingTypeSO, Transform>();
        
        var index = 0;
        
        _arrowBtn = Instantiate(btnTemplate, transform);
        _arrowBtn.gameObject.SetActive(true);

        const float offsetAmount = +125f;
        _arrowBtn.GetComponent<RectTransform>().anchoredPosition = new Vector2(offsetAmount * index, 0);
            
        _arrowBtn.Find("image").GetComponent<Image>().sprite = arrowSprite;
        _arrowBtn.Find("image").GetComponent<RectTransform>().sizeDelta = new Vector2(0, -30);

        _arrowBtn.GetComponent<Button>().onClick.AddListener((() => {
            BuildingManager.Instance.SetActiveBuildingType(null);
        }));
        
        var mouseEnterExitEvents = _arrowBtn.GetComponent<MouseEnterExitEvents>();
        mouseEnterExitEvents.OnMouseEnter += (object sender, EventArgs e) => {
            TooltipUI.Instance.Show("Arrow");
        };
        mouseEnterExitEvents.OnMouseExit += (object sender, EventArgs e) => {
            TooltipUI.Instance.Hide();
        };

        index++;
        
        foreach (var buildingType in buildingTypeList.list) {
            if (ignoreBuildingTypeList.Contains(buildingType)) continue;
            
            var btnTransform = Instantiate(btnTemplate, transform);
            btnTransform.gameObject.SetActive(true);
            
            btnTransform.GetComponent<RectTransform>().anchoredPosition = new Vector2(offsetAmount * index, 0);
            
            btnTransform.Find("image").GetComponent<Image>().sprite = buildingType.sprite;

            btnTransform.GetComponent<Button>().onClick.AddListener((() => {
                BuildingManager.Instance.SetActiveBuildingType(buildingType);
            }));

            mouseEnterExitEvents = btnTransform.GetComponent<MouseEnterExitEvents>();
            mouseEnterExitEvents.OnMouseEnter += (object sender, EventArgs e) => {
                TooltipUI.Instance.Show(buildingType.name + "\n" + buildingType.GetConstructionResourceCostString());
            };
            mouseEnterExitEvents.OnMouseExit += (object sender, EventArgs e) => {
                TooltipUI.Instance.Hide();
            };
            
            btnTransformDictionary[buildingType] = btnTransform;
            
            index++;
        }
    }

    private void Start() {
        BuildingManager.Instance.OnActiveBuildingTypeChanged += BuildingManager_OnActiveBuildingTypeChanged;
        UpdateActiveBuildingTypeButton();
    }

    private void BuildingManager_OnActiveBuildingTypeChanged(object sender, BuildingManager.OnActiveBuildingTypeChangedEventArgs e) {
        UpdateActiveBuildingTypeButton();
    }

    private void UpdateActiveBuildingTypeButton() {
        _arrowBtn.Find("selected").gameObject.SetActive(false);
        foreach (var buildingType in btnTransformDictionary.Keys) {
            var btnTransform = btnTransformDictionary[buildingType];
            btnTransform.Find("selected").gameObject.SetActive(false);
        }

        var activeBuildingType = BuildingManager.Instance.GetActiveBuildingType();
        
        if (activeBuildingType == null) {
            _arrowBtn.Find("selected").gameObject.SetActive(true);
        }
        else {
            btnTransformDictionary[activeBuildingType].Find("selected").gameObject.SetActive(true);
        }
    }
}
