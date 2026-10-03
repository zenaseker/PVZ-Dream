using DG.Tweening;
using System;
using UnityEngine;

public class ObjectMove : MonoBehaviour
{
    Vector3 nowvector = Vector3.zero;
    public Action backJump;
    public void Move(Action action = null)
    {
        backJump = action;
        Vector3 vector = this.gameObject.transform.position;
        vector.y -= 1f;
        transform.DOPath(new Vector3[] { transform.position, vector }, 0.5f).SetEase(Ease.Linear).onComplete += () =>
        {
            backJump?.Invoke();
        };
    }
}
