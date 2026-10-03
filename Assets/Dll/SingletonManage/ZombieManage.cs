using System.Collections.Generic;
using UnityEngine;
using static Attribute;
using System.Collections;
using System;
using Random = UnityEngine.Random;

public class ZombieManage:Singleton<ZombieManage>
{

    #region 关卡录入信息
    /// <summary>
    /// 波次数
    /// </summary>
    int timeflag;
    /// <summary>
    /// 出怪倍率
    /// </summary>
    public float zombiecountpower;
    /// <summary>
    /// 僵尸总列表
    /// </summary>
    List<int> zombienames = new List<int>();
    /// <summary>
    /// 普通僵尸信息列表
    /// </summary>
    List<ZombieInfo> zombielist = new List<ZombieInfo>();
    /// <summary>
    /// 精英僵尸信息列表
    /// </summary>
    public List<ZombieInfo> zombielist_Elite = new List<ZombieInfo>();
    /// <summary>
    /// Boss僵尸
    /// </summary>
    public List<ZombieInfo> zombielist_boss = new List<ZombieInfo>();
    #endregion

    /// <summary>
    /// 当前轮次
    /// </summary>
    public int Range = 0;
    /// <summary>
    /// 当前波次
    /// </summary>
    public int nowflag = 0;
    /// <summary>
    /// 僵尸刷新冷却
    /// </summary>
    [SerializeField]
    float zombiecolltime = 0f;
    /// <summary>
    /// 当前波次僵尸总价值
    /// </summary>
    int ZombieTotalValueInFlag = 0;
    /// <summary>
    /// 僵尸刷新状态
    /// </summary>
    public ZomieUpdateType zomieUpdateType = ZomieUpdateType.DoNot;
    /// <summary>
    /// 该大波次刷新了旗帜僵尸
    /// </summary>
    bool InitedFlagzombie = false;
    /// <summary>
    /// 某一行刷新过的僵尸数
    /// </summary>
    List<int> linezombiecount = new List<int>();
    /// <summary>
    /// 可出现僵尸的行数
    /// </summary>
    List<int> line;
    /// <summary>
    /// 初始僵尸生成范围
    /// </summary>
    Vector4 Vector4 = new Vector4(12f, 9f, 2.5f, -5f);
    /// <summary>
    /// 允许生成僵尸
    /// </summary>
    public bool CanCreateZombie = true;
    /// <summary>
    /// 僵尸待生成列表
    /// </summary>
    Queue<int> waitcreatezombies = new Queue<int>();
    /// <summary>
    /// 存活的Boss僵尸
    /// </summary>
    public List<ZombiesBase> AliveBoss = new List<ZombiesBase>();
    /// <summary>
    /// 大旗帜委托
    /// </summary>
    public Action<int> BigFlag;
    /// <summary>
    /// 最后死亡的僵尸位置
    /// </summary>
    public Vector3 LastZombiePos = Vector3.zero;

    /// <summary>
    /// 僵尸信息储存列表
    /// </summary>
    public ZombiesBase[] Zombies;
    bool isfrist = true;
    public bool BossTime = false;

