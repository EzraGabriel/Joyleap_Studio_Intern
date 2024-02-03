using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public bool collisionEnabled = true;


    private void Update()
    {
        RespondToDebug();
    }

    private void RespondToDebug()
    {
        if(Input.GetKeyDown(KeyCode.L))
        {
            NextLevel();
        }
        else if (Input.GetKeyDown(KeyCode.C))
        {
            collisionEnabled = !collisionEnabled;
        }

    }

    public void ResetGame()
    {
        StartCoroutine(LoadSameLevel());
    }

    public void NextLevel()
    {
        StartCoroutine(LoadNextLevel());
    }

    private IEnumerator LoadSameLevel()
    {
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private IEnumerator LoadNextLevel()
    {
        yield return new WaitForSeconds(2f);

        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;

        if(nextSceneIndex == SceneManager.sceneCountInBuildSettings)
        {
            nextSceneIndex = 0;
        }
        SceneManager.LoadScene(nextSceneIndex);
    }

}
