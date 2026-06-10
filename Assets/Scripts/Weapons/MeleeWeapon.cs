using UnityEngine;

public class MeleeWeapon : WeaponBase
{
    [Header("Melee Settings")]
    public Transform swingPoint;
    public float swingAngle = 60f;
    public float swingDuration = 0.2f;

    private Quaternion initialRotation;
    private Quaternion targetRotation;
    private float swingStartTime;
    private bool isSwinging = false;

    private void Awake()
    {
        if (swingPoint == null)
            swingPoint = transform;
        initialRotation = swingPoint.localRotation;
        targetRotation = Quaternion.Euler(swingAngle, 0, 0);
    }

    private void Update()
    {
        if (isSwinging)
        {
            float progress = (Time.time - swingStartTime) / swingDuration;
            if (progress >= 1f)
            {
                isSwinging = false;
                swingPoint.localRotation = initialRotation;
            }
            else
            {
                swingPoint.localRotation = Quaternion.Slerp(initialRotation, targetRotation, progress);
            }
        }
    }

    protected override void PerformAttack()
    {
        if (isSwinging) return;

        isSwinging = true;
        swingStartTime = Time.time;

        Collider[] hits = Physics.OverlapSphere(swingPoint.position, data.range, data.hitLayers);

        foreach (Collider hit in hits)
        {
            if (hit.TryGetComponent<IDamageable>(out var damageable))
            {
                Vector3 hitDir = (hit.transform.position - transform.position).normalized;
                damageable.TakeDamage(data.damage, hitPoint: hit.transform.position, hitDir);
            }
        }
    }

    public override void OnEquipped()
    {
        isSwinging = false;
        swingPoint.localRotation = initialRotation;
    }
}