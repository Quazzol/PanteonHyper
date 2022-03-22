using System.Collections.Generic;
using System.Linq;
using Pooling;
using UnityEngine;

namespace Misc
{
    public static class Extensions
    {
        public static bool AreEqual(this Vector3 first, Vector3 second)
        {
            return AreEqual(first, second, float.Epsilon);
        }

        public static bool AreEqual(this Vector3 first, Vector3 second, float minDistance)
        {
            return Vector3.Distance(first, second) < minDistance;
        }

        public static bool AreEqual(this float first, float second)
        {
            return Aproximity(first, second, float.Epsilon);
        }

        public static bool Aproximity(this float first, float second, float epsilon)
        {
            return Mathf.Abs(first - second) < epsilon;
        }

        public static bool HasDifference<T>(this List<T> first, List<T> second)
        {
            if (first.Count != second.Count)
                return true;
            return first.Except(second).Count() > 0 || second.Except(first).Count() > 0;
        }

        public static void Disable(this IPooledMonoBehaviour pmb)
        {
            pmb.gameObject.SetActive(false);
        }

        public static void DisableAllChildren(this GameObject go)
        {
            Transform transform = go.transform;
            for (int i = 0; i < transform.childCount; i++)
            {
                if (transform.GetChild(i).TryGetComponent<IPooledMonoBehaviour>(out var pmb))
                {
                    pmb.Disable();
                }
            }
        }
    }
}