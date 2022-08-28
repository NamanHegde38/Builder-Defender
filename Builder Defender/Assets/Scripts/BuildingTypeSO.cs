using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/BuildingType")]
public class BuildingTypeSO : ScriptableObject {

    public new string name;
    public Transform prefab;
    public ResourceGeneratorData ResourceGeneratorData;
}
