using DG.Tweening;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Attribute;
using static SeedWithChoose;


public class UImanage : Singleton<UImanage>
{
    [Tooltip("阳光显示")]
    public Text SunNumber;
    [Tooltip("下方选卡列表")]
    public Transform SeedList;
    [Tooltip("下方特殊选卡列表")]
    public Transform SpecailSeedList;
    [Tooltip("上方UI背景")]
    public Transform BankBackList;
    [Tooltip("上方选卡列表")]
    public Transform BankList;
    [Tooltip("卡片移动父体")]
    public Transform SeedMoveUnit;
    [Tooltip("已选元素背景")]
    public Transform ElementBankBackList;
    [Tooltip("已选元素列表")]
    public Transform ElementBankList;
    [Tooltip("元素选卡列表")]
    public Transform ElementSeedBankList;
    //已选卡的列表
    List<Seed> ChooseID = new List<Seed>();
    //已选择的元素列表
    public List<ElementSeed> elementSeeds = new List<ElementSeed>();
    //已解锁的元素
    public List<Attribute.DreamElement> UnlockElement = new List<Attribute.DreamElement>();
    //启用的元素列表
    public List<Attribute.DreamElement> usingelement = new List<Attribute.DreamElement>();
    private bool startbattle = false;
    bool InChoice = false;
    int canusecardnum = 10;

    public void CheckSun()
    {
        SunNumber.text = BattleManage.Instance.SunNumber.ToString();
    }

