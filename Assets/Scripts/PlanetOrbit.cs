using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetOrbit : MonoBehaviour
{
    private LineRenderer lineRenderer;
    // Start is called before the first frame update
    void Start()
    {
        int size = (int)((2  * Mathf.PI)/ 0.01f);
        Debug.Log("Size " + size);
        gameObject.AddComponent<LineRenderer>();
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = size + 1;
        float angle = 0f;

        for (int i = 0; i < size + 1; i++)
        {
            float x = 1000 * Mathf.Cos(Mathf.Deg2Rad * angle);
            float y = 500 * Mathf.Sin(Mathf.Deg2Rad * angle);
            lineRenderer.SetPosition(i, new Vector3(x, y, 0f));

            angle += (360f / size);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
