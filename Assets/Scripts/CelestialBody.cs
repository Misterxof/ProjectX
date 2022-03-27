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
    public Vector3 acceleration;

    [ReadOnly]
    [SerializeField]
    public float mass;

    Rigidbody rb;

    [ReadOnly]
    public float sphereOfInfluence = 0f;

    public SpaceObjectType spaceObjectType;

    [ReadOnly]
    public GameObject influenseSphere;

    [ReadOnly]
    public CelestialBody influenceSphereCelestialBody;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
       // rb.mass = mass;
        velocity = initialVelocity;
        
        //if (mass < 1f)
        //{
        //    mass = ((surfaceGravity / 1000) * radius * radius / CalculationUtilsMath.G);
        //}
    }

    public void UpdateVelocity(Vector3 acceleration, float timeStep)
    {
        this.acceleration = acceleration;
        velocity += acceleration * timeStep;
        
        if (bodyName == "Moon")
        {
            Debug.Log(timeStep);
          //  Debug.Log(velocity);
        }
    }

    public void UpdatePosition(float timeStep)
    {
        rb.MovePosition(rb.position + velocity * timeStep);
        if(bodyName == "Moon")
        {
            Debug.Log(velocity * 1000);
            Debug.Log(velocity * timeStep);
        }
    }

    void OnValidate()
    {
        if (bodyName == "Test")
        {
            mass = 500f;
        }
        else
        {
            mass = ((surfaceGravity / 1000) * radius * radius / CalculationUtilsMath.G);
        }
       
       // mass = ((surfaceGravity) * radius * radius / CalculationUtilsMath.G);
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

    public void SetInfluenceSphereCelestialBody()
    {
        CelestialBody[] bodies = FindObjectsOfType<CelestialBody>();
        for (int i = 0; i < bodies.Length; i++)
        {
            if (bodies[i].spaceObjectType == SpaceObjectType.Star)
            {
                influenceSphereCelestialBody = bodies[i];
                break;
            }
        }
    }

    public void SetInfluenceSphereCelestialBody(CelestialBody body)
    {
        influenceSphereCelestialBody = body;
    }

}