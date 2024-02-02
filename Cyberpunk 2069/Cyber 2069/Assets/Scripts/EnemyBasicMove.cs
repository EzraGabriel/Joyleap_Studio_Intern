using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBasicMove : MonoBehaviour
{

    Animator myAnim;
    Transform player;

    public bool move, rotate;
    // Start is called before the first frame update
    void Start()
    {
        myAnim = GetComponent<Animator>();
        player = FindObjectOfType<Player>().transform;

        myAnim.SetBool("Move", move);
        myAnim.SetBool("Rotating", rotate);

    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(player);
    }
}