    public void Start()
    {
        CheckSun();
        startbattle = false;
        for (int i = 0; i < BankList.childCount; i++)
        {
            BankList.GetChild(i).GetComponent<Seed>().Destory();
        }
        if(Attribute.Instance.filedInfo.Difficulty >= 4)
        {
            canusecardnum = 10 - Attribute.Instance.filedInfo.Difficulty + 3;
            BankBackList.GetChild(9).GetComponent<Image>().sprite = Attribute.GetSprite("NotUseElementMask");
            if (Attribute.Instance.filedInfo.Difficulty >= 5)
            {
                BankBackList.GetChild(8).GetComponent<Image>().sprite = Attribute.GetSprite("NotUseElementMask");
            }
        }
        for (int i = 0; i < SeedList.childCount; i++)
        {
            SeedList.GetChild(i).GetComponent<Seed>().Destory();
        }
        for (int i = 0; i < ElementBankList.childCount; i++)
        {
            GameObject.Destroy(ElementBankList.GetChild(i).gameObject);
        }
        for (int i = 0; i < ElementSeedBankList.childCount; i++)
        {
            GameObject.Destroy(ElementSeedBankList.GetChild(i).gameObject);
        }
        ChooseID = new List<Seed>();
        UnlockElement = new List<Attribute.DreamElement>();
        usingelement = new List<Attribute.DreamElement>();
        elementSeeds = new List<ElementSeed>();
        InChoice = false;
        if (BattleManage.Instance.level.LevelIcons == LevelIcon.ClockCard)
        {
            ControlCard();
            return;
        }
        foreach (int i in Attribute.Instance.filedInfo.Unclockplantid)
        {
            Attribute.PlantInfo plantCard = Attribute.Instance.GetPlantInfo(i);
            if (i / 100 == 0)
            {
                GameObject obj = PoolManage.Instance.GetPoolGameObject("SeedCard", plantCard.CardPrefab.name, SeedList);
                obj.transform.localScale = Vector3.one * 0.9f;
                Seed seed = RandomUtil.AddOrGetComponent<Seed>(obj);
                seed.Init(plantCard);
                obj.AddComponent<SeedWithChoose>().Init(seed);
            }
            if (Attribute.Instance.GetPlantInfo(i).DreamElement.Contains(DreamElement.Dream))
            {
                GameObject obj = PoolManage.Instance.GetPoolGameObject("SeedCard", plantCard.CardPrefab.name, SpecailSeedList);
                obj.transform.localScale = Vector3.one * 0.9f;
                Seed seed = RandomUtil.AddOrGetComponent<Seed>(obj);
                seed.Init(plantCard);
                obj.AddComponent<SeedWithChoose>().Init(seed);
            }
            foreach(DreamElement dreamElement in Attribute.Instance.GetPlantInfo(i).DreamElement)
            {
                if (dreamElement != DreamElement.Dream && !UnlockElement.Contains(dreamElement))
                {
                    UnlockElement.Add(dreamElement);
                }
            }
        }
        bool OnlyDefault = true;
        foreach(Attribute.DreamElement dreamPlantElement in UnlockElement)
        {
            if (dreamPlantElement != DreamElement.Default)
            {
                OnlyDefault = false;
                GameObject obj = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/SeedElement/ElementSeed_" + dreamPlantElement.ToString()), ElementSeedBankList);
                obj.AddComponent<SeedWithChoose>().Init(null, obj.GetComponent<ElementSeed>());
            }
        }
        ElementBankList.transform.parent.gameObject.SetActive(!OnlyDefault);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1) && ElementBankList.childCount > 0)
        {
            ElementBankList.GetChild(0).GetComponent<ElementSeed>().OnCilck();
        }
        if (Input.GetKeyDown(KeyCode.Alpha2) && ElementBankList.childCount > 1)
        {
            ElementBankList.GetChild(1).GetComponent<ElementSeed>().OnCilck();
        }
        if (Input.GetKeyDown(KeyCode.Alpha3) && ElementBankList.childCount > 2)
        {
            ElementBankList.GetChild(2).GetComponent<ElementSeed>().OnCilck();
        }
    }

    #region 战前选卡
    public void InitCard(SeedWithChoose OriginCard)
    {
        if (InChoice) return;
        if (BankList.childCount >= canusecardnum)
        {
            DebugShow.Instance.Init("选卡列表已满！");
            return;
        }
        InChoice = true;
        MusicManage.Instance.PlayEffect("seedlift", 1);
        Seed seed = CreateSeedCard(OriginCard.seed.card, OriginCard.transform.position, SeedMoveUnit, true);
        RandomUtil.AddOrGetComponent<SeedWithChoose>(seed.gameObject).Init(seed);
        BattleManage.Instance.controlBase?.OnSelcetSeedCard(OriginCard.GetComponent<Seed>());
        SeedWithChoose MoveCard = seed.gameObject.GetComponent<SeedWithChoose>();
        ChooseID.Add(seed);
        seed.gameObject.GetComponent<Button>().onClick.AddListener(MoveCard.OnClick);
        MoveCard.transform.position = OriginCard.transform.position;
        MoveCard.cardIn = CardIn.SeedBankbyReady;
        MoveCard.SeedChooseOrigin = OriginCard;
        Vector3 vector = OriginCard.transform.position;
        Vector3 vector3 = BankBackList.GetChild(ChooseID.Count - 1).transform.position;
        MoveCard.transform.DOScale(Vector3.one, 0.06f).SetEase(Ease.Linear);
        MusicManage.Instance.PlayEffect("tap", 1f);
        InChoice = false;
        MoveCard.transform.DOPath(new Vector3[] { vector, vector3 }, 0.07f).SetEase(Ease.Linear).onComplete += () =>
        {
            MoveCard.transform.SetParent(BankList);
            OriginCard.ChooseCardInit(true);
        };
    }
    //一键选卡
    public void OneCheckInitCard()
    {
        if (BankList.childCount > 0)
        {
            DebugShow.Instance.Init("请清空卡槽！");
            return;
        }
        if (Attribute.Instance.filedInfo.LastChioceCard == null || Attribute.Instance.filedInfo.LastChioceCard.Count <= 0)
        {
            DebugShow.Instance.Init("未记录选卡或上次未选卡！");
            return;
        }
        MusicManage.Instance.PlayEffect("seedlift", 1);
        for(int i = 0; i < Attribute.Instance.filedInfo.LastChioceCard.Count;i++)
        {
            int id = Attribute.Instance.filedInfo.LastChioceCard[i];
            SeedWithChoose OriginCard = SeedList.Find(Attribute.Instance.GetPlantInfo(id).CardPrefab.name).GetComponent<SeedWithChoose>();

            Seed seed = CreateSeedCard(OriginCard.seed.card, OriginCard.transform.position, SeedMoveUnit, true);
            RandomUtil.AddOrGetComponent<SeedWithChoose>(seed.gameObject).Init(seed);
            BattleManage.Instance.controlBase?.OnSelcetSeedCard(OriginCard.GetComponent<Seed>());
            SeedWithChoose MoveCard = seed.gameObject.GetComponent<SeedWithChoose>();
            ChooseID.Add(seed);
            seed.gameObject.GetComponent<Button>().onClick.AddListener(MoveCard.OnClick);
            MoveCard.transform.position = OriginCard.transform.position;
            MoveCard.cardIn = CardIn.SeedBankbyReady;
            MoveCard.SeedChooseOrigin = OriginCard;
            Vector3 vector = OriginCard.transform.position;
            Vector3 vector3 = BankBackList.GetChild(i).transform.position;
            MoveCard.transform.DOScale(Vector3.one, 0.06f).SetEase(Ease.Linear);
            MoveCard.transform.DOPath(new Vector3[] { vector, vector3 }, 0.07f).SetEase(Ease.Linear).onComplete += () =>
            {
                MoveCard.transform.SetParent(BankList);
                OriginCard.ChooseCardInit(true);
            };
        }
        MusicManage.Instance.PlayEffect("tap", 1f);
    }

    public void ControlCard()
    {
        MusicManage.Instance.PlayEffect("seedlift", 1);
        List<int> cardids = Attribute.LevelControlPlantCardes[BattleManage.Instance.level.ControlCardid];
        for (int i = 0; i < cardids.Count; i++)
        {
            int id = cardids[i];
            Seed seed = CreateSeedCard(Attribute.Instance.GetPlantInfo(id), BankBackList.GetChild(i).transform.position, BankList, false);
            seed.GetComponent<Button>().onClick.AddListener(seed.OnCilck);
            seed.incool = Cardtype.CanUse;
            ChooseID.Add(seed);
            BattleManage.Instance.controlBase?.OnSelcetSeedCard(seed);
        }
        MusicManage.Instance.PlayEffect("tap", 1f);
    }

    public void ReInitCard(SeedWithChoose OriginCard, SeedWithChoose MoveCard)
    {
        InChoice = true;
        MusicManage.Instance.PlayEffect("seedlift", 1);
        Vector3 vector = MoveCard.transform.position;
        Vector3 vector3 = OriginCard.transform.position;
        MoveCard.transform.SetParent(SeedMoveUnit);
        ChooseID.Remove(MoveCard.GetComponent<Seed>());
        MusicManage.Instance.PlayEffect("tap2", 1f);
        MoveCard.transform.DOScale(new Vector3(0.818f, 0.818f), 0.06f).SetEase(Ease.Linear);
        InChoice = false;
        MoveCard.transform.DOPath(new Vector3[] { vector, vector3 }, 0.07f).SetEase(Ease.Linear).onComplete += () =>
        {
            OriginCard.ChooseCardInit(false);
            PoolManage.Instance.PushGameObject(MoveCard.gameObject.name,MoveCard.gameObject);
            for (int i = 0; i < BankList.childCount; i++)
            {
                BankList.GetChild(i).position = BankBackList.GetChild(i).transform.position;
            }
        };
    }
    public void InitElementCard(SeedWithChoose OriginCard)
    {
        if (InChoice) return;
        if (ElementBankList.childCount >= ElementBankBackList.childCount)
        {
            DebugShow.Instance.Init("元素列表已满！");
            return;
        }
        InChoice = true;
        MusicManage.Instance.PlayEffect("seedlift", 1);
        GameObject obj = GameObject.Instantiate(OriginCard.gameObject, SeedMoveUnit);
        obj.GetComponent<ElementSeed>().dreamElement = OriginCard.gameObject.GetComponent<ElementSeed>().dreamElement;
        BattleManage.Instance.controlBase?.OnSelcetSeedCard(OriginCard.GetComponent<ElementSeed>());
        SeedWithChoose MoveCard = obj.GetComponent<SeedWithChoose>();
        obj.GetComponent<Button>().onClick.AddListener(MoveCard.OnClick);
        MoveCard.transform.position = OriginCard.transform.position;
        MoveCard.cardIn = CardIn.SeedBankbyReady;
        MoveCard.SeedChooseOrigin = OriginCard;
        elementSeeds.Add(MoveCard.GetComponent<ElementSeed>());
        Vector3 vector = OriginCard.transform.position;
        Vector3 vector3 = ElementBankBackList.GetChild(elementSeeds.Count - 1).transform.position;
        MoveCard.transform.DOScale(Vector3.one, 0.06f).SetEase(Ease.Linear);
        MusicManage.Instance.PlayEffect("tap", 1f);
        InChoice = false;
        MoveCard.transform.DOPath(new Vector3[] { vector, vector3 }, 0.07f).SetEase(Ease.Linear).onComplete += () =>
        {
            MoveCard.transform.SetParent(ElementBankList);
            OriginCard.ChooseCardInit(true);
        };
    }

    public void ReInitElementCard(SeedWithChoose OriginCard, SeedWithChoose MoveCard)
    {
        InChoice = true;
        MusicManage.Instance.PlayEffect("seedlift", 1);
        Vector3 vector = MoveCard.transform.position;
        Vector3 vector3 = OriginCard.transform.position;
        MoveCard.transform.SetParent(SeedMoveUnit);
        elementSeeds.Remove(MoveCard.GetComponent<ElementSeed>());
        MusicManage.Instance.PlayEffect("tap2", 1f);
        MoveCard.transform.DOScale(new Vector3(0.818f, 0.818f), 0.06f).SetEase(Ease.Linear);
        InChoice = false;
        MoveCard.transform.DOPath(new Vector3[] { vector, vector3 }, 0.07f).SetEase(Ease.Linear).onComplete += () =>
        {
            OriginCard.ChooseCardInit(false);
            GameObject.Destroy(MoveCard.gameObject);
            for (int i = 0; i < ElementBankList.childCount; i++)
            {
                ElementBankList.GetChild(i).position = ElementBankBackList.GetChild(i).transform.position;
            }
        };
    }
    #endregion

    public Seed CreateSeedCard(Attribute.PlantInfo card,Vector3 OriginPos,Transform parent,bool inready)
    {
        GameObject obj = PoolManage.Instance.GetPoolGameObject("SeedCard", card.CardPrefab.name, OriginPos, parent);
        obj.transform.localScale = Vector3.one * 0.9f;
        RandomUtil.AddOrGetComponent<Seed>(obj).Init(card);
        if (!inready && obj.GetComponent<Seed>().incool == Cardtype.Ready)
        {
            obj.GetComponent<Seed>().incool = Cardtype.CanUse;
            BattleManage.Instance.checkCardCost += obj.GetComponent<Seed>().SunEnoughorNot;
        }
        return obj.GetComponent<Seed>();
    }

    public void OnMianButtonCheck()
    {
        if (startbattle) return;
        startbattle = true;
        Attribute.Instance.filedInfo.LastChioceCard = new List<int>();
        foreach(Seed sed in ChooseID)
        {
            Attribute.Instance.filedInfo.LastChioceCard.Add(sed.card.ID);
        }
        SaveLoadManager.Save(Attribute.Instance.filedInfo.FliedName, Attribute.Instance.filedInfo);
        BattleManage.Instance.BackSellChoice();
        for (int i = 0; i < BankList.transform.childCount; i++)
        {
            BankList.transform.GetChild(i).GetChild(3).gameObject.SetActive(true);
            GameObject.Destroy(BankList.transform.GetChild(i).GetComponent<SeedWithChoose>());
        }
        for (int i = 0; i < ElementBankList.transform.childCount; i++)
        {
            ElementBankList.transform.GetChild(i).GetChild(2).gameObject.SetActive(true);
            GameObject.Destroy(ElementBankList.transform.GetChild(i).GetComponent<SeedWithChoose>());
            ElementBankList.transform.GetChild(i).GetComponent<ElementSeed>().ChangeUse(false);
        }
    }

    public void ChangeChoiceElement(Attribute.DreamElement element,bool add = true)
    {
        if (add)
        {
            usingelement.Add(element);
        }
        else
        {
            usingelement.Remove(element);
        }
        BattleManage.Instance.controlBase?.OnChangeElement(element);
        for(int i = 0;i < ChooseID.Count;i++)
        {
            Seed seed = ChooseID[i]; 
            int elementid = GetElementPlant(usingelement,seed.card.ID);
            if (seed.card.DreamElement.Contains(DreamElement.Dream))
            {
                elementid = seed.card.ID;
            }
            Attribute.PlantInfo plantCard = Attribute.Instance.GetPlantInfo(elementid);
            if (elementid != seed.card.ID)
            {
                Seed seed1 = CreateSeedCard(plantCard, seed.transform.position, seed.transform.parent, false);
                seed1.transform.localScale = seed.transform.localScale;
                ChooseID[i] = seed1;
                seed1.GetComponent<Button>().onClick.AddListener(seed1.OnCilck);
                seed1.SunEnoughorNot();
                seed.ClearCool();
                PoolManage.Instance.PushGameObject(seed.gameObject.name, seed.gameObject);
                //特效
                GameObject effect = PoolManage.Instance.GetPoolGameObject("ParticleSystem", "ChangeCardElement", seed1.transform);
                effect.transform.localScale = Vector3.one;
                effect.transform.localPosition = Vector3.zero;
                RandomUtil.AddOrGetComponent<TimeDestory>(effect).Init(1f);
                Color color = Color.white;
                foreach(Attribute.DreamElement dreamElement in plantCard.DreamElement)
                {
                    switch (dreamElement)
                    {
                        case DreamElement.Default:
                            color = Color.Lerp(color, Color.black, 0.5f);
                            break;
                        case DreamElement.Light:
                            color = Color.Lerp(color, Color.yellow, 0.5f);
                            break;
                        case DreamElement.Ice:
                            color = Color.Lerp(color, Color.blue, 0.5f);
                            break;
                    }
                }
                color.a = 0.5f;
                ParticleSystem.MainModule particleSystem = effect.GetComponent<ParticleSystem>().main;
                particleSystem.startColor = color;
            }
        }
    }
    public static int GetElementPlant(List<Attribute.DreamElement> list, int originid)
    {
        if (list.Count <= 0)
        {
            return originid % 100;//回传基底植物
        }
        //搜索同基植物
        List<Attribute.PlantInfo> plantcard = new List<PlantInfo>();
        foreach (int i in Attribute.Instance.filedInfo.Unclockplantid)
        {
            if (i % 100 == originid % 100)
            {
                plantcard.Add(Attribute.Instance.GetPlantInfo(i));
            }
        }
        if (plantcard.Count <= 0)
        {
            return originid % 100;//回传基底植物
        }
        //去除不满足条件的植物
        List<Attribute.PlantInfo> plantcard2 = new List<PlantInfo>(plantcard);
        foreach (Attribute.PlantInfo unitCardInfo in plantcard)
        {
            if (GetMatchCount(unitCardInfo, list))
            {
                plantcard2.Remove(unitCardInfo);
            }
        }
        if (plantcard2.Count <= 0)
        {
            return originid % 100;//回传基底植物
        }
        //按tag数排序
        plantcard2.Sort((x, y) => y.DreamElement.Count - x.DreamElement.Count);
        int id = plantcard2.First().ID;
        if (!plantcard2.First().DreamElement.Contains(list.Last()))//若tag数最多的植物不满足最后一个tag
        {
            foreach(PlantInfo PlantInfo in plantcard2)//遍历所有植物
            {
                if (PlantInfo.DreamElement.Contains(list.Last()))//查找第一个满足最后一个tag的植物
                {
                    id = PlantInfo.ID;
                    break;
                }
            }
        }
        return id;//回传基底植物
    }
    public static bool GetMatchCount(PlantInfo unit,List<Attribute.DreamElement> elementBankList)//包含未出现的tag
    {
        foreach(Attribute.DreamElement dreamElement in unit.DreamElement)
        {
            if (!elementBankList.Contains(dreamElement))
            {
                return true;
            }
        }
        return false;
    }
    public void CanUsePlantCard()
    {
        for (int i = 0; i < BankList.transform.childCount; i++)
        {
            BankList.transform.GetChild(i).GetChild(3).gameObject.SetActive(false);
            BankList.transform.GetChild(i).GetComponent<Seed>().incool = Cardtype.CanUse;
            BankList.transform.GetChild(i).GetComponent<Button>().onClick.AddListener(BankList.transform.GetChild(i).GetComponent<Seed>().OnCilck);
            BattleManage.Instance.checkCardCost += BankList.transform.GetChild(i).GetComponent<Seed>().SunEnoughorNot;
        }
        for (int i = 0; i < ElementBankList.transform.childCount; i++)
        {
            ElementBankList.transform.GetChild(i).GetComponent<ElementSeed>().ChangeUse(true);
        }
        BattleManage.Instance.checkCardCost?.Invoke();
    }
}
