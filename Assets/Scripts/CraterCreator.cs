using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CraterCreator : MonoBehaviour
{
    public float mass = 1f;
    public float radius = 1f;
    public float g = 9.8f;
    public float dencity = 2600f;

    private MeshFilter meshFilter;

    PolygonCollider2D polygonCollider2D;

    // Start is called before the first frame update
    void Start()
    {
        meshFilter = gameObject.GetComponent<MeshFilter>();
        gameObject.AddComponent<PolygonCollider2D>();
        polygonCollider2D = gameObject.GetComponent<PolygonCollider2D>();
        Vector3[] vertices = meshFilter.mesh.vertices;
        Vector2[] vec = new Vector2[vertices.Length];
   
        for (int i = 0; i < vertices.Length; i++)
        {
            vec[i] = new Vector2(vertices[i].x, vertices[i].y);
        }
        polygonCollider2D.SetPath(0, vec);
        
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void OnCollision(Collision2D collision, GameObject impacter)
    {
        Debug.Log("Collision");
        Debug.Log("Enter " + collision.contacts[0].point.x + "    " + collision.contacts[0].point.y);
        Asteroid asteroid = impacter.GetComponent<Asteroid>();
        float impacterDensity = asteroid.density;
        float impacterVelocity = asteroid.velocity;
        float impacterRadius = asteroid.radius;

        Vector3 collision2DPoint = collision.contacts[0].point;
        float craterDiametr = CalculationUtilsMath.CalculateCratetrRadius(impacterVelocity, impacterDensity, dencity, g, impacterRadius);   // some formula incorrectness or misinterpretation in the source => radius = diametr
        float craterDepth = 0.4f * craterDiametr;
        float scale = transform.localScale.x;
        Debug.Log(craterDiametr + " km");
        Debug.Log(craterDepth + " km");

        Mesh mesh = meshFilter.mesh;
        Vector3[] allVerticesArray = meshFilter.mesh.vertices;
        Tuple<int, Vector3> middleVertex = FindCollisionVertex(collision2DPoint, allVerticesArray, scale);  // collision vertex
        Debug.Log(middleVertex);

        var angle = CalculationUtilsVectors.CalculateAngleBetweenTwoVectors(middleVertex.Item2, transform.position) + 90f;  // angle for verticesFinderSquare rotation, 90f for correction

        Debug.Log("ANGLE " + angle);

        GameObject verticesFinderSquare = CreateVerticesFinderSquare(middleVertex.Item2, craterDiametr, craterDepth, angle, scale);
        BoxCollider2D boxCollider2D = verticesFinderSquare.GetComponent<BoxCollider2D>();
        Tuple<Dictionary<int, Vector3>, Dictionary<int, Vector3>> innerVertices = FindImpuctVertices(boxCollider2D, allVerticesArray, middleVertex.Item2);

        Dictionary<int, Vector3> beforeVertices = innerVertices.Item1;
        Dictionary<int, Vector3> afterVertices = innerVertices.Item2;

        Debug.Log("B List size : " + beforeVertices.Count);
        Debug.Log("A List size : " + afterVertices.Count);
        Debug.Log("New point : " + CalculationUtilsVectors.CalculateNewPointByDepth(transform.position * scale, middleVertex.Item2 * scale, craterDepth));

        Vector3[] newAllVerticesPositions = CalculationUtilsVectors.CalculateNewVerticesPositions(transform.position, allVerticesArray, middleVertex,
            beforeVertices, afterVertices, craterDepth, scale);

        mesh.vertices = newAllVerticesPositions;
        mesh.RecalculateBounds();
        Destroy(verticesFinderSquare);

        polygonCollider2D.SetPath(0, CalculationUtilsVectors.ConvertVector3ArraytoVector2Array(newAllVerticesPositions));   // update polygonCollider2D
    }

    private GameObject CreateVerticesFinderSquare(Vector3 collision2DPoint, float diametr, float depth, float angle, float scale)
    {
        GameObject verticesFinderSquare = new GameObject("VerticesFinderSquare");
        verticesFinderSquare.transform.position = collision2DPoint * scale;
        verticesFinderSquare.transform.Rotate(0, 0, angle);
        verticesFinderSquare.AddComponent<BoxCollider2D>();
        verticesFinderSquare.GetComponent<BoxCollider2D>().size = new Vector2(diametr, depth);

        return verticesFinderSquare;
    }

    private Tuple<int, Vector3> FindCollisionVertex(Vector3 collision2DPoint, Vector3[] allVerticesArray, float scale)
    {
        float minDistance = 10000f;
        Tuple<int, Vector3> closestPoint = new Tuple<int, Vector3>(0, allVerticesArray[0]);

        for (int i = 0; i < allVerticesArray.Length; i++)
        {
            float distance = (collision2DPoint - allVerticesArray[i] * scale).magnitude;

            if (distance < minDistance)
            {
                minDistance = distance;
                closestPoint = new Tuple<int, Vector3>(i, allVerticesArray[i]);
            }
        }

        return closestPoint;
    }

    private Tuple<Dictionary<int, Vector3>, Dictionary<int, Vector3>> FindImpuctVertices(BoxCollider2D boxCollider2D, Vector3[] allVerticesArray, Vector3 middleVertex)
    {
        Dictionary<int, Vector3> beforeVertices = new Dictionary<int, Vector3>();
        Dictionary<int, Vector3> afterVertices = new Dictionary<int, Vector3>();
        bool middleVertexFlag = false;

        for (int i = 0; i < allVerticesArray.Length; i++)
        {
            // Debug.Log(allVerticesArray[i]);
            if (boxCollider2D.bounds.Contains(allVerticesArray[i] * gameObject.transform.localScale.x))
            {
                Debug.Log("" + i + " Bounds contain the point : " + allVerticesArray[i]);

                if (allVerticesArray[i].Equals(middleVertex))
                {
                    middleVertexFlag = true;
                }
                else
                {
                    if (!middleVertexFlag)
                    {
                        beforeVertices.Add(i, allVerticesArray[i]);
                    }
                    else
                    {
                        afterVertices.Add(i, allVerticesArray[i]);
                    }
                }
            }
        }

        return new Tuple<Dictionary<int, Vector3>, Dictionary<int, Vector3>>(beforeVertices, afterVertices);
    }

    
}
