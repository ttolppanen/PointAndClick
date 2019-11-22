using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemDragHandler : MonoBehaviour, IDragHandler, IEndDragHandler
{

    Vector3 startingPoint;

    private void Awake()
    {
        startingPoint = transform.localPosition;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        transform.localPosition = startingPoint;
        int itemIndex =  (int)char.GetNumericValue(gameObject.name[0]); //Objektin nimestä indeksi...
        ItemEnum itemName = Inventory.instance.inventory[itemIndex].name;
        PlayerController.instance.OnMouseLeftClick(itemName);
    }
}
