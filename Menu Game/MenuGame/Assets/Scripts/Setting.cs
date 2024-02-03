using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Setting : MonoBehaviour
{
    Text persenTeks;

    public Dropdown resolusiDropdown;

    Resolution[] resolutions;
    // Start is called before the first frame update
    void Start()
    {
        persenTeks = GetComponent<Text>();
        resolutions = Screen.resolutions;
        resolusiDropdown.ClearOptions();

        List<string> options = new List<string>();

        int currentResolutionIndex = 0;

        for(int i = 0; i<resolutions.Length;i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);

            if(resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }
        resolusiDropdown.AddOptions(options);
        resolusiDropdown.value = currentResolutionIndex;
        resolusiDropdown.RefreshShownValue();
        Debug.Log(resolutions.Length);
        Debug.Log(resolutions[currentResolutionIndex]);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TampilPenuh(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }

    public void setResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }
    public void Volume(float value)
    {
        persenTeks.text = Mathf.RoundToInt(value * 100) + "%";
    }
}
