using UnityEngine;

public abstract class WeaponBase : MonoBehaviour
{
    [Header("Weapon Data")]
    public WeaponData data;

    [Header("Attack Settings")]
    protected float lastAttackTime = 0f;

    public virtual bool CanAttack()
    {
        return Time.time >= lastAttackTime + (1f / data.attackRate);
    }

    public virtual bool Attack()
    {
        if (!CanAttack()) return false;

        PerformAttack();
        lastAttackTime = Time.time;
        return true;
    }

    protected abstract void PerformAttack();

    public virtual void OnEquipped() { }

    public virtual void OnUnequipped() { }
}