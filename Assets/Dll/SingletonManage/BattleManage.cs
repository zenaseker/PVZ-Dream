using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using UnityEngine.UI;
using static Attribute;
using static BattleManage;
using Random = UnityEngine.Random;
using Transform = UnityEngine.Transform;

public class BattleManager
{
    private static BattleManager _instance;
    public static BattleManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new BattleManager();
            }
            return _instance;
        }
    }

    //攻击范围射线检测
    public static RaycastHit2D GetRay(Vector2 Pos, Vector2 dir, float dis, int layer)
    {
        if (layer == -1)
        {
            RaycastHit2D raycastHit2D2 = Physics2D.Raycast(Pos, dir, dis);
            return raycastHit2D2;
        }
        RaycastHit2D raycastHit2D = Physics2D.Raycast(Pos, dir, dis, layer);
        return raycastHit2D;
    }
    public static RaycastHit2D[] GetRayAll(Vector2 Pos, Vector2 dir, float dis, int layer)
    {
        if (layer == -1)
        {
            RaycastHit2D[] raycastHit2D2 = Physics2D.RaycastAll(Pos, dir, dis);
            return raycastHit2D2;
        }
        RaycastHit2D[] raycastHit2D = Physics2D.RaycastAll(Pos, dir, dis, layer);
        return raycastHit2D;
    }

}

public enum BattleStage
{
    ChooseCard,//选卡
    BattleReady,//战前预备
    InBattle,//战斗中
    LastFlagOut,//最后一波出完
    End//结束
}

public enum ZomieUpdateType
{
    DoNot,//不刷新
    Default,//常速
    InFLag//大波次
}

public class BattleManage : Singleton<BattleManage>
{
    public Level level = null;
    #region 时间相关
    [Header("时间控制")]
    [Tooltip("时间流动")]
    public float TimeRun;
    [Tooltip("关卡阶段")]
    public BattleStage battleStage = BattleStage.ChooseCard;
    //"BGM"
    string Music = "battle";
    //是否暂停
    public bool TimeStop = false;
    //是否可以生产阳光
    public bool CanInitSun = false;
    #endregion
    #region 实体
    [Header("实体")]
    //阳光生成X轴范围
    float leftestPosX = -8f;
    float rightestPosX = 4.7f;
    [Tooltip("阳光基体")]
    public GameObject Sun;
    //阳光生成冷却
    float suninitcooltime;
    [Tooltip("僵尸时间轴")]
    public GameObject FlagMeter;
    [Tooltip("背景及天气")]
    public Transform SkyManage;
    [Tooltip("中央字幕")]
    public GameObject CenterTex;
    [Tooltip("僵尸管理器")]
    public Transform zombiesmanage;
    [Tooltip("进行图标")]
    public Slider Image;
    [Tooltip("卡片栏")]
    public GameObject CardUI;
    [Tooltip("工具栏")]
    public GameObject FunctionalArea;
    [Tooltip("旗帜图标")]
    public GameObject Flag;
    [Tooltip("奖杯")]
    public GameObject Trophy;
    [Tooltip("死亡显示")]
    public GameObject GameLose;
    [Tooltip("关卡名称")]
    public GameObject LevelName;
    [Tooltip("梦境深度显示")]
    public GameObject DreamDepth;
    [Tooltip("难度显示")]
    public GameObject Difficulty;
    [Tooltip("自由卡片位置")]
    public Transform CardJump;
    [Tooltip("动画列表")]
    public List<TimelineAsset> Assets;
    #endregion
    #region 梦境有关
    public int LevelDreamDepth = 0;
    public Action<int> dreamdepthchange;
    #endregion
    public BattleControlBase controlBase = null;


    #region 阳光
    public static GameObject CreateSun(Vector3 pos, int sun, bool jumpormove = true)
    {
        GameObject obj2 = PoolManage.Instance.GetPoolGameObject("Bullet", "Sun");
        obj2.transform.position = pos;
        if (jumpormove)
        {
            RandomUtil.AddOrGetComponent<ObjectJump>(obj2).Move(new Vector3(pos.x + Random.Range(-1f,1f),pos.y - 1f,0), obj2.gameObject.GetComponent<Sun>().CheckSun);
        }
        else
        {
            RandomUtil.AddOrGetComponent<ObjectMove>(obj2).Move(obj2.gameObject.GetComponent<Sun>().CheckSun);
        }
        RandomUtil.AddOrGetComponent<Sun>(obj2).ChangeNum(sun);
        return obj2;
    }

    public Action checkCardCost;

