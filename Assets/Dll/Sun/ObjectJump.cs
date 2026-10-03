using UnityEngine;
using DG.Tweening;
using System;

public class ObjectJump : MonoBehaviour
{
    public Vector3 centervector = Vector3.zero;
    public Vector3 pvector = Vector3.zero;
    public Action backJump;
    public void Move(Vector3 EndPos, Action action = null)
    {
        backJump = action;
        if (EndPos != Vector3.zero)
        {
            pvector = EndPos;
        }
        else
        {
            pvector = this.transform.position;
            pvector.x -= 1f;
            pvector.y -= 1f;
        }
        while (!IsInView(pvector, out Vector3 viewPos))
        {
            if (viewPos.x < 0.1f)
            {
                pvector += Vector3.right * 0.2f;
            }
            if (viewPos.x > 0.9f)
            {
                pvector += Vector3.left * 0.2f;
            }
            if (viewPos.y < 0.1f)
            {
                pvector += Vector3.up * 0.2f;
            }
            if (viewPos.y > 0.9f)
            {
                pvector += Vector3.down * 0.2f;
            }
        }
        centervector = new Vector3((this.transform.position.x + pvector.x) / 2, this.transform.position.y + 1f, pvector.z);
        transform.DOPath(new Vector3[] { transform.position, centervector, pvector }, 0.5f, PathType.CatmullRom).SetEase(Ease.Linear).onComplete += () =>
        {
            backJump?.Invoke();
        };
    }
    public bool IsInView(Vector3 worldPos, out Vector3 viewPos)
    {
        viewPos = Camera.main.WorldToViewportPoint(worldPos);
        if (viewPos.z < 0)  return false;
        if (viewPos.z > Camera.main.farClipPlane) return false;
        if (viewPos.x >= 0.1f && viewPos.x <= 0.9f && viewPos.y >= 0.1f && viewPos.y <= 0.9f) return true;
            return false;
    }

}
