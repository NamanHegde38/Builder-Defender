using UnityEngine;
using UnityEngine.UI;

public class BuildingRepairBtn : MonoBehaviour {

    [SerializeField] private HealthSystem healthSystem;
    [SerializeField] private ResourceTypeSO goldResourceType;
    
    private void Awake() {
        transform.Find("button").GetComponent<Button>().onClick.AddListener(() => {
            var missingHealth = healthSystem.GetHealthAmountMax() - healthSystem.GetHealthAmount();
            var repairCost = missingHealth / 2;
            
            var resourceAmountCost = new ResourceAmount[] {new ResourceAmount {resourceType = goldResourceType, amount = repairCost} };
            if (ResourceManager.Instance.CanAfford(resourceAmountCost)) {
                ResourceManager.Instance.SpendResources(resourceAmountCost);
                healthSystem.HealFull();
            }
            else {
                TooltipUI.Instance.Show("Cannot afford repair cost!", new TooltipUI.TooltipTimer{ Timer = 2f });
            }
            
        });
    }
}
