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

    private InputManager inputManager;

    private void Awake()
    {
        inputManager = GetComponent<InputManager>();
        InitializeWeapons();
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

    private void Update()
    {
        HandleWeaponCycling();
        HandleAttack();
    }

    private void HandleWeaponCycling()
    {
        float cycleInput = inputManager.CycleWeaponInput;

        if (cycleInput > 0.5f)
        {
            CycleNext();
        }
        else if (cycleInput < -0.5f)
        {
            CyclePrevious();
        }
    }

    private void HandleAttack()
    {
        if (currentWeapon == null) return;

        if (inputManager.AttackPressed)
        {
            currentWeapon.Attack();
        }

        if (inputManager.MeleePressed)
        {
            if (currentWeapon.data.type == WeaponType.Melee)
            {
                currentWeapon.Attack();
            }
        }
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