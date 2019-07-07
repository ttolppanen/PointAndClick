using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory instance;

    public List<GameObject> inventory = new List<GameObject>();

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
    }

    public void AddItem(GameObject item)
    {
        inventory.Add(item);
        item.transform.position = new Vector3(-100, -100, -100);
    }
}
