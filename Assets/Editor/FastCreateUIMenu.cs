using UnityEngine;
using UnityEditor;


public class FastCreateUIMenu : EditorWindow
{
    Vector2Int plantxy = Vector2Int.zero;
    int plantid = 0;
    bool plantline = false;
    int zombiex = 0;
    int zombieid = 0;
    bool zombieline = false;
    int sunnumber = 0;
    int dreamdepth = 0;
    bool IgnoreCoolTime = false;
    bool CanCreateZombie = true;

    [MenuItem("Window/FastCreate")]
    public static void ShowWindow()
    {
       EditorWindow.GetWindow<FastCreateUIMenu>("FastCreate");
    }
    void OnGUI()
    {

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.BeginVertical("box", GUILayout.MaxWidth(400), GUILayout.ExpandHeight(true));
        GUILayout.Label("关卡", EditorStyles.boldLabel);
        if (GUILayout.Button("跳过植物选择"))
        {
            UImanage.Instance?.OnMianButtonCheck();
        }
        sunnumber = EditorGUILayout.IntField("阳光数", sunnumber);
        if (GUILayout.Button("更改阳光"))
        {
            BattleManage.Instance.SunNumber = sunnumber;
            BattleManage.Instance.SunnumberSet();
        }
        dreamdepth = EditorGUILayout.IntField("梦境深度", dreamdepth);
        if (GUILayout.Button("更改梦境深度"))
        {
            BattleManage.Instance.LevelDreamDepth = dreamdepth;
            BattleManage.Instance.ChangeDreamDepth();
        }
        if (GUILayout.Button("一键通关"))
        {
            BattleManage.Instance.EndLevel();
            Time.timeScale = Attribute.Instance.filedInfo.GameSpeed;
            BattleManage.Instance.controlBase?.OnLevelEnd();
            Attribute.SaveFiled(Attribute.Instance.levelAttribute.ID, Attribute.Instance.levelAttribute.Type);
            Attribute.ChangeScene("MianMenu");
        }
        PropManage.IgnoreCoolTime = this.IgnoreCoolTime = EditorGUILayout.Toggle("忽略道具冷却", this.IgnoreCoolTime);


        EditorGUILayout.EndVertical();
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.BeginVertical("box", GUILayout.MaxWidth(400), GUILayout.ExpandHeight(true));
        GUILayout.Label("植物",EditorStyles.boldLabel);
        plantxy = EditorGUILayout.Vector2IntField("植物坐标", plantxy);
        plantid = EditorGUILayout.IntField("植物id", plantid);
        if (GUILayout.Button("生成植物"))
        {
            HandManage.Instance.FastCellPlant(plantxy, plantid);
        }
        if (HandManage.Instance != null)
        {
            HandManage.Instance.plantline = this.plantline = EditorGUILayout.Toggle("排山倒海", this.plantline);
        }
        EditorGUILayout.EndVertical();
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.BeginVertical("box", GUILayout.MaxWidth(400), GUILayout.ExpandHeight(true));
        GUILayout.Label("僵尸", EditorStyles.boldLabel);
        zombiex = EditorGUILayout.IntField("僵尸出现行", zombiex);
        zombieid = EditorGUILayout.IntField("僵尸id", zombieid);
        if (GUILayout.Button("生成僵尸"))
        {
            if (zombieline)
            {
                for (int i = 0; i < MapManage.Instance.meshxy.x; i++)
                {
                    ZombieManage.Instance?.CreateZombie(zombieid, i);
                }
            }
            else
            {
                ZombieManage.Instance?.CreateZombie(zombieid, zombiex);
            }
        }
        if (GUILayout.Button("下一波次"))
        {
            ZombieManage.Instance?.SetCreateZombie();
        }
        if (GUILayout.Button("清除全场僵尸"))
        {
            ZombieManage.Instance?.ClearZombie();
        }
        zombieline = EditorGUILayout.Toggle("排山倒海", zombieline);
        if (ZombieManage.Instance != null)
        {
            ZombieManage.Instance.CanCreateZombie = this.CanCreateZombie = EditorGUILayout.Toggle("允许出僵尸", this.CanCreateZombie);
        }
        EditorGUILayout.EndVertical();
        EditorGUILayout.EndHorizontal();
    }
}



public class TestSaveSprite
{
[MenuItem("Tools/导出精灵")]
    static void SaveSprite()
    {
        string resourcesPath = "Assets/Resources/";
        foreach (Object obj in Selection.objects)
        {
            string selectionPath = AssetDatabase.GetAssetPath(obj);
            // 必须最上级是"Assets/Resources/"
            if (selectionPath.StartsWith(resourcesPath))
            {
                string selectionExt = System.IO.Path.GetExtension(selectionPath);
                if (selectionExt.Length == 0)
                {
                    continue;
                }
                // 从路径"Assets/Resources/UI/testUI.png"得到路径"UI/testUI"
                string loadPath = selectionPath.Remove(selectionPath.Length - selectionExt.Length);
                loadPath = loadPath.Substring(resourcesPath.Length);
                // 加载此文件下的所有资源
                Sprite[] sprites = Resources.LoadAll<Sprite>(loadPath);
                if (sprites.Length > 0)
                {
                    // 创建导出文件夹
                    string outPath = Application.dataPath + "/Editor/outSprite/" + loadPath;
                    System.IO.Directory.CreateDirectory(outPath);
                    foreach (Sprite sprite in sprites)
                    {
                        // 创建单独的纹理
                        Texture2D tex = new Texture2D((int)sprite.rect.width, (int)sprite.rect.height, sprite.texture.format, false);
                        tex.SetPixels(sprite.texture.GetPixels((int)sprite.rect.xMin, (int)sprite.rect.yMin,
                        (int)sprite.rect.width, (int)sprite.rect.height));
                        tex.Apply();
                        // 写入成PNG文件
                        System.IO.File.WriteAllBytes(outPath + "/" + sprite.name + ".png", tex.EncodeToPNG());
                    }
                    Debug.Log("SaveSprite to " + outPath);
                }
            }
        }
        Debug.Log("SaveSprite Finished");
    }
}
