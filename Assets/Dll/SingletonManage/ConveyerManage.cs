using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConveyerManage :Singleton<ConveyerManage>
{
    public Transform Left;
    public Transform Right;
    public Transform List;
    //Éú³É¿¨Æ¬
    public float cooltime = 5f;
    float timeing = -5f;
    //¿¨Æ¬ÒÆ¶¯
    float MoveSpeed = 0.3f;
    List<Vector3> pos = new List<Vector3>
        {
            new Vector3(-375f, -50, 0),
            new Vector3(-375f + 87.5f, -50, 0),
            new Vector3(-375f + 87.5f * 2, -50, 0),
            new Vector3(-375f + 87.5f * 3, -50, 0),
            new Vector3(-375f + 87.5f * 4, -50, 0),
            new Vector3(-375f + 87.5f * 5, -50, 0),
            new Vector3(-375f + 87.5f * 6, -50, 0),
            new Vector3(-375f + 87.5f * 7, -50, 0),
            new Vector3(-375f + 87.5f * 8, -50, 0),
            new Vector3(-375f + 87.5f * 9, -50, 0),
            new Vector3(500f, -50, 0),
        };
    public List<int> plant = new List<int>();
    public int[] plantweight;

    public void Start()
    {
        for (int i = 0;i < List.childCount;i++)
        {
            List.GetChild(i).GetComponent<Seed>().Destory();
        }
        plant = new List<int>(Attribute.Instance.Getplantclocklist(2,0));
        plantweight = new int[plant.Count];
        for(int i = 0; i < plant.Count; i++)
        {
            if (plant[i] % 100 == 1)
            {
                plantweight[i] = 1;
            }
            plantweight[i] = 5;
        }
    }
    public void FixedUpdate()
    {
        if (Time.timeScale == 0f) return;
        if (List.childCount <= 10)
        {
            timeing += Time.deltaTime;
            if (timeing > cooltime)
            {
                timeing = 0f;
                cooltime -= 0.2f;
                if (cooltime < 2f)
                {
                    cooltime = 2f;
                }
                CreateSeed();
            }
        }
        for (int i = 0; i < List.childCount; i++)
        {
            if (List.GetChild(i).transform.localPosition.x > pos[i].x)
            {
                List.GetChild(i).transform.localPosition = new Vector3(List.GetChild(i).transform.localPosition.x - MoveSpeed * Time.timeScale, -50, 0);
            }
            else
            {
                List.GetChild(i).transform.localPosition = pos[i];
            }
        }
    }

    public void CreateSeed()
    {
        int num = SelectSeed();
        GameObject obj = PoolManage.Instance.GetPoolGameObject("SeedCard", Attribute.Instance.GetPlantInfo(num).CardPrefab.name, List);
        obj.transform.localPosition = new Vector3(500, -50, 0);
        obj.transform.localScale = Vector3.one * 0.9f;
        RandomUtil.AddOrGetComponent<SeedOnceUse>(obj).Init(Attribute.Instance.GetPlantInfo(num));
        RandomUtil.AddOrGetComponent<Button>(obj).onClick.AddListener(RandomUtil.AddOrGetComponent<SeedOnceUse>(obj).OnCilck);
    }
    public int SelectSeed()
    {
        float totalWeight = 0;
        foreach (var weightedEvent in plantweight)
        {
            totalWeight += weightedEvent;
        }
        float randomValue = Random.Range(0, totalWeight);
        float currentWeight = 0;
        for(int i = 0; i < plant.Count; i++)
        {
            currentWeight += plantweight[i];
            if (currentWeight >= randomValue)
            {
                return plant[i];
            }
        }
        return plant[0];
    }
}
