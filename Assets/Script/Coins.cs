using System;
using UnityEngine;

public class Coins : MonoBehaviour, ICollectable
{
    [SerializeField] private GameObject _effect;
    public void Collect()
    {
        GameObject effectObj = Instantiate(_effect, transform.position, Quaternion.identity);
        Destroy(effectObj, 3f);
        Destroy(gameObject);
    }
}
