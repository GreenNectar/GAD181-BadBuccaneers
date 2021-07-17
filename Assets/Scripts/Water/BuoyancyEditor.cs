#if UNITY_EDITOR

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Buoyancy))]
public class BuoyancyEditor : Editor
{
    private void OnSceneGUI()
    {
        Buoyancy t = (Buoyancy)target;
        //Buoyancy target = (Buoyancy)serializedObject.targetObject;

        foreach (var position in t.positions)
        {
            if (position.isEditing)
            {
                Quaternion r = Quaternion.identity; // Dunno why, but I have to do this :|

                EditorGUI.BeginChangeCheck();

                Vector3 handlePos = t.transform.TransformPoint(position.position);
                Handles.TransformHandle(ref handlePos, ref r);

                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(t, "Changed buoyancy point position");
                    position.position = t.transform.InverseTransformPoint(handlePos);
                }
            }
        }
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        Buoyancy t = (Buoyancy)target;

        if (GUILayout.Button("Toggle Editing"))
        {
            bool turnOff = false;

            foreach (var position in t.positions)
            {
                if (position.isEditing)
                {
                    turnOff = true;
                    break;
                }
            }

            foreach (var position in t.positions)
            {
                position.isEditing = !turnOff;
            }
        }
    }
}

#endif