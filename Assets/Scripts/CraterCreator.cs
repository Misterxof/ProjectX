using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraterCreator : MonoBehaviour
{
    public float mass = 1f;
    public float radius = 1f;

    [ReadOnly]
    public float craterDiametr = 1f;

    [ReadOnly]
    public float craterDepth = 1f;

    private MeshFilter meshFilter;

    private Vector3[] allVerticesArray;
    private Tuple<int, Vector3> middleVertex;
    public GameObject gameObject;
    public GameObject gameObject2;
    BoxCollider2D m_Collider;
    Mesh mesh;

    // Start is called before the first frame update
    void Start()
    {
        m_Collider = GetComponent<BoxCollider2D>();
        Debug.Log(m_Collider);

        meshFilter = gameObject.GetComponent<MeshFilter>();
        mesh = meshFilter.mesh;
        allVerticesArray = meshFilter.mesh.vertices;
        middleVertex = new Tuple<int, Vector3>(128, allVerticesArray[128]);
        Debug.Log(middleVertex);

        craterDiametr = CalculationUtils.CalculateCratetrRadius(17, 2600, 2600, 9.8f, 100);
        craterDepth = 0.4f * craterDiametr;
        Debug.Log(craterDiametr + " km");
        Debug.Log(craterDepth + " km");
        Dictionary<int, Vector3> insideVertices = new Dictionary<int, Vector3>();
        Dictionary<int, Vector3> beforeVertices = new Dictionary<int, Vector3>();
        Dictionary<int, Vector3> afterVertices = new Dictionary<int, Vector3>();
        bool middleVertexFlag = false;

        for (int i = 0; i < allVerticesArray.Length; i++)
        {
            // Debug.Log(allVerticesArray[i]);
            if (m_Collider.bounds.Contains(allVerticesArray[i] * gameObject.transform.localScale.x))
            {
                Debug.Log("" + i + " Bounds contain the point : " + allVerticesArray[i]);
                insideVertices.Add(i, allVerticesArray[i]);

                if (allVerticesArray[i].Equals(middleVertex.Item2))
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
        float scale = gameObject.transform.localScale.x;
        Debug.Log("List size : " + insideVertices.Count);
        Debug.Log("B List size : " + beforeVertices.Count);
        Debug.Log("A List size : " + afterVertices.Count);
        Debug.Log("test New point : " + CalculationUtils.CalculateNewPointByDepth(new Vector3(2,0,0), new Vector3(0,0,0), 1));
        Debug.Log("New point : " + CalculationUtils.CalculateNewPointByDepth(gameObject.transform.position * scale, middleVertex.Item2 * scale, craterDepth));

        allVerticesArray[128] = CalculationUtils.CalculateNewPointByDepth(gameObject.transform.position * scale, middleVertex.Item2 * scale, craterDepth) / scale;
        mesh.vertices = allVerticesArray;
        mesh.RecalculateBounds();
        Debug.Log("allpoints : " + allVerticesArray[128]);
        Debug.Log("mesh allpoints : " + mesh.vertices[128]);
        Debug.Log("meshFilter allpoints : " + meshFilter.mesh.vertices[128]);
    }

    // Update is called once per frame
    void Update()
    {
    }
}
