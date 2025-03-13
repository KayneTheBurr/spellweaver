using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using Unity.VisualScripting;

[CustomEditor(typeof(Enemy))]
public class EnemyEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector(); // Draws the default inspector first

        Enemy enemy = (Enemy)target;

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Active Status Effects", EditorStyles.boldLabel);

        if (enemy.activeEffects.Count == 0)
        {
            EditorGUILayout.LabelField("None");
        }
        else
        {
            foreach (StatusEffect effect in enemy.activeEffects)
            {
                EditorGUILayout.LabelField(effect.GetType().Name);
            }
        }

        serializedObject.ApplyModifiedProperties();
    }
}
