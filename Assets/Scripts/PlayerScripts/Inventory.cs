using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public static Inventory instance;

    public RawImage[] inventoryImages;
    public List<Item> inventory = new List<Item>();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            print("Öööö jätän Inventory scriptiin tän viestin. Testaan pääsekö tämä koodi koskaan tänne(?)");
        }
        UpdateInventoryUI();
    }

    public void AddItem(Item item)
    {
        inventory.Add(item);
        UpdateInventoryUI();
    }

    public void RemoveItem(Item item)
    {
        inventory.Remove(item);
        UpdateInventoryUI();
    }

    void UpdateInventoryUI()
    {
        for (int i = 0; i < inventoryImages.Length; i++)
        {
            inventoryImages[i].enabled = false;
            if (i < inventory.Count)
            {
                inventoryImages[i].enabled = true;
                inventoryImages[i].texture = inventory[i].inventoryImage.texture;
            }
        }
    }
}
