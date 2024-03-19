using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPicker : Interactable
{
    public Item item;
    public override void Interact()
    {
        base.Interact();

        PickUp();
    }

    private void PickUp()
    {

        Debug.Log("Picking up " + item.name);
        bool wasPickup = Inventory.instance.Add(item);
        if(wasPickup)
        {
        Destroy(gameObject);
        }
    }
}
