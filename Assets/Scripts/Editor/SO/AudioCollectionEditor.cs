using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(AudioCollection))]
public sealed class AudioCollectionEditor : Editor
{
    private SerializedProperty _audioClips;

    private SerializedProperty _outputAudioGroup;

    private SerializedProperty _volume1MeterAway;
    private SerializedProperty _volume1MeterAwayRandomness;

    private SerializedProperty _priority;

    private SerializedProperty _spatialBlend;
    private SerializedProperty _spatialBlendRandomness;

    private SerializedProperty _pitch;
    private SerializedProperty _pitchRandomness;

    private void OnEnable()
    {
        _audioClips = serializedObject.FindProperty("_audioClips");

        _outputAudioGroup = serializedObject.FindProperty("_outputAudioGroup");

        _volume1MeterAway = serializedObject.FindProperty("_volume1MeterAway");
        _volume1MeterAwayRandomness = serializedObject.FindProperty("_volume1MeterAwayRandomness");

        _priority = serializedObject.FindProperty("_priority");

        _spatialBlend = serializedObject.FindProperty("_spatialBlend");
        _spatialBlendRandomness = serializedObject.FindProperty("_spatialBlendRandomness");

        _pitch = serializedObject.FindProperty("_pitch");
        _pitchRandomness = serializedObject.FindProperty("_pitchRandomness");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(_audioClips, new GUIContent("Audio clips"));

        EditorGUILayout.Separator();
        EditorGUILayout.PropertyField(_outputAudioGroup, new GUIContent("Output audio group"));

        EditorGUILayout.Separator();
        EditorGUILayout.Slider(_volume1MeterAway, 0f, 1f, "Volume 1 meter away");
        EditorGUILayout.Slider(_volume1MeterAwayRandomness, 0f, 1f, "Randomness");

        EditorGUILayout.Separator();
        EditorGUILayout.IntSlider(_priority, byte.MinValue, byte.MaxValue, "Priority");

        EditorGUILayout.Separator();
        EditorGUILayout.Slider(_spatialBlend, 0f, 1f, "Spatial blend");
        EditorGUILayout.Slider(_spatialBlendRandomness, 0f, 1f, "Randomness");

        EditorGUILayout.Separator();
        EditorGUILayout.Slider(_pitch, AudioCollection.MinPitch, AudioCollection.MaxPitch, "Pitch");
        EditorGUILayout.Slider(_pitchRandomness, 0f, 1f, "Randomness");

        serializedObject.ApplyModifiedProperties();
    }
}
