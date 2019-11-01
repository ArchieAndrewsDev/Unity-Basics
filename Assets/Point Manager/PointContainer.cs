using System.Collections.Generic;
using UnityEngine;

namespace Basics.PointContainer
{
    public class PointContainer : MonoBehaviour
    {
        public List<PointData> points = new List<PointData>();
        public int activeId = 0;

        private List<GameObject> cachedChildren = new List<GameObject>();
        private int roundRobinCount = -1;

        public void AddPoint(float radius, Vector3 position)
        {
            PointData data = new PointData(radius, position, false);
            points.Add(data);
        }

        public void RemovePoint(int id)
        {
            points.RemoveAt(id);
        }

        public Vector3 GetRandomPosition()
        {
            int rnd = Random.Range(0, points.Count);
            return GetPosition(rnd);
        }

        public Vector3 GetRoundRobinPosition()
        {
            roundRobinCount = GetAdjacentId(roundRobinCount, points.Count, 1);
            return GetPosition(roundRobinCount);
        }

        //Converts points into a list of gameobjects
        public List<GameObject> GetPointsAsGameObjects()
        {
            if (cachedChildren.Count <= 0)
            {
                GameObject parent = new GameObject(string.Format("{0}-Points({1})", name, points.Count));
                List<GameObject> children = new List<GameObject>();

                for (int i = 0; i < points.Count; i++)
                {
                    GameObject clone = new GameObject(string.Format("Point {0}", i));
                    clone.transform.position = GetPosition(i);
                    clone.transform.parent = parent.transform;
                    children.Add(clone);
                }

                cachedChildren = children;
            }

            return cachedChildren;
        }

        public void SnapToGround(int id)
        {
            if (id >= points.Count)
                return;

            Ray ray = new Ray(GetPosition(id, true) + (Vector3.up * 2), Vector3.down);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
                points[id].LocalPosition = transform.InverseTransformPoint(hit.point);
            else
                Debug.LogWarning("No collider found under point, unable to snap");
        }

        private Vector3 GetPosition(int id, bool getCenterPoint = false)
        {
            //Center point is the roots position and the local position that is stored on the point manager
            Vector3 centerPoint = (transform.position + points[id].LocalPosition);
            //If asked for get a random point within the radius 
            Vector3 returnPos = (!points[id].GetPointInRadius || getCenterPoint) ? centerPoint : centerPoint + (Random.insideUnitSphere * points[id].Radius);
            //Reset the y position so it's no lower or higher than the original point
            returnPos.y = centerPoint.y;

            return returnPos;
        }

        private Vector3 GetExactPosition(int id)
        {
            return (transform.position + points[id].LocalPosition);
        }

        public Vector3 GetStartPoint()
        {
            return GetExactPosition(0);
        }

        public Vector3 GetEndPoint()
        {
            return GetExactPosition(points.Count - 1);
        }

        private int GetAdjacentId(int curId, int size, int direction)
        {
            int newId = curId += direction;

            if (newId >= size)
                newId = 0;

            if (newId < 0)
                newId = size - 1;

            return newId;
        }
    }
}
