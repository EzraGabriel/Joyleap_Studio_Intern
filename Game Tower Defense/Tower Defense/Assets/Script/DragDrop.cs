using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragDrop : MonoBehaviour
{
    [HideInInspector]
    public Vector3 screenPoint;
    [HideInInspector]
    public Vector3 offset;
    [HideInInspector]
    public int cost;
    [HideInInspector]
    public bool follow = true;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(follow)
        {
            Vector3 curscreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
            Vector3 curPosition = Camera.main.ScreenToWorldPoint(curscreenPoint) + offset;
            transform.position = curPosition;
            if(Input.GetMouseButtonUp(0))
            {
                if(transform.position.x < 0 && transform.position.y > -2f)
                {
                    follow = false;
                    Data.coin -= cost;
                    foreach (Behaviour childCompnet in GetComponentsInChildren<Behaviour>())
                        childCompnet.enabled = true;
                }
                else
                {
                    Destroy(gameObject);
                    Debug.Log("Meletakkan area yang tidak diijinkan");
                }
            }
            else if(Input.touchCount > 0)
            {
                curscreenPoint = Input.GetTouch(0).position;
            }
        }
    }
}