    public void Start()
    {
        //载入信息
        Level level = Attribute.Instance.levelAttribute;
        timeflag = level.FlagCount;
        zombiecountpower = level.ZombieCountPower;
        if (level.Type == LevelType.Life && Range > 0)
        {
            zombienames = Zombieidinlife(level);
        }
        else
        {
            zombienames = level.ZombiesID;
        }
        if (isfrist)
        {
            StartCoroutine(CreateZombieByWait());
            isfrist = false;
        }
        nowflag = 0;
        zombiecolltime = 0f;
        ZombieTotalValueInFlag = 0;
        InitedFlagzombie = false;
        linezombiecount = new List<int>();
        line = new List<int>();
        zombielist = new List<ZombieInfo>();
        zombielist_Elite = new List<ZombieInfo>();
        zombielist_boss = new List<ZombieInfo>();
        waitcreatezombies = new Queue<int>();
        BigFlag = null;
        Zombies = new ZombiesBase[256];
        LastZombiePos = Vector3.zero;
        //载入可生成僵尸行信息
        for (int i = 0;i < MapManage.Instance.meshxy.x; i++)
        {
            line.Add(i);
            linezombiecount.Add(10);
        }
        //分类僵尸 载入列表
        foreach (int i in zombienames)
        {
            if (i / 10000 > 0)
            {
                zombielist_boss.Add(Attribute.Instance.GetZombieInfo(i));
            }
            if (i / 1000 > 0)
            {
                zombielist_Elite.Add(Attribute.Instance.GetZombieInfo(i));
            }
            else
            {
                zombielist.Add(Attribute.Instance.GetZombieInfo(i));
            }
        }
        //生成旗帜 演示僵尸
        this.CreateFlag();
        this.ClearZombie();
        this.CreateShowZombie();
    }
    public List<int> Zombieidinlife(Level level)
    {
        List<int> ints = new List<int>();
        int range = Range + 1;
        if (range > 9)
        {
            range = 9;
        }
        ints.Add(0);
        while (range > 0)
        {
            ZombieInfo zb = RandomUtil.SelectOne(Attribute.ZombieInfos.ZombieInfoes);
            if (zb.IsCreateZombie || ints.Contains(zb.ID))
            {
                continue;
            }
            if (zb.ID > 1000)
            {
                if (Random.Range(0f, 1f) > 0.5f)
                {
                    ints.Add(zb.ID);
                    range--;
                }
                continue;
            }
            ints.Add(zb.ID);
            range--;
        }
        return ints;
    }
    public void CreateFlag()
    {
        //生成旗帜
        if (timeflag > 10)
        {
            for(int i = 10;i < timeflag; i += 10)//加载整数波数
            {
                if (i > timeflag)
                {
                    i = timeflag;
                }
                GameObject gameObject = GameObject.Instantiate(BattleManage.Instance.Flag, BattleManage.Instance.Flag.transform.parent);
                gameObject.SetActive(true);
                gameObject.transform.localPosition = new Vector3(148f - 142f * ((float)i / (float)timeflag), -4f, -0f);
            }
        }
        else//单大波次
        {
            GameObject gameObject = GameObject.Instantiate(BattleManage.Instance.Flag, BattleManage.Instance.Flag.transform.parent);
            gameObject.SetActive(true);
        }
    }
    public void ClearZombie()//清除僵尸
    {
        for (int i = 0; i < BattleManage.Instance.zombiesmanage.childCount; i++)
        {
            BattleManage.Instance.zombiesmanage.GetChild(i).gameObject.GetComponent<ZombiesBase>().Destroy();
        }
    }
    public void CreateShowZombie()//生成演示僵尸
    {
        List<Vector3> vecs = new List<Vector3>();
        float y = (Vector4.w - Vector4.z) / (zombienames.Count + 3);
        for (int i = 0; i < zombienames.Count + 3; i++)
        {
            vecs.Add(new Vector3(Random.Range(Vector4.x, Vector4.y), Vector4.w - y * i));
        }
        foreach (int id in zombienames)
        {
            GameObject gameObject = this.InitZombie(id,0);
            Vector3 vector3 = RandomUtil.SelectOne(vecs);
            gameObject.transform.position = vector3;
            vecs.Remove(vector3);
            gameObject.GetComponent<ZombiesBase>().Init((int)(90 - vector3.y * 5), Attribute.Instance.GetZombieInfo(id), 0);
            GameObject.Destroy(gameObject.GetComponent<BoxCollider2D>());
            GameObject.Destroy(gameObject.GetComponent<Rigidbody2D>());
        }
    }

