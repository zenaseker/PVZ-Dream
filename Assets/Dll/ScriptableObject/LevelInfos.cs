using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class LevelInfos : ScriptableObject
{
    [SerializeField]
    public List<Attribute.Level> LevelInfoes = new List<Attribute.Level>();

    public static Dictionary<int, Attribute.Level> LevelInfoInGame = new Dictionary<int, Attribute.Level>();

    public Attribute.Level GetValue(int key)
    {
        if (LevelInfoInGame.TryGetValue(key, out var value))
        {
            return value;
        }
        Debug.Log("未找到植物信息");
        return null;
    }
    public void InitializeDictionary()
    {
        LevelInfoInGame.Clear();
        foreach (var entry in LevelInfoes)
        {
            if (entry == null) continue;
            LevelInfoInGame[entry.ID] = entry;
        }
    }
}
