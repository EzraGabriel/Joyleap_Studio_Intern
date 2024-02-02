using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Information : MonoBehaviour
{
    public GameObject panelInfo;
    public Text infoText;
    public Image target;
    private string namaGameobject;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void clickInfo(Sprite sprite)
    {
        namaGameobject = gameObject.name;
        panelInfo.SetActive(true);
        target.sprite = sprite;
        infoText.text = namaGameobject;
        Debug.Log(target.sprite);
        Debug.Log(infoText.text);
    }

    public void CloseInfo()
    {
        panelInfo.SetActive(false);
        //target.sprite = null;
        //infoText.text = null;
    }    
}
