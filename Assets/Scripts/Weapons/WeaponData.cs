using UnityEngine;

public enum WeaponType
{
    Melee,
    Ranged
}

[CreateAssetMenu(fileName = "WeaponData", menuName = "Weapons/WeaponData")]
public class WeaponData : ScriptableObject
{
    public string weaponName = "Weapon";
    public WeaponType type = WeaponType.Ranged;
    public float damage = 10f;
    public float range = 100f;
    public float attackRate = 1f;
    public bool isAutomatic = false;
    public LayerMask hitLayers = 1;
}
