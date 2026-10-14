using UnityEngine;

[CreateAssetMenu(fileName = "WeaponStat", menuName = "Data/Weapon")]
public class WeaponData : ScriptableObject
{
    [Range(0, 1)] public float lowSpeedPercent;
    [Range(0, 1)] public float mediumSpeedPercent;
    [Range(0, 1)] public float highSpeedPercent;
    public float lowSpeedDamage;
    public float mediumSpeedDamage;
    public float highSpeedDamage;

}