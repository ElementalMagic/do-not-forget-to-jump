using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BottomEdge : MonoBehaviour
{
    private GameController GC;

    private void Start()
    {
        GC = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            GC.KillPlayer();
        }
    }
}
