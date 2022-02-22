using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planet : MonoBehaviour
{
    [SerializeField]
    public float radius = 1f;

    private float G = 6.67408e-11f;

    private PlanetOrbit planetOrbit;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(G);
        float x = 1000 * Mathf.Cos(Mathf.Deg2Rad * 0f);
        float y = 500 * Mathf.Sin(Mathf.Deg2Rad * 0f);
        transform.position = new Vector2(x, y);

        GameObject orbit = new GameObject("orbit");
        planetOrbit = orbit.AddComponent<PlanetOrbit>();
        transform.localScale = new Vector2(culcRadius(), culcRadius());
        GameObject new1 = new GameObject("pl", typeof(SpriteRenderer));
        new1.transform.position = new Vector2(-radius, 0);
        new1.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/Moon");

        GameObject new2 = new GameObject("pl", typeof(SpriteRenderer));
        new2.transform.position = new Vector2(radius, 0);
        new2.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/Moon");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private float culcRadius()
    { 
        if (radius > 20f)
        {
            return radius / 20f;
        } else
        {
            return radius;
        }
    }
}
