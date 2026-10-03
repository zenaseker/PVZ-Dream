using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Newtonsoft.Json;
using Random = UnityEngine.Random;
using UnityEngine.SceneManagement;
using System;
using DG.Tweening;

public class Attribute
{
    private static Attribute _instance;
    public static Attribute Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new Attribute();
                Attribute._plantInfos  = Resources.Load<PlantInfos>("ScriptableObject/PlantInfo");
                _plantInfos.InitializeDictionary();
                Attribute._zombieInfos = Resources.Load<ZombieInfos>("ScriptableObject/ZombieInfo");
                _zombieInfos.InitializeDictionary();
                Attribute._levelInfos = Resources.Load<LevelInfos>("ScriptableObject/LevelInfo");
                _levelInfos.InitializeDictionary();
            }
            return _instance;
        }
    }

    /// <summary>
    /// 切换场景
    /// </summary>
    /// <param name="sence">场景名</param>
    public static void ChangeScene(string sence)
    {
        DOTween.KillAll(true);
        if (PoolManage.Instance != null)
        {
            PoolManage.Instance.ClearPool();
        }
        GC.Collect();
        //if (sence == "NormalLevel")
        //{
        //    GameObject obj = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/ParticleSystem/GotoLevel"));
        //    obj.GetComponent<GotoLevel>().StartCoroutine(Attribute.Instance.GotoLevel(sence));
        //    return;
        //}
        //LoadingSence.StartGame(sence);
        SceneManager.LoadScene(sence);
    }

    //public IEnumerator GotoLevel(string sence)
    //{
    //    AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sence,LoadSceneMode.Single);
    //    asyncOperation.allowSceneActivation = false;
    //    while(true)
    //    {
    //        if (asyncOperation.progress >= 0.9f)
    //        {
    //            break;
    //        }
    //        yield return null;
    //    }
    //    asyncOperation.allowSceneActivation = true;
    //}


    #region 植僵ID储存序列

    /// <summary>
    /// 元素枚举
    /// </summary>
    public enum DreamElement
    {
        Default,//基本
        Light,//光系
        Ice,//冰系
        Dark,//暗系
        Soul,//灵系
        Dream,//梦系
    }

    /// <summary>
    /// 查找植物信息
    /// </summary>
    /// <param name="num">id</param>
    /// <returns>查找到的植物信息（没找到返回空）</returns>
    public PlantInfo GetPlantInfo(int id)
    {
        return PlantInfos.GetValue(id);
    }
    private static PlantInfos _plantInfos = null;
    public static PlantInfos PlantInfos
    {
        get
        {
            return _plantInfos;
        }
    }
    /// <summary>
    /// 植物组件字典
    /// </summary>
    public readonly Dictionary<string, ComponentBase> ComponentList = new Dictionary<string, ComponentBase>()
    {
        //光环
        {"LightSunAura",new LightSunAura() },
        {"LightChomperAura",new LightChomperAura() },
        {"AuroraAura",new AuroraAura() },
        {"MoonShroomAura",new MoonShroomAura() },
        {"StarShroomAura",new StarShroomAura() },
        {"SunMoonDoublePeaAura",new SunMoonDoublePeaAura() },
        {"DarkFrostAura",new DarkFrostAura() },
        //亡语
        {"BlinkDeathRattle",new BlinkDeathRattle() },
        {"FluctuatLightDeathRattle",new FluctuatLightDeathRattle() },
        {"IceCrystalDeathRattle",new IceCrystalDeathRattle() },
        //附魔
        {"IceblastEnchantment",new IceblastEnchantment() },
        {"IceRecoverEnchantment",new IceRecoverEnchantment() },
        {"IceSunEnchantment",new IceSunEnchantment() },
        {"AuroraEnchantment",new AuroraEnchantment() },
        {"MoonErosionEnchantment",new MoonErosionEnchantment() },
        {"MoonFumeEnchantment",new MoonFumeEnchantment() },
        {"MoonShroomEnchantment",new MoonShroomEnchantment() },
        {"SnowFumeShroomEnchantment",new SnowFumeShroomEnchantment() },
        {"DarkFrostEnchantment",new DarkFrostEnchantment() },
        //先天
        {"FluctuatLightInnate",new FluctuatLightInnate() },
        {"LunarEclipseScaredyShroomInnate",new PhantomShroomInnate() },
        {"StarShroomInnate",new StarShroomInnate() },
        {"PhantomShroomInnate",new PhantomShroomInnate() },
        //术阵
        {"FireFlyMatrix",new FireFlyMatrix() },
        {"HalationMatrix",new HalationMatrix() },
        {"ShadowPuffMatrix",new ShadowPuffMatrix() },
        {"MoonFumeMatrix",new MoonFumeMatrix() },
        {"LunarEclipseScaredyShroomMatrix",new LunarEclipseScaredyShroomMatrix() },
        {"SunMoonDoubleMatrix",new SunMoonDoubleMatrix() },
        {"MidNightDoubleMatrix",new MidNightDoubleMatrix() },
    };

    /// <summary>
    /// 查询融合列表
    /// </summary>
    /// <param name="one">融合植物1id</param>
    /// <param name="two">融合植物2id</param>
    /// <returns>融合后的植物id</returns>
    public int GetFusion(int one,int two)
    {
        int num = -1;
        Vector2Int try1 = new Vector2Int(one, two);
        Vector2Int try2 = new Vector2Int(two, one);
        if (Attribute.Instance.FusionList.ContainsKey(try1))
        {
            num = Attribute.Instance.FusionList[try1];
        }
        if (Attribute.Instance.FusionList.ContainsKey(try2))
        {
            num = Attribute.Instance.FusionList[try2];
        }
        return num;
    }

    /// <summary>
    /// 融合植物列表
    /// </summary>
    public readonly Dictionary<Vector2Int, int> FusionList = new Dictionary<Vector2Int, int>
    {
        { new Vector2Int(101,201),1001 },
        { new Vector2Int(107,307),1007 },
        { new Vector2Int(210,310),1010 },
    };


    /// <summary>
    /// 随机植物筛选
    /// </summary>
    /// <param name="plantlevel">植物等级选取：0:仅原版植物; 1:原版+梦境单元素; 2:原版+梦境单或双元素; 3:原版+梦境单或双或三元素; 4:所有植物</param>
    /// <param name="daynum">植物进度选取：0:白天; 1:黑夜; 2:泳池; 3:迷雾;: 4:屋顶; 5:屋顶-黑夜</param>
    /// <returns>经过筛选后的植物列表</returns>
    public List<int> Getplantclocklist(int plantlevel,int daynum)
    {
        List<int> list = new List<int>();
        foreach (PlantInfo plantInfo in _plantInfos.PlantInfoes)
        {
            list.Add(plantInfo.ID);
        }
        switch (plantlevel)
        {
            case 0:
                list.RemoveAll(x => x >= 100);
                break;
            case 1:
                list.RemoveAll(x => x >= 1000);
                break;
            case 2:
                list.RemoveAll(x => x >= 10000);
                break;
            case 3:
                list.RemoveAll(x => x >= 100000);
                break;
            case 4:
                break;
        }
        switch (daynum)
        {
            case 0:
                list.RemoveAll(x => x % 100 >= 8);
                break;
            //case 1:
            //    list.RemoveAll(x => x % 100 >= 8);
            //    break;
            //case 2:
            //    list.RemoveAll(x => x % 100 >= 8);
            //    break;
            //case 3:
            //    list.RemoveAll(x => x % 100 >= 8);
            //    break;
            //case 4:
            //    list.RemoveAll(x => x % 100 >= 8);
            //    break;
            //case 5:
            //    list.RemoveAll(x => x % 100 >= 8);
            //    break;
        }
        return list;
    }


    /// <summary>
    /// 查找植物信息
    /// </summary>
    /// <param name="num">id</param>
    /// <returns>查找到的植物信息（没找到返回空）</returns>
    public ZombieInfo GetZombieInfo(int id)
    {
        return ZombieInfos.GetValue(id);
    }
    private static ZombieInfos _zombieInfos = null;
    public static ZombieInfos ZombieInfos
    {
        get
        {
            return _zombieInfos;
        }
    }
    /// <summary>
    /// 僵尸信息
    /// </summary>
    [System.Serializable]
    public class ZombieInfo:UnitInfo
    {
        //生成权重
        public int Weight = 4000;
        //价值
        public int Value = 1;
        //最小生成波次
        public int MinFlag = 0;
        //是衍生僵尸
        public bool IsCreateZombie = false;
        //僵尸信息
        public ZombieDescribe zombieDescribe = new ZombieDescribe();
        public ZombieInfo Clone()
        {
            return (ZombieInfo)MemberwiseClone();
        }
    }

    /// <summary>
    /// 植物信息
    /// </summary>
    [System.Serializable]
    public class PlantInfo : UnitInfo
    {
        //梦境深度增加
        public int DreamAdd;
        //植物位置
        public PlantPosType Plantpostype = PlantPosType.Default;
        //植物种类
        public Plantstics Plantstic = Plantstics.Other;
        //植物类型
        public PlantType Planttype = PlantType.Attack;
        //植物信息
        public PlantDescribe plantDescribe = new PlantDescribe();
        public PlantInfo Clone()
        {
            return (PlantInfo)MemberwiseClone();
        }

    }
    public class UnitInfo
    {
        //id
        public int ID;
        //卡片费用
        public int Cost;
        //血量
        public int HP;
        //攻击伤害
        public int Damage;
        //卡片冷却时间
        public float UseCoolTime;
        //预制体名称
        public GameObject Prefab;
        //预制体名称
        public GameObject CardPrefab;
        //僵尸元素列表
        public List<DreamElement> DreamElement;
    }

    #endregion




    private static Dictionary<string,AudioClip> music = new Dictionary<string,AudioClip>();
    /// <summary>
    /// 调取音频（仅在指定文件夹内）
    /// </summary>
    /// <param name="name">音频名</param>
    /// <returns>读取到的音频</returns>
    public static AudioClip GetMusic(string name)
    {
        if (music.ContainsKey(name))
        {
            return music[name];
        }
        AudioClip audioClip = Resources.Load<AudioClip>("AudioClip/"+name);
        music.Add(name, audioClip);
        return audioClip;
    }
    /// <summary>
    /// 常用概率音频列表
    /// </summary>
    public readonly Dictionary<string,List<string>> NormalMusic = new Dictionary<string,List<string>>
    {
        {"ZombieAttack",new List<string> {"chomp", "chomp2", "chompsoft"}},
        {"PeaBulletHit",new List<string>{"splat","splat2","splat3" }},
        {"IconAmrorHited",new List<string>{ "shieldhit", "shieldhit2"}},
        {"bowling",new List<string>{ "bowlingimpact", "bowlingimpact2"}},
    };




    /// <summary>
    /// 图片列表
    /// </summary>
    private static Dictionary<string, Sprite> Sprites = new Dictionary<string, Sprite>();
    /// <summary>
    ///读取图片
    /// </summary>
    /// <param name="name">图片名</param>
    /// <param name="floder">详细文件夹名称</param>
    /// <returns>读取到的图片</returns>
    public static Sprite GetSprite(string name,string floder = null)
    {
        if (Sprites.ContainsKey(name))
        {
            return Sprites[name];
        }
        Sprite gameObject = Resources.Load<Sprite>("Texture2D/" + ((floder!=null)?floder + "/":"") + name);
        Sprites.Add(name, gameObject);
        return gameObject;
    }
    private static LevelInfos _levelInfos = null;
    public static LevelInfos LevelInfos
    {
        get
        {
            return _levelInfos;
        }
    }
    /// <summary>
    /// 关卡信息字典
    /// </summary>
    public readonly Dictionary<LevelType,List< Level>> Levels = new Dictionary<LevelType, List<Level>>
    {
        {
            LevelType.Other,new List<Level>
            {
            new Level{ID = 0, Name = "Debug关卡",Type = LevelType.Other,plantmesh = "Night",Map = "Night",Music = "Day",girdKey = GridKey.Day,StartSunnumber = 5000,StartTime = 0f
            ,ZombieCountPower = 1,FlagCount = 30,ZombiesID = new List<int>{ 0, 1, 2, 3, 4,7,101,102,1001 },unclockplantid = new List<int>{ 0 }  },
            }
        },
        {
            LevelType.Adventure,new List<Level>
            {
            new Level{ID = 0, Name = "白天-1",Type = LevelType.Adventure,plantmesh = "DayOneLine",Map = "DayOneLine",Music = "Day",KeyImage = "PeaShooter",StartSunnumber = 400,StartTime = 20f,girdKey = GridKey.Day
            ,ZombieCountPower = 1,CanCreateSun = true,FlagCount = 10,ZombiesID = new List<int>{ 0 },unclockplantid = new List<int>{ 1 }  },
            
                new Level{ID = 1, Name = "白天-2",Type = LevelType.Adventure,plantmesh = "DayOneLine",Map = "DayOneLine",Music = "Day",KeyImage = "SunFlower",StartSunnumber = 300,StartTime = 20f,girdKey = GridKey.Day
            ,ZombieCountPower = 1,CanCreateSun = true,FlagCount = 10,ZombiesID = new List<int>{ 0 },unclockplantid = new List<int>{ 2 }  },
           
                new Level{ID = 2, Name = "白天-3",Type = LevelType.Adventure,plantmesh ="Day",Map = "Day",Music = "Day",KeyImage = "CherryBomb",StartSunnumber = 200,StartTime = 20f,girdKey = GridKey.Day
            ,ZombieCountPower = 1,CanCreateSun = true,FlagCount = 10,ZombiesID = new List<int>{ 0,1 },unclockplantid = new List<int>{ 3 }  },
           
                new Level{ID = 3, Name = "白天-4",Type = LevelType.Adventure,plantmesh = "Day",Map = "Day",Music = "Day",KeyImage = "WallNut",StartSunnumber = 150,StartTime = 20f,girdKey = GridKey.Day
            ,ZombieCountPower = 1,CanCreateSun = true,FlagCount = 20,ZombiesID = new List<int>{ 0,1,2 },unclockplantid = new List<int>{ 4 }  },
          
                new Level{ID = 4, Name = "白天-5",Type = LevelType.Adventure,plantmesh = "Day",Map = "Winter",Music = "Winter",KeyImage = "PotatoMine",girdKey = GridKey.Snow,StartSunnumber = 150,StartTime = 20f
            ,ZombieCountPower = 1.1f,CanCreateSun = true,FlagCount = 20,ZombiesID = new List<int>{ 102,1,2,3 },unclockplantid = new List<int>{ 5 }  },
          
                new Level{ID = 5, Name = "白天-6",Type = LevelType.Adventure,plantmesh = "Day",Map = "Winter",Music = "Winter",KeyImage = "SnowPea",girdKey = GridKey.Snow,StartSunnumber = 150,StartTime = 20f
            ,ZombieCountPower = 1.15f,CanCreateSun = true,FlagCount = 20,ZombiesID = new List<int>{ 102,1,2,3,4 },unclockplantid = new List<int>{ 6 }  },
          
                new Level{ID = 6, Name = "白天-7",Type = LevelType.Adventure,plantmesh = "Day",Map = "Winter",Music = "Winter",KeyImage = "Chomper",girdKey = GridKey.Snow,StartSunnumber = 100,StartTime = 20f
            ,ZombieCountPower = 1.2f,CanCreateSun = true,FlagCount = 30,ZombiesID = new List<int>{ 102,1,3,4,101,103 },unclockplantid = new List<int>{ 7 }  },
          
                new Level{ID = 7, Name = "白天-8",Type = LevelType.Adventure,plantmesh = "Day",Map = "Winter",Music = "Winter",KeyImage = "DoublePea",girdKey = GridKey.Snow,StartSunnumber = 100,StartTime = 20f
            ,ZombieCountPower = 1.5f,CanCreateSun = true,FlagCount = 30,ZombiesID = new List<int>{ 0,1,2,102,3,4,101,103 },unclockplantid = new List<int>{ 8 }  },
         
                new Level{ID = 8, Name = "黑夜-1",Type = LevelType.Adventure,plantmesh = "Night",Map = "Night",Music = "Night",KeyImage = "PuffShroom",girdKey = GridKey.Night,StartSunnumber = 200,StartTime = 20f
            ,ZombieCountPower = 1,FlagCount = 10,ZombiesID = new List<int>{ 0,1 },unclockplantid = new List<int>{ 9 },BattleControl = new BattleControl_Night1()  },
        
                new Level{ID = 9, Name = "黑夜-2",Type = LevelType.Adventure,plantmesh = "Night",Map = "Night",Music = "Night",KeyImage = "SunShroom",girdKey = GridKey.Night,StartSunnumber = 200,StartTime = 20f
            ,ZombieCountPower = 1,FlagCount = 10,ZombiesID = new List<int>{ 0,1,2 },unclockplantid = new List<int>{ 10 }  },

                new Level{ID = 10, Name = "黑夜-3",Type = LevelType.Adventure,plantmesh = "Night",Map = "Night",Music = "Night",KeyImage = "FumeShroom",girdKey = GridKey.Night,StartSunnumber = 200,StartTime = 20f
            ,ZombieCountPower = 1,FlagCount = 12,ZombiesID = new List<int>{ 0,1,2,5,102 },unclockplantid = new List<int>{ 11 } },

                new Level{ID = 11, Name = "黑夜-4",Type = LevelType.Adventure,plantmesh = "Cemetery",Map = "Cemetery",Music = "Cemetery",KeyImage = "ScaredyShroom",girdKey = GridKey.Cemetery,StartSunnumber = 200,StartTime = 20f
            ,ZombieCountPower = 1.1f,FlagCount = 14,ZombiesID = new List<int>{ 0,1,2,5,102,101 },unclockplantid = new List<int>{ 12 },Tombston = 2  },

                new Level{ID = 12, Name = "黑夜-5",Type = LevelType.Adventure,plantmesh = "Cemetery",Map = "Cemetery",Music = "Cemetery",KeyImage = "Gravebuster",girdKey = GridKey.Cemetery,StartSunnumber = 200,StartTime = 20f
            ,ZombieCountPower = 1.15f,FlagCount = 16,ZombiesID = new List<int>{ 0,1,2,5,6,101,102 },unclockplantid = new List<int>{ 13 },Tombston = 3  },

                new Level{ID = 13, Name = "黑夜-6",Type = LevelType.Adventure,plantmesh = "Cemetery",Map = "Cemetery",Music = "Cemetery",KeyImage = "HypnoShroom",girdKey = GridKey.Cemetery,StartSunnumber = 200,StartTime = 20f
            ,ZombieCountPower = 1.2f,FlagCount = 18,ZombiesID = new List<int>{ 102,5,7,102,106,110 },unclockplantid = new List<int>{ 14 },Tombston = 4  },

                new Level{ID = 14, Name = "黑夜-7",Type = LevelType.Adventure,plantmesh = "Cemetery",Map = "Cemetery",Music = "Cemetery",KeyImage = "IceShroom",girdKey = GridKey.Cemetery,StartSunnumber = 200,StartTime = 20f
            ,ZombieCountPower = 1.25f,FlagCount = 20,ZombiesID = new List<int>{ 102,1,5,6,7,101,106,111 },unclockplantid = new List<int>{ 15 },Tombston = 5  },

                new Level{ID = 15, Name = "黑夜-End",Type = LevelType.Adventure,plantmesh = "Cemetery",Map = "Cemetery",Music = "Cemetery",KeyImage = "DoomShroom",StartSunnumber = 200,StartTime = 20f,girdKey = GridKey.Cemetery
            ,ZombieCountPower = 1.4f,CanCreateSun = false,FlagCount = 20,ZombiesID = new List<int>{ 10001,306,6,7,111,307,407 },unclockplantid = new List<int>{ 0 },Tombston = 7 },

            }
        },
        {
            LevelType.Challenge,new List<Level>
            {
            new Level{ID = 1, Name = "撑杆集会",Type = LevelType.Challenge,plantmesh = "Day",Map = "Day",Music = "loon",KeyImage = "Challenge_Thumbnails_18",StartSunnumber = 300,StartTime = 20f
            ,ZombieCountPower = 1,CanCreateSun = true,FlagCount = 20,ZombiesID = new List<int>{ 3,1001 },unclockplantid = new List<int>{ 0 } },
        
                new Level{ID = 2, Name = "传送带",Type = LevelType.Challenge,plantmesh = "Day",Map = "Day",Music = "loon",KeyImage = "Conveyer",StartSunnumber = 300,StartTime = 20f
            ,ZombieCountPower = 1.15f,FlagCount = 18,ZombiesID = new List<int>{ 0,1,2,3,4 },unclockplantid = new List<int>{ 0 },LevelIcons = LevelIcon.Conveyer },
       
                new Level{ID = 3, Name = "晨昏线",Type = LevelType.Challenge,plantmesh = "Day",Map = "Terminator",Music = "loon",KeyImage = "GroundDayAndNight",StartSunnumber = 300,StartTime = 20f
            ,ZombieCountPower = 1.2f,CanCreateSun = true,FlagCount = 26,ZombiesID = new List<int>{ 0,102,2,4,101,5 },unclockplantid = new List<int>{ 0 },BattleControl = new BattleControl_DayNightLine() },

                new Level{ID = 4, Name = "植物僵尸",Type = LevelType.Challenge,plantmesh = "Day",Map = "Day",Music = "loon",KeyImage = "Challenge_Thumbnails_0",StartSunnumber = 400,StartTime = 20f
            ,ZombieCountPower = 1.1f,FlagCount = 10,ZombiesID = new List<int>{ 108,109 },unclockplantid = new List<int>{ 0 },CanCreateSun = true },

                new Level{ID = 5, Name = "仲夏夜之梦",Type = LevelType.Challenge,plantmesh = "Night",Map = "Night",Music = "loon",KeyImage = "Zombie_Mozzie",StartSunnumber = 400,StartTime = 20f
            ,ZombieCountPower = 1,FlagCount = 20,ZombiesID = new List<int>{ 112 },unclockplantid = new List<int>{ 0 },LevelIcons = LevelIcon.Default },

                new Level{ID = 6, Name = "暴风雪",Type = LevelType.Challenge,plantmesh = "Day",Map = "Winter",Music = "loon",KeyImage = "SnowStrom",StartSunnumber = 400,StartTime = 20f
            ,ZombieCountPower = 1.1f,FlagCount = 20,ZombiesID = new List<int>{ 102,103,3,4,101,6 },unclockplantid = new List<int>{ 0 },LevelIcons = LevelIcon.Default,CanCreateSun = true,BattleControl = new BattleControl_SnowStrom() },

                new Level{ID = 7, Name = "随机盒子",Type = LevelType.Challenge,plantmesh = "Day",Map = "Day",Music = "loon",KeyImage = "Present",StartSunnumber = 1000,StartTime = 20f
            ,ZombieCountPower = 1.1f,FlagCount = 20,ZombiesID = new List<int>{ 0,1,2,3,4,5,6,7 },unclockplantid = new List<int>{ 503 },LevelIcons = LevelIcon.ClockCard,CanCreateSun = true,ControlCardid = 1 },

            new Level{ID = 8, Name = "舞王巡演",Type = LevelType.Challenge,plantmesh = "Night",Map = "Night",Music = "loon",KeyImage = "Zombie_Jackson",StartSunnumber = 500,StartTime = 20f
            ,ZombieCountPower = 1,CanCreateSun = false,FlagCount = 30,ZombiesID = new List<int>{ 7,111,307,407 },unclockplantid = new List<int>{ 0 } },

                new Level{ID = 9, Name = "植物僵尸2",Type = LevelType.Challenge,plantmesh = "Day",Map = "Day",Music = "loon",KeyImage = "Challenge_Thumbnails_0",StartSunnumber = 300,StartTime = 16f
            ,ZombieCountPower = 1.2f,FlagCount = 20,ZombiesID = new List<int>{ 108,109,114,115 },unclockplantid = new List<int>{ 0 },CanCreateSun = true },

            new Level{ID = 10, Name = "舞王巡演-终焉降临",Type = LevelType.Challenge,plantmesh = "Cemetery",Map = "Cemetery",Music = "Cemetery",KeyImage = "Challenge_Thumbnails_23",StartSunnumber = 600,StartTime = 20f
            ,ZombieCountPower = 2,CanCreateSun = false,FlagCount = 30,ZombiesID = new List<int>{ 10001,7,111,307,407 },unclockplantid = new List<int>{ 0 } },

            }
        },
        {
            LevelType.LittleGame,new List<Level>
            {
                new Level{ID = 1, Name = "坚果保龄球",Type = LevelType.LittleGame,plantmesh = "WallNutBall",Map = "WallNutBall",Music = "loon",KeyImage = "Challenge_Thumbnails_6",StartSunnumber = 0,StartTime = 20f
            ,ZombieCountPower = 1,FlagCount = 16,ZombiesID = new List<int>{ 0,1,2 },unclockplantid = new List<int>{ 0 },LevelIcons = LevelIcon.Conveyer,LaterZombieInit = true,BattleControl = new BattleControl_WallNutBall() },

                new Level{ID = 2, Name = "元素保龄球",Type = LevelType.LittleGame,plantmesh = "WallNutBall",Map = "WallNutBall",Music = "loon",KeyImage = "HalationWallNut",StartSunnumber = 0,StartTime = 20f
            ,ZombieCountPower = 1,FlagCount = 16,ZombiesID = new List<int>{ 0,1,2,6 },unclockplantid = new List<int>{ 0 },LevelIcons = LevelIcon.Conveyer,LaterZombieInit = true,BattleControl = new BattleControl_WallNutBall2() },

                new Level{ID = 3, Name = "123木头人",Type = LevelType.LittleGame,plantmesh = "DayOneLine",Map = "DayOneLine",Music = "loon",KeyImage = "Challenge_Thumbnails_22",StartSunnumber = 0,StartTime = 5f
            ,ZombieCountPower = 1,FlagCount = 10,ZombiesID = new List<int>{ 0 },unclockplantid = new List<int>{ 0 },LevelIcons = LevelIcon.NoCard,BattleControl = new BattleControl_OneTwoThree() },

                new Level{ID = 4, Name = "链式反应",Type = LevelType.LittleGame,plantmesh = "Day",Map = "Day",Music = "loon",KeyImage = "GNCattail",StartSunnumber = 500,StartTime = 20f
            ,ZombieCountPower = 2f,FlagCount = 40,ZombiesID = new List<int>{ 0 },unclockplantid = new List<int>{ 0 },LevelIcons = LevelIcon.ClockCard,CanCreateSun = true,LaterZombieInit = true,ControlCardid = 2,BattleControl = new BattleControl_Chainreaction() },

                new Level{ID = 5, Name = "变色龙",Type = LevelType.LittleGame,plantmesh = "DayOneLine",Map = "DayOneLine",Music = "loon",KeyImage = "Almanac_Chameleon",StartSunnumber = 0,StartTime = 5f
            ,ZombieCountPower = 0f,FlagCount = 30,ZombiesID = new List<int>{ 0 },unclockplantid = new List<int>{ 0 },LevelIcons = LevelIcon.NoCard,BattleControl = new BattleControl_Chameleon() },

            }
        },
        {
            LevelType.Life,new List<Level>
            {
            new Level{ID = 1, Name = "白天-无尽",Type = LevelType.Life,plantmesh = "Day",Map = "Day",Music = "Day",KeyImage = "PeaShooter",StartSunnumber = 400,StartTime = 20f,girdKey = GridKey.Day
            ,ZombieCountPower = 1,CanCreateSun = true,FlagCount = 20,ZombiesID = new List<int>{ 0,1,2 },unclockplantid = new List<int>{ 0 } },

            new Level{ID = 2, Name = "黑夜-无尽",Type = LevelType.Life,plantmesh = "Night",Map = "DNightay",Music = "Night",KeyImage = "PuffShroom",StartSunnumber = 400,StartTime = 20f,girdKey = GridKey.Night
            ,ZombieCountPower = 1,CanCreateSun = false,FlagCount = 20,ZombiesID = new List<int>{ 0,1,2 },unclockplantid = new List<int>{ 0 } },

            }
        },
        {
            LevelType.Dream,new List<Level>
            {
            new Level{ID = 100, Name = "昨日-1",Type = LevelType.Dream,plantmesh = "Day",Map = "Day",Music = "Day",KeyImage = "LightLevel_1",StartSunnumber = 400,StartTime = 20f,girdKey = GridKey.Day
            ,ZombieCountPower = 1,CanCreateSun = true,FlagCount = 10,ZombiesID = new List<int>{ 102,1,2 },Dream = new List<DreamElement>{ DreamElement.Light },unclockplantid = new List<int>{ 100,101,102 },BattleControl = new BattleControl_Light() },
            
                new Level{ID = 101, Name = "昨日-2",Type = LevelType.Dream,plantmesh = "Day",Map = "Day",Music = "Day",KeyImage = "LightLevel_2",StartSunnumber = 300,StartTime = 18f,girdKey = GridKey.Day
            ,ZombieCountPower = 1.2f,CanCreateSun = true,FlagCount = 12,ZombiesID = new List<int>{ 102,1,101,3 },Dream = new List<DreamElement>{ DreamElement.Light },unclockplantid = new List<int>{ 103,104,106 },BattleControl = new BattleControl_Light2() },

                new Level{ID = 102, Name = "昨日-3",Type = LevelType.Dream,plantmesh = "Day",Map = "Day",Music = "Day",KeyImage = "StarShroom",StartSunnumber = 300,StartTime = 18f,girdKey = GridKey.Day
            ,ZombieCountPower = 1.4f,CanCreateSun = true,FlagCount = 14,ZombiesID = new List<int>{ 102,2,101,4,6 },Dream = new List<DreamElement>{ DreamElement.Light },unclockplantid = new List<int>{ 107,109 } },

                new Level{ID = 110, Name = "昨日挑战-1",Type = LevelType.Dream,plantmesh = "Day",Map = "Day",Music = "Day",KeyImage = "Trophy",StartSunnumber = 200,StartTime = 18f,girdKey = GridKey.Day
            ,ZombieCountPower = 1.6f,CanCreateSun = true,FlagCount = 20,ZombiesID = new List<int>{ 102,101,2,3,4,5 },Dream = new List<DreamElement>{ DreamElement.Light },unclockplantid = new List<int>{ 0 } },
            
                new Level{ID = 111, Name = "昨日挑战-2",Type = LevelType.Dream,plantmesh = "Day",Map = "Day",Music = "Day",KeyImage = "Trophy",StartSunnumber = 200,StartTime = 15f,girdKey = GridKey.Day
            ,ZombieCountPower = 1.8f,CanCreateSun = true,FlagCount = 22,ZombiesID = new List<int>{ 101,102,3,4,5,106 },Dream = new List<DreamElement>{ DreamElement.Light },unclockplantid = new List<int>{ 0 } },
            
                new Level{ID = 112, Name = "昨日挑战-3",Type = LevelType.Dream,plantmesh = "Day",Map = "Day",Music = "Day",KeyImage = "Trophy",StartSunnumber = 150,StartTime = 15f,girdKey = GridKey.Day
            ,ZombieCountPower = 2,CanCreateSun = true,FlagCount =26,ZombiesID = new List<int>{ 101,102,3,4,5,106,107 },Dream = new List<DreamElement>{ DreamElement.Light },unclockplantid = new List<int>{ 0 } },
            
                new Level{ID = 200, Name = "初雪-1",Type = LevelType.Dream,plantmesh = "Day",Map = "Winter",Music = "Winter",KeyImage = "SnowLevel_1",StartSunnumber = 400,StartTime = 20f,girdKey = GridKey.Snow
            ,ZombieCountPower = 1,CanCreateSun = true,FlagCount = 10,ZombiesID = new List<int>{ 102,1,2,3 },Dream = new List<DreamElement>{ DreamElement.Ice },unclockplantid = new List<int>{ 201,202,203 },BattleControl = new BattleControl_Ice()  },
            
                new Level{ID = 201, Name = "初雪-2",Type = LevelType.Dream,plantmesh = "Night",Map = "WinterNight",Music = "Winter",KeyImage = "IceChomper",StartSunnumber = 300,StartTime = 18f,girdKey = GridKey.Snow
            ,ZombieCountPower = 1.2f,FlagCount = 12,ZombiesID = new List<int>{ 102,101,3,4 },Dream = new List<DreamElement>{ DreamElement.Ice },unclockplantid = new List<int>{ 206,210 },BattleControl = new BattleControl_Ice2()  },
            
                new Level{ID = 210, Name = "初雪挑战-1",Type = LevelType.Dream,plantmesh = "Day",Map = "Winter",Music = "Winter",KeyImage = "Trophy",StartSunnumber = 200,StartTime = 16f,girdKey = GridKey.Snow
            ,ZombieCountPower = 1.6f,CanCreateSun = true,FlagCount = 20,ZombiesID = new List<int>{ 2,3,4,101,102,103 },Dream = new List<DreamElement>{ DreamElement.Ice },unclockplantid = new List<int>{ 0 }},
           
                new Level{ID = 211, Name = "初雪挑战-2",Type = LevelType.Dream,plantmesh = "Day",Map = "Winter",Music = "Winter",KeyImage = "Trophy",StartSunnumber = 200,StartTime = 15f,girdKey = GridKey.Snow
            ,ZombieCountPower = 1.8f,FlagCount = 22,ZombiesID = new List<int>{ 101,102,103,3,4,6 },Dream = new List<DreamElement>{ DreamElement.Ice },unclockplantid = new List<int>{ 0 }},

                new Level{ID = 300, Name = "黯月-1",Type = LevelType.Dream,plantmesh = "Night",Map = "Night",Music = "Night",KeyImage = "NightPeaShooter",StartSunnumber = 400,StartTime = 20f,girdKey = GridKey.Night
            ,ZombieCountPower = 1,FlagCount = 10,ZombiesID = new List<int>{ 0,1,2 },Dream = new List<DreamElement>{ DreamElement.Dark },unclockplantid = new List<int>{ 300,307 }  },

                new Level{ID = 301, Name = "黯月-2",Type = LevelType.Dream,plantmesh = "Night",Map = "Night",Music = "Night",KeyImage = "DarkLevel_1",StartSunnumber = 400,StartTime = 20f,girdKey = GridKey.Night
            ,ZombieCountPower = 1f,FlagCount = 10,ZombiesID = new List<int>{ 102,1,2,5,7,101 },Dream = new List<DreamElement>{ DreamElement.Dark },unclockplantid = new List<int>{ 308,309,310 }  },

                new Level{ID = 302, Name = "黯月-3",Type = LevelType.Dream,plantmesh = "Night",Map = "Night",Music = "Night",KeyImage = "DarkLevel_2",StartSunnumber = 350,StartTime = 20f,girdKey = GridKey.Night
            ,ZombieCountPower = 1.2f,FlagCount = 12,ZombiesID = new List<int>{ 101,102,2,6,7,306,107 },Dream = new List<DreamElement>{ DreamElement.Dark },unclockplantid = new List<int>{ 312,313,315 }  },

                new Level{ID = 310, Name = "黯月挑战-1",Type = LevelType.Dream,plantmesh = "Night",Map = "Night",Music = "Night",KeyImage = "Trophy",StartSunnumber = 300,StartTime = 20f,girdKey = GridKey.Night
            ,ZombieCountPower = 1.4f,FlagCount = 20,ZombiesID = new List<int>{ 106,101,3,4,5,6,102,306 },Dream = new List<DreamElement>{ DreamElement.Dark },unclockplantid = new List<int>{ 0 } },

                new Level{ID = 311, Name = "黯月挑战-2",Type = LevelType.Dream,plantmesh = "Night",Map = "Night",Music = "Night",KeyImage = "Trophy",StartSunnumber = 300,StartTime = 18f,girdKey = GridKey.Night
            ,ZombieCountPower = 1.6f,FlagCount = 22,ZombiesID = new List<int>{ 4,101,5,6,102,107,306,307 },Dream = new List<DreamElement>{ DreamElement.Dark },unclockplantid = new List<int>{ 0 } },

                new Level{ID = 312, Name = "黯月挑战-3",Type = LevelType.Dream,plantmesh = "Night",Map = "Night",Music = "Night",KeyImage = "Trophy",StartSunnumber = 300,StartTime = 16f,girdKey = GridKey.Night
            ,ZombieCountPower = 1.8f,FlagCount = 25,ZombiesID = new List<int>{ 101,6,7,102,307,306,111 },Dream = new List<DreamElement>{ DreamElement.Dark },unclockplantid = new List<int>{ 0 } },

                new Level{ID = 400, Name = "魂灵-1",Type = LevelType.Dream,plantmesh = "Night",Map = "Cemetery",Music = "Cemetery",KeyImage = "SoulLevel_1",StartSunnumber = 400,StartTime = 20f,girdKey = GridKey.Cemetery
            ,ZombieCountPower = 1,FlagCount = 10,ZombiesID = new List<int>{ 0,1,102 },Dream = new List<DreamElement>{ DreamElement.Soul },unclockplantid = new List<int>{ 404, 406, 408 },Tombston = 3  },

                new Level{ID = 401, Name = "魂灵-2",Type = LevelType.Dream,plantmesh = "Night",Map = "Cemetery",Music = "Cemetery",KeyImage = "SoulGravebuster",StartSunnumber = 400,StartTime = 20f,girdKey = GridKey.Cemetery
            ,ZombieCountPower = 1.1f,FlagCount = 20,ZombiesID = new List<int>{ 0,1,5,6,7,102,106 },Dream = new List<DreamElement>{ DreamElement.Soul },unclockplantid = new List<int>{ 410, 411 },Tombston = 6  },

                new Level{ID = 500, Name = "梦沫-1",Type = LevelType.Dream,plantmesh = "Day",Map = "Day",Music = "Day",KeyImage = "LittleFish",StartSunnumber = 400,StartTime = 20f,girdKey = GridKey.Dream
            ,ZombieCountPower = 1,CanCreateSun = true,FlagCount = 10,ZombiesID = new List<int>{ 0,1,2,3,4,5,6,7 },Dream = new List<DreamElement>{ DreamElement.Dream },unclockplantid = new List<int>{ 501,502 }  },

                new Level{ID = 501, Name = "梦沫-2",Type = LevelType.Dream,plantmesh = "Day",Map = "Day",Music = "Day",KeyImage = "MeteorFlower",StartSunnumber = 200,StartTime = 20f,girdKey = GridKey.Dream
            ,ZombieCountPower = 1.5f,CanCreateSun = true,FlagCount = 20,ZombiesID = new List<int>{ 0,1,5,6,7,102,111 },Dream = new List<DreamElement>{ DreamElement.Dream },unclockplantid = new List<int>{ 504 }  },

                new Level{ID = 502, Name = "梦沫-3",Type = LevelType.Dream,plantmesh = "Day",Map = "Day",Music = "Day",KeyImage = "ProtoPeaShooter",StartSunnumber = 300,StartTime = 20f,girdKey = GridKey.Dream
            ,ZombieCountPower = 1f,CanCreateSun = true,FlagCount = 20,ZombiesID = new List<int>{ 0,3,4,5,6,101,109 },Dream = new List<DreamElement>{ DreamElement.Dream },unclockplantid = new List<int>{ 505 }  },

            new Level{ID = 1000, Name = "北地极光",Type = LevelType.Dream,plantmesh = "Day",Map = "Winter",Music = "Winter",KeyImage = "AuroraSunFlower",StartSunnumber = 200,StartTime = 16f,girdKey = GridKey.Snow
            ,ZombieCountPower = 2,CanCreateSun = true,FlagCount = 20,ZombiesID = new List<int>{ 102,101,103,1,2,3,4,5,106 },Dream = new List<DreamElement>{ DreamElement.Light,DreamElement.Ice },unclockplantid = new List<int>{ 1001 } },

            new Level{ID = 1001, Name = "日月争辉",Type = LevelType.Dream,plantmesh = "Night",Map = "Night",Music = "Night",KeyImage = "SunMoonDoublePea",StartSunnumber = 200,StartTime = 16f,girdKey = GridKey.Night
            ,ZombieCountPower = 2,FlagCount = 30,ZombiesID = new List<int>{ 102,1,2,3,4,5,6,101,107,106 },Dream = new List<DreamElement>{ DreamElement.Light,DreamElement.Dark },unclockplantid = new List<int>{ 1007 } },

            new Level{ID = 1002, Name = "冥寒",Type = LevelType.Dream,plantmesh = "Night",Map = "Night",Music = "Night",KeyImage = "DarkFrostFumeShroom",StartSunnumber = 200,StartTime = 15f,girdKey = GridKey.Night
            ,ZombieCountPower = 2,FlagCount = 30,ZombiesID = new List<int>{ 101,102,103,110,112,306 },Dream = new List<DreamElement>{ DreamElement.Ice,DreamElement.Dark },unclockplantid = new List<int>{ 1010 } },

            }
        },
    };
    /// <summary>
    /// 查找关卡信息
    /// </summary>
    /// <param name="levelType">关卡类型</param>
    /// <param name="id">id</param>
    /// <returns>查找到的关卡信息</returns>
    public Level GetLevel(LevelType levelType, int id)
    {
        if (Levels[levelType].Find(x=> x.ID == id) != null)
        {
            return Levels[levelType].Find(x => x.ID == id);
        }
        Debug.Log("Error: Level not Get.");
        return null;
    }
    [System.Serializable]
    public class Level
    {
        [Header("ID")]
        public int ID;
        [Header("地图名")]
        public string Name = "自定义关卡";
        [Header("关卡类型")]
        public LevelType Type;
        [Header("梦境关卡元素")]
        public List<DreamElement> Dream;
        [Header("关卡背景图")]
        public string Map;//
        [Header("BGM")]
        public string Music;
        [Header("关卡按钮图片名")]
        public string KeyImage = null;
        [Header("关卡时间及关卡按钮背景图")]
        public GridKey girdKey = GridKey.Day;
        [Header("初始阳光数")]
        public int StartSunnumber = 200;//
        [Header("僵尸刷怪倍率")]
        public float ZombieCountPower = 1;//
        [Header("开始出怪时间")]
        public float StartTime = 20;//
        [Header("波次数(每10波为1大波)")]
        public int FlagCount = 20;//
        [Header("僵尸列表")]
        public List<int> ZombiesID = new List<int>();//
        [Header("种植网格")]
        public string plantmesh;//
        [Header("解锁植物")]
        public List<int> unclockplantid;
        [Header("关卡控制器")]
        public BattleControlBase BattleControl = null;
        [Header("关卡机制")]
        public LevelIcon LevelIcons = LevelIcon.Default;//
        [Header("允许使用道具")]
        public bool CanUseProp = true;
        [Header("关卡内掉落阳光")]
        public bool CanCreateSun = false;
        [Header("墓碑数量")]
        public int Tombston = 0;
        [Header("指定卡片")]
        public int ControlCardid = 0;
        [Header("延缓出怪")]
        public bool LaterZombieInit = false;
        public Level Clone()
        {
            return (Level)MemberwiseClone();
        }
    }
    /// <summary>
    /// 特殊关卡机制
    /// </summary>
    public enum LevelIcon
    {
        Default,//普通
        Conveyer,//传送带
        NoCard,//跳过选牌
        SeedRian,//种子雨
        ClockCard,//指定植物
    }
    public static readonly Dictionary<int, List<int>> LevelControlPlantCardes = new Dictionary<int, List<int>>
    {
        {1,new List<int>{ 503,503,503,503,503,503,503,503 } },
        {2,new List<int>{ 502 } },
    };

    public string GetBuffDescribe(string id)
    {
        if (buffdescribes.ContainsKey(id))
        {
            return buffdescribes[id];
        }
        return null;
    }
    public readonly Dictionary<string, string> buffdescribes = new Dictionary<string, string>
    {
        {
            "光豌豆子弹","伤害为20+发射时阳光/5，至多50。"
        },
        {
            "眩光","移动速度-25%，造成的伤害-25%，重复获得延长时间。"
        },
        {
            "寒冷","上限为4，速度-n*25%，达到4层时进入冻结状态，该状态最多持续25-层数*5秒，重复获得延长时间，持续时间结束后移除该效果。"
        },
        {
            "霜流","施加同等级寒冷且达到最大持续时间。"
        },
        {
            "极光","上限为6，植物生产间隔-n*3%，极光向日葵依据该效果获得强化。"
        },
        {
            "月蚀","初始上限为10，达到上限后爆发，造成200+生命上限*n%的暗元素伤害(至多计入4000点生命上限)，随后移除该效果。"
        },
        {
            "光明豌豆子弹","20伤害，击中僵尸时施加5秒眩光效果。"
        },
        {
            "暗影豌豆子弹","20伤害，击中僵尸时施加1层月蚀。"
        },
        {
            "魅影","生命上限+50*n，每5秒回复10*n点体力。"
        },
        {
            "缠绕","一层：定身0.2s，本格溅射20点\r\n二层：定身0.5s，本格溅射20点\r\n三层：定身1s，本格溅射30点，层数归零，并朝六个方向发射一个猫毛。"
        },
        {
            "霜蚀","上限100层，层数达到15时，受到生命上限30%的暗元素伤害;层数达到40时，释放半径1.5格的3级霜流;\r\n层数达到65后死亡时释放冥霜爆炸(对半径n/20格范围的所有僵尸造成10*霜蚀层数的冰元素伤害)"
        },
    };



    [System.Serializable]
    public class ZombieDescribe
    {
        public string Name;//名称

        [TextArea(0, 7)]
        public string MiniInfo;//简略信息

        [TextArea(0, 7)]
        public string KEYWORD;//数据

        [TextArea(0, 7)]
        public string Characteristic;//特性

        [TextArea(0, 7)]
        public string Description;//描述

        public string FirstShow;//速览
    }

    [System.Serializable]
    public class PlantDescribe
    {
        public string Name;//名称
        [TextArea(0,7)]
        public string MiniInfo;//简略信息

        [TextArea(0, 7)]
        public string KEYWORD;//数据

        [TextArea(0, 7)]
        public string Characteristic;//特性

        [TextArea(0, 7)]
        public string Description;//描述

        public string FirstShow;//速览
    }

    /// <summary>
    /// 数据储存用
    /// </summary>
    public FiledInfo filedInfo;//存档
    public Attribute.Level levelAttribute;//即将加载的关卡信息
    public TutorialKey waitingtutorial = TutorialKey.None;//即将进行的教学
    public AudioClip custommusic = null;//自定义音乐

    public static void SaveTutorial(TutorialKey key)
    {
        if (Attribute.Instance.filedInfo.tutorialKeys.Contains(key))
        {
            return;
        }
        Attribute.Instance.filedInfo.tutorialKeys.Add(key);
        SaveLoadManager.Save(Attribute.Instance.filedInfo.FliedName, Attribute.Instance.filedInfo);
    }
    public static void SaveFiled(int levelid,LevelType levelType = LevelType.Other)
    {
        if (Attribute.Instance.GetLevel(levelType, levelid) != null)
        {
            foreach(int id in Attribute.Instance.GetLevel(levelType, levelid).unclockplantid)
            {
                if (!Attribute.Instance.filedInfo.Unclockplantid.Contains(id))
                {
                    Attribute.Instance.filedInfo.Unclockplantid.Add(id);
                }
            }
        }
        if (levelType == LevelType.Adventure)
        {
            if (Attribute.Instance.filedInfo.MianFinishLevel <= levelid)
            {
                Attribute.Instance.filedInfo.MianFinishLevel = levelid;
            }
        }
        else 
        {
            switch (levelType)
            {
                case LevelType.Other:
                    if (Attribute.Instance.filedInfo.OtherFinishLevel.Contains(levelid)) return;
                    Attribute.Instance.filedInfo.OtherFinishLevel.Add(levelid);
                    break;
                case LevelType.Challenge:
                    if (Attribute.Instance.filedInfo.ChallengeFinishLevel.Contains(levelid)) return;
                    Attribute.Instance.filedInfo.ChallengeFinishLevel.Add(levelid);
                    break;
                case LevelType.LittleGame:
                    if (Attribute.Instance.filedInfo.LittleGameFinishLevel.Contains(levelid)) return;
                    Attribute.Instance.filedInfo.LittleGameFinishLevel.Add(levelid);
                    break;
                case LevelType.Life:
                    if (Attribute.Instance.filedInfo.LifeFinishLevel.Contains(levelid)) return;
                    Attribute.Instance.filedInfo.LifeFinishLevel.Add(levelid);
                    break;
                case LevelType.Dream:
                    if (Attribute.Instance.filedInfo.DreamFinishLevel.Contains(levelid)) return;
                    Attribute.Instance.filedInfo.DreamFinishLevel.Add(levelid);
                    break;
            }
        }
        SaveLoadManager.Save(Attribute.Instance.filedInfo.FliedName, Attribute.Instance.filedInfo);
    }

    public readonly Dictionary<Type, bool> InsetanceCanNew = new Dictionary<Type, bool>
    {
        {typeof(DebugShow),true },
        {typeof(GameDebugUI),true },
        {typeof(StopMenu),true },
        {typeof(MusicManage),true },
        {typeof(BattleManage),false },
        {typeof(HandManage),false },
        {typeof(MapManage),false },
        {typeof(UImanage),false },
        {typeof(ZombieManage),false },
        {typeof(Filed),false },
        {typeof(AlmmanacsAllbox),true },
    };
}
public class FiledInfo
{
    //存档名
    public string FliedName = "Player";
    //主线进度
    public int MianFinishLevel = -1;
    //背景音大小
    public float BGMValue = 1f;
    //特效音大小
    public float battlemusicValue = 1f;
    //游戏速度
    public float GameSpeed = 1f;
    //梦境深度
    public int dreamdepth = 0;
    //自定义音乐
    public string CustomMusic = null;
    //梦境关卡挑战进度
    public List<int> DreamFinishLevel = new List<int>();
    //其他关卡进度
    public List<int> OtherFinishLevel = new List<int>();
    //挑战关卡进度
    public List<int> ChallengeFinishLevel = new List<int>();
    //解谜关卡进度
    public List<int> LittleGameFinishLevel = new List<int>();
    //生存关卡进度
    public List<int> LifeFinishLevel = new List<int>();
    //解锁植物id列表
    public List<int> Unclockplantid = new List<int>();
    //新手教程完成进度
    public List<TutorialKey> tutorialKeys = new List<TutorialKey>();
    //自定义关卡
    public Attribute.Level CustomLevel = null;
    //储存的道具列表
    public Prop[] SaveProps;
    //难度
    public int Difficulty = 0;
    //上次选卡
    public List<int> LastChioceCard = new List<int>();
}
public class DefaultFiled
{
    //存档名
    public string FiledName = "Player";
}
public enum LevelType
{
    Other,//其他
    Adventure,//冒险模式
    Challenge,//挑战模式
    LittleGame,//LittleGame
    Life,//生存模式
    Dream,//里世界
}

