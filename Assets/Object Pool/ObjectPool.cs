using System.Collections.Generic;
using UnityEngine;

namespace Basics.ObjectPool
{
    [System.Serializable]
    public class ObjectPool
    {
        public GameObject objectToPool;
        public string stringID;
        public int startCount;
        public List<GameObject> activePool;
        public List<GameObject> inactivePool;
        [HideInInspector]
        public Transform parentTransform;

        public ObjectPool(GameObject ObjectToPool, string StringId, int StartCount, List<GameObject> ActivePool, List<GameObject> InactivePool, Transform ParentTransform)
        {
            objectToPool = ObjectToPool;
            stringID = StringId;
            startCount = StartCount;
            activePool = ActivePool;
            inactivePool = InactivePool;
            parentTransform = ParentTransform;
        }
    }
}
