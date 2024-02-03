using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerDestroy : MonoBehaviour
{
    public bool isPlayer = true;
    // Start is called before the first frame update
    private void OnDestroy()
    {
        if(!Data.isGameOver)
        {
            if(isPlayer)
            {
                Data.isGameOver = true;
                Data.isComplete = false;
                Debug.Log("Player Lost");
            }
            else
            {
                Data.isComplete = true;
                Data.isGameOver = true;
                Debug.Log("Player win");
            }
        }
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
