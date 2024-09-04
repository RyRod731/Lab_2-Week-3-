using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.EventSystems;

public class Cube : MonoBehaviour
{
    public float size = 1f;
}

#if UNITY_EDITOR

[CustomEditor(typeof(Cube)), CanEditMultipleObjects]
public class Cube_Editor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        var size = serializedObject.FindProperty("size");

        if (GUI.changed)
        {
            foreach (var cube in GameObject.FindObjectsOfType<Cube>(true))
            {
                Undo.RecordObject(cube.gameObject, "Size Cube Objects");
                Undo.RecordObject(cube.transform, "Size Cube Transforms");
                cube.gameObject.transform.localScale = size.floatValue * Vector3.one;
            }
        }

        if (size.floatValue < 1f)
            EditorGUILayout.HelpBox("The cubes' sizes cannot be smaller than 1", MessageType.Warning);
        if (size.floatValue > 2f)
            EditorGUILayout.HelpBox("The cubes' sizes cannot be bigger than 2", MessageType.Warning);
        
        EditorGUILayout.BeginHorizontal();

        if (GUILayout.Button("Select All Cubes"))
        {
            var allCubeBehaviors = FindObjectsOfType<Cube>();
            var allCubeGOs = allCubeBehaviors
                .Select(cube => cube.gameObject)
                .ToArray();
            Selection.objects = allCubeGOs;
        }

        if (GUILayout.Button("Clear Selection")) Selection.objects = null;

        EditorGUILayout.EndHorizontal();

        bool SelectedIsEnabled = Selection.activeGameObject != null && Selection.activeGameObject.activeSelf;

        var defaultColor = GUI.backgroundColor;
        GUI.backgroundColor = SelectedIsEnabled ? Color.red : Color.green;

        if (GUILayout.Button("Disable/Enable all enemy", GUILayout.Height(40)))
        {
            foreach (var cube in GameObject.FindObjectsOfType<Cube>(true))
            {
                Undo.RecordObject(cube.gameObject, "Disable/Enable Cubes");
                cube.gameObject.SetActive(!SelectedIsEnabled);
            }
            
        }

        GUI.backgroundColor = defaultColor;

    }
}

#endif