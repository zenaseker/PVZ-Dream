using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlnatInfoButton : MonoBehaviour
{
    public int id = 0;
    private GameObject obj = null;
    public TextMeshProUGUI Text;
    public void InitPlantID(string plantID)
    {
        if (int.TryParse(plantID, out id))
        {
            if(Attribute.Instance.GetPlantInfo(id) == null)
            {
                this.GetPlant();
                return;
            }
            DebugShow.Instance.Init("不存在这个ID的植物！");
            id = 0;
            return;
        }
        DebugShow.Instance.Init("请输入正确的数字！");
    }

    private void GetPlant()
    {
        if (obj != null)
        {
            GameObject.Destroy(obj);
        }
        Attribute.PlantDescribe plantDescribe = Attribute.Instance.GetPlantInfo(id).plantDescribe;
        if (plantDescribe == null)
        {
            DebugShow.Instance.Init("该植物信息尚未录入");
            return;
        }
        obj = GameObject.Instantiate(Attribute.Instance.GetPlantInfo(id).CardPrefab, this.transform.Find("InitCardPos"));
        try
        {
            GameObject.Destroy(obj.GetComponent<Button>());
        }
        catch
        {

        }
    }
}
