using DG.Tweening;
using UnityEngine;

public class RotationMovement : MonoBehaviour
{
    [SerializeField] private float _time = 1.0f;
    [SerializeField] private Vector3 _rotationVector = new Vector3(0, 0, 1);
    [SerializeField] private Ease _ease = Ease.Linear;

    private void Start()
    {
        _rotationVector.Normalize();
        transform.DORotate(_rotationVector * 360, _time, RotateMode.FastBeyond360)
            .SetLoops(-1, LoopType.Incremental).SetEase(_ease);
    }

}
