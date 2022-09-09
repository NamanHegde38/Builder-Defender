using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(menuName = "ScriptableObjects/BuildingType")]
public class BuildingTypeSO : ScriptableObject {

    public new string name;
    public Transform prefab;
    public bool hasResourceGeneratorData;
    public ResourceGeneratorData resourceGeneratorData;
    public Sprite sprite;
    public float minConstructionRadius;
    public ResourceAmount[] constructionResourceCostArray;
    public int healthAmountMax;
    public float constructionTimerMax;

    public string GetConstructionResourceCostString() {
        var str = "";
        foreach (var resourceAmount in constructionResourceCostArray) {
            str += "<color=#" + resourceAmount.resourceType.colorHex + ">" +
                   resourceAmount.resourceType.nameShort + resourceAmount.amount +
                   "</color> ";
        }

        return str;
    }
}
