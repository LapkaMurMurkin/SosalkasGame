//using System;
using System;
using System.Collections.Generic;
using System.Linq;

//using System.Numerics;


using UnityEngine;
using UnityEngine.AI;

namespace Extensions
{
    public static class Utils
    {
        public static bool IsNullOrWhitespace(this string str)
        {
            if (!string.IsNullOrEmpty(str))
            {
                for (int i = 0; i < str.Length; i++)
                {
                    if (!char.IsWhiteSpace(str[i]))
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        public static bool IsNullOrEmpty<T>(this IEnumerable<T> collection)
        {
            if (collection != null)
                return collection.Count() == 0;

            return true;
        }

        public static IEnumerable<T> GetEmptyIfNull<T>(this IEnumerable<T> source)
        {
            return source ?? Enumerable.Empty<T>();
        }

        public static T GetRandom<T>(this IEnumerable<T> source)
        {
            if (source.IsNullOrEmpty())
                return default;

            int randomIndex = UnityEngine.Random.Range(0, source.Count());
            return source.ElementAt(randomIndex);
        }

        public static IEnumerable<T> GetRandom<T>(this IEnumerable<T> source, int count)
        {
            if (source.IsNullOrEmpty())
                return Enumerable.Empty<T>();

            List<T> result = new List<T>();
            List<T> buffer = new List<T>(source);

            for (int i = 0; i < count && buffer.Count > 0; i++)
            {
                T randomElement = buffer.GetRandom();
                result.Add(randomElement);
                buffer.Remove(randomElement);
            }

            return result;
        }

        /// <summary>
        /// Find with Physics.OverlapSphere. Must have colliders.
        /// </summary>
        public static List<T> FindObjectsInRadius<T>(Vector3 position, float radius, T[] exclude = default, int layerMask = Physics.AllLayers)
        {
            Collider[] colliderArray = Physics.OverlapSphere(position, radius, layerMask);
            List<T> objectsWithTypeArray = new List<T>();

            foreach (Collider collider in colliderArray)
            {
                if (collider.TryGetComponent<T>(out T target))
                    objectsWithTypeArray.Add(target);
            }

            if (exclude is not null)
                objectsWithTypeArray = objectsWithTypeArray.Except(exclude).ToList();

            return objectsWithTypeArray;
        }

        public static T FindAnyObjectInRadius<T>(Vector3 position, float radius, T[] exclude = default)
        {
            Collider[] colliderArray = Physics.OverlapSphere(position, radius);

            foreach (Collider collider in colliderArray)
            {
                if (collider.TryGetComponent<T>(out T target))
                {
                    if (exclude is not null && exclude.Contains(target) == true)
                        continue;

                    return target;
                }
            }

            return default;
        }

        public static T FindAnyObjectInRadius<T>(Vector2 position, float radius, T[] exclude = default)
        {
            return FindAnyObjectInRadius<T>(new Vector3(position.x, 0, position.y), radius, exclude);
        }

        public static T[] GetSubArray<T>(T[] array, int index, int length = 0)
        {
            if (length == 0)
                length = array.Length - index;

            T[] subArray = new T[length];
            Array.Copy(array, index, subArray, 0, length);
            return subArray;
        }

        public static Vector3 GetRandomNavMeshPosition(Vector3 position, float radius, int navMeshAreaMask = NavMesh.AllAreas)
        {
            // Генерируем случайные координаты в пределах радиуса
            Vector2 randomCircle = UnityEngine.Random.insideUnitCircle * radius;
            Vector3 randomPosition = new Vector3(randomCircle.x, 0f, randomCircle.y) + position;

            if (NavMesh.SamplePosition(randomPosition, out NavMeshHit hit, radius, navMeshAreaMask))
                return hit.position; // Возвращаем точку на навмеш

            return Vector3.zero; // Если не нашли, возвращаем нулевую точку
        }
    }
}