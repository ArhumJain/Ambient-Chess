using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class LineTweener : MonoBehaviour, IObjectTweener
{   
    [SerializeField] private float speed;
    public void MoveTo(Transform transform, Vector3 targetPosition)
    {
        double distance = Vector3.Distance(targetPosition, transform.position)/1.5;
        transform.DOMove(targetPosition, (float) distance / speed);
    }
    
}
