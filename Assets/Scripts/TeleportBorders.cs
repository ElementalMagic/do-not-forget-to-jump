using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportBorders : MonoBehaviour
{
    public GameObject TeleportTo;
    public TeleportBorders AnotherBorder;
    public float x_offset;

    private bool ban_teleport = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject gm = collision.gameObject;
        if(gm.tag == "Player")
        {
            if (!ban_teleport)
            {
                // AnotherBorder.BanTeleport();
                gm.transform.position = new Vector2(TeleportTo.transform.position.x + x_offset, gm.transform.position.y);
            } else
            {
                ban_teleport = false;
            }
        }
    }

    public void BanTeleport()
    {
        ban_teleport = true;
    }
}
