using UnityEngine;
using UnityEngine.UI;
using static Attribute;


public enum ElementSeedType
{
    InChoice,
    InWait,
    InUse,
    InCool,
}
public class ElementSeed : MonoBehaviour
{
    public DreamElement dreamElement = DreamElement.Default;
    public ElementSeedType type = ElementSeedType.InChoice;
    float totime = 0f;
    public float cooltime = 0f;
    public void OnCilck()
    {
        if (type == ElementSeedType.InWait)
        {
            type = ElementSeedType.InUse;
            this.transform.GetChild(1).gameObject.SetActive(false);
            UImanage.Instance.ChangeChoiceElement(dreamElement, true);
            this.transform.GetChild(3).gameObject.SetActive(true);
            this.transform.GetChild(3).GetComponent<Image>().color = Color.green;
            totime = cooltime = 60f;
        }
        else if (type == ElementSeedType.InUse)
        {
            type = ElementSeedType.InCool;
            this.transform.GetChild(1).gameObject.SetActive(true);
            UImanage.Instance.ChangeChoiceElement(dreamElement, false);
            this.transform.GetChild(3).gameObject.SetActive(true);
            this.transform.GetChild(3).GetComponent<Image>().color = Color.red;
            totime = cooltime = (60 - cooltime < 30f) ? 30f : 60 - cooltime;
        }
    }
    public void ChangeUse(bool flag)
    {
        if (flag)
        {
            type = ElementSeedType.InWait;
            this.transform.GetChild(2).gameObject.SetActive(false);
        }
        else
        {
            type = ElementSeedType.InChoice;
            this.transform.GetChild(2).gameObject.SetActive(true);
        }
    }
    public void Update()
    {
        if (type == ElementSeedType.InUse || type == ElementSeedType.InCool)
        {
            this.transform.GetChild(3).localRotation = Quaternion.Euler(0f, 0f, 360f * cooltime / totime);
        }
        if (cooltime >= 0)
        {
            cooltime -= Time.deltaTime;
            if (cooltime <= 0)
            {
                if (type == ElementSeedType.InUse)
                {
                    type = ElementSeedType.InCool;
                    this.transform.GetChild(1).gameObject.SetActive(true);
                    UImanage.Instance.ChangeChoiceElement(dreamElement, false);
                    this.transform.GetChild(3).gameObject.SetActive(true);
                    this.transform.GetChild(3).GetComponent<Image>().color = Color.red;
                    totime = cooltime = 60f;
                }
                else if (type == ElementSeedType.InCool)
                {
                    type = ElementSeedType.InWait;
                    this.transform.GetChild(1).gameObject.SetActive(true);
                    this.transform.GetChild(3).gameObject.SetActive(false);
                }
            }
        }
    }
}
