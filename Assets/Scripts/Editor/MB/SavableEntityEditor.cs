using System;
using UnityEditor;

[CustomEditor(typeof(SavableEntity))]
public sealed class SavableEntityEditor : Editor
{
    private SerializedProperty _id;

    private void OnEnable()
    {
        _id = serializedObject.FindProperty("_id");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.LabelField($"ID: {_id.stringValue}");

        if (string.IsNullOrEmpty(_id.stringValue))
        {
            _id.stringValue = Guid.NewGuid().ToString();
        }

        serializedObject.ApplyModifiedProperties();
    }
}