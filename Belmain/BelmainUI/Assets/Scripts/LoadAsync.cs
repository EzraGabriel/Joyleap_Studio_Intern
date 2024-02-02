using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadAsync : MonoBehaviour
{

    public GameObject loadingScreen;
    public Image LoadingBarFill;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(LoadSceneAsync(1));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator LoadSceneAsync(int sceneId)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneId);

        loadingScreen.SetActive(true);

        while(!operation.isDone)
        {
            float progressValue = Mathf.Clamp01(operation.progress / 0.9f);

            LoadingBarFill.fillAmount = progressValue;

            yield return new WaitForSeconds(2f);
        }
    }
}
