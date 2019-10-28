using UnityEngine;

namespace Basics.PointContainer.Editor
{
    using UnityEditor;

    [CustomEditor(typeof(PointContainer))]
    public class PointContainerEditor : Editor
    {
        private void Awake()
        {
            //Add delegate
            Selection.selectionChanged += SelectionCheck;
        }

        private void OnDestroy()
        {
            //Remove delegate
            Selection.selectionChanged -= SelectionCheck;
        }

        private void SelectionCheck()
        {
            PointContainer myTarget = (PointContainer)target;

            //If the target has been deleted by the time this delegate gets called then don't run the following code
            if(myTarget != null)
                Tools.current = (Selection.activeGameObject == myTarget.gameObject) ? Tool.None : Tool.Move;
        }

        public override void OnInspectorGUI()
        {
            //Create a target to grab data from
            PointContainer myTarget = (PointContainer)target;

            //This makes sure that the active id is not out of bounds of the points array
            if (myTarget.activeId >= myTarget.points.Count)
                myTarget.activeId = Mathf.Clamp(myTarget.points.Count - 1, 0, myTarget.points.Count);

            GUILayout.Label("Tools");
            EditorGUILayout.BeginVertical("box");

            if (GUILayout.Button("Add New Point"))
            {
                Vector3 startPoint = (myTarget.points.Count > 0) ? myTarget.points[myTarget.activeId].LocalPosition : Vector3.zero;

                myTarget.AddPoint(1, startPoint);
                myTarget.activeId = myTarget.points.Count;
            }

            if (GUILayout.Button("Snap All To Ground"))
            {
                for (int i = 0; i < myTarget.points.Count; i++)
                {
                    myTarget.SnapToGround(i);
                }
            }

            //Change colour of points based on selected point
            GUI.color = (Tools.current == Tool.None) ? Color.red : Color.green;

            GUI.color = Color.white;

            EditorGUILayout.EndVertical();

            GUILayout.Label("Points");
            EditorGUILayout.BeginVertical("box");

            //If we have active points then update the UI with their editors
            if (myTarget.points.Count > 0)
            {
                for (int i = 0; i < myTarget.points.Count; i++)
                {
                    DrawPoint(i, myTarget);
                }
            }

            EditorGUILayout.EndVertical();
        }

        private void DrawPoint(int id, PointContainer target)
        {
            EditorGUILayout.BeginVertical("box");
            EditorGUILayout.BeginHorizontal();

            GUI.color = (id == target.activeId) ? Color.green : Color.white;

            if (GUILayout.Button(string.Format("Select Point {0}", id)))
                target.activeId = id;

            GUI.color = Color.white;
            EditorGUILayout.BeginVertical("box");

            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("X");
            target.points[id].LocalPosition.x = EditorGUILayout.FloatField(target.points[id].LocalPosition.x);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Y");
            target.points[id].LocalPosition.y = EditorGUILayout.FloatField(target.points[id].LocalPosition.y);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Z");
            target.points[id].LocalPosition.z = EditorGUILayout.FloatField(target.points[id].LocalPosition.z);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.EndVertical();

            if (target.points[id].GetPointInRadius)
            {
                EditorGUILayout.BeginVertical("box");
                GUILayout.Label("Radius");
                target.points[id].Radius = EditorGUILayout.Slider(target.points[id].Radius, .1f, 10f);
                EditorGUILayout.EndVertical();
            }

            if (GUILayout.Button("Snap To Ground"))
                target.SnapToGround(id);

            GUI.color = Color.red;
            if (GUILayout.Button("X"))
                target.RemovePoint(id);

            GUI.color = Color.white;

            EditorGUILayout.EndHorizontal();

            target.points[id].GetPointInRadius = EditorGUILayout.Toggle("Get random point within radius", target.points[id].GetPointInRadius);

            EditorGUILayout.EndVertical();


            SceneView.RepaintAll();
        }

        protected virtual void OnSceneGUI()
        {
            PointContainer myTarget = (PointContainer)target;

            if (myTarget.points.Count <= 0)
                return;

            EditorGUI.BeginChangeCheck();
            Vector3 newTargetPosition = Vector3.zero;
            if(myTarget.activeId >= 0 && myTarget.activeId < myTarget.points.Count)
                newTargetPosition = Handles.PositionHandle(myTarget.transform.position + myTarget.points[myTarget.activeId].LocalPosition, Quaternion.identity);

            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(myTarget, "Move Point");
                myTarget.points[myTarget.activeId].LocalPosition = newTargetPosition - myTarget.transform.position;
            }

            for (int i = 0; i < myTarget.points.Count; i++)
            {
                Handles.color = (i == myTarget.activeId) ? new Color(0, 1, 0, .4f) : new Color(1, .5f, 0, .4f);
                float radius = (myTarget.points[i].GetPointInRadius) ? myTarget.points[i].Radius : .5f;
                Handles.DrawSolidDisc(myTarget.transform.position + myTarget.points[i].LocalPosition, Vector3.up, radius);
                Handles.color = Color.black;
                Handles.Label(myTarget.transform.position + myTarget.points[i].LocalPosition, i.ToString());
            }
        }
    }
}