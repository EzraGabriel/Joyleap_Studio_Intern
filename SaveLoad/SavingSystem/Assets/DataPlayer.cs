using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataPlayer : MonoBehaviour
{

    public int level = 3;
    public int health = 40;

    public void SavePlayer()
    {
        SaveSystem.SavePlayer(this);
    }
    public void LoadPlayer()
    {
        PlayerSaving data = SaveSystem.LoadPlayer();
        level = data.level;
        health = data.health;
        Debug.Log(level);
        Debug.Log(health);
    }
    


    public void IncreaseLevel()
    {
        level++;
    }

    public void DecreaseLevel()
    {
        level--;
    }

    public void IncreaseHealth()
    {
        health++;
    }

    public void DecreaseHealth()
    {
        health--;
    }
}
