using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallBehaviour : MonoBehaviour
{
    // Start is called before the first frame update
    public float gravity;
    public float y_speed;
    public Rigidbody2D rb;
    public Animator animator;

    public AudioClip JumpSound;

    private AudioSource audioSource;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Gravity(gravity);
    }

    private void Gravity(float gravity)
    {
        rb.velocity -= (new Vector2(0, -gravity));
    }

    private void Update()
    {
        animator.SetFloat("velocity_y", rb.velocity.y);
    }

    public bool JumpForce(float force)
    {
        if(rb.velocity.y <= 0)
        {
            rb.velocity = (new Vector2(rb.velocity.x, force));
            animator.SetTrigger("Jump");
            audioSource.PlayOneShot(JumpSound);
            return true;
        }
        return false;
    }
}
