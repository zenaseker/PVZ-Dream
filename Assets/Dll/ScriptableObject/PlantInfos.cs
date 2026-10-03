using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class PlantInfos : ScriptableObject
{
    [SerializeField]
    public List<Attribute.PlantInfo> PlantInfoes = new List<Attribute.PlantInfo>();

    public static Dictionary<int, Attribute.PlantInfo> PlantInfoInGame = new Dictionary<int, Attribute.PlantInfo>();

    public Attribute.PlantInfo GetValue(int key)
    {
        if (PlantInfoInGame.TryGetValue(key, out var value))
        {
            return value;
        }
        Debug.Log("未找到植物信息");
        return null;
    }
    public void InitializeDictionary()
    {
        PlantInfoInGame.Clear();
        foreach (var entry in PlantInfoes)
        {
            if (entry == null) continue;
            PlantInfoInGame[entry.ID] = entry;
        }
    }
}