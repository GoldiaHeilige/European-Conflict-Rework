public enum TeamType { Alliance, Enemy, Neutral }
public enum UnitType { Soldier, Jeep, Panzer, Helicopter, Air }

[System.Serializable]
public struct MinimapIconInfo
{
    public TeamType team;
    public UnitType type;
}
