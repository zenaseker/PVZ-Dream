using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class AnimationStateResetter : EditorWindow
{
    private GameObject targetObject;
    private Animator targetAnimator;

    private string stateName = "";
    private int layerIndex = 0;
    private AnimatorOverrideController overrideController;

    private bool resetPosition = true;
    private bool resetRotation = true;
    private bool resetScale = true;
    private bool resetActiveState = true;

    private Vector2 scrollPosition;
    private Dictionary<Transform, TransformState> recordedStates = new Dictionary<Transform, TransformState>();

    [System.Serializable]
    public class TransformState
    {
        public Vector3 localPosition;
        public Quaternion localRotation;
        public Vector3 localScale;
        public bool isActive;
        public string path;

        public TransformState(Transform transform)
        {
            localPosition = transform.localPosition;
            localRotation = transform.localRotation;
            localScale = transform.localScale;
            isActive = transform.gameObject.activeSelf;
            path = GetTransformPath(transform);
        }

        private string GetTransformPath(Transform transform)
        {
            string path = transform.name;
            Transform parent = transform.parent;
            while (parent != null)
            {
                path = parent.name + "/" + path;
                parent = parent.parent;
            }
            return path;
        }
    }

    [MenuItem("Tools/马蹄金的妙妙动画重置器")]
    public static void ShowWindow()
    {
        GetWindow<AnimationStateResetter>("动画状态重置器");
    }

    void OnGUI()
    {
        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);

        GUILayout.Label("动画状态重置工具", EditorStyles.boldLabel);
        EditorGUILayout.Space();

        EditorGUILayout.LabelField("目标设置", EditorStyles.boldLabel);
        targetObject = (GameObject)EditorGUILayout.ObjectField("目标物体", targetObject, typeof(GameObject), true);

        if (targetObject != null)
        {
            targetAnimator = targetObject.GetComponent<Animator>();
            if (targetAnimator == null)
            {
                EditorGUILayout.HelpBox("目标物体没有Animator组件！", MessageType.Warning);
            }
        }

        EditorGUILayout.Space();

        EditorGUILayout.LabelField("动画设置", EditorStyles.boldLabel);
        stateName = EditorGUILayout.TextField("状态名称", stateName);
        layerIndex = EditorGUILayout.IntField("动画层级", layerIndex);
        overrideController = (AnimatorOverrideController)EditorGUILayout.ObjectField(
            "动画覆盖控制器", overrideController, typeof(AnimatorOverrideController), false);

        if (!string.IsNullOrEmpty(stateName))
        {
            EditorGUILayout.HelpBox($"将重置到状态: {stateName}", MessageType.Info);
        }
        else
        {
            EditorGUILayout.HelpBox("留空将使用当前默认状态", MessageType.Info);
        }

        EditorGUILayout.Space();

        EditorGUILayout.LabelField("重置选项", EditorStyles.boldLabel);
        resetPosition = EditorGUILayout.Toggle("重置位置", resetPosition);
        resetRotation = EditorGUILayout.Toggle("重置旋转", resetRotation);
        resetScale = EditorGUILayout.Toggle("重置缩放", resetScale);
        resetActiveState = EditorGUILayout.Toggle("重置激活状态", resetActiveState);

        EditorGUILayout.Space();

        EditorGUILayout.LabelField("操作", EditorStyles.boldLabel);

        GUI.enabled = targetObject != null && targetAnimator != null;

        if (GUILayout.Button("记录第一帧状态", GUILayout.Height(30)))
        {
            RecordFirstFrameStates();
        }

        GUI.enabled = recordedStates.Count > 0;

        if (GUILayout.Button("重置到记录状态", GUILayout.Height(30)))
        {
            ResetToRecordedStates();
        }

        GUI.enabled = targetObject != null && targetAnimator != null;

        if (GUILayout.Button("直接重置到第一帧", GUILayout.Height(30)))
        {
            DirectResetToFirstFrame();
        }

        GUI.enabled = true;

        EditorGUILayout.Space();

        if (recordedStates.Count > 0)
        {
            EditorGUILayout.LabelField($"已记录状态数量: {recordedStates.Count}", EditorStyles.boldLabel);
            EditorGUILayout.Space();

            if (GUILayout.Button("清除记录"))
            {
                recordedStates.Clear();
            }
        }

        EditorGUILayout.EndScrollView();
    }

    private void RecordFirstFrameStates()
    {
        if (targetAnimator == null) return;

        recordedStates.Clear();

        RuntimeAnimatorController originalController = targetAnimator.runtimeAnimatorController;

        try
        {
            if (overrideController != null)
            {
                targetAnimator.runtimeAnimatorController = overrideController;
            }

            bool wasEnabled = targetAnimator.enabled;
            targetAnimator.enabled = true;

            if (!string.IsNullOrEmpty(stateName))
            {
                targetAnimator.Play(stateName, layerIndex, 0f);
            }
            else
            {
                AnimatorStateInfo stateInfo = targetAnimator.GetCurrentAnimatorStateInfo(layerIndex);
                targetAnimator.Play(stateInfo.fullPathHash, layerIndex, 0f);
            }

            targetAnimator.Update(0f);

            RecordTransformRecursive(targetObject.transform);

            targetAnimator.enabled = wasEnabled;

            Debug.Log($"已记录 {recordedStates.Count} 个物体的第一帧状态");
            EditorUtility.DisplayDialog("完成", $"已记录 {recordedStates.Count} 个物体的状态", "确定");
        }
        finally
        {
            if (overrideController != null)
            {
                targetAnimator.runtimeAnimatorController = originalController;
            }
        }
    }

    private void RecordTransformRecursive(Transform transform)
    {
        recordedStates[transform] = new TransformState(transform);

        foreach (Transform child in transform)
        {
            RecordTransformRecursive(child);
        }
    }

    private void ResetToRecordedStates()
    {
        if (recordedStates.Count == 0) return;

        Undo.RecordObjects(recordedStates.Keys.ToArray(), "Reset Animation States");

        int resetCount = 0;
        foreach (var kvp in recordedStates)
        {
            Transform transform = kvp.Key;
            TransformState state = kvp.Value;

            if (transform == null) continue;

            if (resetPosition)
                transform.localPosition = state.localPosition;

            if (resetRotation)
                transform.localRotation = state.localRotation;

            if (resetScale)
                transform.localScale = state.localScale;

            if (resetActiveState)
                transform.gameObject.SetActive(state.isActive);

            resetCount++;
        }

        EditorUtility.DisplayDialog("完成", $"已重置 {resetCount} 个物体", "确定");
    }

    private void DirectResetToFirstFrame()
    {
        RecordFirstFrameStates();
        ResetToRecordedStates();
    }
}

public static class CollectionExtensions
{
    public static T[] ToArray<T>(this Dictionary<T, AnimationStateResetter.TransformState>.KeyCollection keys)
    {
        T[] array = new T[keys.Count];
        int index = 0;
        foreach (T key in keys)
        {
            array[index++] = key;
        }
        return array;
    }
}