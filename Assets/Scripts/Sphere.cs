using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.EventSystems;

public class Sphere : MonoBehaviour
{
    public float size = 1f;
}

#if UNITY_EDITOR

[CustomEditor(typeof(Sphere)), CanEditMultipleObjects]
public class Sphere_Editor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        var size = serializedObject.FindProperty("size");

        if (size.floatValue < 1f)
            EditorGUILayout.HelpBox("The spheres' sizes cannot be smaller than 1", MessageType.Warning);
        if (size.floatValue > 2f)
            EditorGUILayout.HelpBox("The spheres' sizes cannot be bigger than 2", MessageType.Warning);

        EditorGUILayout.BeginHorizontal();

        if (GUILayout.Button("Select All Spheres"))
        {
            var allSphereBehaviors = FindObjectsOfType<Sphere>();
            var allSphereGOs = allSphereBehaviors
                .Select(enemy => enemy.gameObject)
                .ToArray();
            Selection.objects = allSphereGOs;
        }

        if (GUILayout.Button("Clear Selection")) Selection.objects = null;

        EditorGUILayout.EndHorizontal();

        bool SelectedIsEnabled = Selection.activeGameObject != null && Selection.activeGameObject.activeSelf;

        var defaultColor = GUI.backgroundColor;
        GUI.backgroundColor = SelectedIsEnabled ? Color.red : Color.green;

        if (GUILayout.Button("Disable/Enable all enemy", GUILayout.Height(40)))
        {
            foreach (var enemy in GameObject.FindObjectsOfType<Sphere>(true))
            {
                Undo.RecordObject(enemy.gameObject, "Disable/Enable enemy");
                enemy.gameObject.SetActive(!SelectedIsEnabled);
            }

        }

        GUI.backgroundColor = defaultColor;
    }
}

#endif