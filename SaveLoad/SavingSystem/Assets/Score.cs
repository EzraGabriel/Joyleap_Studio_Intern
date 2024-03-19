using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    [SerializeField]
    private Text level;
    [SerializeField]
    private Text health;
    [SerializeField]
    private DataPlayer player;
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        level.text = player.level.ToString();
        health.text = player.health.ToString();
    }
}
