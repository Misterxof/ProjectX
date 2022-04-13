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
    public GameObject completetPathGameObject;

    [ReadOnly]
    public CelestialBody influenceSphereCelestialBody;

    //[ReadOnly]
    //public LineRenderer complitePath;

    private int iteration = 0;

    private bool isStarted = false;

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

    private void Start()
    {
        isStarted = true;
        if (spaceObjectType == SpaceObjectType.Planet || spaceObjectType == SpaceObjectType.Moon)
        {
            completetPathGameObject = new GameObject("Complete Path");
            completetPathGameObject.transform.parent = transform;
            completetPathGameObject.AddComponent<LineRenderer>();
            //complitePath = completetPathGameObject.GetComponent<LineRenderer>();
            completetPathGameObject.GetComponent<LineRenderer>().enabled = true;
            completetPathGameObject.GetComponent<LineRenderer>().positionCount = 0;
            completetPathGameObject.GetComponent<LineRenderer>().startColor = gameObject.GetComponentInChildren<SpriteRenderer>().color;
            completetPathGameObject.GetComponent<LineRenderer>().endColor = gameObject.GetComponentInChildren<SpriteRenderer>().color;
            //completetPathGameObject.GetComponent<LineRenderer>().material = Resources.Load<Material>("Resources/unity_builtin_extra/Default-Line");
            completetPathGameObject.GetComponent<LineRenderer>().material = new Material(Shader.Find("Legacy Shaders/Particles/Alpha Blended Premultiply"));
            completetPathGameObject.GetComponent<LineRenderer>().material.color = gameObject.GetComponentInChildren<SpriteRenderer>().color;
            //Debug.Log("" + complitePath.startColor.ToString());
            completetPathGameObject.GetComponent<LineRenderer>().widthMultiplier = 100;
            //this.completetPathGameObject = completetPathGameObject;
        }
       
    }

    private void Update()
    {
        if (!Application.isPlaying)
        {
            DestroyImmediate(completetPathGameObject);  // for debug modert
        }

        if (Application.isPlaying && (spaceObjectType == SpaceObjectType.Planet || spaceObjectType == SpaceObjectType.Moon)) {
            Debug.Log(" " + isStarted + "  " + name + "  i = " + iteration);
            completetPathGameObject.GetComponent<LineRenderer>().positionCount = iteration + 1;
            completetPathGameObject.GetComponent<LineRenderer>().SetPosition(iteration, this.transform.position);
            Debug.Log(" " + completetPathGameObject.GetComponent<LineRenderer>().positionCount);
            iteration++;
        }
            

      
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

    private void OnDestroy()
    {
        Destroy(completetPathGameObject);
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