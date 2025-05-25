using UnityEngine;

[System.Serializable]
public class MinimapIconData
{
    public GameObject prefab;

    public TeamType team => prefab.GetComponent<MinimapIcon>()?.iconInfo.team ?? TeamType.Neutral;
    public UnitType type => prefab.GetComponent<MinimapIcon>()?.iconInfo.type ?? UnitType.Soldier;
}
