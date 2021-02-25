using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ArcTweener : MonoBehaviour, IObjectTweener
{
    [SerializeField] private float speed;
    [SerializeField] private float height;

    public void MoveTo(Transform transform, Vector3 targetPosition)
    {
        double distance = Vector3.Distance(targetPosition, transform.position)/1.5;
        transform.DOJump(targetPosition, height, 1, (float) distance / speed);
    }
}