    /// <summary>
    /// 计算存活的非魅惑僵尸数
    /// </summary>
    /// <returns></returns>
    public int AliveZombieCount()
    {
        int num = 0;
        if (ZombieManage.Instance.transform.childCount <= 0)
        {
            return -1;
        }
        for(int i = 0;i < Zombies.Length; i++)
        {
            if (Zombies[i] != null && !Zombies[i].Reverse && Zombies[i].HP <= 0)
            {
                num++;
            }
        }
        return num;
    }
    public void FixedUpdate()
    {
        if (BattleManage.Instance.battleStage == BattleStage.InBattle)//战中
        {
            if (!CanCreateZombie)//不允许生成僵尸
            {
                return;
            }
            if (zomieUpdateType != ZomieUpdateType.DoNot || BossTime)//战中（不为准备阶段且不为最后一波）
            {
                this.zombiecolltime -= Time.deltaTime;
            }
            if (waitcreatezombies.Count <= 0 && (this.zombiecolltime <= 0f || ZombieManage.Instance.transform.childCount == 0))//倒计时结束或场上没有僵尸
            {
                ToFlag();
            }
        }
        if (!BossTime && waitcreatezombies.Count <= 0 && nowflag >= timeflag && ZombieManage.Instance.transform.childCount == 0 && BattleManage.Instance.battleStage != BattleStage.End)//最终波等待僵尸死亡
        {
            End();
        };
    }
    void ToFlag()
    {
        if (BossTime)
        {
            if (BattleManage.Instance.level.LaterZombieInit)
            {
                this.zombiecolltime = Random.Range(15, 21);//冷却时间变动
            }
            else
            {
                this.zombiecolltime = Random.Range(25, 31);//冷却时间变动
            }
            SetCreateZombie();//僵尸生成
            return;
        }
        nowflag++;
        BattleManage.Instance.Image.value = (float)nowflag / (float)timeflag;//进度条改变
        if (nowflag % 10 == 0 || nowflag == timeflag)//大波次显示
        {
            ChangeFlag(nowflag == timeflag);
            BigFlag?.Invoke(nowflag);
        }
        if (nowflag >= timeflag)//最终波
        {
            BattleManage.Instance.battleStage = BattleStage.LastFlagOut;
            if (zombielist_boss.Count > 0)
            {
                BossTime = true;
            }
        }
        else
        {
            if (BattleManage.Instance.level.LaterZombieInit)
            {
                this.zombiecolltime = Random.Range(15, 21);//冷却时间变动
            }
            else
            {
                this.zombiecolltime = Random.Range(25, 31);//冷却时间变动
            }
        }
        SetCreateZombie();//僵尸生成
    }
    public void End()
    {
        BattleManage.Instance.battleStage = BattleStage.End;
        BattleManage.Instance.Trophy.transform.position = LastZombiePos;
        if (BattleManage.Instance.level.Type == LevelType.Life)
        {
            Range++;
            Invoke("AfterLifeShow", 5f);
            DebugShow.Instance.Init("更多的僵尸要来了！");
            return;
        }
        BattleManage.Instance.Trophy.SetActive(true);
    }
    public void AfterLifeShow()
    {
        BattleManage.Instance.EndByLifeLevel();
        Start();
    }
    private void ChangeFlag(bool islast)
    {
        if (nowflag / 10 > 1)
        {
            InitedFlagzombie = true;
        }
        BattleManage.Instance.CenterTex.SetActive(true);
        BattleManage.Instance.CenterTex.GetComponent<Animator>().enabled = true;
        int falg = islast ? 0 : nowflag / 10;
        BattleManage.Instance.Flag.transform.parent.GetChild(falg).transform.position += Vector3.up * 0.25f;
        if (islast)
        {
            BattleManage.Instance.CenterTex.GetComponent<Animator>().Play("LastFlag");
        }
        else
        {
            BattleManage.Instance.CenterTex.GetComponent<Animator>().Play("OnFlagStart");
        }
    }//大波次开始

