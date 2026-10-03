
using UnityEngine;

public class DreamMianLevelInforation : MianLevelInformation
{
    public int[] Uncolckuse;
    public GameObject[] line;
    public override void OnEnable()
    {
        this.transform.GetChild(2).gameObject.SetActive(Attribute.Instance.filedInfo.DreamFinishLevel.Contains(MianLevelID.ID));
    }

}
