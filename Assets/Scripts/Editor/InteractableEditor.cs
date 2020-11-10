using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Interactable), true)]
public class InteractableEditor : Editor
{
    public override void OnInspectorGUI()
    {
        Interactable script = (Interactable)target;

        script.clickable = EditorGUILayout.Toggle("Clickable", script.clickable);
        if (script.clickable)
        {
            script.clickStressValue = EditorGUILayout.FloatField("Click Stress Value:", script.clickStressValue);
            script.clickCompletionValue = EditorGUILayout.FloatField("Click Completion Value:", script.clickCompletionValue);
        }

        script.holdable = EditorGUILayout.Toggle("Holdable", script.holdable);
        if (script.holdable)
        {
            script.holdStressValue = EditorGUILayout.FloatField("Hold Stress Value:", script.holdStressValue);
            script.holdCompletionValue = EditorGUILayout.FloatField("Hold Completion Value:", script.holdCompletionValue);
        }

        script.lookable = EditorGUILayout.Toggle("Lookable", script.lookable);
        if (script.lookable)
        {
            script.lookStressValue = EditorGUILayout.FloatField("Look Stress Value:", script.lookStressValue);
            script.lookCompletionValue = EditorGUILayout.FloatField("Look Completion Value:", script.lookCompletionValue);
        }

        script.draggable = EditorGUILayout.Toggle("Draggable", script.draggable);
        if (script.draggable)
        {
            script.dragStressValue = EditorGUILayout.FloatField("Drag Stress Value:", script.dragStressValue);
            script.dragCompletionValue = EditorGUILayout.FloatField("Drag Completion Value:", script.dragCompletionValue);
        }

        DrawDefaultInspector();
    }
}
