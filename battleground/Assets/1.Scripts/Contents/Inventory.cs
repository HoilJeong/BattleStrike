﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 무기 인벤토리 역할, 무기를 소지하고 있는지 확인용
/// </summary>
public class Inventory : MonoBehaviour
{
    private ShootBehaviour shootBehaviour;
    public int activeWeapon = 0;
    

    public List<InteractiveWeapon> weapons; //소지하고 있는 무기들
    
    public Dictionary<InteractiveWeapon.WeaponType, int> weaponSlotMap;
    

    private void Start()
    {       
        shootBehaviour = GetComponent<ShootBehaviour>();      
        weapons = new List<InteractiveWeapon>(new InteractiveWeapon[3]);
        weaponSlotMap = new Dictionary<InteractiveWeapon.WeaponType, int>
        {
            {InteractiveWeapon.WeaponType.SHORT, 1 },
            {InteractiveWeapon.WeaponType.LONG, 2 }
        };      
    }

    /// <summary>
    /// 인벤토리 역할을 하게될 함수
    /// </summary>
    public void AddWeapon(InteractiveWeapon newWeapon)
    {
        newWeapon.gameObject.transform.SetParent(shootBehaviour.rightHand);
        newWeapon.transform.localPosition = newWeapon.rigthHandPosition;
        newWeapon.transform.localRotation = Quaternion.Euler(newWeapon.relativeRotation);

        if (weapons[weaponSlotMap[newWeapon.weaponType]])
        {
            if (weapons[weaponSlotMap[newWeapon.weaponType]].label_weaponName == newWeapon.label_weaponName)
            {
                weapons[weaponSlotMap[newWeapon.weaponType]].ResetBullet();
                ChangeWeapon(activeWeapon, weaponSlotMap[newWeapon.weaponType]);
                Destroy(newWeapon.gameObject);

                return;
            }
            else
            {
                weapons[weaponSlotMap[newWeapon.weaponType]].Drop();
            }
        }

        weapons[weaponSlotMap[newWeapon.weaponType]] = newWeapon;
        ChangeWeapon(activeWeapon, weaponSlotMap[newWeapon.weaponType]);
    }

    public void ChangeWeapon(int oldWeapon, int newWeapon)
    {
        if (oldWeapon > 0)
        {
            weapons[oldWeapon].gameObject.SetActive(false);
            shootBehaviour.gunMuzzle = null;
            weapons[oldWeapon].Toggle(false);
        }

        //빈곳을 찾는다
        while (weapons[newWeapon] == null && newWeapon > 0)
        {
            newWeapon = (newWeapon + 1) % weapons.Count;
        }
        if (newWeapon > 0)
        {
            weapons[newWeapon].gameObject.SetActive(true);
            shootBehaviour.gunMuzzle = weapons[newWeapon].transform.Find("Muzzle");
            weapons[newWeapon].Toggle(true);
        }
        activeWeapon = newWeapon;
        if (oldWeapon != newWeapon)
        {
            shootBehaviour.behaviourController.GetAnimator.SetTrigger(shootBehaviour.changeWeaponTrigger);
            shootBehaviour.behaviourController.GetAnimator.SetInteger(shootBehaviour.weaponType, weapons[newWeapon] ? (int)weapons[newWeapon].weaponType : 0);
        }
        shootBehaviour.SetWeaponCrossHair(newWeapon > 0);
    }
}