public enum TutorialKey//新手教程
{
    None,//无
    FirstDreamElementAdd,//第一个梦境元素获得
    SecondDreamElementAdd,//第二个梦境元素获得
}

public static class RandomUtil
{
    public static T SelectOne<T> (List<T> list)
    {
        T t = list[Random.Range(0,list.Count)];
        return t;
    }

    public static bool Probability(float num,bool less)
    {
        if (less)
        {
            return Random.Range(0f, 1f) <= num;
        }
        return Random.Range(0f, 1f) >= num;
    }
    public static T AddOrGetComponent<T>(GameObject obj) where T : Component
    {
        T component;
        if (!obj.TryGetComponent<T>(out component))
        {
            component = obj.AddComponent<T>();
        }
        return component;
    }
    public static bool AreContentsEqual<T>(List<T> list1, List<T> list2)
    {
        if (list1 == null || list2 == null || list1.Count != list2.Count)
            return false;

        var set1 = new HashSet<T>(list1);
        var set2 = new HashSet<T>(list2);

        return set1.SetEquals(set2);
    }
}

public static class SaveLoadManager
{
    /// <summary>
    /// 存档位置
    /// </summary>
    public static string jsonFolder = $"{Application.persistentDataPath}/SAVE/";

    /// <summary>
    /// 判断是否存在存档文件
    /// </summary>
    /// <param name="path">存档名</param>
    /// <param name="type">存档后缀</param>
    /// <returns></returns>
    public static bool IsExistsData(string path, string type = ".sav")
    {
        string filePath = $"{jsonFolder}{path}{type}";
        bool isExistsdata = File.Exists(filePath);
        return isExistsdata;
    }
    /// <summary>
    /// 保存存档文件
    /// </summary>
    /// <typeparam name="T">存档类型</typeparam>
    /// <param name="path">存档名</param>
    /// <param name="data">存档类型</param>
    /// <param name="type">存档后缀</param>
    public static void Save<T>(string path, T data,string type = ".sav")
    {
        string filePath = $"{jsonFolder}{path}{type}";
        if(!Directory.Exists(filePath)) { Directory.CreateDirectory(jsonFolder); };
       var jsondata = JsonConvert.SerializeObject(data,Formatting.Indented);
       File.WriteAllText(filePath, jsondata);
    }
    /// <summary>
    /// 读取存档文件
    /// </summary>
    /// <typeparam name="T">存档类型</typeparam>
    /// <param name="path">存档名</param>
    /// <param name="type">存档后缀</param>
    /// <returns></returns>
    public static T Load<T>(string path, string type = ".sav")
    {
        string filePath = $"{jsonFolder}{path}{type}";
        if (!Directory.Exists(jsonFolder))
        {
            Debug.Log(jsonFolder + " Not found");
            return default; 
        };
        var jsonData = File.ReadAllText(filePath);
        var jsondata = JsonConvert.DeserializeObject<T>(jsonData);
        return jsondata;
    }
    /// <summary>
    /// 删除存档
    /// </summary>
    /// <param name="path">存档名</param>
    public static void Destory(string path, string type = ".sav")
    {
        string filePath = $"{jsonFolder}{path}{type}";
        if (!File.Exists(filePath)) { return; } ;
        File.Delete(filePath);
    }

}