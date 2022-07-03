using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MovementConfig))]
public class MovementConfigEditor : Editor
{
    private SerializedProperty _horizontalSpeed;
    private SerializedProperty _horizontalSpeedRandomness;

    private SerializedProperty _verticalSpeed;
    private SerializedProperty _verticalSpeedRandomness;

    private SerializedProperty _customMovementBoundsEnabled;
    private SerializedProperty _upperBoundDisplacementFactor;
    private SerializedProperty _lowerBoundDisplacementFactor;
    private SerializedProperty _verticalBoundsDisplacementFactor;

    private SerializedProperty _collisionDamageEnabled;
    private SerializedProperty _collisionDamage;
    private SerializedProperty _collisionDamageRandomness;
    private SerializedProperty _collisionAudio;
    private SerializedProperty _cameraShakeOnCollisionEnabled;

    protected virtual void OnEnable()
    {
        _horizontalSpeed = serializedObject.FindProperty("_horizontalSpeed");
        _horizontalSpeedRandomness = serializedObject.FindProperty("_horizontalSpeedRandomness");

        _verticalSpeed = serializedObject.FindProperty("_verticalSpeed");
        _verticalSpeedRandomness = serializedObject.FindProperty("_verticalSpeedRandomness");

        _customMovementBoundsEnabled = serializedObject.FindProperty("_customMovementBoundsEnabled");
        _upperBoundDisplacementFactor = serializedObject.FindProperty("_upperBoundDisplacementFactor");
        _lowerBoundDisplacementFactor = serializedObject.FindProperty("_lowerBoundDisplacementFactor");
        _verticalBoundsDisplacementFactor = serializedObject.FindProperty("_verticalBoundsDisplacementFactor");

        _collisionDamageEnabled = serializedObject.FindProperty("_collisionDamageEnabled");
        _collisionDamage = serializedObject.FindProperty("_collisionDamage");
        _collisionDamageRandomness = serializedObject.FindProperty("_collisionDamageRandomness");
        _collisionAudio = serializedObject.FindProperty("_collisionAudio");
        _cameraShakeOnCollisionEnabled = serializedObject.FindProperty("_cameraShakeOnCollisionEnabled");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.Slider(_horizontalSpeed, 0f, MovementConfig.MaxSpeed, "Horizontal speed");
        EditorGUILayout.Slider(_horizontalSpeedRandomness, 0f, 1f, "Randomness");

        EditorGUILayout.Separator();
        EditorGUILayout.Slider(_verticalSpeed, 0f, MovementConfig.MaxSpeed, "Vertical speed");
        EditorGUILayout.Slider(_verticalSpeedRandomness, 0f, 1f, "Randomness");

        EditorGUILayout.Separator();
        EditorGUILayout.PropertyField(_customMovementBoundsEnabled, new GUIContent("Enable custom movement bounds"));

        if (_customMovementBoundsEnabled.boolValue)
        {
            EditorGUILayout.Slider(_upperBoundDisplacementFactor,
                                   MovementConfig.MinBoundsDisplacementFactor,
                                   MovementConfig.MaxBoundsDisplacementFactor,
                                   "Upper bound displacement factor");

            EditorGUILayout.Slider(_lowerBoundDisplacementFactor,
                                   MovementConfig.MinBoundsDisplacementFactor,
                                   MovementConfig.MaxBoundsDisplacementFactor,
                                   "Lower bound displacement factor");

            EditorGUILayout.Slider(_verticalBoundsDisplacementFactor,
                                   MovementConfig.MinBoundsDisplacementFactor,
                                   MovementConfig.MaxBoundsDisplacementFactor,
                                   "Left & right bounds displacement factor");
        }

        EditorGUILayout.Separator();
        EditorGUILayout.PropertyField(_collisionDamageEnabled, new GUIContent("Enable collision damage"));

        if (_collisionDamageEnabled.boolValue)
        {
            EditorGUILayout.Slider(_collisionDamage, MovementConfig.MinCollisionDamage, MovementConfig.MaxCollisionDamage, "Damage");
            EditorGUILayout.Slider(_collisionDamageRandomness, 0f, 1f, "Randomness");

            EditorGUILayout.PropertyField(_collisionAudio, new GUIContent("Collision audio"));

            EditorGUILayout.PropertyField(_cameraShakeOnCollisionEnabled, new GUIContent("Enable camera shake on collision"));
        }

        serializedObject.ApplyModifiedProperties();
    }
}
