using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingCamera : MonoBehaviour
{
    GameObject player;
    public decimal y_offset = 2; 
    private Vector3 cam_pos, pl_pos;
    public float cam_speed_change;
   
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
       
    }

    // Update is called once per frame
    void Update()
    {
        cam_pos = gameObject.transform.position;
        pl_pos = player.transform.position;
        decimal diff = decimal.Round((decimal)(pl_pos.y - cam_pos.y), 3);
        if (diff >= y_offset)
        {
            transform.position = Vector3.Lerp(cam_pos, new Vector3(cam_pos.x, pl_pos.y - (float) y_offset, cam_pos.z), cam_speed_change);
        }
    }
}
