using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attacker : MonoBehaviour
{
    public bool isPlayer = true;
    private bool isMove = true;
    public int attack = 100;
    public int defense = 200;
    [HideInInspector]
    public int underattack;
    private float timer = 0;
    private float posYlawan;
    private bool isCari = false;
    private string NameTagLawan;
    // Start is called before the first frame update
    void Start()
    {
        if(isPlayer)
        {
            NameTagLawan = "Enemy";
        }
        else
        {
            NameTagLawan = "Player";
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(isPlayer)
        {
            if(isMove)
            {
                transform.position += transform.right * Time.deltaTime * 0.5f;
                if(transform.position.y > (posYlawan + 0.1f) && isCari)
                {
                    transform.position = new Vector2(transform.position.x, (transform.position.y - Time.deltaTime));
                }
                if(transform.position.y < (posYlawan - 0.1f)&& isCari)
                {
                    transform.position = new Vector2(transform.position.x, (transform.position.y + Time.deltaTime));
                }
            }
            else
            {
                timer += Time.deltaTime;
                if(timer > 0.6f)
                {
                    defense -= underattack;
                    transform.localScale = new Vector3(1, 1f);
                    timer = 0;
                }
                else if(timer > 0.5f)
                {
                    transform.localScale = new Vector3(1, 1.2f);
                }
            }
        }
        else
        {
            if(isMove)
            {
                transform.position -= transform.right * Time.deltaTime * 0.5f;
                if (transform.position.y > (posYlawan + 0.1f) && isCari)
                {
                    transform.position = new Vector2(transform.position.x, (transform.position.y - Time.deltaTime));
                }
                if (transform.position.y < (posYlawan - 0.1f) && isCari)
                {
                    transform.position = new Vector2(transform.position.x, (transform.position.y + Time.deltaTime));
                }
            }
            else
            {
                timer += Time.deltaTime;
                if(timer > 0.6f)
                {
                    defense -= underattack;
                    transform.localScale = new Vector3(1, 1f);
                    timer = 0;
                }
                else if(timer > 0.5f)
                {
                    transform.localScale = new Vector3(1, 1.2f);
                }    
            }
        }
        if(defense <=0)
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
        if(collision.transform.tag.Equals(NameTagLawan) && isMove)
        {
            isMove = false;
            Attacker m = collision.gameObject.GetComponent<Attacker>();
            if (m != null) m.underattack = attack;
            Defender d = collision.gameObject.GetComponent<Defender>();
            if (d != null) d.underattack = attack;
        }
    }
    public void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.transform.tag.Equals(NameTagLawan))
        {
            isCari = true;
            posYlawan = collision.transform.position.y;
        }
    }
    public void OnCollisionExit2D(Collision2D collision)
    {
        isMove = true;
        transform.localScale = new Vector3(1, 1f);
    }
}
