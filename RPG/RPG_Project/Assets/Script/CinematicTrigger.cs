using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

namespace RPG.Cinematics
{
    public class CinematicTrigger : MonoBehaviour
    {
        private bool once = true;
        private void OnTriggerEnter(Collider other)
        {
            if(other.gameObject.tag == "Player" && once == true)
            {
                GetComponent<PlayableDirector>().Play();
                once = false;
            }
        }
    }
}