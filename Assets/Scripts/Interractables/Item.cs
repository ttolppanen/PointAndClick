using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item
{
    public ItemEnum name;
    public Sprite inventoryImage;

    public Item(ItemEnum name, Sprite inventoryImage)
    {
        this.name = name;
        this.inventoryImage = inventoryImage;
    }
}
public enum ItemEnum {noItem, rope, dagger}
