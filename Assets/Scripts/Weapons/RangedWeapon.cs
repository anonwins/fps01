using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class RangedWeapon : WeaponBase
{
    private LineRenderer lineRenderer;
    private Camera playerCamera;

    [Header("Visuals")]
    public float tracerDuration = 0.05f;

    [Header("Debug")]
    public bool showDebugRay = false;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.enabled = false;
        playerCamera = Camera.main;
    }

    protected override void PerformAttack()
    {
        if (data.type != WeaponType.Ranged) return;

        Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        RaycastHit hit;

        Vector3 endPoint = ray.origin + ray.direction * data.range;

        if (Physics.Raycast(ray, out hit, data.range, data.hitLayers))
        {
            endPoint = hit.point;

            if (hit.collider.TryGetComponent<IDamageable>(out var damageable))
            {
                damageable.TakeDamage(data.damage, hit.point, ray.direction);
            }

            if (showDebugRay)
            {
                Debug.DrawLine(ray.origin, hit.point, Color.red, 2f);
            }
        }

        ShowTracer(ray.origin, endPoint);
    }

    private void ShowTracer(Vector3 start, Vector3 end)
    {
        lineRenderer.SetPosition(0, start);
        lineRenderer.SetPosition(1, end);
        lineRenderer.enabled = true;

        Invoke(nameof(HideTracer), tracerDuration);
    }

    private void HideTracer()
    {
        lineRenderer.enabled = false;
    }

    public override void OnEquipped()
    {
        lineRenderer.enabled = false;
    }
}