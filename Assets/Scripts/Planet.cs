using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planet : MonoBehaviour
{
    [SerializeField]
    public bool planet = true;

    [SerializeField]
    public float radius = 1f;

    [SerializeField]
    public float mass = 1f;

    [ReadOnly]
    [SerializeField]
    private float distance = 1f;

    [SerializeField]
    public float speedX = 0f;

    [SerializeField]
    public float speedY = 100f;

    [ReadOnly]
    [SerializeField]
    private float accelerationX = 1f;

    [ReadOnly]
    [SerializeField]
    private float accelerationY = 1f;

    private PlanetOrbit planetOrbit;

    [SerializeField]
    private Vector2 velocity;

    [SerializeField]
    Vector2 acceleration;

    private GameObject sun;

    private LineRenderer lineRenderer;

    int i = 0;

    float T = 1f;

    // Start is called before the first frame update
    void Start()
    {
        if (planet)
        {
            sun = GameObject.Find("Sun");

            distance = CalculationUtils.CalculateDistanceBetweenTwoPoints(transform.position.x, transform.position.y, sun.transform.position.x, sun.transform.position.y);
            Debug.Log("Distance = " + distance + " km");

            accelerationX = CalculationUtils.CalculateAcceleration(sun.GetComponent<Planet>().mass, transform.position.x, sun.transform.position.x, distance);
            Debug.Log("Acceleration X = " + accelerationX + "m/c^2");

            accelerationY = CalculationUtils.CalculateAcceleration(sun.GetComponent<Planet>().mass, transform.position.y, sun.transform.position.y, distance);
            Debug.Log("Acceleration Y = " + accelerationY + "m/c^2");

            //float x = 1000 * Mathf.Cos(Mathf.Deg2Rad * 0f);
            //float y = 500 * Mathf.Sin(Mathf.Deg2Rad * 0f);
            //transform.position = new Vector2(x, y);

            GameObject orbit = new GameObject("orbit");
            planetOrbit = orbit.AddComponent<PlanetOrbit>();

            gameObject.AddComponent<LineRenderer>();
            lineRenderer = GetComponent<LineRenderer>();
            lineRenderer.positionCount = 100000;
            lineRenderer.startWidth = 100;
            lineRenderer.endWidth = 100;
            // transform.localScale = new Vector2(culcRadius(), culcRadius());
            Debug.Log("Radius = " + culcRadius());
        }
        //GameObject new1 = new GameObject("pl", typeof(SpriteRenderer));
        //new1.transform.position = new Vector2(-radius, 0);
        //new1.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/Moon");

        //GameObject new2 = new GameObject("pl", typeof(SpriteRenderer));
        //new2.transform.position = new Vector2(radius, 0);
        //new2.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/Moon");
    }

    // Update is called once per frame
    void Update()
    {
        if (planet)
        {
            i++;
            //distance = CalculationUtils.CalculateDistanceBetweenTwoPoints(transform.position.x, transform.position.y, sun.transform.position.x, sun.transform.position.y);
            //Debug.Log("Distance = " + distance + " km");

            //accelerationX = CalculationUtils.CalculateAcceleration(sun.GetComponent<Planet>().mass, transform.position.x, sun.transform.position.x, distance);
            //Debug.Log("Acceleration X = " + accelerationX + "m/c^2");

            //accelerationY = CalculationUtils.CalculateAcceleration(sun.GetComponent<Planet>().mass, transform.position.y, sun.transform.position.y, distance);
            //Debug.Log("Acceleration Y = " + accelerationY + "m/c^2");

            //speedX += T * accelerationX;
            //speedY += T * accelerationY;
            //Debug.Log("Speed X = " + T * speedX + "m/c");
            //Debug.Log("Speed Y = " + T * speedY + "m/c");

            //transform.position = new Vector2(transform.position.x + T * speedX, transform.position.y + T * speedY);
            //Debug.Log("Position = " + transform.position.x + T * speedX + "  |  " + transform.position.y + T * speedY);

            float sqrDst = (sun.GetComponent<Rigidbody2D>().position - GetComponent<Rigidbody2D>().position).sqrMagnitude;
            Vector2 forceDir = (sun.GetComponent<Rigidbody2D>().position - GetComponent<Rigidbody2D>().position).normalized;

            mass = 10 * 300 * 300 / CalculationUtils.G;

            acceleration = forceDir * CalculationUtils.G * mass / sqrDst;
            velocity += acceleration * T;

            GetComponent<Rigidbody2D>().MovePosition(GetComponent<Rigidbody2D>().position + velocity * T);

            lineRenderer.SetPosition(i, new Vector3(transform.position.x + T * speedX, transform.position.y + T * speedY, 0f));
        }
    }

    private float culcRadius()
    {
        return radius / 20f;
    }
}
