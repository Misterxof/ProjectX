using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CalculationUtils
{
    public static float G = 6.67408e-20f;   // km^3/kg s^2
    //public static float G = 6.67408e-11f;   // m^3/kg s^2
    //public static float G = 0.0001f;
    //public static float G = 0.0006674f;
    public const float T = 100f;

    public static float CalculateDistanceBetweenTwoPoints(float x1, float y1, float x2, float y2)
    {
        float result = Mathf.Sqrt(Mathf.Pow(x1 - x2, 2) + Mathf.Pow(y1 - y2, 2));
        return result;
    }

    public static float CalculateAcceleration(float M, float xP, float xS, float r)
    {
        //float result = G * M * (xP * 1000 - xS * 1000) / Mathf.Pow(r * 1000, 3);
        float result = G * M  / r * 1000;
        return result;
    }

    public static float CalculateSphereOfInfluence(float r, float m, float M)
    {
        float result = r * Mathf.Pow((m / M), (2f / 5f));
        return result;
    }

    public static float CalculateCratetrRadius(float velocity, float densityAsteroid, float densityTarget, float g, float radiusAsteroid)
    {
        float deltaDensity = densityAsteroid / densityTarget;
        float result = Mathf.Pow(Mathf.Pow(velocity * 1000, 2) / g * deltaDensity, (1f / 4f)) * Mathf.Pow(radiusAsteroid, (3f / 4f));
        return result / 1000;
    }

    public static Vector3 CalculateNewPointByDepth(Vector3 firstPoint, Vector3 secondPoint, float depth)
    {
        float r = (firstPoint - secondPoint).magnitude;
        Vector3 result = secondPoint + (firstPoint - secondPoint) * (depth / r);
        return result;
    }
}
