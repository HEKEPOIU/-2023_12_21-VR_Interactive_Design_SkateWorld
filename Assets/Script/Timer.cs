using System;
using UnityEngine;

public class Timer : MonoBehaviour
{
    [SerializeField] private float _totalTime = 1;
    [SerializeField] private bool _autoStart = false;
    [SerializeField] private bool _loop = false;
    public float TotalTime => _totalTime;
    private float _currentTime = 0;
    public event Action OnTimerEnd;

    private void Start()
    {
        if (_autoStart)
        {
            StartTimer();
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
    private void Update()
    {
        _currentTime += Time.deltaTime;
        if (_currentTime >= _totalTime)
        {
            _currentTime = 0;
            OnTimerEnd?.Invoke();
            if (_loop) return;
            gameObject.SetActive(false);
        }
    }

    public void StartTimer()
    {
        gameObject.SetActive(true);
        _currentTime = 0;
    }
}