using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Defender : MonoBehaviour
{
    public bool isPlayer = true;
    private bool isDefend = false;
    public int defence = 300;
    [HideInInspector]
    public int underattack;
    private float timer = 0;
    private string nameTagLawan;
    // Start is called before the first frame update
    void Start()
    {
        if(isPlayer)
        {
            nameTagLawan = "Enemy";
        }
        else
        {
            nameTagLawan = "Player";
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(isDefend)
        {
            timer += Time.deltaTime;
            if(timer > 0.6f)
            {
                defence -= underattack;
                timer = 0;
            }
        }
        if(defence <= 0)
        {
            Destroy(gameObject);
        }
        if(transform.position.x > 9 || transform.position.x < -9)
        {
            Destroy(gameObject);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.transform.tag.Equals(nameTagLawan))
        {
            isDefend = true;
            Attacker m = collision.gameObject.GetComponent<Attacker>();
            if (m != null) m.underattack = 0;
            Defender d = collision.gameObject.GetComponent<Defender>();
            if (d != null) d.underattack = 0;
        }
    }
    public void OnCollisionExit2D(Collision2D collision)
    {
        isDefend = false;
    }
}
