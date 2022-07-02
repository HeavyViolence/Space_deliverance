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

        _cameraShakeOnCollisionEnabled = serializedObject.FindProperty("_cameraShakeOnCollisionEnabled");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.Slider(_horizontalSpeed, 0f, MovementConfig.MaxSpeed, "Horizontal Speed");
        EditorGUILayout.Slider(_horizontalSpeedRandomness, 0f, 1f, "Randomness");

        EditorGUILayout.Separator();
        EditorGUILayout.Slider(_verticalSpeed, 0f, MovementConfig.MaxSpeed, "Vertical Speed");
        EditorGUILayout.Slider(_verticalSpeedRandomness, 0f, 1f, "Randomness");

        EditorGUILayout.Separator();
        EditorGUILayout.PropertyField(_customMovementBoundsEnabled, new GUIContent("Enable Custom Movement Bounds"));

        if (_customMovementBoundsEnabled.boolValue)
        {
            EditorGUILayout.Slider(_upperBoundDisplacementFactor,
                                   MovementConfig.MinBoundsDisplacementFactor,
                                   MovementConfig.MaxBoundsDisplacementFactor,
                                   "Upper Bound Displacement Factor");

            EditorGUILayout.Slider(_lowerBoundDisplacementFactor,
                                   MovementConfig.MinBoundsDisplacementFactor,
                                   MovementConfig.MaxBoundsDisplacementFactor,
                                   "Lower Bound Displacement Factor");

            EditorGUILayout.Slider(_verticalBoundsDisplacementFactor,
                                   MovementConfig.MinBoundsDisplacementFactor,
                                   MovementConfig.MaxBoundsDisplacementFactor,
                                   "Left & Right Bounds Displacement Factor");
        }

        EditorGUILayout.Separator();
        EditorGUILayout.PropertyField(_collisionDamageEnabled, new GUIContent("Enable Collision Damage"));

        if (_collisionDamageEnabled.boolValue)
        {
            EditorGUILayout.Slider(_collisionDamage, MovementConfig.MinCollisionDamage, MovementConfig.MaxCollisionDamage, "Damage");
            EditorGUILayout.Slider(_collisionDamageRandomness, 0f, 1f, "Randomness");
        }

        EditorGUILayout.Separator();
        EditorGUILayout.PropertyField(_cameraShakeOnCollisionEnabled, new GUIContent("Enable Camera Shake On Collision"));

        serializedObject.ApplyModifiedProperties();
    }
}
