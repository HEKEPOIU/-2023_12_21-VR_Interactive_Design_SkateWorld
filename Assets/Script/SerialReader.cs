using System;
using System.IO.Ports;
using System.Threading;
using Singleton;
using UnityEngine;

public class SerialReader : PersistentSingleton<SerialReader>
{
    //COM改成自己，後面數字跟Arduino一樣。
    [SerializeField] private string _portName = "COM5";
    private SerialPort _serialPort;
    private Thread _readThread;
    public event Action<string> OnDataReceived;

    protected override void Awake()
    {
        base.Awake();
        if (Instance != this)
        {
            return;
        }

        _serialPort = new SerialPort(_portName, 115200);
        _readThread = new Thread(ReadSerial);
        _readThread.Start();
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        if (Instance != this)
        {
            return;
        }
        _readThread.Abort();
        _serialPort.Close();
    }
    
    private void ReadSerial()
    {
        _serialPort.Open();
        while (_serialPort.IsOpen)
        {
            string data = _serialPort.ReadLine();
            OnDataReceived?.Invoke(data);
        }
    }
}