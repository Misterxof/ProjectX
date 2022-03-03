using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraterCreator : MonoBehaviour
{
    public float mass = 1f;
    public float radius = 1f;
    // Start is called before the first frame update
    void Start()
    {
        CalculationUtils.CalculateCratetrRadius(17, 2600, 5520, 9.8f, 100);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
