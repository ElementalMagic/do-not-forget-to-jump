using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class GameController : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject player;
    public GameObject cam;
    public PlanksCreater spawner;
    public Text score;
    public Text best_score_text;

    public GameObject playerStartPos;
    private Vector3 initialCameraPosition, initialPlayerPosition;
    private int best_score = 0;
    
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        cam = GameObject.FindGameObjectWithTag("MainCamera");

        initialPlayerPosition = playerStartPos.transform.position;
        initialCameraPosition = cam.transform.position;

        player.transform.position = initialPlayerPosition;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        int current_y = (int)player.transform.position.y;
        if (current_y > best_score)
        {
            score.text = current_y.ToString();
            best_score = current_y;
        }
    }

    public void KillPlayer()
    {
        player.transform.position = initialPlayerPosition;
        cam.transform.position = initialCameraPosition;
        spawner.ResetParams();
        if (Int32.Parse(best_score_text.text) < best_score)
        {
            best_score_text.text = best_score.ToString();
        }
        best_score = 0;
        score.text = 0.ToString();
    }
}
