using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum InterfaceType
{
    ItemInventory,
    Equipment,
    QuickSlot,
    Box,
}

[CreateAssetMenu(fileName = "New Inventory", menuName = "InVentory System/Item Inventory")]
public class ItemInventoryObject : ScriptableObject
{
    public ItemObjectDataBase dataBase;
    public InterfaceType type;

    [SerializeField]
    private ItemInventory container = new ItemInventory();

    public ItemInventorySlot[] slots => container.slots;

    public int EmptySlotCount
    {
        get
        {
            int counter = 0;
            foreach (ItemInventorySlot slot in slots)
            {
                if (slot.item.id > 0)
                {
                    counter++;
                }
            }

            return counter;
        }
    }

    public bool AddItem(Item item, int amount)
    {
        if (EmptySlotCount <= 0)
        {
            return false;
        }

        ItemInventorySlot slot = FindItemInInventory(item);
        if (!dataBase.itemObjects[item.id].stackable || slot == null)
        {
            GetEmptySlot().AddItem(item, amount);
        }
        else
        {
            slot.AddAmount(amount);
        }

        return true;
    }

    public ItemInventorySlot FindItemInInventory(Item item)
    {
        return slots.FirstOrDefault(i => i.item.id == item.id);
    }

    public ItemInventorySlot GetEmptySlot()
    {
        return slots.FirstOrDefault(i => i.item.id < 0);
    }

    public bool IsContainItem(ItemObject itemObject)
    {
        return slots.FirstOrDefault(i => i.item.id == itemObject.data.id) != null;
    }

    public void SwapItems(ItemInventorySlot itemSlotA, ItemInventorySlot itemSlotB)
    {
        if (itemSlotA == itemSlotB)
        {
            return;
        }
        if (itemSlotB.CanPlaceInSlot(itemSlotA.ItemObject) && itemSlotA.CanPlaceInSlot(itemSlotB.ItemObject))
        {
            ItemInventorySlot tempSlot = new ItemInventorySlot(itemSlotB.item, itemSlotB.amount);
            itemSlotB.UpdateSlot(itemSlotA.item, itemSlotA.amount);
            itemSlotA.UpdateSlot(tempSlot.item, tempSlot.amount);
        }
    }
}
