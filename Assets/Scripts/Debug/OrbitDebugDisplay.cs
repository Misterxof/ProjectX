using UnityEngine;

[ExecuteInEditMode]
public class OrbitDebugDisplay : MonoBehaviour
{

    public int numSteps = 1000;
    public float timeStep = 0.1f;
    public bool usePhysicsTimeStep;

    public bool relativeToBody;
    public CelestialBody centralBody;
    public float width = 100;
    public bool useThickLines;
    private VirtualBody star;

    void Start()
    {
        if (Application.isPlaying)
        {
            HideOrbits();
        }
    }

    void Update()
    {

        if (!Application.isPlaying)
        {
            DrawOrbits();
        }
    }

    void DrawOrbits()
    {
        CelestialBody[] bodies = FindObjectsOfType<CelestialBody>();
        var virtualBodies = new VirtualBody[bodies.Length];
        var drawPoints = new Vector3[bodies.Length][];
        int referenceFrameIndex = 0;
        Vector3 referenceBodyInitialPosition = Vector3.zero;

        // Initialize virtual bodies (don't want to move the actual bodies)
        for (int i = 0; i < virtualBodies.Length; i++)
        {
            virtualBodies[i] = new VirtualBody(bodies[i]);
            drawPoints[i] = new Vector3[numSteps];

            if (bodies[i] == centralBody && relativeToBody)
            {
                referenceFrameIndex = i;
                referenceBodyInitialPosition = virtualBodies[i].position;
            }

            if (virtualBodies[i].spaceObjectType == SpaceObjectType.Star) 
                star = virtualBodies[i];
        }

        // Calculate sphereOfInfluence of the planets
        for (int i = 0; i < virtualBodies.Length; i++)
        {
            if (virtualBodies[i].spaceObjectType == SpaceObjectType.Planet)
            {
                float distance = ((star.position - virtualBodies[i].position) * 10000).magnitude;
                virtualBodies[i].sphereOfInfluence = CalculationUtils.CalculateSphereOfInfluence(distance, virtualBodies[i].mass, star.mass) / 100;
                //Debug.Log(virtualBodies[i].bodyName + " SOI = " + virtualBodies[i].sphereOfInfluence);
            }
        }

        // Simulate
        for (int step = 0; step < numSteps; step++)
        {
            Vector3 referenceBodyPosition = (relativeToBody) ? virtualBodies[referenceFrameIndex].position : Vector3.zero;
            // Update velocities
            for (int i = 0; i < virtualBodies.Length; i++)
            {
                virtualBodies[i].velocity += CalculateAcceleration(i, virtualBodies) * timeStep;
            }
            // Update positions
            for (int i = 0; i < virtualBodies.Length; i++)
            {
                Vector3 newPos = virtualBodies[i].position + virtualBodies[i].velocity * timeStep;
                virtualBodies[i].position = newPos;
                if (relativeToBody)
                {
                    var referenceFrameOffset = referenceBodyPosition - referenceBodyInitialPosition;
                    newPos -= referenceFrameOffset;
                }
                if (relativeToBody && i == referenceFrameIndex)
                {
                    newPos = referenceBodyInitialPosition;
                }

                drawPoints[i][step] = newPos;
            }
        }

        // Draw paths
        for (int bodyIndex = 0; bodyIndex < virtualBodies.Length; bodyIndex++)
        {
            var pathColour = bodies[bodyIndex].gameObject.GetComponentInChildren<SpriteRenderer>().color;

            if (useThickLines)
            {
                var lineRenderer = bodies[bodyIndex].gameObject.GetComponentInChildren<LineRenderer>();
                lineRenderer.enabled = true;
                lineRenderer.positionCount = drawPoints[bodyIndex].Length;
                lineRenderer.SetPositions(drawPoints[bodyIndex]);
                lineRenderer.startColor = pathColour;
                lineRenderer.endColor = pathColour;
                lineRenderer.widthMultiplier = width;
            }
            else
            {
                for (int i = 0; i < drawPoints[bodyIndex].Length - 1; i++)
                {
                    Debug.DrawLine(drawPoints[bodyIndex][i], drawPoints[bodyIndex][i + 1], pathColour);
                }

                // Hide renderer
                var lineRenderer = bodies[bodyIndex].gameObject.GetComponentInChildren<LineRenderer>();
                if (lineRenderer)
                {
                    lineRenderer.enabled = false;
                }
            }

        }
    }

    Vector3 CalculateAcceleration(int i, VirtualBody[] virtualBodies)
    {
        Vector3 acceleration = Vector3.zero;
        for (int j = 0; j < virtualBodies.Length; j++)
        {
            if (i == j)
            {
                continue;
            }
            Vector3 forceDir = ((virtualBodies[j].position - virtualBodies[i].position)*10000).normalized;
            float sqrDst = ((virtualBodies[j].position - virtualBodies[i].position)*10000).sqrMagnitude;
            acceleration += forceDir * CalculationUtils.G * virtualBodies[j].mass / sqrDst;
            //Debug.Log("X " + virtualBodies[j].position.x*10000);
            //Debug.Log("X " + virtualBodies[i].position.x * 10000);
            //Debug.Log("Sqrt " + sqrDst);
            //float distance = CalculationUtils.CalculateDistanceBetweenTwoPoints(virtualBodies[j].position.x, virtualBodies[j].position.y, virtualBodies[i].position.x, virtualBodies[i].position.y);
            //Debug.Log("Distance = " + distance*distance + " km");
        }
        return acceleration;
    }

    void HideOrbits()
    {
        CelestialBody[] bodies = FindObjectsOfType<CelestialBody>();

        // Draw paths
        for (int bodyIndex = 0; bodyIndex < bodies.Length; bodyIndex++)
        {
            var lineRenderer = bodies[bodyIndex].gameObject.GetComponentInChildren<LineRenderer>();
            if (lineRenderer != null)
            {
                lineRenderer.positionCount = 0;
            }
        }
    }

    void OnValidate()
    {
        if (usePhysicsTimeStep)
        {
            timeStep = CalculationUtils.T;
        }
    }

    class VirtualBody
    {
        public Vector3 position;
        public Vector3 velocity;
        public float mass;
        public float sphereOfInfluence;
        public SpaceObjectType spaceObjectType;
        public string bodyName;

        public VirtualBody(CelestialBody body)
        {
            position = body.transform.position;
            velocity = body.initialVelocity;
            mass = body.mass;
            spaceObjectType = body.spaceObjectType;
            bodyName = body.bodyName;
        }
    }
}