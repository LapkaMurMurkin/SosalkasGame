using System;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.AI;

namespace Extensions
{
    public static class VectorMath
    {
        public static Vector3 FindNearest(Vector3 departurePoint, Vector3[] arrayToSearch, out int index)
        {
            index = 0;

            if (arrayToSearch.IsNullOrEmpty())
                return Vector3.zero;

            Vector3 nearestPoint = arrayToSearch[0];
            float distance = float.MaxValue;
            float shortestDistance = distance;

            for (int i = 0; i < arrayToSearch.Length; i++)
            {
                distance = Vector3.Distance(departurePoint, arrayToSearch[i]);
                if (distance < shortestDistance)
                {
                    shortestDistance = distance;
                    index = i;
                    nearestPoint = arrayToSearch[i];
                }
            }

            return nearestPoint;
        }

        public static Vector2 FindNearest(Vector2 departurePoint, Vector2[] arrayToSearch, out int index)
        {
            index = 0;
            Vector2 nearestPoint = arrayToSearch[0];
            float distance = Vector2.Distance(departurePoint, nearestPoint);
            float shortestDistance = distance;

            for (int i = 1; i < arrayToSearch.Length; i++)
            {
                distance = Vector2.Distance(departurePoint, arrayToSearch[i]);
                if (distance < shortestDistance)
                {
                    shortestDistance = distance;
                    index = i;
                    nearestPoint = arrayToSearch[i];
                }
            }

            return nearestPoint;
        }

        public static Transform FindNearest(Transform departurePoint, Transform[] arrayToSearch, out int index)
        {
            Vector3[] convertedArray = new Vector3[arrayToSearch.Length];
            for (int i = 0; i < convertedArray.Length; i++)
            {
                convertedArray[i] = arrayToSearch[i].position;
            }

            FindNearest(departurePoint.position, convertedArray, out index);

            return arrayToSearch[index];
        }

        public static void SortByDistance(Vector3 departurePoint, Vector3[] arrayToSort)
        {
            float[] distances = new float[arrayToSort.Length];

            for (int i = 0; i < distances.Length; i++)
            {
                distances[i] = Vector3.Distance(departurePoint, arrayToSort[i]);
            }

            Array.Sort(distances, arrayToSort);
        }

        public static void SortByDistance(Transform departurePoint, Transform[] arrayToSort)
        {
            float[] distances = new float[arrayToSort.Length];

            for (int i = 0; i < distances.Length; i++)
            {
                distances[i] = Vector3.Distance(departurePoint.position, arrayToSort[i].position);
            }

            Array.Sort(distances, arrayToSort);
        }

        public static bool InterceptionPoint(Vector3 targetPosition, Vector3 interceptorPosition, Vector3 targetVelocity, float interceptorSpeed, out Vector3 interceptionPoint)
        {
            Vector3 distanceToTargetDelta = interceptorPosition - targetPosition;
            float distanceToTarget = (distanceToTargetDelta).magnitude;
            float angleFromTargetPosition = Vector3.Angle(distanceToTargetDelta, targetVelocity) * Mathf.Deg2Rad;
            float targetSpeed = targetVelocity.magnitude;
            float speedRatio = targetSpeed / interceptorSpeed;

            float a = 1 - speedRatio * speedRatio;
            float b = 2 * speedRatio * distanceToTarget * (float)System.Math.Cos(angleFromTargetPosition);
            float c = -(distanceToTarget * distanceToTarget);
            if (Math.SolveQuadratic(a, b, c, out float root1, out float root2) == 0)
            {
                interceptionPoint = Vector3.zero;
                return false;
            }

            float interceptionDistance = Mathf.Max(root1, root2);
            float interceptionTime = interceptionDistance / interceptorSpeed;

            interceptionPoint = targetPosition + targetVelocity * interceptionTime;
            return true;
        }
    }
}