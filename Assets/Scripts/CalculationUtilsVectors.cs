using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CalculationUtilsVectors
{
    public static Vector3 CalculateNewPointByDepth(Vector3 firstPoint, Vector3 secondPoint, float depth)
    {
        float r = (firstPoint - secondPoint).magnitude;
        Vector3 result = secondPoint + (firstPoint - secondPoint) * (depth / r);
        return result;
    }

    public static Vector3 CalculateNewVirtualCentralPoint(Vector3 point, Vector3 collisionPoint, Vector3 centralPoint)
    {
        Vector3 offsetPoint = point - collisionPoint;   // offcet from middle(collision) point
        Vector3 result = centralPoint + offsetPoint;    // new virtual central point on one line with point
        return result;
    }

    public static Vector3[] CalculateNewVerticesPositions(Vector3 objectPosition, Vector3[] allVerticesArray, Tuple<int, Vector3> middleVertex,
        Dictionary<int, Vector3> beforeVertices, Dictionary<int, Vector3> afterVertices, float craterDepth, float scale)
    {
        allVerticesArray[middleVertex.Item1] = CalculateNewPointByDepth(objectPosition * scale, middleVertex.Item2 * scale, craterDepth) / scale;
        float craterDepthStepBefore = craterDepth / (beforeVertices.Count + 1);
        float currentCraterDepth = 0f;

        foreach (var valuePair in beforeVertices)
        {
            currentCraterDepth += craterDepthStepBefore;
            Debug.Log("1 Depth : " + currentCraterDepth);
            Vector3 virtualCentralPoint = CalculateNewVirtualCentralPoint(valuePair.Value, middleVertex.Item2, objectPosition);
            allVerticesArray[valuePair.Key] = CalculateNewPointByDepth(virtualCentralPoint * scale, valuePair.Value * scale, currentCraterDepth) / scale;
        }

        foreach (var valuePair in afterVertices)
        {
            Debug.Log("2 Depth : " + currentCraterDepth);
            Vector3 virtualCentralPoint = CalculateNewVirtualCentralPoint(valuePair.Value, middleVertex.Item2, objectPosition);
            allVerticesArray[valuePair.Key] = CalculateNewPointByDepth(virtualCentralPoint * scale, valuePair.Value * scale, currentCraterDepth) / scale;

            currentCraterDepth -= craterDepthStepBefore;
        }

        return allVerticesArray;
    }
}
