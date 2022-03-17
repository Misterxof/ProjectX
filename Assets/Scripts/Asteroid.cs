using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    public float radius = 1f;
    public float velocity = 1f;
    public float density = 2600f;

    [ReadOnly]
    public SpaceObjectType spaceObjectType = SpaceObjectType.Asteroid;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
