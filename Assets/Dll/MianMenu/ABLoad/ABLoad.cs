using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Xml.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

public class ABLoad : Singleton<ABLoad>
{
    public Image loadline;
    public Image loadline2;
    public TextMeshProUGUI nowloading;
    public TMP_Dropdown typedropdown;
    public TMP_Dropdown Objectdropdown;
    public Transform ShowPos;
    public Transform ShowPosInUI;
    public Dictionary<Type, List<Object>> LoadingObject;
    public Type nowtype = null;
    public Object nowshowobject = null;
    public Image ShowSprite;
    public MeshRenderer ShowMaterial;
    public AudioSource ShowAudio;
    public string fliePath;
    bool InUI = false;
    int AllLoadCount = 0;
    int AllLoadCount2 = 0;
    int finish = 0;
    int lose = 0;
    public void Start()
    {
        StartCoroutine(LoadAssetAsync());
    }
    public void ChangeShowType(int index)
    {
        Debug.Log(index);
        nowtype = LoadingObject.Keys.ToList()[index];
        Objectdropdown.options = new List<TMP_Dropdown.OptionData>();
        foreach (Object @object in LoadingObject[nowtype])
        {
            Objectdropdown.options.Insert(Objectdropdown.options.Count,new TMP_Dropdown.OptionData(@object.name));
            Objectdropdown.value = 0;
        }
    }
    public void ChangeShowIndex(int index)
    {
        Debug.Log(index);
        ClearShow();
        nowshowobject = LoadingObject[nowtype][index];
        ShowNewObject();
    }
    public void ToUI(bool flag)
    {
        InUI = true;
    }
    public void ClearShow()
    {
        switch (nowshowobject.GetType().Name)
        {
            case "GameObject":
                GameObject.Destroy(nowshowobject as GameObject);
                break;
            case "Material":
                ShowMaterial.gameObject.SetActive(false);
                break;
            case "Sprite":
            case "Texture2D":
                ShowSprite.gameObject.SetActive(false);
                break;
            case "Audio":
                ShowAudio.gameObject.SetActive(false);
                break;
        }
    }
    public void ShowNewObject()
    {
        switch (nowtype.Name)
        {
            case "GameObject":
                GameObject gameObject = nowshowobject as GameObject;
                if (InUI)
                {
                    GameObject.Instantiate(gameObject, ShowPosInUI);
                }
                else
                {
                    GameObject.Instantiate(gameObject, ShowPos);
                }
                return;
            case "Material":
                ShowMaterial.gameObject.SetActive(true);
                ShowMaterial.material = nowshowobject as Material;
                return;
            case "Sprite":
                ShowSprite.gameObject.SetActive(true);
                Sprite sprite1 = nowshowobject as Sprite;
                ShowSprite.sprite = sprite1;
                ShowSprite.rectTransform.sizeDelta = new Vector2(sprite1.rect.width, sprite1.rect.height);

                return;
            case "Texture2D":
                Sprite sprite = Sprite.Create(nowshowobject as Texture2D,Rect.zero,Vector2.zero);
                ShowSprite.gameObject.SetActive(true);
                ShowSprite.sprite = sprite;
                ShowSprite.rectTransform.sizeDelta = new Vector2(sprite.rect.width, sprite.rect.height);
                return;
            case "Audio":
                ShowAudio.gameObject.SetActive(true);
                ShowAudio.clip = nowshowobject as AudioClip;
                break;
        }
        Debug.Log($"不受支持的预览类型:{nowtype.Name}");
    }
    public IEnumerator LoadAssetAsync()
    {
        finish = 0;
        LoadingObject = new Dictionary<Type, List<Object>>();
        DirectoryInfo SAPath = new DirectoryInfo(fliePath);
        if (SAPath != null)
        {
            FileInfo[] fileInfos = SAPath.GetFiles("*", SearchOption.AllDirectories);
            AllLoadCount = fileInfos.Length;
            for (int i = 0;i < AllLoadCount;i++)
            {
                FileInfo fileInfo3 = fileInfos[i];
                if (fileInfo3.Name.Contains("meta")) continue;
                loadline.fillAmount = (float)i / (float)AllLoadCount;
                nowloading.text = $"正在加载{fileInfo3.Name}";
                AssetBundleCreateRequest assetasync = AssetBundle.LoadFromFileAsync(fileInfo3.FullName);
                yield return assetasync;
                var myLoadAssetBundle = assetasync.assetBundle;
                if (myLoadAssetBundle == null)
                {
                    Debug.Log($"无法加载AssetBundle{fileInfo3.Name}({fileInfo3.FullName})");
                    lose++;
                    continue;
                }
                var assetLoadRequest = myLoadAssetBundle.LoadAllAssetsAsync();
                yield return assetLoadRequest;
                if (assetLoadRequest == null)
                {
                    Debug.Log($"无法解析AssetBundle内容{fileInfo3.Name}({fileInfo3.FullName})");
                    lose++;
                    continue;
                }
                if (assetLoadRequest.allAssets == null)
                {
                    Debug.Log($"读取到可能为空的AssetBundle内容{fileInfo3.Name}({fileInfo3.FullName})");
                    lose++;
                    continue;
                }
                Object[] Assets = assetLoadRequest.allAssets;
                AllLoadCount2 = Assets.Length;
                for (int j = 0; j < AllLoadCount2; j++)
                {
                    Object @object = Assets[i];
                    loadline2.fillAmount = (float)j / (float)AllLoadCount2;
                    if (@object == null) continue;
                    Type type = @object.GetType();
                    nowloading.text = $"正在加载{fileInfo3.Name}{@object.name}({type})";
                    if (!LoadingObject.ContainsKey(type))
                    {
                        LoadingObject.Add(type, new List<Object>());
                        typedropdown.options.Add(new TMP_Dropdown.OptionData(type.Name));
                    }
                    LoadingObject[type].Add(@object);
                    finish++;
                }
                yield return null;
            }
            yield return null;
        }
        Debug.Log("AB包读取完毕");
        nowloading.text = $"读取完毕!共{AllLoadCount}个包体,成功{finish}个,失败{lose}个";
        yield return null;
    }
}