    public void SetCreateZombie()//波次僵尸生成
    {
        int num = (int)((float)(nowflag * zombiecountpower * (1 + (float)Attribute.Instance.filedInfo.Difficulty * 0.2f) + Range * timeflag) * 0.4f) + 1;//计算总权重
        if (BattleManage.Instance.level.Map == "DayOneLine") { num = (int)( num * 0.4f) + 1; };//单行削弱
        if (nowflag % 10 == 0)
        {
            num = (int)((BattleManage.Instance.level.Type == LevelType.Life ? 2.5f: 1.5f) * num); 
        };//大波次计算
        Queue<int> select = GetZombieWeightSelect(num);
        while (select.Count > 0)
        {
            waitcreatezombies.Enqueue(select.Dequeue());
        }
    }
    /// <summary>
    /// 僵尸随机
    /// </summary>
    /// <param name="value">剩余价值</param>
    /// <returns>筛选后的僵尸id</returns>
    public Queue<int> GetZombieWeightSelect(int value)
    {
        Queue<int> createzombieid = new Queue<int>();
        if (zomieUpdateType == ZomieUpdateType.InFLag && nowflag / 10 > 1 && InitedFlagzombie)
        {
            InitedFlagzombie = false;
            createzombieid.Enqueue(100);
        }
        ZombieTotalValueInFlag = value;//记录可用价值

        while (ZombieTotalValueInFlag > 0)
        {
            if (nowflag == timeflag && zombielist_boss.Count > 0)
            {
                foreach (Attribute.ZombieInfo zombieInfo in zombielist_boss)
                {
                    createzombieid.Enqueue(zombieInfo.ID);
                }
                zombielist_boss.Clear();//Boss僵尸仅会在最后一波生成一次
            }
            if (zombielist_Elite.Count > 0 && Random.Range(0, 100) < (5 + BattleManage.Instance.LevelDreamDepth))//精英僵尸池
            {
                List<ZombieInfo> Zombieidesinselect = new List<ZombieInfo>();//储存所有可生成的僵尸信息
                int randomnum = 0;
                foreach (Attribute.ZombieInfo zombieInfo in zombielist_Elite)
                {
                    if (zombieInfo.Value <= ZombieTotalValueInFlag && zombieInfo.MinFlag <= nowflag + Range * timeflag)
                    {
                        Zombieidesinselect.Add(zombieInfo);
                        randomnum += zombieInfo.Weight;
                    }
                }
                if (Zombieidesinselect.Count > 0)
                {
                    createzombieid.Enqueue(GetEliteZombie(Zombieidesinselect, randomnum));
                }
            }
            else if (zombielist.Count > 0)//普通僵尸池
            {
                List<ZombieInfo> Zombieidesinselect = new List<ZombieInfo>();//储存所有可生成的僵尸信息
                int randomnum = 0;
                foreach (Attribute.ZombieInfo zombieInfo in zombielist)
                {
                    if (zombieInfo.Value <= ZombieTotalValueInFlag && zombieInfo.MinFlag <= nowflag + Range * timeflag)
                    {
                        Zombieidesinselect.Add(zombieInfo);
                        randomnum += zombieInfo.Weight;
                    }
                }
                if (Zombieidesinselect.Count <= 0)
                {
                    createzombieid.Enqueue(zombielist[0].ID);
                    ZombieTotalValueInFlag -= zombielist[0].Value;
                    continue;
                }
                createzombieid.Enqueue(GetNorMalZombie(Zombieidesinselect, randomnum));
            }
            else
            {
                DebugShow.Instance.Init("僵尸池中没有僵尸，请检查关卡设置");
                break;
            }
        }
        if (createzombieid.Count <= 0)
        {
            createzombieid.Enqueue(0);
            DebugShow.Instance.Init("没有可以生成的僵尸");
        }
        while (createzombieid.Count >= 200)
        {
            createzombieid.Dequeue();
        }
        return createzombieid;//若所有池均无僵尸则生成普通僵尸
    }
    /// <summary>
    /// 普通僵尸筛选
    /// </summary>
    /// <returns></returns>
    public int GetNorMalZombie(List<ZombieInfo> ZombieInfos,int TotalWeight)
    {
        int zombieweight = Random.Range(0, TotalWeight);//随机数选取僵尸
        for (int i = 0; i < ZombieInfos.Count; i++)//查询对应僵尸
        {
            if (zombieweight <= ZombieInfos[i].Weight)
            {
                ZombieTotalValueInFlag -= ZombieInfos[i].Value;//减去价值
                return ZombieInfos[i].ID;
            }
            else
            {
                zombieweight -= ZombieInfos[i].Weight;
            }
        }
        return 0;
    }
    /// <summary>
    /// 精英僵尸筛选
    /// </summary>
    /// <returns></returns>
    public int GetEliteZombie(List<ZombieInfo> ZombieInfos, int TotalWeight)
    {
        int zombieweight = Random.Range(0, TotalWeight);//随机数选取僵尸
        for (int i = 0; i < ZombieInfos.Count; i++)//查询对应僵尸
        {
            if (zombieweight <= ZombieInfos[i].Weight)
            {
                ZombieTotalValueInFlag -= ZombieInfos[i].Value;//减去价值
                return ZombieInfos[i].ID;
            }
            else
            {
                zombieweight -= ZombieInfos[i].Weight;
            }
        }
        return 0;
    }

