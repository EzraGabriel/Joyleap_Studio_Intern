using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStat : CharacterStats
{
    // Start is called before the first frame update
    void Start()
    {
        EquipmentManager.instance.onEquipmentChanged += OnEquipmentChanged;
    }

    void OnEquipmentChanged(Equipment newItem, Equipment oldItem)
    {
        if (newItem != null)
        {
        armor.AddModifier(newItem.armorModifier);
        damage.AddModifier(newItem.damageModifier);
        }
        if(oldItem != null)
        {
            armor.RemoveModifiers(oldItem.armorModifier);
            damage.RemoveModifiers(oldItem.damageModifier);
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            TakeDamage(10);
        }
    }

    public override void Die()
    {
        base.Die();

        PlayerManager.instance.KillPlayer();
    }
}
