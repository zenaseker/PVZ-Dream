using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class BattleControl_Chameleon : BattleControlBase
{
    float time = 0f;
    float used = 0f;
    float bigflagtime = 0f;
    public static PlantBase plant = null;
    GameObject square;
    public static int color = 0;
    public override void OnLevelStart()
    {
        base.OnLevelInit();
        HandManage.Instance.FastCellPlant(Vector2Int.zero, 0);
        GameObject i = GameObject.Find("PlantManage").transform.GetChild(0).gameObject;
        plant = i.GetComponent<PlantBase>();
        square = Resources.Load<GameObject>("Prefabs/LittleGame/Chameleon_Square");
        GameObject ui = GameObject.Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/LittleGame/ChameleonUI"));
        ui.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(ChangePlant1);
        ui.transform.GetChild(1).GetComponent<Button>().onClick.AddListener(ChangePlant2);
        ui.transform.GetChild(2).GetComponent<Button>().onClick.AddListener(ChangePlant3);
        ui.transform.GetChild(3).GetComponent<Button>().onClick.AddListener(ChangePlant4);
        ZombieManage.Instance.BigFlag += InBigFlag;
        DebugShow.Instance.Init("点击下方按钮切换元素");
    }
    public override void OnStartCreateZombie()
    {
        base.OnLevelStart();
        DebugShow.Instance.Init("不要让豌豆射手接触到颜色不同的障碍！");
    }
    public override void OnUpdate()
    {
        base.OnUpdate();
        time += Time.deltaTime;
        bigflagtime -= Time.deltaTime;
        if (bigflagtime > 0 && time > 3f)
        {
            CreateSquare();
            time = 0f;
        }
        if (time > 10f - used)
        {
            CreateSquare();
            time = 0f;
        }
    }
    void CreateSquare()
    {
        used += 0.1f;
        if (used > 6f)
        {
            used = 6f;
        }
        GameObject gameObject = PoolManage.Instance.GetPoolGameObject("LittleGame", "Chameleon_Square", new Vector3(12, 0, 0));
        gameObject.GetComponent<Rigidbody2D>().velocity = Vector3.left * 2;
        int num = Random.Range(0, 4);
        gameObject.GetComponent<Chameleon_Square>().Color = num;
        switch (num)
        {
            case 0:
                gameObject.GetComponent<SpriteRenderer>().color = Color.green;
                break;
            case 1:
                gameObject.GetComponent<SpriteRenderer>().color = DifficultySet.color[3];
                break;
            case 2:
                gameObject.GetComponent<SpriteRenderer>().color = Color.yellow;
                break;
            case 3:
                gameObject.GetComponent<SpriteRenderer>().color = DifficultySet.color[5];
                break;
            case 4:
                gameObject.GetComponent<SpriteRenderer>().color = Color.red;
                break;
        }
    }
    public void InBigFlag(int num)
    {
        bigflagtime = 10f;
    }
    public void ChangePlant1()
    {
        ChangePlant(0);
    }
    public void ChangePlant2()
    {
        ChangePlant(1);
    }
    public void ChangePlant3()
    {
        ChangePlant(2);
    }
    public void ChangePlant4()
    {
        ChangePlant(3);
    }
    public void ChangePlant(int num)
    {
        MapManage.Instance.DestoryCellPlant(0, 0, PlantPosType.Default);
        GameObject destoryplant = plant.gameObject;
        switch (num)
        {
            case 0:
                HandManage.Instance.FastCellPlant(Vector2Int.zero, 0);
                break;
            case 1:
                HandManage.Instance.FastCellPlant(Vector2Int.zero, 5);
                break;
            case 2:
                HandManage.Instance.FastCellPlant(Vector2Int.zero, 100);
                break;
            case 3:
                HandManage.Instance.FastCellPlant(Vector2Int.zero, 300);
                break;
        }
        color = num;
        plant = GameObject.Find("PlantManage").transform.GetChild(1).gameObject.GetComponent<PlantBase>();
        DOTween.Kill(destoryplant, true);
        GameObject.Destroy(destoryplant);
    }
    public static void Lose()
    {
        Time.timeScale = 0f;
        BattleManage.Instance.TimeStop = true;
        MusicManage.Instance.BGMCheck(false);
        GameDebugUI.Instance.Init("游戏结束", "重新开始", "", "回到主菜单", AfterLose1, null, AfterLose2);
    }
    public static void AfterLose1()
    {
        BattleManage.Instance.ReStart();
        Time.timeScale = Attribute.Instance.filedInfo.GameSpeed;
        MusicManage.Instance.BGMCheck(true);
    }
    public static void AfterLose2()
    {
        Attribute.ChangeScene("MianMenu");
        Time.timeScale = Attribute.Instance.filedInfo.GameSpeed;
        MusicManage.Instance.BGMCheck(true);
    }
}
