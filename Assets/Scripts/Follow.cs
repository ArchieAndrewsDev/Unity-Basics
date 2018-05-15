﻿//Author: Archie Andrews

using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Basics
{
  public class Follow : MonoBehaviour
  {
    public UpdateState updateState;
    public FollowStates followState;

    public List<Transform> targets = new List<Transform>();
    public bool followPosition = true, followRotation = false;
    public float positionSmoothTime = 1, rotationSmoothTime = 1;

    private Vector3 posVelocity, rotVelocity;
    private Transform[] subjectArray, targetArray;

    private void Awake()
    {
      subjectArray = new Transform[1];
      targetArray = new Transform[1];
    }

    private void Update()
    {
      if (updateState == UpdateState.Update)
        CheckFollow();
    }

    private void FixedUpdate()
    {
      if (updateState == UpdateState.FixedUpdate)
        CheckFollow();
    }

    private void LateUpdate()
    {
      if (updateState == UpdateState.LateUpdate)
        CheckFollow();
    }

    private void CheckFollow()
    {
      TransformNullCheck(targets);

      switch (followState)
      {
        case FollowStates.Me:
          FollowTarget(targets.ToArray(), transform);
          break;
        case FollowStates.You:
          FollowTarget(transform, targets[0]);
          break;
        case FollowStates.Mean:
          FollowTarget(transform, targets.ToArray());
          break;
      }
    }

    #region FollowMethods

    private void FollowTarget(Transform subject, Transform target)
    {
      subjectArray[0] = subject;
      targetArray[0] = target;

      FollowTarget(subjectArray, targetArray);
    }

    private void FollowTarget(Transform[] subject, Transform target)
    {
      targetArray[0] = target;
      FollowTarget(subject, targetArray);
    }

    private void FollowTarget(Transform subject, Transform[] target)
    {
      subjectArray[0] = subject;
      FollowTarget(subjectArray, target);
    }

    private void FollowTarget(Transform[] subject, Transform[] target)
    {
      Vector3 meanPosition = Vector3.zero;

      if (target.Length > 1)
      {
        for (int i = 0; i < target.Length; i++)
        {
          meanPosition += target[i].position;
        }
        meanPosition = meanPosition / target.Length;
      }
      else
        meanPosition = target[0].position;

      for (int i = 0; i < subject.Length; i++)
      {
        if (followPosition)
          subject[i].position = Vector3.SmoothDamp(subject[i].position, meanPosition, ref posVelocity, positionSmoothTime);

        if (followRotation)
          subject[i].eulerAngles = Vector3.SmoothDamp(subject[i].eulerAngles, target[0].eulerAngles, ref rotVelocity, rotationSmoothTime);
      }
    }
    #endregion

    private void TransformNullCheck(List<Transform> list)
    {
      for (int i = 0; i < list.Count; i++)
      {
        if (list[i] == null)
        {
          list.RemoveAt(i);
          Debug.LogWarning("Warning: Follow script found a null Transform at " + i);
        }
      }
    }

    public void AddTarget(Transform newTarget)
    {
      for (int i = 0; i < targets.Count; i++)
      {
        if (targets[i] == newTarget)
          return;
      }

      targets.Add(newTarget);
    }

    public void RemoveTarget(Transform oldTarget)
    {
      targets.Remove(oldTarget);
    }
  }

  public enum UpdateState { Update, FixedUpdate, LateUpdate, None }
  public enum FollowStates { Me, You, Mean }

  [CustomEditor(typeof(Follow))]
  public class FollowEditor : Editor
  {
    public override void OnInspectorGUI()
    {
      Follow instance = (Follow)target;

      instance.updateState = (UpdateState)EditorGUILayout.EnumPopup("Update Type", instance.updateState);
      instance.followState = (FollowStates)EditorGUILayout.EnumPopup("Follow Type", instance.followState);

      switch (instance.followState)
      {
        case FollowStates.Me:
          EditorGUILayout.HelpBox("All of the target objects will follow this transform.", MessageType.Info);
          break;
        case FollowStates.You:
          EditorGUILayout.HelpBox("This transform will follow the target transform.", MessageType.Info);
          break;
        case FollowStates.Mean:
          EditorGUILayout.HelpBox("This transform will follow the center point of all of the targets.", MessageType.Info);
          break;
      }

      instance.followPosition = EditorGUILayout.Toggle("Follow Position", instance.followPosition);
      instance.followRotation = EditorGUILayout.Toggle("Follow Rotation", instance.followRotation);

      instance.positionSmoothTime = EditorGUILayout.FloatField("Position Smooth Time", instance.positionSmoothTime);
      instance.rotationSmoothTime = EditorGUILayout.FloatField("Rotation Smooth Time", instance.rotationSmoothTime);

      TargetGUI(instance);
    }

    public void TargetGUI(Follow instance)
    {
      Event evt = Event.current;
      Rect drop_area = GUILayoutUtility.GetRect(0.0f, 50.0f, GUILayout.ExpandWidth(true));
      GUI.Box(drop_area, "Drag Target Objects Here");

      for (int i = 0; i < instance.targets.Count;i ++)
      {
        DrawPrefabIcon(instance, i);
      }

      switch (evt.type)
      {
        case EventType.DragUpdated:
        case EventType.DragPerform:
          if (!drop_area.Contains(evt.mousePosition))
            return;

          DragAndDrop.visualMode = DragAndDropVisualMode.Generic;

          if (evt.type == EventType.DragPerform)
          {
            DragAndDrop.AcceptDrag();
            foreach (GameObject prefab in DragAndDrop.objectReferences)
            {
              instance.AddTarget(prefab.transform);
            }
          }
          break;
      }
    }

    private void DrawPrefabIcon(Follow instance, int id)
    {
      EditorGUILayout.BeginHorizontal("box");
      instance.targets[id] = (Transform)EditorGUILayout.ObjectField(instance.targets[id], typeof(Transform), true);

      GUI.color = Color.red;
      if (GUILayout.Button("X"))
        instance.targets.RemoveAt(id);
      GUI.color = Color.white;

      EditorGUILayout.EndHorizontal();
    }
  }
}
