using Codice.CM.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class ShowPlantMesh : EditorWindow
{
    public Vector2 scrollview;
    public int scale = 150;
    public Dictionary<int, string> names = new Dictionary<int, string>();
    [MenuItem("Window/ShowPlantMesh")]
    public static void ShowWindow()
    {
        ShowPlantMesh showPlantMesh = ShowPlantMesh.GetWindow<ShowPlantMesh>("ShowPlantMesh");
        showPlantMesh.names.Add(0, "[空]");
    }
    void OnGUI()
    {
        EditorGUILayout.BeginVertical("box", GUILayout.ExpandHeight(true), GUILayout.ExpandHeight(true));
        if (MapManage.Instance == null)
        {
            GUILayout.Label("地图组件未加载或不存在");
            EditorGUILayout.EndVertical();
            return;
        }
        scale = EditorGUILayout.IntSlider("缩放大小",scale, 0, 300);
        scrollview = EditorGUILayout.BeginScrollView(scrollview, GUILayout.ExpandHeight(true), GUILayout.ExpandHeight(true));

        EditorGUILayout.BeginVertical();
        for (int i = MapManage.Instance.meshxy.x - 1; i >= 0; i--)
        {
            EditorGUILayout.BeginHorizontal();
            for (int j = 0; j < MapManage.Instance.meshxy.y - 1; j++)
            {
                DrawOneMesh(MapManage.Instance.meshPlants[i, j]);
            }
            EditorGUILayout.EndHorizontal();
        }
        EditorGUILayout.EndVertical();


        EditorGUILayout.EndScrollView();

        EditorGUILayout.EndVertical();
    }

    void DrawOneMesh(MapMeshPlant mapMeshPlant)
    {
        EditorGUILayout.BeginVertical("box", GUILayout.MaxWidth(scale), GUILayout.MaxHeight(scale));

        EditorGUILayout.BeginHorizontal("box", GUILayout.MaxWidth(scale / 3), GUILayout.MaxHeight(scale / 3));

        GUILayout.Label("", GUILayout.Width(scale / 3), GUILayout.Height(scale / 3));
        GetName(mapMeshPlant, PlantPosType.Top);
        GUILayout.Label("", GUILayout.Width(scale / 3), GUILayout.Height(scale / 3));

        EditorGUILayout.EndHorizontal();
        EditorGUILayout.BeginHorizontal("box", GUILayout.MaxWidth(scale / 3), GUILayout.MaxHeight(scale / 3));

        GetName(mapMeshPlant, PlantPosType.Shell);
        GetName(mapMeshPlant, PlantPosType.Default);
        GUILayout.Label("", GUILayout.Width(scale / 3), GUILayout.Height(scale / 3));

        EditorGUILayout.EndHorizontal();
        EditorGUILayout.BeginHorizontal("box", GUILayout.MaxWidth(scale / 3), GUILayout.MaxHeight(scale / 3));

        GUILayout.Label("", GUILayout.Width(scale / 3), GUILayout.Height(scale / 3));
        GetName(mapMeshPlant, PlantPosType.Base);
        GetName(mapMeshPlant, PlantPosType.Little);

        EditorGUILayout.EndHorizontal();

        EditorGUILayout.EndVertical();
    }
    void GetName(MapMeshPlant mapMeshPlant, PlantPosType plantPosType)
    {
        PlantBase plantb = mapMeshPlant.GetPlant(plantPosType)?.GetComponent<PlantBase>();
        if (plantb != null)
        {
            int id = plantb.unitInfo.ID;
            if (!names.ContainsKey(id))
            {
                names.Add(id, ((Attribute.PlantInfo)plantb.unitInfo).plantDescribe.Name);
            }
            GUILayout.Label(names[id], GUILayout.Width(scale / 3), GUILayout.Height(scale / 3));
        }
        else
        {
            GUILayout.Label(names[0], GUILayout.Width(scale / 3), GUILayout.Height(scale / 3));
        }
    }
}
