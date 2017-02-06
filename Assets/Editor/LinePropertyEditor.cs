using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

[CustomPropertyDrawer(typeof(LineCurve))]
public class LinePropertyEditor : PropertyDrawer {

    private static SerializedProperty myLineCurve;
    private static UnityEngine.Object target;

    static LinePropertyEditor()
    {
        SceneView.onSceneGUIDelegate += OnSceneGUI;
    }

    private bool onOpen = false;

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return ((property.FindPropertyRelative("points").arraySize * 2) + 1) * EditorGUIUtility.singleLineHeight;
    }

    public override void OnGUI (Rect position, SerializedProperty property, GUIContent label)
    {
        
        if (!onOpen)
        { 
            onOpen = true;
            myLineCurve = property;
            target = myLineCurve.serializedObject.targetObject;
            SceneView.RepaintAll();
        }

       

        EditorGUI.PrefixLabel(position, label);
        position.x += 30;
        position.width -= 30;
        position.height = EditorGUIUtility.singleLineHeight;
        EditorGUI.LabelField(position, "Size: " + sizeOfCurve());
        position.y += position.height;

        while (sizeOfCurve() - property.FindPropertyRelative("speed").arraySize > 1)
        {
            property.FindPropertyRelative("speed").InsertArrayElementAtIndex(property.FindPropertyRelative("speed").arraySize);
            property.FindPropertyRelative("speed")
                .GetArrayElementAtIndex(property.FindPropertyRelative("speed").arraySize - 1)
                .floatValue = 1f;
        }

        
        for (int i = 0; i < sizeOfCurve(); ++i)
        {
            EditorGUI.PropertyField(position, getPointSerialized(i), new GUIContent("Point: " + (i + 1).ToString()));
            position.y += position.height;
            if (i + 1 < sizeOfCurve())
            {
                EditorGUI.PropertyField(position, property.FindPropertyRelative("speed").GetArrayElementAtIndex(i),
                    new GUIContent("Speed: "));
                position.y += position.height;
            }
        }
    


        position.width /= 2;
        if (GUI.Button(position, "Add"))
        {
            addPointToCurve();
        }
        position.x += position.width;

        if (GUI.Button(position, "Remove"))
        {
            removePointFromCurve();
        }
    }

    private static SerializedProperty getPointSerialized(int i)
    {
        return myLineCurve.FindPropertyRelative("points").GetArrayElementAtIndex(i);
    }

    private static Vector3 getPoint(int i)
    {
        return getPointSerialized(i).vector3Value;
    }

    private static void setPoint(int i, Vector3 newPos)
    {
        myLineCurve.FindPropertyRelative("points").GetArrayElementAtIndex(i).vector3Value = newPos;
    }
        
    private static void addPointToCurve()
    {
        int size = sizeOfCurve();
        myLineCurve.FindPropertyRelative("points").InsertArrayElementAtIndex(size);
        if (size >= 1)
        {
            setPoint(size, getPoint(size - 1));
            myLineCurve.FindPropertyRelative("speed").InsertArrayElementAtIndex(size - 1);
        }
        
    }

    private static void removePointFromCurve()
    {
        int size = sizeOfCurve();
        if (size > 0)
        {
            myLineCurve.FindPropertyRelative("points").DeleteArrayElementAtIndex(size - 1);
            if (size > 1)
            {
                myLineCurve.FindPropertyRelative("speed").DeleteArrayElementAtIndex(size - 2);
            }
        }
    }

    private static int sizeOfCurve()
    {
        return myLineCurve.FindPropertyRelative("points").arraySize;
    }

    private static void OnSceneGUI(SceneView scene)
    {
        if (myLineCurve != null)
        {
            Vector3 p;
            Handles.color = Color.green;
            try
            {
                myLineCurve.serializedObject.Update();
                if (sizeOfCurve() >= 1)
                {
                    p = getPoint(0);
                    EditorGUI.BeginChangeCheck();
                    
                    p = Handles.DoPositionHandle(p, Quaternion.identity);
                    setPoint(0, p); 
                    if (EditorGUI.EndChangeCheck())
                    {
                        Undo.RecordObject(target, "Move Point");
                        EditorUtility.SetDirty(target);
                        myLineCurve.serializedObject.ApplyModifiedProperties();
                    }
                }

                for (int i = 1; i < sizeOfCurve(); ++i)
                {
                    Handles.DrawLine(getPoint(i - 1), getPoint(i));
                    p = getPoint(i);
                    EditorGUI.BeginChangeCheck();
                    
                    p = Handles.DoPositionHandle(p, Quaternion.identity);
                    setPoint(i, p);
                    if (EditorGUI.EndChangeCheck())
                    {
                        Undo.RecordObject(target, "Move Point");
                        EditorUtility.SetDirty(target);
                        myLineCurve.serializedObject.ApplyModifiedProperties();
                    }

                }
            } catch (Exception exception)
            {
                myLineCurve = null;
            }
        }
    }
}
