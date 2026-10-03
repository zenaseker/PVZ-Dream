using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.UI;

public class ShowGetSeed : Singleton<ShowGetSeed>
{
    public Transform SeedPos;
    public TextMeshProUGUI Name;
    public TextMeshProUGUI Info;
    public GameObject LeftOne;
    public GameObject RightOne;
    List<int> ids = new List<int>();
    GameObject nowshowseed = null;
    int nowshow = 0;
    public void Start()
    {
#if UNITY_EDITOR
        if (Attribute.Instance.levelAttribute == null)
        {
            Attribute.Instance.levelAttribute = new Attribute.Level
            {
                unclockplantid = new List<int> { 1,3,8,101,106,203,201,1001 }
            };
        }
#endif
        if (Attribute.Instance.levelAttribute.unclockplantid.Count > 0 && Attribute.Instance.levelAttribute.unclockplantid[0] != 0)
        {
            ids = new List<int>(Attribute.Instance.levelAttribute.unclockplantid);
            nowshow = 0; 
            nowshowseed = null;
            Show();
        }
        MusicManage.Instance.ChangeBGM("MainMenu");
    }

    public void Show()
    {
        Attribute.PlantInfo card = Attribute.Instance.GetPlantInfo(ids[nowshow]);
        if (nowshowseed != null)
        {
            PoolManage.Instance.PushGameObject(nowshowseed.name, nowshowseed);
        }
        nowshowseed = PoolManage.Instance.GetPoolGameObject("SeedCard",card.CardPrefab.name, SeedPos);
        nowshowseed.transform.localScale = Vector3.one;
        nowshowseed.GetComponent<RectTransform>().anchorMax = nowshowseed.GetComponent<RectTransform>().anchorMin = Vector2.one / 2;
        nowshowseed.transform.localPosition = Vector3.zero;
        Button button;
        if (nowshowseed.TryGetComponent<Button>(out button))
        {
            GameObject.Destroy(nowshowseed.GetComponent<Button>());
        }
        Name.text = Attribute.Instance.GetPlantInfo(card.ID).plantDescribe.Name;
        Info.text = Attribute.Instance.GetPlantInfo(card.ID).plantDescribe.MiniInfo;
        LeftOne.SetActive(nowshow > 0);
        RightOne.SetActive(nowshow < ids.Count - 1);
    }
    public void ChangeShow(int num)
    {
        if (nowshow <= 0 && num < 0) return;
        if (nowshow >= ids.Count - 1 && num > 0) return;
        nowshow += num;
        Show();
    }
    public void Confirm()
    {
        Attribute.ChangeScene("MianMenu");
    }
}
