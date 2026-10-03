using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace SimulatedUniverse
{
    [CustomEditor(typeof(EventInfos))]
    public class LevelObject : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (GUILayout.Button("Open Editor"))
            {
                EventInfoEditorWindow.Open((EventInfos)target);
            }
        }
    }
    public class EventInfoEditorWindow : EditorWindow
    {
        protected SerializedObject serializedObject;
        protected SerializedProperty serializedProperty;
        protected EventInfos plantInfo;

        private string selectedPropertyPath;
        protected SerializedProperty selectedProperty;
        protected Vector2 scrollview;
        protected Vector2 windowscrollview;
        protected bool showtype = false;
        public static void Open(EventInfos eventInfo)//ÔÚInspectorÃæ°å´ò¿ª
        {
            EventInfoEditorWindow window = EventInfoEditorWindow.GetWindow<EventInfoEditorWindow>("EventInfoEditorWindow");
            window.ReInit(eventInfo);
            window.plantInfo = eventInfo;
        }
        [MenuItem("Window/EventInfoEditorWindow")]
        public static void ShowWindow()//ÔÚÉÏ·½Ãæ°å´ò¿ª
        {
            Open(Resources.Load<EventInfos>("ScriptableObject/EventInfo"));
        }
        public void ReInit(EventInfos eventInfo)//ÖØÐÂ¶ÁÈ¡ÄÚÈÝ
        {
            serializedObject = new SerializedObject(eventInfo);
            serializedProperty = serializedObject.FindProperty("EventInfoes");
        }
        void OnGUI()//GUIäÖÈ¾
        {
            //¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª´´½¨Ë®Æ½ÅÅÐò¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª
            EditorGUILayout.BeginHorizontal();
            //¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª´´½¨¹ö¶¯ÇøÓò¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª
            scrollview = EditorGUILayout.BeginScrollView(scrollview, GUILayout.MaxWidth(150));
            //¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª´´½¨´¹Ö±ÅÅÐò¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª
            EditorGUILayout.BeginVertical("box", GUILayout.MaxWidth(150), GUILayout.ExpandHeight(true));
            DrawSidebar(serializedProperty);
            //¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª´´½¨Ë®Æ½ÅÅÐò¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("+"))
            {
                serializedProperty.InsertArrayElementAtIndex(serializedProperty.arraySize);
            }
            if (GUILayout.Button("-"))
            {
                serializedProperty.DeleteArrayElementAtIndex(serializedProperty.arraySize - 1);
            }
            //¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª½áÊøË®Æ½ÅÅÐò¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª
            EditorGUILayout.EndHorizontal();
            //¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª½áÊø´¹Ö±ÅÅÐò¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª
            EditorGUILayout.EndVertical();
            //¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª½áÊø¹ö¶¯ÇøÓò¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª
            EditorGUILayout.EndScrollView();

            //¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª´´½¨´¹Ö±ÅÅÐò¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª
            EditorGUILayout.BeginVertical("box", GUILayout.ExpandHeight(true));
            //¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª´´½¨¹ö¶¯ÇøÓò¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª
            windowscrollview = EditorGUILayout.BeginScrollView(windowscrollview);
            EditorGUILayout.BeginVertical("box", GUILayout.MaxHeight(25), GUILayout.ExpandWidth(true));
            showtype = EditorGUILayout.Toggle("¹Ø¿¨Õ¹Ê¾·½Ê½£ºÄ¬ÈÏ/·ÖÀà", showtype);
            EditorGUILayout.EndVertical();
            if (serializedProperty != null)
            {
                if (showtype)
                {
                    DrawProperties2(serializedProperty);
                }
                else
                {
                    DrawProperties(serializedProperty, true);
                }
            }
            //¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª½áÊø¹ö¶¯ÇøÓò¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª
            EditorGUILayout.EndScrollView();
            //¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª½áÊø´¹Ö±ÅÅÐò¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª
            EditorGUILayout.EndVertical();
            //¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª½áÊøË®Æ½ÅÅÐò¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª
            EditorGUILayout.EndHorizontal();

            serializedObject.ApplyModifiedProperties();
        }
        protected void DrawProperties(SerializedProperty prop, bool drawChildren)//»æ»­ÏêÏ¸ÄÚÈÝ
        {
            foreach (SerializedProperty p in prop)
            {
                if (p.displayName == selectedPropertyPath)
                {
                    p.isExpanded = true;
                    EditorGUILayout.BeginVertical();
                    EditorGUILayout.PropertyField(p, drawChildren);
                    EditorGUILayout.EndVertical();
                }
            }
        }
        protected void DrawProperties2(SerializedProperty prop)//»æ»­ÏêÏ¸ÄÚÈÝ
        {
            foreach (SerializedProperty p in prop)
            {
                if (p.displayName == selectedPropertyPath)
                {
                    p.isExpanded = true;
                    EditorGUILayout.BeginVertical();
                    p.FindPropertyRelative("ID").intValue = EditorGUILayout.IntField("ID", p.FindPropertyRelative("ID").intValue);
                    p.FindPropertyRelative("Name").stringValue = EditorGUILayout.TextField(p.FindPropertyRelative("Name").stringValue, "Name");
                    p.FindPropertyRelative("MainImage").objectReferenceValue = EditorGUILayout.ObjectField( "MainImage",p.FindPropertyRelative("MainImage").objectReferenceValue,typeof(Sprite));
                    SerializedProperty Events = p.FindPropertyRelative("Events");
                    if (Events != null)
                    {
                        EditorGUILayout.LabelField("ÒÔÏÂÎªÊÂ¼þÖÐ¸÷½×¶ÎÄÚÈÝ£º");

                        foreach (SerializedProperty q in Events)
                        {
                            EditorGUILayout.PropertyField(q, true);
                        }

                    }
                    EditorGUILayout.EndVertical();
                }
            }
        }
        protected void DrawSidebar(SerializedProperty prop)//»æ»­×ó²àÁÐ±íÇøÓò
        {
            if (prop is null) return;
            foreach (SerializedProperty p in prop)
            {
                if (GUILayout.Button(p.FindPropertyRelative("Name").stringValue))
                {
                    selectedPropertyPath = p.displayName;
                    selectedProperty = p;
                    p.isExpanded = true;
                }
            }
        }
        public void OnDestroy()//´°¿Ú¹Ø±ÕÊ±
        {
            foreach (SerializedProperty p in serializedProperty)
            {
                p.isExpanded = false;
            }
            serializedObject.ApplyModifiedProperties();
        }
    }
}

