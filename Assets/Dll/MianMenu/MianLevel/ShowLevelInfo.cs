using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShowLevelInfo : Singleton<ShowLevelInfo>
{
    public Attribute.Level Level;
    public Image LevelBG;
    public Image LevelKeyImage;
    public TextMeshProUGUI Name;
    public TextMeshProUGUI Tex;
    public TextMeshProUGUI Info;
    public TextMeshProUGUI ReWard;
    public Transform levelBackContent;
    public Transform levelBack;
    bool isshow = false;
    public void Start()
    {
        this.gameObject.SetActive(false);
    }
    public void Init(MianLevelInformation mianLevel)
    {
        try
        {
            this.gameObject.SetActive(true);
            MianLevel mianLevelID = mianLevel.MianLevelID;
            if (mianLevelID.ID < 100)
            {
                this.Level = Attribute.Instance.GetLevel(LevelType.Adventure, mianLevelID.ID).Clone();
            }
            else
            {
                this.Level = Attribute.Instance.GetLevel(LevelType.Dream, mianLevelID.ID).Clone();
            }
            Sprite sprite = Attribute.GetSprite(this.Level.KeyImage);
            LevelKeyImage.sprite = sprite;
            LevelKeyImage.GetComponent<RectTransform>().sizeDelta = new Vector2(sprite.rect.width / sprite.rect.height * 250, 250);
            LevelKeyImage.sprite = Attribute.GetSprite(this.Level.KeyImage);
            this.LevelBG.sprite = Attribute.GetSprite("Almanac_Ground" + this.Level.girdKey.ToString(), "LevelBack");
            Name.text = $"{this.Level.Name} {mianLevelID.Name}";
            Tex.text = mianLevelID.Text;
            Info.text = mianLevelID.Info;
            ReWard.text = $"奖励： {mianLevelID.ReWard}";
            CheckLevelToCenter(mianLevel);
            if (!isshow)
            {
                this.GetComponent<Animator>().Play("Show");
                isshow = true;
            }
        }
        catch
        {
            DebugShow.Instance.Init("貌似出了点小问题……");
        }

    }
    public void Hide()
    {
        levelBack.GetComponent<ScrollRect>().enabled = true;
        if (isshow)
        {
            this.GetComponent<Animator>().Play("Hide");
            isshow = false;
        }
    }
    public void CheckLevelToCenter(MianLevelInformation mianLevel)
    {
        Vector3 way = Camera.main.transform.position - mianLevel.transform.position;
        way.z = 0;
        way.x -= 2;
        levelBack.GetComponent<ScrollRect>().enabled = false;
        int mapid = 0;
        if (mianLevel.MianLevelID.ID <= 3)
        {
            mapid = 0;
        }
        else if (mianLevel.MianLevelID.ID <= 7)
        {
            mapid = 1;
        }
        else if (mianLevel.MianLevelID.ID <= 10)
        {
            mapid = 2;
        }
        else
        {
            mapid = 3;
        }
        levelBack.GetComponent<LevelBack>().ChangeTexture(mapid);
        levelBackContent.transform.DOPath(new Vector3[] { levelBackContent.transform.position + way }, 0.5f, PathType.Linear);
    }
    public void GotoLevel()
    {
        Attribute.Instance.levelAttribute = Attribute.Instance.GetLevel(this.Level.Type, this.Level.ID);
        Attribute.ChangeScene("NormalLevel");
    }
}
