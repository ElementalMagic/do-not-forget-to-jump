using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ballControl : MonoBehaviour
{
    public float speed = 1f;
    [Range(0, 1)]
    public float drag = 1f;
    private Rigidbody2D rb;
    SpriteRenderer sr; 
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            rb.velocity = new Vector2(rb.velocity.x, 30);
        }
        if (Input.GetKey("a"))
        {
            rb.velocity = new Vector2(-speed, rb.velocity.y);
            if (!sr.flipX)
            {
                sr.flipX = true;
            }
        } else if (Input.GetKey("d"))
        {
            rb.velocity = new Vector2(speed, rb.velocity.y);
            if (sr.flipX)
            {
                sr.flipX = false;
            }
        } else
        {
            float x_speed = rb.velocity.x;

            if(x_speed != 0)
            {
                float _drag = drag * Mathf.Sign(x_speed);
                if(Mathf.Abs(x_speed) - drag >= 0)
                {
                    rb.velocity -= new Vector2(_drag, 0);
                } else
                {
                    rb.velocity = new Vector2(0, rb.velocity.y);
                }
            }
            
        }
    }
}
