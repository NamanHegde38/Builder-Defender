using UnityEngine;
using UnityEngine.UI;

public class BuildingDemolishBtn : MonoBehaviour {

    [SerializeField] private Building building;
    
    private void Awake() {
        transform.Find("button").GetComponent<Button>().onClick.AddListener(() => {
            var buildingType = building.GetComponent<BuildingTypeHolder>().buildingType;

            foreach (var resourceAmount in buildingType.constructionResourceCostArray) {
                ResourceManager.Instance.AddResource(resourceAmount.resourceType, Mathf.FloorToInt(resourceAmount.amount * .5f));
            }
            
            Destroy(building.gameObject);
        });
    }
}
