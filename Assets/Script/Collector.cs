using System;
using UnityEngine;

public class Collector : MonoBehaviour
{
    public event Action OnCollect;
    private void OnTriggerEnter(Collider other)
    {
        ICollectable collectable = other.GetComponent<ICollectable>();
        if (collectable == null) return;
        OnCollect?.Invoke();
        collectable.Collect();
    }
}