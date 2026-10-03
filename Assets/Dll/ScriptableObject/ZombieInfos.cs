using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ZombieInfos : ScriptableObject
{
    [SerializeField]
    public List<Attribute.ZombieInfo> ZombieInfoes = new List<Attribute.ZombieInfo>();

    public static Dictionary<int, Attribute.ZombieInfo> ZombieInfoInGame = new Dictionary<int, Attribute.ZombieInfo>();

    public Attribute.ZombieInfo GetValue(int key)
    {
        if (ZombieInfoInGame.TryGetValue(key, out var value))
        {
            return value;
        }
        Debug.Log("Œ¥’“µΩΩ© ¨–≈œ¢");
        return null;
    }
    public void InitializeDictionary()
    {
        ZombieInfoInGame.Clear();
        foreach (var entry in ZombieInfoes)
        {
            if (entry == null) continue;
            ZombieInfoInGame[entry.ID] = entry;
        }
    }
}