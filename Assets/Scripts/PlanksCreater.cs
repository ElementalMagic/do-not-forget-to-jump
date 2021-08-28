using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class PlanksCreater : MonoBehaviour
{
    [Header("Начальные параметры генератора")]
    public float createEveryYchange = 2;
    public float min_x_diff = 0.5f, min_y_diff = 1;
    public int initial_plank_y = 0;
    public int initial_planks_count = 10;

    [Header("Варианты платформ")]
    public List<Platform> platforms = new List<Platform>();

    [Header("Платформы в сцене")]
    [SerializeField]
    private SortedList<float, GameObject> Planks = new SortedList<float, GameObject>();

    private Queue<Platform> platforms_queue = new Queue<Platform>();

    [HideInInspector]
    public float screen_height;

    private Vector3 screenBounds, camPos;
    private float[] width, height;
    private float last_plank_y = 0, last_plank_x = 0;
    private float max_jump_height = 2;
    private float base_jump_height = 0;
    private float lastCameraY;
    private Camera _camera;

    private float gravity;



    // Start is called before the first frame update
    void Start()
    {
        _camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        gravity = -GameObject.FindGameObjectWithTag("Player").GetComponent<BallBehaviour>().gravity;
        camPos = _camera.transform.position;
        lastCameraY = camPos.y;
        screenBounds = _camera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, _camera.transform.position.z));
        width = new float[] { camPos.x - screenBounds.x, screenBounds.x + camPos.x };
        height = new float[] { camPos.y - screenBounds.y, screenBounds.y + camPos.y };
        screen_height = Mathf.Abs(height[1] - height[0]);
        last_plank_y = initial_plank_y;
        base_jump_height = platforms[0].max_jump_height;

        InitiateSpawn();
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void FixedUpdate()
    {
        camPos = _camera.transform.position;
        /* height = new float[] { camPos.y - screenBounds.y, screenBounds.y + camPos.y };*/

        if (camPos.y - lastCameraY > createEveryYchange)
        {
            lastCameraY = camPos.y;
        }
    }

    private void Spawn(Platform platform, bool define_type = false)
    {
        if (define_type)
        {

        }

        if (Planks.Count >= 2)
        {
            GameObject _pl = Planks.Values[Planks.Count - 2];
            PlatformLogic _last_plank = Planks.Values[Planks.Count - 1].GetComponent<PlatformLogic>();
            float _last_plank_y = _last_plank.pos_y;
            PlatformLogic double_previous_platform = _pl.GetComponent<PlatformLogic>();
            float double_previous_platform_y = double_previous_platform.pos_y;
            if (Mathf.Abs(_last_plank_y - double_previous_platform_y) > base_jump_height - _last_plank.size[1])
            {
                last_plank_y = double_previous_platform_y;
                last_plank_x = _pl.gameObject.transform.position.x;
                max_jump_height = base_jump_height;
            }
            else
            {
                last_plank_y = _last_plank_y;
                last_plank_x = _last_plank.gameObject.transform.position.x;
            }
        }

        GameObject prefab = platform.platform_prefab;
        float[] size = platform.size;

        float centerX, centerY;

        float[] x_left = new float[2];
        float[] x_right = new float[2];
        bool left = false, right = false;

        x_left[0] = width[0] + size[0];
        x_left[1] = last_plank_x - size[0] / 1.8f - min_x_diff;

        x_right[0] = last_plank_x + size[0] / 1.8f + min_x_diff;
        x_right[1] = width[1] - size[0];

        if (x_left[0] < x_left[1]) left = true;
        if (x_right[0] < x_right[1]) right = true;


        if (left && !right)
        {
            centerX = Random.Range(x_left[0], x_left[1]);
        }
        else if (!left && right)
        {
            centerX = Random.Range(x_right[0], x_right[1]);
        }
        else
        {
            if (Random.Range(0f, 1f) > 0.5f)
            {
                centerX = Random.Range(x_left[0], x_left[1]);
            }
            else
            {
                centerX = Random.Range(x_right[0], x_right[1]);
            }
        }


        if (/*max_jump_height > screen_height*/ false)
        {
            centerY = Random.Range(last_plank_y + max_jump_height + size[1] - screen_height / 2, max_jump_height + last_plank_y - size[1]);
        }
        else
        {
            centerY = Random.Range(last_plank_y + min_y_diff + size[1] / 2, max_jump_height + last_plank_y - size[1]);
        }

        Vector3 prefabPos = new Vector3(centerX, centerY, 0);
        GameObject plank = Instantiate(prefab, prefabPos, Quaternion.identity);
        PlatformLogic _logic = plank.AddComponent<PlatformLogic>();
        _logic.JumpForce = platform.force_power;
        _logic.pos_y = centerY;
        _logic.size = platform.size;
        _logic.moving_platform = platform.moving_platform;
        _logic.move_between = new float[] { x_left[0], x_right[1] };
        _logic.move_speed = platform.move_speed;
        _logic.destroyable = platform.destroyable;


        Planks.Add(centerY, plank);

        last_plank_y = centerY;
        last_plank_x = centerX;

        max_jump_height = platform.max_jump_height;
    }

    private void InitiateSpawn()
    {
        int i = 0;
        while (i < initial_planks_count)
        {
            foreach (Platform _platform in platforms)
            {
                float chance = Random.Range(0f, 1f);

                if (chance <= _platform.chance_to_appear)
                {
                    Spawn(_platform);
                    i++;
                }
            }
        }
        platforms_queue.Enqueue(platforms[0]);
    }

    public void ResetParams()
    {
        lastCameraY = 0;
        last_plank_x = 0;
        last_plank_y = initial_plank_y;
        max_jump_height = platforms[0].max_jump_height;
        for (int i = 0; i < Planks.Count; i++)
        {
            Destroy(Planks.Values[i]);
        }
        Planks = new SortedList<float, GameObject>();
        platforms_queue = new Queue<Platform>();
        InitiateSpawn();

    }

    [MenuItem("Game Systems/Spawner/Calculate planks size")]
    static void CalculateSize()
    {
        float gravity = -GameObject.FindGameObjectWithTag("Player").GetComponent<BallBehaviour>().gravity;
        foreach (GameObject spawner in GameObject.FindGameObjectsWithTag("PlatformSpawner"))
        {
            foreach (Platform _platform in spawner.GetComponent<PlanksCreater>().platforms)
            {
                float _width = 0, _height = 0;

                /*  for (int i = 0; i < 1000; i++)
                  {
                      PlatformLogic scr = _platform.platform_prefab.GetComponent<PlatformLogic>();
                      DestroyImmediate(scr, true);
                  }*/


                /*foreach (Transform child in _platform.platform_prefab.transform)
                {
                    _width += child.GetComponent<SpriteRenderer>().bounds.size.x;
                    _height = child.GetComponent<SpriteRenderer>().bounds.size.y;
                }
                _platform.size = new float[] { _width, _height };*/

                float best_t = Mathf.Sqrt(2 * _platform.max_jump_height / (gravity * 50));
                _platform.force_power = best_t * gravity * 50;
            }
        }
    }

    public void DeletePlank(GameObject plank)
    {
        Planks.Remove(plank.GetComponent<PlatformLogic>().pos_y);

        Spawn(platforms_queue.Dequeue());

        float chance = Random.Range(0f, 1f);

        float cumulative_probability = 0;
        for (int i = 0; i < platforms.Count; i++)
        {
            if (chance <= platforms[i].chance_to_appear + cumulative_probability)
            {
                platforms_queue.Enqueue(platforms[i]);
                break;
            }
            cumulative_probability += platforms[i].chance_to_appear;
        }
    }
}
