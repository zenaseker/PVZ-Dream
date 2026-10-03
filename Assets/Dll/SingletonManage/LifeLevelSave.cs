using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeLevelSave
{
    public LifeLevelSave(int _id,int _sun,int _range)
    {
        ID = _id;
        SunNumber = _sun;
        RangeCount = _range;
    }
    public int ID;
    public int SunNumber;
    public int RangeCount;
    public List<int> DreamDepthPlant;
    public List<PlantInfoInLifeSave> plantInfoInLifeSaves;

    public class PlantInfoInLifeSave
    {
        public PlantInfoInLifeSave(int _id, Vector2Int _pos, PlantPosType _postype, int _hp)
        {
            ID = _id;
            Pos = _pos;
            PlantPosType = _postype;
            Hp = _hp;
        }
        public int ID;
        public Vector2Int Pos;
        public PlantPosType PlantPosType;
        public int Hp;
    }
}
