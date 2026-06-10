using UnityEngine;
using System.Collections.Generic;

public class WeaponManager : MonoBehaviour
{
    [Header("Weapons")]
    public List<WeaponBase> weapons = new List<WeaponBase>();
    public Transform weaponHolder;

    [Header("UI")]
    public UnityEngine.UI.Text weaponNameText;

    private int currentWeaponIndex = 0;
    private WeaponBase currentWeapon;

    private void Awake()
    {
        InitializeWeapons();
    }

    private void OnEnable()
    {
        var inputMgr = GetComponent<InputManager>();
        if (inputMgr != null)
        {
            inputMgr.OnCycleWeapon += HandleCycleWeapon;
        }
    }

    private void OnDisable()
    {
        var inputMgr = GetComponent<InputManager>();
        if (inputMgr != null)
        {
            inputMgr.OnCycleWeapon -= HandleCycleWeapon;
        }
    }

    private void InitializeWeapons()
    {
        foreach (WeaponBase weapon in weapons)
        {
            weapon.gameObject.SetActive(false);
        }

        if (weapons.Count > 0)
        {
            EquipWeapon(0);
        }
    }

    private void HandleCycleWeapon(float scroll)
    {
        if (scroll > 0.5f)
            CycleNext();
        else if (scroll < -0.5f)
            CyclePrevious();
    }

    public void HandleAttackInput()
    {
        if (currentWeapon == null) return;
        currentWeapon.Attack();
    }

    public void CycleNext()
    {
        EquipWeapon((currentWeaponIndex + 1) % weapons.Count);
    }

    public void CyclePrevious()
    {
        int newIndex = currentWeaponIndex - 1;
        if (newIndex < 0) newIndex = weapons.Count - 1;
        EquipWeapon(newIndex);
    }

    public void EquipWeapon(int index)
    {
        if (index < 0 || index >= weapons.Count) return;

        if (currentWeapon != null)
        {
            currentWeapon.OnUnequipped();
            currentWeapon.gameObject.SetActive(false);
        }

        currentWeaponIndex = index;
        currentWeapon = weapons[index];

        if (weaponHolder != null)
        {
            currentWeapon.transform.SetParent(weaponHolder, false);
        }

        currentWeapon.gameObject.SetActive(true);
        currentWeapon.OnEquipped();

        if (weaponNameText != null)
        {
            weaponNameText.text = currentWeapon.data.weaponName;
        }
    }

    public WeaponBase GetCurrentWeapon() => currentWeapon;
}