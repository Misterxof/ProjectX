using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CalculationUtils
{
    //public static float G = 6.67408e-11f;
    public static float G = 1f;

    public static float CalculateDistanceBetweenTwoPoints(float x1, float y1, float x2, float y2)
    {
        float result = Mathf.Sqrt(Mathf.Pow(x1 - x2, 2) + Mathf.Pow(y1 - y2, 2));
        return result;
    }

    public static float CalculateAcceleration(float M, float xP, float xS, float r)
    {
        float result = G * M * (xP * 1000 - xS * 1000) / Mathf.Pow(r * 1000, 3);
        return result;
    }
}
