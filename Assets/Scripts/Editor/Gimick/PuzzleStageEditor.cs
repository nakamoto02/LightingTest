using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using Lumiere.Gimick.Lamp;

namespace Lumiere.Gimick
{
    [CustomEditor(typeof(PuzzleStage))]
    public class PuzzleStageEditor : Editor
    {
        public PuzzleStage Stage { get { return target as PuzzleStage; } }

        SerializedProperty connectObj;
        SerializedProperty startLamp;
        SerializedProperty endLamp;
        ReorderableList    reorderableList;

        bool isArea = false;
        bool isGimick = false;

        void OnEnable()
        {
            var list = serializedObject.FindProperty("stageLampAry");

            reorderableList = new ReorderableList(serializedObject, list);
            reorderableList.drawElementCallback = (rect, index, isActive, isFocused) => {
                var element = list.GetArrayElementAtIndex(index);
                rect.height -= 4;
                rect.y += 2;
                EditorGUI.PropertyField(rect, element);
            };

            connectObj = serializedObject.FindProperty("connectObj");
            startLamp  = serializedObject.FindProperty("startLamp");
            endLamp    = serializedObject.FindProperty("endLamp");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.LabelField(Stage.AreaState.ToString());
            EditorGUILayout.Space();

            // Area
            if(isArea = EditorGUILayout.Foldout(isArea, "Area"))
            {
                EditorGUI.indentLevel++;

                Stage.areaPosition = EditorGUILayout.Vector3Field("Position", Stage.areaPosition);
                Stage.areaSize     = EditorGUILayout.Vector3Field("Size",     Stage.areaSize);
                Stage.areaPivot    = EditorGUILayout.Vector3Field("Pivot",    Stage.areaPivot);

                EditorGUI.indentLevel--;
            }

            EditorGUILayout.Space();
            // LineFirefly
            EditorGUILayout.PropertyField(connectObj, new GUIContent("LineFirefly"));
            // Start & End Lamp
            EditorGUILayout.PropertyField(startLamp,  new GUIContent("StartLamp"));
            EditorGUILayout.PropertyField(endLamp,    new GUIContent("EndLamp"));
            // StageLamp
            reorderableList.DoLayoutList();

            serializedObject.ApplyModifiedProperties();
        }
    }
}


