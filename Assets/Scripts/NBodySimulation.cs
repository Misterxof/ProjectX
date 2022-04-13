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
            bodies[i].SetInfluenceSphereCelestialBody();
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

        SetCurrentInfluenceSphereCelestialBody();
    }

    void FixedUpdate()
    {
        for (int i = 0; i < bodies.Length; i++)
        {
            Vector3 acceleration = CalculateAcceleration(bodies[i], bodies);
            bodies[i].UpdateVelocity(acceleration, CalculationUtilsMath.T);
            //bodies[i].UpdateVelocity (bodies, Universe.physicsTimeStep);
        }

        for (int i = 0; i < bodies.Length; i++)
        {
            bodies[i].UpdatePosition(CalculationUtilsMath.T);
           // bodies[i].influenseSphere.transform.position = bodies[i].Position;
        }

    }

    private void Update()
    {
        SetCurrentInfluenceSphereCelestialBody();
    }

    public static Vector3 CalculateAcceleration(CelestialBody point, CelestialBody[] bodies)
    {
        Vector3 acceleration = Vector3.zero;
        //foreach (var body in Instance.bodies)
        //{
        //    if (body != ignoreBody)
        //    {
        //        float sqrDst = ((body.Position - point) * 10000).sqrMagnitude;    // r^2
        //        Vector3 forceDir = ((body.Position - point) * 10000).normalized;  // Vector3
        //        acceleration += forceDir * CalculationUtilsMath.G * body.mass / sqrDst;
        //    }
        //}

        for (int j = 0; j < bodies.Length; j++)
        {
            if (point == bodies[j])
            {
                continue;
            }

            if (point.influenceSphereCelestialBody == bodies[j])
            {
                // Debug.Log("I " + virtualBodies[i].bodyName + " inside " + virtualBodies[j].bodyName);
                Vector3 forceDir = ((bodies[j].Position - point.Position) * 10000).normalized;
                float sqrDst = ((bodies[j].Position - point.Position) * 10000).sqrMagnitude;
                acceleration += forceDir * CalculationUtilsMath.G * bodies[j].mass / sqrDst;
            }

            //Debug.Log("X " + virtualBodies[j].position.x*10000);
            //Debug.Log("X " + virtualBodies[i].position.x * 10000);
            //Debug.Log("Sqrt " + sqrDst);
            //float distance = CalculationUtilsMath.CalculateDistanceBetweenTwoPoints(virtualBodies[j].position.x, virtualBodies[j].position.y, virtualBodies[i].position.x, virtualBodies[i].position.y);
            //Debug.Log("Distance = " + distance*distance + " km");
        }

        return acceleration;
    }

    public void SetCurrentInfluenceSphereCelestialBody()
    {
        // Set SetInfluenceSphereCelestialBody of the planets
        for (int i = 0; i < bodies.Length; i++)
        {
            if (bodies[i].spaceObjectType == SpaceObjectType.Planet || bodies[i].spaceObjectType == SpaceObjectType.Moon)
            {
                float xPlus = bodies[i].Position.x + bodies[i].sphereOfInfluence;
                float xMinus = bodies[i].Position.x - bodies[i].sphereOfInfluence;
                float yPlus = bodies[i].Position.y + bodies[i].sphereOfInfluence;
                float yMinus = bodies[i].Position.y - bodies[i].sphereOfInfluence;

                //Debug.Log(virtualBodies[i].bodyName + " X- " + xMinus + " :  X+ " + xPlus + "   Y- " + yMinus + " :  Y+ " + yPlus);

                if (bodies[i].spaceObjectType == SpaceObjectType.Planet)
                {
                    for (int j = 0; j < bodies.Length; j++)
                    {
                        if (bodies[i] != bodies[j])
                        {
                            //Debug.Log(virtualBodies[j].bodyName + " P X " + virtualBodies[j].position.x + " :  " + virtualBodies[j].position.y);
                            if ((bodies[j].Position.x < xPlus && bodies[j].Position.x > xMinus) &&
                                (bodies[j].Position.y < yPlus && bodies[j].Position.y > yMinus))
                            {
                                bodies[j].SetInfluenceSphereCelestialBody(bodies[i]);
                                //Debug.Log("VIRTUAL BODY INFLUENCE " + bodies[j].bodyName + " inside " + bodies[i].bodyName);
                                break;
                            }
                        }
                    }
                }

                //Debug.Log(virtualBodies[i].bodyName + " SOI = " + virtualBodies[i].sphereOfInfluence);
            }
        }
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