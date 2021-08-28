using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformKiller : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlatformLogic _pl = collision.gameObject.GetComponent<PlatformLogic>();
        if (_pl)
        {
            _pl.DestroyPlatform();
        }
    }
}
