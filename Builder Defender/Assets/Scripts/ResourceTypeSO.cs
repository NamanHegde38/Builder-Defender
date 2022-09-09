using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/ResourceType")]
public class ResourceTypeSO : ScriptableObject {
    
    public new string name;
    public string nameShort;
    public Sprite sprite;
    public string colorHex;
}
