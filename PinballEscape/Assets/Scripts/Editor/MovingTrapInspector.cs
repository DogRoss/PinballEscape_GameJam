using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MovingTrap))]
public class MovingTrapInspector : Editor
{
    Vector3 endPos;
    Vector3 endRot;
    float startOffset;
    float extendSpeed;
    float waitTime;
    float retractSpeed;
    float loopTime;
    bool rotate;

    MovingTrap trap;

    public override void OnInspectorGUI()
    {
        trap = (MovingTrap)target;

        EditorGUILayout.LabelField("Position Info", EditorStyles.boldLabel);

        // set whether move or rotate
        EditorGUI.BeginChangeCheck();
        rotate = EditorGUILayout.Toggle(new GUIContent("Rotate", "Click if trap is rotating rather than moving"), trap.rotate);
        if (EditorGUI.EndChangeCheck()) //returns true if editor changes
        {
            Undo.RecordObject(trap, "Change Rotate");
            trap.rotate = rotate;
            EditorUtility.SetDirty(trap);
        }

        if (trap.rotate)
        {
            // set end rotation
            EditorGUI.BeginChangeCheck();
            endRot = EditorGUILayout.Vector3Field("End Rotation", trap.endRot.eulerAngles);
            if (EditorGUI.EndChangeCheck()) //returns true if editor changes
            {
                Undo.RecordObject(trap, "Change End Rotation");
                trap.endRot = Quaternion.Euler(endRot);
                EditorUtility.SetDirty(trap);
            }
        } else
        {
            // set end position
            EditorGUI.BeginChangeCheck();
            endPos = EditorGUILayout.Vector3Field("End Position", trap.endPos);
            if (EditorGUI.EndChangeCheck()) //returns true if editor changes
            {
                Undo.RecordObject(trap, "Change End Position");
                trap.endPos = endPos;
                EditorUtility.SetDirty(trap);
            }
        }

        EditorGUILayout.LabelField("Movement Info", EditorStyles.boldLabel);

        // set start offset
        EditorGUI.BeginChangeCheck();
        startOffset = EditorGUILayout.FloatField(new GUIContent("Start Offset", "Time until trap extends"), trap.startOffset);
        if (EditorGUI.EndChangeCheck()) //returns true if editor changes
        {
            Undo.RecordObject(trap, "Change Extend Speed");
            trap.startOffset = startOffset;
            EditorUtility.SetDirty(trap);
        }

        // set extend speed
        EditorGUI.BeginChangeCheck();
        extendSpeed = EditorGUILayout.FloatField(new GUIContent("Extend Speed", "How fast the trap will extend"), trap.extendSpeed);
        if (EditorGUI.EndChangeCheck()) //returns true if editor changes
        {
            Undo.RecordObject(trap, "Change Extend Speed");
            trap.extendSpeed = extendSpeed;
            EditorUtility.SetDirty(trap);
        }

        // set wait time
        EditorGUI.BeginChangeCheck();
        waitTime = EditorGUILayout.FloatField(new GUIContent("Wait Time", "How long the trap will wait to retract"), trap.waitTime);
        if (EditorGUI.EndChangeCheck()) //returns true if editor changes
        {
            Undo.RecordObject(trap, "Change Wait Time");
            trap.waitTime = waitTime;
            EditorUtility.SetDirty(trap);
        }

        // set retract speed
        EditorGUI.BeginChangeCheck();
        retractSpeed = EditorGUILayout.FloatField(new GUIContent("Retract Speed", "How fast the trap will retract"), trap.retractSpeed);
        if (EditorGUI.EndChangeCheck()) //returns true if editor changes
        {
            Undo.RecordObject(trap, "Change Retract Speed");
            trap.retractSpeed = retractSpeed;
            EditorUtility.SetDirty(trap);
        }

        // set total loop time
        EditorGUI.BeginChangeCheck();
        loopTime = EditorGUILayout.FloatField(new GUIContent("Loop Time", "Total time for single loop"), trap.loopTime);
        if (EditorGUI.EndChangeCheck()) //returns true if editor changes
        {
            Undo.RecordObject(trap, "Change Loop Time");
            loopTime = Mathf.Max(trap.startOffset + trap.extendSpeed + trap.waitTime + trap.retractSpeed, loopTime);
            trap.loopTime = loopTime;
            EditorUtility.SetDirty(trap);
        }
    }
}
