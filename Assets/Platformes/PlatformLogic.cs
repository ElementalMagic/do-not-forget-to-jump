using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformLogic : MonoBehaviour
{
    public float pos_y;
    public float JumpForce = 1f;
    public bool keepPlankAlive = false;
    public bool moving_platform = false;
    public float move_speed = 0.1f;
    public float[] move_between;
    private GameObject cam;
    private PlanksCreater spawner;
    private float halfScreenHeight;
    private bool move_left = false;
    public bool destroyable = false;
    public float[] size;
    private GameObject attention_sign;
    // Start is called before the first frame update
    void Start()
    {
        cam = GameObject.FindGameObjectWithTag("MainCamera");
        spawner = GameObject.FindGameObjectWithTag("PlatformSpawner").GetComponent<PlanksCreater>();
        halfScreenHeight = spawner.screen_height / 2;

        if (destroyable)
        {
            foreach(Transform children in transform)
            {
                if(children.gameObject.name == "AttentionSign")
                {
                    attention_sign = children.gameObject;
                    attention_sign.SetActive(true);

                    break;
                }
            }
        }
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            bool jump_succeed = collision.gameObject.GetComponent<BallBehaviour>().JumpForce(JumpForce);

            if (destroyable && jump_succeed)
            {
                StartCoroutine(DestoyWithDelay(0f));
            }
        }
    }

    private IEnumerator DestoyWithDelay(float time)
    {
        moving_platform = false;
        if(attention_sign != null)
        {
            attention_sign.SetActive(false);
        }
        GetComponent<Animator>().SetTrigger("Destroy");

        yield return new WaitForSeconds(time);

        for(int i = 0; i < 120; i++)
        {
            transform.position = new Vector2(transform.position.x, transform.position.y - 0.04f);
            yield return new WaitForSeconds(1 / 60);
        }


        DestroyPlatform();
    }
    public void DestroyPlatform()
    {
        if (!keepPlankAlive)
        {
            spawner.DeletePlank(gameObject);
            Destroy(gameObject);
        }
    }


    private void FixedUpdate()
    {
        if (moving_platform)
        {
            if (!move_left)
            {
                transform.position = new Vector2(transform.position.x + move_speed, pos_y);
            }
            else
            {
                transform.position = new Vector2(transform.position.x - move_speed, pos_y);
            }

            if (transform.position.x + 0.05f >= move_between[1])
            {
                move_left = true;
            }

            if (transform.position.x - 0.05f <= move_between[0])
            {
                move_left = false;
            }
        }
    }
}
