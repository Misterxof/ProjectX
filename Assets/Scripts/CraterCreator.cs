using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CraterCreator : MonoBehaviour
{
    public float mass = 1f;
    public float radius = 1f;

    private MeshFilter meshFilter;

    // Start is called before the first frame update
    void Start()
    {
        meshFilter = gameObject.GetComponent<MeshFilter>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void OnCollision(Collision2D collision)
    {
        Debug.Log("Collision");
        Debug.Log("Enter " + collision.contacts[0].point.x + "    " + collision.contacts[0].point.y);

        Vector3 collision2DPoint = collision.contacts[0].point;
        float craterDiametr = CalculationUtilsMath.CalculateCratetrRadius(17, 2600, 2600, 9.8f, 100);
        float craterDepth = 0.4f * craterDiametr;
        float scale = transform.localScale.x;
        Debug.Log(craterDiametr + " km");
        Debug.Log(craterDepth + " km");

        Mesh mesh = meshFilter.mesh;
        Vector3[] allVerticesArray = meshFilter.mesh.vertices;
        Tuple<int, Vector3> middleVertex = FindCollisionVertex(collision2DPoint, allVerticesArray, scale);
        Debug.Log(middleVertex);

        GameObject verticesFinderSquare = CreateVerticesFinderSquare(middleVertex.Item2, craterDiametr, craterDepth, scale);
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
    }

    private GameObject CreateVerticesFinderSquare(Vector3 collision2DPoint, float diametr, float depth, float scale)
    {
        GameObject verticesFinderSquare = new GameObject("VerticesFinderSquare");
        verticesFinderSquare.transform.position = collision2DPoint * scale;
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
