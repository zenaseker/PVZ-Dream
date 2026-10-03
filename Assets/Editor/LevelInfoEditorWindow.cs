using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(LevelInfos))]
public class LevelObject3 : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUILayout.Button("Open Editor"))
        {
            LevelInfoEditorWindow.Open((LevelInfos)target);
        }
    }
}
public class LevelInfoEditorWindow : EditorWindow
{
    protected SerializedObject serializedObject;
    protected SerializedProperty serializedProperty;
    protected LevelInfos plantInfo;

    private string selectedPropertyPath;
    protected SerializedProperty selectedProperty;
    protected Vector2 scrollview;
    protected Vector2 windowscrollview;
    protected LevelType showleveltype = LevelType.Other;
    public static void Open(LevelInfos levelInfo)
    {
        LevelInfoEditorWindow window = PlantInfoEditorWindow.GetWindow<LevelInfoEditorWindow>("LevelInfoEditorWindow");
        window.ReInit(levelInfo);
        window.plantInfo = levelInfo;
    }
    [MenuItem("Window/LevelInfoEditorWindow")]
    public static void ShowWindow()
    {
        Open(Resources.Load<LevelInfos>("ScriptableObject/LevelInfo"));
    }
    public void ReInit(LevelInfos levelInfo)
    {
        serializedObject = new SerializedObject(levelInfo);
        serializedProperty = serializedObject.FindProperty("LevelInfoes");
    }
    void OnGUI()
    {
        EditorGUILayout.BeginVertical();
        EditorGUILayout.BeginVertical("box", GUILayout.MaxHeight(50), GUILayout.ExpandWidth(true));
        showleveltype = (LevelType)EditorGUILayout.EnumPopup("œ‘ æ…∏—°πÿø®", showleveltype);
        EditorGUILayout.EndVertical();
        EditorGUILayout.BeginHorizontal();
        scrollview = EditorGUILayout.BeginScrollView(scrollview, GUILayout.MaxWidth(150));
        EditorGUILayout.BeginVertical("box", GUILayout.MaxWidth(150), GUILayout.ExpandHeight(true));
        DrawSidebar(serializedProperty);
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("+"))
        {
            serializedProperty.InsertArrayElementAtIndex(serializedProperty.arraySize);
        }
        if (GUILayout.Button("-"))
        {
            serializedProperty.DeleteArrayElementAtIndex(serializedProperty.arraySize - 1);
        }
        EditorGUILayout.EndScrollView();
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.EndVertical();
        EditorGUILayout.BeginVertical("box", GUILayout.ExpandHeight(true));
        windowscrollview = EditorGUILayout.BeginScrollView(windowscrollview);
        if (serializedProperty != null)
        {
            DrawProperties(serializedProperty, true);
        }
        EditorGUILayout.EndScrollView();
        EditorGUILayout.EndVertical();
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.EndVertical();
        serializedObject.ApplyModifiedProperties();
    }
    protected void DrawProperties(SerializedProperty prop, bool drawChildren)
    {
        foreach (SerializedProperty p in prop)
        {
            if (p.displayName == selectedPropertyPath)
            {
                p.isExpanded = true;
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.PropertyField(p, drawChildren);
                EditorGUILayout.EndHorizontal();
            }
        }
    }
    protected void DrawSidebar(SerializedProperty prop)
    {
        if (prop is null) return;
        foreach (SerializedProperty p in prop)
        {
            if (p.FindPropertyRelative("Type").enumValueIndex != (int)showleveltype)
            {
                continue;
            }
            if (GUILayout.Button(p.FindPropertyRelative("Name").stringValue))
            {
                selectedPropertyPath = p.displayName;
                selectedProperty = p;
                p.isExpanded = true;
            }
        }
    }
    public void OnDestroy()
    {
        foreach (SerializedProperty p in serializedProperty)
        {
            p.isExpanded = false;
        }
        serializedObject.ApplyModifiedProperties();
    }
}
