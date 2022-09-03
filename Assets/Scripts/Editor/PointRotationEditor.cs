﻿using UnityEditor;

[CanEditMultipleObjects]
[CustomEditor(typeof(PointRotation))]
public class PointRotationEditor : Editor
{
    private PointRotation pointRotation;

    private SerializedProperty targetType;
    private SerializedProperty usePlayerAsTarget;
    private SerializedProperty rotationTarget;
    private SerializedProperty useLocalPos;
    private SerializedProperty offset;
    private SerializedProperty coefficient;
    private SerializedProperty angle;

    private void OnEnable()
    {
        pointRotation = target as PointRotation;
        {
            targetType = serializedObject.FindProperty("targetType");
            usePlayerAsTarget = serializedObject.FindProperty("usePlayerAsTarget");
            rotationTarget = serializedObject.FindProperty("target");
            useLocalPos = serializedObject.FindProperty("useLocalPosition");
            offset = serializedObject.FindProperty("offset");
            coefficient = serializedObject.FindProperty("coefficient");
            angle = serializedObject.FindProperty("angle");
        }
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        {
            //Settings
            EditorGUILayout.PropertyField(targetType);
            EditorGUILayout.PropertyField(useLocalPos);

            if (pointRotation.TargetType == PointRotationTargetType.Other)
            {
                EditorGUILayout.PropertyField(usePlayerAsTarget);
                if(!pointRotation.UsePlayerAsTarget) EditorGUILayout.PropertyField(rotationTarget);
            }

            //Info
            EditorGUILayout.PropertyField(offset);
            EditorGUILayout.PropertyField(coefficient);
            EditorGUILayout.PropertyField(angle);
        }
        serializedObject.ApplyModifiedProperties();
    }
}