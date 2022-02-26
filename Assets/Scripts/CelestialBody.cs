using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(Rigidbody))]
public class CelestialBody : GravityObject
{

    public float radius;
    public float surfaceGravity;
    public Vector3 initialVelocity;
    public string bodyName = "Unnamed";

    public Vector3 velocity { get; private set; }

    [ReadOnly]
    [SerializeField]
    public float mass;

    Rigidbody rb;

    [ReadOnly]
    public float sphereOfInfluence = 0f;

    public SpaceObjectType spaceObjectType;

    [ReadOnly]
    public GameObject influenseSphere;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
       // rb.mass = mass;
        velocity = initialVelocity;
        
        if (mass < 1f)
        {
            mass = ((surfaceGravity / 1000) * radius * radius / CalculationUtils.G);
        }
    }

    public void UpdateVelocity(Vector3 acceleration, float timeStep)
    {
        velocity += acceleration * timeStep;
    }

    public void UpdatePosition(float timeStep)
    {
        rb.MovePosition(rb.position + velocity * timeStep);
        if(bodyName == "Earth")
        {
            Debug.Log(influenseSphere.transform.position);
        }
    }

    void OnValidate()
    {
        mass = ((surfaceGravity / 1000) * radius * radius / CalculationUtils.G);
       // mass = ((surfaceGravity) * radius * radius / CalculationUtils.G);
        //meshHolder = transform.GetChild(0);
       // meshHolder.localScale = Vector3.one * radius;
        gameObject.name = bodyName;
    }

    public Rigidbody Rigidbody
    {
        get
        {
            return rb;
        }
    }

    public Vector3 Position
    {
        get
        {
            return rb.position;
        }
    }

    public void SetInfluenseSphere(GameObject influenseSphere)
    {
        this.influenseSphere = influenseSphere;
        this.influenseSphere.transform.parent = transform;
    }

}