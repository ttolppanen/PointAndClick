using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : Interractable
{
    public ItemEnum itemName;
    public Sprite itemInventoryImage;
    Item item;

    public void Start()
    {
        item = new Item(itemName, itemInventoryImage);
    }

    public override void Activate()
    {
        base.Activate();
        Inventory.instance.AddItem(item);
        Destroy(gameObject);
    }
}