using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Attribute;

public class MatrixBase : ComponentBase
{
    [HideInInspector] public ComponentDetail _detail;
    protected List<Vector2Int> Area = new List<Vector2Int>();
    public int _RegetMatrixnum { get; private set; }
    public int _RegetMatrixnum2 { get; private set; }
    public int _RegetMatrixnum3 { get; private set; }
    public int _RegetMatrixnum4 { get; private set; }
    public int _RegetMatrixnum5 { get; private set; }
    public virtual void Init(ComponentDetail detail)
    {
        _detail = detail;
        _RegetMatrixnum = 0;
        _RegetMatrixnum2 = 0;
        _RegetMatrixnum3 = 0;
        _RegetMatrixnum4 = 0;
        _RegetMatrixnum5 = 0;
        foreach (Vector2Int vector in Area)
        {
            _RegetMatrixnum += MapManage.Instance.meshPlants[vector.x, vector.y].GetPlants().FindAll(x=> MatrixGetJudge(x)).Count;
            _RegetMatrixnum2 += MapManage.Instance.meshPlants[vector.x, vector.y].GetPlants().FindAll(x => MatrixGetJudge2(x)).Count;
            _RegetMatrixnum3 += MapManage.Instance.meshPlants[vector.x, vector.y].GetPlants().FindAll(x => MatrixGetJudge3(x)).Count;
            _RegetMatrixnum4 += MapManage.Instance.meshPlants[vector.x, vector.y].GetPlants().FindAll(x => MatrixGetJudge4(x)).Count;
            _RegetMatrixnum5 += MapManage.Instance.meshPlants[vector.x, vector.y].GetPlants().FindAll(x => MatrixGetJudge5(x)).Count;
            MapManage.Instance.meshPlants[vector.x, vector.y].meshplantchange += MatrixVoid;
        }
    }
    public virtual void Destory()
    {
        foreach (Vector2 vector in Area)
        {
            MapManage.Instance.meshPlants[(int)vector.x, (int)vector.y].meshplantchange -= MatrixVoid;
        }
    }
    public virtual void MatrixVoid(PlantBase plant, PlantBase originplant)
    {
        if (!MatrixGetJudge(originplant) && MatrixGetJudge(plant)) _RegetMatrixnum++;
        if (MatrixGetJudge(originplant) && !MatrixGetJudge(plant)) _RegetMatrixnum--;
        if (!MatrixGetJudge2(originplant) && MatrixGetJudge2(plant)) _RegetMatrixnum2++;
        if (MatrixGetJudge2(originplant) && !MatrixGetJudge2(plant)) _RegetMatrixnum2--;
        if (!MatrixGetJudge3(originplant) && MatrixGetJudge3(plant)) _RegetMatrixnum3++;
        if (MatrixGetJudge3(originplant) && !MatrixGetJudge3(plant)) _RegetMatrixnum3--;
        if (!MatrixGetJudge4(originplant) && MatrixGetJudge4(plant)) _RegetMatrixnum4++;
        if (MatrixGetJudge4(originplant) && !MatrixGetJudge4(plant)) _RegetMatrixnum4--;
        if (!MatrixGetJudge5(originplant) && MatrixGetJudge5(plant)) _RegetMatrixnum5++;
        if (MatrixGetJudge5(originplant) && !MatrixGetJudge5(plant)) _RegetMatrixnum5--;
        this._detail._owner.OnChangeMesh();
    }
    public virtual bool MatrixGetJudge(PlantBase plant)
    {
        return false;
    }
    public virtual bool MatrixGetJudge2(PlantBase plant)
    {
        return false;
    }
    public virtual bool MatrixGetJudge3(PlantBase plant)
    {
        return false;
    }
    public virtual bool MatrixGetJudge4(PlantBase plant)
    {
        return false;
    }
    public virtual bool MatrixGetJudge5(PlantBase plant)
    {
        return false;
    }
}
