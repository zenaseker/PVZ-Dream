
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelInformation : MonoBehaviour
{
    public LevelType LevelType;
    public int level;

    public void Init(Attribute.Level level)
    {
        this.level = level.ID;
        LevelType = level.Type;
        transform.Find("LevelText").GetComponent<TextMeshProUGUI>().text = level.Name;
        Sprite sprite = Attribute.GetSprite(level.KeyImage);
        transform.Find("LevelIcon").GetComponent<Image>().sprite = sprite; 
        transform.Find("LevelIcon").GetComponent<RectTransform>().sizeDelta = new Vector2(sprite.rect.width / sprite.rect.height * 50, 50);
        transform.Find("LevelImage").GetComponent<Image>().sprite = Attribute.GetSprite("Almanac_Ground" + level.girdKey.ToString(), "LevelBack");
    }

    public void OnClick()
    {
        Attribute.Instance.levelAttribute = Attribute.Instance.GetLevel(LevelType, this.level);
        Attribute.ChangeScene("NormalLevel");
    }
}
