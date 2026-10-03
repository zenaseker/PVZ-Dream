using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static Attribute;
using static UnityEngine.GraphicsBuffer;
using static UnityEngine.UI.CanvasScaler;

[CustomEditor(typeof(ZombieInfos))]
public class LevelObject2 : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUILayout.Button("Open Editor"))
        {
            ZombieInfoEditorWindow.Open((ZombieInfos)target);
        }
    }
}
public class ZombieInfoEditorWindow : EditorWindow
{
    protected SerializedObject serializedObject;
    protected SerializedProperty serializedProperty;
    protected ZombieInfos zombieinfo;

    private string selectedPropertyPath;
    protected SerializedProperty selectedProperty;
    protected Vector2 scrollview;
    public static void Open(ZombieInfos levelInfo)
    {
        ZombieInfoEditorWindow window = PlantInfoEditorWindow.GetWindow<ZombieInfoEditorWindow>("ZombieInfoEditorWindow");
        window.ReInit(levelInfo);
        window.zombieinfo = levelInfo;
    }
    [MenuItem("Window/ZombieInfoEditorWindow")]
    public static void ShowWindow()
    {
        Open(Resources.Load<ZombieInfos>("ScriptableObject/ZombieInfo"));
    }
    public void ReInit(ZombieInfos levelInfo)
    {
        serializedObject = new SerializedObject(levelInfo);
        serializedProperty = serializedObject.FindProperty("ZombieInfoes");
    }
    void OnGUI()
    {
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
        DrawProperties(serializedProperty, true);
        EditorGUILayout.EndVertical();
        EditorGUILayout.EndHorizontal();
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
            SerializedProperty zombieDescribe = p.FindPropertyRelative("zombieDescribe");
            if (GUILayout.Button(zombieDescribe.FindPropertyRelative("Name").stringValue))
            {
                selectedPropertyPath = p.displayName;
                selectedProperty = p;
                zombieDescribe.isExpanded = true;
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
