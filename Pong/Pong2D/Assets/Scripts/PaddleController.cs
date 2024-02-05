using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaddleController : MonoBehaviour
{
    public float batasbawah;
    public float batasatas;
    public float kecepatan;
    public string axis;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float gerak = Input.GetAxis(axis) * kecepatan * Time.deltaTime;
        float nextPos = transform.position.y + gerak;
        if(nextPos > batasatas)
        {
            gerak = 0;
        }
        if(nextPos < batasbawah)
        {
            gerak = 0;
        }


        transform.Translate(0, gerak, 0);
    }
}
