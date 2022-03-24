using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NBodySimulation : MonoBehaviour
{
    CelestialBody[] bodies;
    static NBodySimulation instance;
    private CelestialBody star;

    void Awake()
    {
        bodies = FindObjectsOfType<CelestialBody>();
        Time.fixedDeltaTime = 0.01f;
        Debug.Log("Setting fixedDeltaTime to: " + CalculationUtilsMath.T);
    }

    private void Start()
    {
        // find central object - star
        for (int i = 0; i < bodies.Length; i++)
        {
            if (bodies[i].spaceObjectType == SpaceObjectType.Star)
            {
                star = bodies[i];
                break;
            }
        }

        // Calculate sphereOfInfluence of the planets
        for (int i = 0; i < bodies.Length; i++)
        {
            if (bodies[i].spaceObjectType == SpaceObjectType.Planet)
            {
                float distance = ((star.Position - bodies[i].Position) * 10000).magnitude;
                float sphereOfInfluence = CalculationUtilsMath.CalculateSphereOfInfluence(distance, bodies[i].mass, star.mass) * ConstantsUtils.INFLUENNCE_SPHERE_MODIFICATOR;
                bodies[i].sphereOfInfluence = sphereOfInfluence;
                GameObject influenseSphere = new GameObject("Influense Sphere");
                influenseSphere.transform.position = bodies[i].Position;
                influenseSphere.AddComponent<CircleCollider2D>();
                influenseSphere.GetComponent<CircleCollider2D>().radius = sphereOfInfluence;
                influenseSphere.GetComponent<CircleCollider2D>().isTrigger = true;
                influenseSphere.AddComponent<InfluenceTrigger>();

                bodies[i].SetInfluenseSphere(influenseSphere);
                Debug.Log(bodies[i].bodyName + " SOI = " + bodies[i].sphereOfInfluence);
            }
        }
    }

    void FixedUpdate()
    {
        for (int i = 0; i < bodies.Length; i++)
        {
            Vector3 acceleration = CalculateAcceleration(bodies[i].Position, bodies[i]);
            bodies[i].UpdateVelocity(acceleration, CalculationUtilsMath.T);
            //bodies[i].UpdateVelocity (bodies, Universe.physicsTimeStep);
        }

        for (int i = 0; i < bodies.Length; i++)
        {
            bodies[i].UpdatePosition(CalculationUtilsMath.T);
           // bodies[i].influenseSphere.transform.position = bodies[i].Position;
        }

    }

    public static Vector3 CalculateAcceleration(Vector3 point, CelestialBody ignoreBody = null)
    {
        Vector3 acceleration = Vector3.zero;
        foreach (var body in Instance.bodies)
        {
            if (body != ignoreBody)
            {
                float sqrDst = ((body.Position - point) * 10000).sqrMagnitude;    // r^2
                Vector3 forceDir = ((body.Position - point) * 10000).normalized;  // Vector3
                acceleration += forceDir * CalculationUtilsMath.G * body.mass / sqrDst;
            }
        }

        return acceleration;
    }

    public static CelestialBody[] Bodies
    {
        get
        {
            return Instance.bodies;
        }
    }

    static NBodySimulation Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<NBodySimulation>();
            }
            return instance;
        }
    }
}