    public void GetZombieCreate(int id)
    {
        int num = RandomUtil.SelectOne<int>(line);
        line.Remove(num);
        if (line.Count <= 0)
        {
            line = new List<int>();
            for (int i = 0; i < MapManage.Instance.meshxy.x; i++)
            {
                line.Add(i);
            }
        }//随机选行
        linezombiecount[MapManage.Instance.meshxy.x - 1 - num]--;
        if (linezombiecount[MapManage.Instance.meshxy.x - 1 - num] <= 0)
        {
            linezombiecount[MapManage.Instance.meshxy.x - 1 - num] = 10;
        }//重置行列表
        if (BattleManage.Instance.controlBase != null)
        {
            id = BattleManage.Instance.controlBase.ChangeCreateZombie(id);
        }//控制台更新僵尸
        CreateZombie(id, num);
    }

    public void CreateZombie(int id,int line)
    {
        if (Attribute.Instance.GetZombieInfo(id) == null)
        {
            DebugShow.Instance.Init("试图生成不存在的僵尸！");
            return;
        }
        if (line >= MapManage.Instance.meshxy.x || line < 0) 
        { 
            DebugShow.Instance.Init("非法尝试生成僵尸：场地外");
            return;
        }
        LoadZombieMess(InitZombie(id, line),id, line);
    }

    public void LoadZombieMess(GameObject gameObject,int id, int line,float startspeed = 1f)
    {
        gameObject.transform.position = new Vector3(10, MapManage.Instance.meshpos[line, 0].y - 0.8f, 0f);
        ZombiesBase zombies = gameObject.GetComponent<ZombiesBase>();
        zombies.Init((MapManage.Instance.meshxy.x - line) * 10 + linezombiecount[MapManage.Instance.meshxy.x - 1 - line], Attribute.Instance.GetZombieInfo(id).Clone(), line, startspeed);
        BattleManage.Instance.controlBase?.OnCreateZombie(zombies);
        Zombies[Array.IndexOf(Zombies, null)] = zombies;
        if (id / 10000 > 0) AliveBoss.Add(zombies);
    }

    public GameObject InitZombie(int zombieid, int line)
    {
        return GameObject.Instantiate(Attribute.Instance.GetZombieInfo(zombieid).Prefab, new Vector3(10 + Random.Range(-0.2f,0.2f), MapManage.Instance.meshpos[line, 0].y - 0.8f, 0f), Quaternion.identity, BattleManage.Instance.zombiesmanage);
    }

    IEnumerator CreateZombieByWait()
    {
        while (true)
        {
            if (BattleManage.Instance.level.LaterZombieInit)
            {
                yield return new WaitForSeconds(1f);
            }
            else
            {
                yield return null;
            }
            if (waitcreatezombies.Count > 0)
            {
                int id = waitcreatezombies.Dequeue();
                GetZombieCreate(id);
            }
        }
    }
}
