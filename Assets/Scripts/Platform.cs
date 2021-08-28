using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="New Platform", menuName ="Platform")]

public class Platform : ScriptableObject
{
    public GameObject platform_prefab;
    public float chance_to_appear;
    public float force_power;
    public float minimum_y_to_appear;
    public float[] size;
    public float max_jump_height;
    public bool moving_platform;
    public float move_speed = 0.1f;
    public bool destroyable = false;

    public SpawnProbability spawn_probabilities;
}

[System.Serializable]
public class SpawnProbability
{
    public List<SpawnProbabilityUnit> spawn_probabilities;

    public float GetProbability(float height)
    {
        for(int i = 0; i < spawn_probabilities.Count; i++)
        {
            SpawnProbabilityUnit _sp = spawn_probabilities[i];
            if (height >= _sp.y_min && height <= _sp.y_max)
            {
                return _sp.probability;
            }
        }

        return 0;
    }
}

[System.Serializable]
public class SpawnProbabilityUnit
{
    public float y_min, y_max;
    public float probability;

    public SpawnProbabilityUnit(float min, float max, float _probability)
    {
        if(min < max && probability >= 0 && _probability <= 1)
        {
            y_min = min;
            y_max = max;
            probability = _probability;
        } else
        {
            throw new System.Exception();
        }
    }
}
