using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class YoyoLocalMotion : MonoBehaviour
{
    [SerializeField] private float _time = 1.0f;
    [SerializeField] private Vector3 _target = new Vector3(0, 0, 1);
    [SerializeField] private Ease _ease = Ease.Linear;
    [SerializeField] private bool _local = true;
    void Start()
    {
        if (_local)
        {
            transform.DOLocalMove(_target, _time).SetLoops(-1, LoopType.Yoyo).SetEase(_ease);
        }
        else
        {
            transform.DOMove(_target, _time).SetLoops(-1, LoopType.Yoyo).SetEase(_ease);
        }
    }


}
