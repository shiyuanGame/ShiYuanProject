using UnityEngine;
using System.IO.Ports;
using System.Text;
using UnityEngine.UI;

public class SerialPortTest : MonoBehaviour
{
    private SerialPort sp = new SerialPort();
    public Text m_TextSendDataPar;
    public Text m_TextShowData;

    // Use this for initialization
    void Start()
    {
        //打开串口
        Init("COM3", 230400, Parity.None, 8, StopBits.None);
    }

    //发送数据按钮
    public void Btn_SendData()
    {
        byte dHead = (byte)0xA5;
        byte message = (byte)0x90;
        byte[] data = new byte[2];
        data[0] = dHead;
        data[1] = message;
        // prot.SendData(data);
        Data_Send(data);
    }

    //初始化串口类
    public void Init(string _portName, int _baudRate, Parity _parity, int dataBits, StopBits _stopbits)
    {
        sp = new SerialPort(_portName, _baudRate, _parity, dataBits, _stopbits);//绑定端口
        sp.DataReceived += new SerialDataReceivedEventHandler(Data_Received);//订阅委托
        sp.Open();
    }

    //接收数据
    private void Data_Received(object sender, SerialDataReceivedEventArgs e)
    {
        byte[] ReDatas = new byte[sp.BytesToRead];
        sp.Read(ReDatas, 0, ReDatas.Length);//读取数据
        this.Data_Show(ReDatas);//显示数据
    }

    /// <summary>
    /// 显示数据
    /// </summary>
    /// <param name="data">字节数组</param>
    public void Data_Show(byte[] data)
    {
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < data.Length; i++)
        {
            sb.AppendFormat("{0:x2}" + "", data[i]);
        }
        Debug.Log(sb.ToString());
        m_TextShowData.text = sb.ToString();
    }

    //发送数据
    public void Data_Send(string _parameter)
    {

        sp.WriteLine(_parameter);
    }
    public void Data_Send(byte[] data)
    {

        sp.Write(data, 0, data.Length);
    }


}