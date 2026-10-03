using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;
public class AssetHandler
{
    [OnOpenAsset]
    public static bool OpenEditor(int instanceId, int line)
    {
        PlantInfos obj = EditorUtility.InstanceIDToObject(instanceId) as PlantInfos;
        if (obj != null)
        {
            PlantInfoEditorWindow.Open(obj);
            return true;
        }
        return false;
    }
    [OnOpenAsset]
    public static bool OpenEditor2(int instanceId, int line)
    {
        ZombieInfos obj = EditorUtility.InstanceIDToObject(instanceId) as ZombieInfos;
        if (obj != null)
        {
            ZombieInfoEditorWindow.Open(obj);
            return true;
        }
        return false;
    }
}

[CustomEditor(typeof(PlantInfos))]
public class LevelObject : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUILayout.Button("Open Editor"))
        {
            PlantInfoEditorWindow.Open((PlantInfos)target);
        }
    }
}
public class PlantInfoEditorWindow : EditorWindow
{
    protected SerializedObject serializedObject;
    protected SerializedProperty serializedProperty;
    protected PlantInfos plantInfo;

    private string selectedPropertyPath;
    protected SerializedProperty selectedProperty;
    protected Vector2 scrollview;
    public static void Open(PlantInfos levelInfo)
    {
        PlantInfoEditorWindow window = PlantInfoEditorWindow.GetWindow<PlantInfoEditorWindow>("PlantInfoEditorWindow");
        window.ReInit(levelInfo);
        window.plantInfo = levelInfo;
    }
    [MenuItem("Window/PlantInfoEditorWindow")]
    public static void ShowWindow()
    {
        Open(Resources.Load<PlantInfos>("ScriptableObject/PlantInfo"));
    }
    public void ReInit(PlantInfos levelInfo)
    {
        serializedObject = new SerializedObject(levelInfo);
        serializedProperty = serializedObject.FindProperty("PlantInfoes");
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
            SerializedProperty plantDescribe = p.FindPropertyRelative("plantDescribe");
            if (GUILayout.Button(plantDescribe.FindPropertyRelative("Name").stringValue))
            {
                selectedPropertyPath = p.displayName;
                selectedProperty = p;
                plantDescribe.isExpanded = true;
            }
        }
    }
    protected void DrawSiderbarTatal(SerializedProperty prop)
    {
        if (prop is null) return;
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.PropertyField(prop);
        EditorGUILayout.EndHorizontal();
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




 