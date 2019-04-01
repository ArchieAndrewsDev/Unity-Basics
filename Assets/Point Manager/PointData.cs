using UnityEngine;

namespace Basics.PointContainer
{
    [System.Serializable]
    public class PointData
    {
        public float Radius = 1;
        public Vector3 LocalPosition;
        public bool GetPointInRadius;

        public PointData(float radius, Vector3 localPosition, bool getPointInRadius)
        {
            Radius = radius;
            LocalPosition = localPosition;
            GetPointInRadius = getPointInRadius;
        }
    }
}