    //显示阳光数目
    public void SunnumberSet()
    {
        if (GameObject.Find("CardChooseUI") != null)
        {
            UImanage.Instance.CheckSun();
        }
        checkCardCost?.Invoke();
    }

    //变更阳光
    public void SunnumberChange(int num)
    {
        if (SunNumber < 0)
        {
            SunNumber = 0;
        }
        SunNumber += num;
        if (num < 0)
        {
            SunNumberUse += num;
        }
        int max = 9999;
        if (LevelDreamDepth >= 15)
        {
            max -= LevelDreamDepth * 50;
        }
        if (SunNumber > max)
        {
            SunNumber = max;
        }
        SunnumberSet();
    }


    //dange阳光数量
    public int OneSunNumber = 25;
    //阳光数量(只读)
    public int SunNumber { get; set; }
    //累计消耗阳光数
    public int SunNumberUse { get; set; }
    #endregion
    private void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            this.StopGame();
        }
        if (battleStage == BattleStage.ChooseCard || battleStage == BattleStage.End) return;
        CreateSun();
        TimeRun += Time.deltaTime;
        controlBase?.OnUpdate();
    }
    public void CreateSun()
    {
        if (!CanInitSun) return;
        this.suninitcooltime -= Time.deltaTime;
        if (this.suninitcooltime < 0)
        {
            BattleManage.CreateSun(new Vector3(Random.Range(leftestPosX, rightestPosX), 6f, -5f), 25, false);
            this.suninitcooltime = 8f;
        }
    }
    public void TimeStart()
    {
        TimeRun = 0f;
        battleStage = BattleStage.InBattle;
        ZombieManage.Instance.zomieUpdateType = ZomieUpdateType.Default;
        FlagMeter.SetActive(true);
        MusicManage.Instance.PlayEffect("awooga",1);
        controlBase?.OnStartCreateZombie();
    }
    public void PlayAssets(int num)
    {
        PlayableDirector playableDirector = BattleManage.Instance.GetComponent<PlayableDirector>();
        playableDirector.playableAsset = null;
        playableDirector.playableAsset = Assets[num];
        playableDirector.time = 0;
        playableDirector.Play();
    }
    public void ChangeDreamDepth()
    {
        DreamDepth.GetComponent<TextMeshProUGUI>().text = "梦深-" + LevelDreamDepth;
        dreamdepthchange?.Invoke(LevelDreamDepth);
    }
    public void EndLevel()
    {
        if (level.Type == LevelType.Life)
        {
            EndByLifeLevel();
            return;
        }
        PlayableDirector playableDirector = BattleManage.Instance.GetComponent<PlayableDirector>();
        playableDirector.playableAsset = null;
        playableDirector.time = 0;
        playableDirector.playableAsset = Assets[0];
        playableDirector.Stop();
        CancelInvoke();
    }
    public void ReturnMianMenu(bool win)
    {
        Time.timeScale = Attribute.Instance.filedInfo.GameSpeed;
        if (win)
        {
            bool show = false;
            if (Attribute.Instance.levelAttribute.unclockplantid.Count > 0 && Attribute.Instance.levelAttribute.unclockplantid[0] != 0)
            {
                switch (level.Type)
                {
                    case LevelType.Adventure:
                        if (Attribute.Instance.filedInfo.MianFinishLevel < level.ID)
                        {
                            show = true;
                        }
                        break;
                    case LevelType.Dream:
                        if (!Attribute.Instance.filedInfo.DreamFinishLevel.Contains(level.ID))
                        {
                            show = true;
                        }
                        break;
                }
            }
            Attribute.SaveFiled(Attribute.Instance.levelAttribute.ID, Attribute.Instance.levelAttribute.Type);
            controlBase?.OnLevelEnd();
            if (show)
            {
                Attribute.ChangeScene("GetCardShow");
                return;
            }
        }
        Attribute.ChangeScene("MianMenu");
    }
    public void ReStart()
    {
        if (level.Type == LevelType.Life)
        {
            if (SaveLoadManager.IsExistsData("life_" + level.ID, ".lfe"))
            {
                SaveLoadManager.Destory("life_" + level.ID, ".lfe");
            }
        }
        Attribute.ChangeScene("NormalLevel");
    }
    public void Start()
    {
        if (level.Type == LevelType.Life && SaveLoadManager.IsExistsData("life_" + level.ID, ".lfe"))
        {
            LifeLevelSave lifeLevelSave = SaveLoadManager.Load<LifeLevelSave>("life_" + level.ID, ".lfe");
            SunNumber = lifeLevelSave.SunNumber;
            SunNumberUse = 0;
            BattleManage.Instance.checkCardCost = null;
            SunnumberSet();
            ChangeDreamDepth();
            ZombieManage.Instance.Range = lifeLevelSave.RangeCount;
            HandManage.Instance.cellplantid = lifeLevelSave.DreamDepthPlant;
            foreach(LifeLevelSave.PlantInfoInLifeSave plantInfoInLifeSave in lifeLevelSave.plantInfoInLifeSaves)
            {
                HandManage.Instance.LifeCellPlant(plantInfoInLifeSave.Pos, plantInfoInLifeSave.ID,plantInfoInLifeSave.PlantPosType,out PlantBase plant);
                plant.HP = plantInfoInLifeSave.Hp;
            }
        }
    }
    public override void Awake()
    {
        base.Awake();
        //防出错关卡信息读取
#if UNITY_EDITOR
        if (Attribute.Instance.filedInfo == null)
        {
            DefaultFiled defaultFiled = SaveLoadManager.Load<DefaultFiled>("Filed", ".dfd");
            Attribute.Instance.filedInfo = SaveLoadManager.Load<FiledInfo>(defaultFiled.FiledName);
        }
#endif
        if (Attribute.Instance.levelAttribute == null)
        {
            Attribute.Instance.levelAttribute = Attribute.Instance.GetLevel(LevelType.Other, 0);
        }
        //载入关卡信息
        level = Attribute.Instance.levelAttribute.Clone();
        Music = level.Music;
        //载入阳光设置
        SunNumber = level.StartSunnumber;
        SunNumberUse = 0;
        BattleManage.Instance.checkCardCost = null;
        SunnumberSet();
        //修改地图背景
        GameObject.Destroy(SkyManage.GetChild(0).gameObject);
        GameObject newmap = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/Sky/BackGround_" + level.Map), SkyManage.transform);
        //修改显示
        LevelName.GetComponent<TextMeshProUGUI>().text = level.Name;
        Difficulty.GetComponent<TextMeshProUGUI>().text = "难度：" + Attribute.Instance.filedInfo.Difficulty;
        Difficulty.GetComponent<TextMeshProUGUI>().color = DifficultySet.color[Attribute.Instance.filedInfo.Difficulty];
        //载入关卡控制器
        if (level.BattleControl != null)
        {
            this.controlBase = level.BattleControl;
            this.controlBase.OnLevelInit();
            this.controlBase.Map = newmap;
        }
        //载入梦深设置
        switch (Attribute.Instance.filedInfo.Difficulty)
        {
            case 2:
                BattleManage.Instance.LevelDreamDepth = 2;
                break;
            case 3:
                BattleManage.Instance.LevelDreamDepth = 4;
                break;
            case 4:
                BattleManage.Instance.LevelDreamDepth = 6;
                break;
            case 5:
                BattleManage.Instance.LevelDreamDepth = 10;
                break;
        }
        dreamdepthchange = null;
        ChangeDreamDepth();
        OnLevelAwake();
    }
    public void OnLevelAwake()
    {
        //初始化时间
        battleStage = BattleStage.ChooseCard;
        TimeRun = 0f;
        Image.value = 0f;
        //BGM变为选卡
        MusicManage.Instance.ChangeBGM("SelectCard");
        //初始化动画
        FlagMeter.SetActive(false);
        GameLose.SetActive(false);
        this.PlayAssets(0);
    }
    public void EndByLifeLevel()
    {
        SaveLoadManager.Save<LifeLevelSave>("life_" + level.ID, GetLifeLevelSave(), ".lfe");
        GameObject.Destroy(this.CardUI);
        GameObject.Destroy(PropManage.Instance.gameObject);
        OnLevelAwake();
    }

    public LifeLevelSave GetLifeLevelSave()
    {
        LifeLevelSave lifeLevelSave = new LifeLevelSave(level.ID,SunNumber,ZombieManage.Instance.Range);
        lifeLevelSave.plantInfoInLifeSaves = new List<LifeLevelSave.PlantInfoInLifeSave>();
        lifeLevelSave.DreamDepthPlant = HandManage.Instance.cellplantid;
        GameObject plantmanage = GameObject.Find("PlantManage");
        for (int i = 0; i < plantmanage.transform.childCount; i++)
        {
            if (plantmanage.transform.GetChild(i).gameObject.tag == "Plant")
            {
                PlantBase plantBase = plantmanage.transform.GetChild(i).GetComponent<PlantBase>();
                lifeLevelSave.plantInfoInLifeSaves.Add(new LifeLevelSave.PlantInfoInLifeSave(plantBase.unitInfo.ID, plantBase.XY, plantBase.posType, plantBase.HP));
            }
        }
        return lifeLevelSave;
    }
    public void OnSelectCard()
    {
        if (level.CanUseProp)
        {
            FunctionalArea = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/Singleton/PropUI"));
            FunctionalArea.name = "PropUI";
            FunctionalArea.GetComponent<Canvas>().worldCamera = Camera.main;
        }
        if (level.LevelIcons == LevelIcon.Default)
        {
            this.CardUI = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/Singleton/CardChooseUI"));
            this.CardUI.name = "CardChooseUI";
            this.CardUI.GetComponent<Canvas>().worldCamera = Camera.main;
            this.CardUI.SetActive(true);
            this.controlBase?.BeforeSelcetCard();
        }
        else
        {
            Invoke("BackSellChoice", 0.1f);
        }
    }

    public void InitCart()
    {
        ClearCart();
        for (int i = 0; i < MapManage.Instance.meshxy.x; i++)
        {
            GameObject obj = GameObject.Find("CartManage");
            Vector3 vector3 = MapManage.Instance.meshpos[i, 0];
            vector3.x -= 2.6f;
            vector3.y -= 0.38f;
            GameObject gameObject = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/Cart"), obj.transform);
            gameObject.transform.position = vector3;
            gameObject.GetComponent<Cart>().Goto();
        }
    }
    public void ClearCart()
    {
        GameObject obj = GameObject.Find("CartManage");
        for (int i = 0;i < obj.transform.childCount; i++)
        {
            GameObject.Destroy(obj.transform.GetChild(i).gameObject);
        }
    }

    public void StopGame()
    {
        StopMenu.Instance.TimeStop();
    }
    public Action _BackSellChoice;
    public void BackSellChoice()
    {
        PlayAssets(1);
        _BackSellChoice?.Invoke();
    }
    public Action _GameStartOne;
    public void GameStartOne()
    {
        if (level.Type != LevelType.Life)
        {
            this.InitCart();
        }
        if (level.LevelIcons == LevelIcon.Conveyer)
        {
            this.CardUI = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/Singleton/ConveyerUI"));
            this.CardUI.name = "ConveyerUI";
            CardUI.SetActive(true);
            CardUI.GetComponent<Canvas>().worldCamera = Camera.main;
        }
        if (level.LevelIcons == LevelIcon.ClockCard)
        {
            this.CardUI = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/Singleton/CardChooseUI"));
            this.CardUI.name = "CardChooseUI";
            CardUI.SetActive(true);
            CardUI.GetComponent<Canvas>().worldCamera = Camera.main;
            CardUI.GetComponent<Animator>().Play("CardChooseUI3");
        }
        _GameStartOne?.Invoke();
        MusicManage.Instance.BGMCheck(false);
    }
    public void GameStartTwo()
    {
        if (level.LevelIcons == LevelIcon.Default)
        {
            UImanage.Instance.CanUsePlantCard();
        }
        MapManage.Instance.CellTombston();
        MusicManage.Instance.ChangeBGM(Music);
        suninitcooltime = 8f;
        ZombieManage.Instance.ClearZombie();
        battleStage = BattleStage.BattleReady;
        TimeStop = false;
        this.CanInitSun = level.CanCreateSun;
        this.controlBase?.OnLevelStart();
        Invoke("TimeStart", level.StartTime);
    }

    public void ZombieInHome()
    {
        Time.timeScale = 0f;
        TimeStop = true;
        GameLose.SetActive(true);
        MusicManage.Instance.BGMCheck(false);
        MusicManage.Instance.PlayEffect("scream", 1f);
        if (level.Type == LevelType.Life)
        {
            if (SaveLoadManager.IsExistsData("life_" + level.ID, ".lfe"))
            {
                SaveLoadManager.Destory("life_" + level.ID, ".lfe");
            }
        }
    }
    public void ClearPlant()
    {
        GameObject plantmanage = GameObject.Find("PlantManage");
        for (int i = 0;i < plantmanage.transform.childCount; i++)
        {
            GameObject.Destroy(plantmanage.transform.GetChild(i).gameObject);
        }
    }

    public void CreateJumpCard(int cardid,Vector3 pos)
    {
        int num = cardid;
        GameObject obj = PoolManage.Instance.GetPoolGameObject("SeedCard", Attribute.Instance.GetPlantInfo(num).CardPrefab.name, pos, CardJump);
        obj.transform.position = pos;
        RandomUtil.AddOrGetComponent<SeedJump>(obj).Init(Attribute.Instance.GetPlantInfo(num));
        RandomUtil.AddOrGetComponent<Button>(obj).onClick.AddListener(RandomUtil.AddOrGetComponent<SeedJump>(obj).OnCilck);
    }
}
