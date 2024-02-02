using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIFunction : MonoBehaviour
{
    public GameObject[] panel;
    public GameObject panelPause;
    public GameObject panelSatuan, panelSemua;
    public GameObject buttonHewanAR;
    public float transitionTime = 1f;
    public Animator anim, anim_Pause;
    private static string sceneName;
    private bool islicked = false;
    private int i = 0;

    private void Start()
    {
       if(panelPause!= null)
        {
            panelPause.SetActive(false);
        }
    }
    public void NextDescription()
    {
        int jumlahPanel = panel.Length -1;
        if (i < jumlahPanel)
        {
            panel[i].SetActive(false);
            panel[i+1].SetActive(true);
            i++;
            Debug.Log(i);
            Debug.Log(jumlahPanel);
        }
        else
        {
            panel[i].SetActive(false);
            panel[0].SetActive(true);
            i = 0;
        }
    }

    public void PreviousDescription()
    {
        int jumlahPanel = panel.Length - 1;
        if (i != 0)
        {
            panel[i].SetActive(false);
            panel[i - 1].SetActive(true);
            i--;
            Debug.Log("i" + i);
            Debug.Log(jumlahPanel);
        }
        else
        {
            panel[i].SetActive(false);
            panel[jumlahPanel].SetActive(true);
            i = jumlahPanel;
        }
    }

    public void NextScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        anim.SetTrigger("Start");
    }

    public void GoToThisScene(string namaScene)
    {
        if (panelPause)
        {
            Time.timeScale = 1f;
            StartCoroutine(LoadLevel(namaScene));
        }
        else if (islicked)
        {
            StartCoroutine(ButtonClicked(namaScene));
            islicked = false;
        }
        else
        {
            StartCoroutine(LoadLevel(namaScene));
        }
    }

    public void GoToJeda()
    {
        panelPause.SetActive(true);
        anim_Pause.SetTrigger("Start");
        Time.timeScale = 0f;
    }

    public void Continue()
    {
        anim_Pause.SetTrigger("End");
        Time.timeScale = 1f;
        StartCoroutine(FinishAnimation());
    }

    public void PanelUiSatuan()
    {
        panelSatuan.SetActive(true);
        panelSemua.SetActive(false);
    }

    public void PanelUiSemua()
    {
        panelSatuan.SetActive(false);
        panelSemua.SetActive(true);
    }

    IEnumerator LoadLevel(string levelName)
    {
        if(this.gameObject.GetComponent<Animator>())
        {
            gameObject.GetComponent<Animator>().SetTrigger("Clicked");
        }
        anim.SetTrigger("Start");
        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadScene(levelName);
    }

    IEnumerator FinishAnimation()
    {
        yield return new WaitForSeconds(transitionTime);
        panelPause.SetActive(false);
    }

    IEnumerator ButtonClicked(string jeneng)
    {
        buttonHewanAR.GetComponent<Animator>().SetTrigger("Clicked");
        yield return new WaitForSeconds(transitionTime);
        StartCoroutine(LoadLevel(jeneng));
    }
    public void IsClicked()
    {
        islicked = true;
    }
       

}
