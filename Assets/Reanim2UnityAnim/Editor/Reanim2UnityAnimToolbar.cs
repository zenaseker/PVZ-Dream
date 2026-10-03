using JetBrains.Annotations;
using Reanim2UnityAnim.Editor.Data;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Reanim2UnityAnim.Editor
{
	public class Reanim2UnityAnimToolbar : EditorWindow
	{
		[CanBeNull]
		private Reanim2UnityAnimConfig data;
		private bool useparent = false;
        private bool pivot = false;

        [MenuItem("Window/Reanim2UnityAnim")]
		public static void ShowWindow()
		{
            Reanim2UnityAnimToolbar reanim2UnityAnimToolbar = GetWindow<Reanim2UnityAnimToolbar>("Reanim2UnityAnimToolbar");
			reanim2UnityAnimToolbar.data = AssetDatabase.LoadAssetAtPath<Reanim2UnityAnimConfig>("Assets/Reanim2UnityAnim/Reanim2UnityAnimConfig.asset");
		}

		private void SaveData()
		{
			EditorUtility.SetDirty(data);
			AssetDatabase.SaveAssets();
			AssetDatabase.Refresh();
		}

		private void OnGUI()
		{
			data = (Reanim2UnityAnimConfig)EditorGUILayout.ObjectField(data, typeof(Reanim2UnityAnimConfig), false);

			if (data == null)
			{
				GUILayout.Label("未选择配置文件。");
				return;
			}
			
			GUILayout.Label($"当前选择的reanim文件: \n{data.filePath}");

			// 文件夹选择按钮
			if (GUILayout.Button("选择文件"))
			{
				string path = EditorUtility.OpenFilePanelWithFilters("选择.reanim文件", Application.dataPath + "/Reanim2UnityAnim/reanim_all/", new[]
				{ "reanim", "Reanim" });
				if (!string.IsNullOrEmpty(path) && path.EndsWith(".reanim"))
				{
					data.filePath = path;
				}
				else
				{
					Debug.Log("未选择.reanim文件");
				}
			}

			data.center = EditorGUILayout.Vector2Field("中心", data.center);

			// 自定义分段
			if (GUILayout.Button("添加自定义分段"))
			{
				data.customPartitions.Add(new Partition("UnNamedPartition", 0, 0));
            }

            for (int i = 0; i < data.customPartitions.Count; i++)
			{
				GUILayout.BeginHorizontal();
				data.customPartitions[i].name = EditorGUILayout.TextField($"分段 {i + 1}", data.customPartitions[i].name);
				data.customPartitions[i].startIndexInclude
					= EditorGUILayout.IntField("开始帧（包含）", data.customPartitions[i].startIndexInclude);
				data.customPartitions[i].endIndexExclude
					= EditorGUILayout.IntField("结束帧（不包含）", data.customPartitions[i].endIndexExclude);
				// 删除轨道按钮
				if (GUILayout.Button("删除"))
				{
					data.customPartitions.RemoveAt(i);
					GUILayout.EndHorizontal();
					continue;
				}
				GUILayout.EndHorizontal();
            }
            if (GUILayout.Button("保存配置"))
            {
                SaveData();
            }
            GUILayout.Space(20f);
			// 操作按钮
			useparent = GUILayout.Toggle(useparent, "使用父对象(在实体外额外添加一个父对象)");
            pivot = GUILayout.Toggle(pivot, "图像原点为中心(该效果会导致某些图像出现错位)");
            if (GUILayout.Button("生成"))
			{
				UnitBuilder.Create(data, pivot, useparent);
			}
        }
    }
}