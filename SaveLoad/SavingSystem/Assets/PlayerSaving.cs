using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerSaving
{
    public int level;
    public int health;

    public PlayerSaving(DataPlayer player)
    {
        level = player.level;
        health = player.health;
    }
}
