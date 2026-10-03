using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;
using static Attribute;

public class AlmanacsByZombie : MonoBehaviour
{
    public Transform ZombiePos;
    public TextMeshProUGUI ZombieName;
    public TextMeshProUGUI ZombieDescription;
    public Transform BoxList;
    GameObject showzombie = null;
    bool isInit = false;

    void Start()
    {
        if (isInit) return;
        foreach (Attribute.ZombieInfo zombie in Attribute.ZombieInfos.ZombieInfoes)
        {
            if (zombie.IsCreateZombie) { continue; }
            if (zombie.zombieDescribe == null)
            {
                DebugShow.Instance.Init("存在未写入信息的僵尸，请联系制作者！");
                continue;
            }
            GameObject box = PoolManage.Instance.GetPoolGameObject("AlmanacsBox", "AlmanacsBox_Zombie", BoxList);
            RandomUtil.AddOrGetComponent<AlmanacsZombieBox>(box).Init(this, zombie.ID);

        }
        isInit = true;
    }

    public void OnCilck(int id)
    {
        if (showzombie != null)
        {
            PoolManage.Instance.PushGameObject(showzombie.name, showzombie.gameObject);
        }
        showzombie = PoolManage.Instance.GetPoolGameObject("Zombie", Attribute.Instance.GetZombieInfo(id).Prefab.name, ZombiePos);
        showzombie.transform.localScale = Vector3.one * 100;
        showzombie.GetComponent<SortingGroup>().sortingLayerName = "Effect";
        ZombiesBase zombiesBase;
        if (showzombie.TryGetComponent<ZombiesBase>(out zombiesBase))
        {
            GameObject.Destroy(showzombie.GetComponent<ZombiesBase>());
        }
        ZombieDescribe zombieDescribe = Attribute.Instance.GetZombieInfo(id).zombieDescribe;
        ZombieName.text = zombieDescribe.Name;
        ZombieDescription.text = "<color=#0000FF>";
        ZombieDescription.text += zombieDescribe.MiniInfo;
        ZombieDescription.text += "\r\n \r\n<size=13>";
        ZombieDescription.text += zombieDescribe.KEYWORD;
        ZombieDescription.text += "\r\n";
        ZombieDescription.text += zombieDescribe.Characteristic;
        ZombieDescription.text += "</size></color>\r\n \r\n";
        ZombieDescription.text += zombieDescribe.Description;
    }
}
