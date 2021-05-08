using System.Collections.Generic;
using System.IO.Ports;
using System.Threading;
using UnityEngine;

class Demo : MonoBehaviour
{
    public string portName = "COM3";
    public int baudrate = 230400;
    public Parity parite = Parity.None;
    public int dataBits = 8;
    public StopBits stopbits = StopBits.One;
    SerialPort port;
    Thread portRev, portDeal;
    Queue<string> dataQueue;
    string outStr = string.Empty;
    private void Start()
    {
        dataQueue = new Queue<string>();
        port = new SerialPort(portName, baudrate, parite, dataBits, stopbits);
        port.ReadTimeout = 4000;
        try
        {
            port.Open();
            Debug.Log("串口打开成功！");
            portRev = new Thread(PortReceivedThread);
            portRev.IsBackground = true;
            portRev.Start();
            portDeal = new Thread(DealData);
            portDeal.Start();
        }
        catch (System.Exception ex)
        {
            Debug.Log(ex.Message);
        }
    }
    void PortReceivedThread()
    {
        try
        {
            byte[] buf = new byte[1];
            string resStr = string.Empty;
            if (port.IsOpen)
            {
                port.Read(buf, 0, 1);
            }
            if (buf.Length == 0)
            {
                return;
            }
            if (buf != null)
            {
                for (int i = 0; i < buf.Length; i++)
                {
                    resStr += buf[i].ToString("X2");
                    dataQueue.Enqueue(resStr);
                }
            }
        }
        catch (System.Exception ex)
        {
            Debug.Log(ex.Message);
        }
    }
    void DealData()
    {
        while (dataQueue.Count != 0)
        {
            for (int i = 0; i < dataQueue.Count; i++)
            {
                outStr += dataQueue.Dequeue();
                if (outStr.Length == 2)
                {
                    if (outStr == "AB")
                    {
                        byte[] ba = new byte[10];
                        for (int j = 0; j < 10; j++)
                        {
                            ba[j] = (byte)j;
                        }
                        SendData(ba);
                    }
                    Debug.Log(outStr);
                    outStr = string.Empty;
                }
            }
        }
    }
    void SendData(byte[] msg)
    {
        //这里要检测一下msg
        if (port.IsOpen)
        {
            port.Write(msg, 0, msg.Length);
        }
    }
    private void Update()
    {
        if (!portRev.IsAlive)
        {
            portRev = new Thread(PortReceivedThread);
            portRev.IsBackground = true;
            portRev.Start();

        }
        if (!portDeal.IsAlive)
        {
            portDeal = new Thread(DealData);
            portDeal.Start();
        }
    }
    void OnApplicationQuit()
    {
        Debug.Log("退出！");
        if (portRev.IsAlive)
        {
            portRev.Abort();
        }
        if (portDeal.IsAlive)
        {
            portDeal.Abort();
        }
        port.Close();
    }
}