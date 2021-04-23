using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using System;
// using UnityEngine.EventSystems;
using UnityEngine.Events;
using System.Xml;
using System.IO;
// using Foundation;
// using Foundation.Coroutine;
/// <summary>
/// 录音识别
/// </summary>
public class SpeechRecognition : MonoBehaviour
{

    private string token = "";                           //access_token
    private string cuid = "plants";                  //用户标识
    private string format = "pcm";                  //语音格式
    private int rate = 16000;                        //采样率
    private int channel = 1;                        //声道数
    private string speech;                          //语音数据，进行base64编码
    private int len;                                //原始语音长度
    private int dev_pid = 1536;                     //识别语言及模型选择，不填写lan参数生效，都不填写，默认1537（普通话 输入法模型）1536（普通话(支持简单的英文识别) 支持自定义词库）
    //private string lan = "zh";                      //语种

    private string grant_Type = "client_credentials";
    private string client_ID = "0sdd9tySIpBWzMiOdYd8iOuh";                       //百度appkey
    private string client_Secret = "IDe8w46Is2LTruKdC0MlRDT2P7kwECnp";                   //百度Secret Key

    private string baiduAPI = "http://vop.baidu.com/server_api";
    private string getTokenAPIPath = "https://openapi.baidu.com/oauth/2.0/token";

    private byte[] clipByte;

    private bool isRecognize = false; //是否开始识别
    //private bool isGetToken = false; //是否正在获取token

    /// <summary>
    /// 
    /// 转换出来的TEXT
    /// </summary>
    public static string audioToString;

    private AudioClip audioClip;
    private int audioLength;//录音的长度


    public string Result;

    public void Init()
    {
        StartCoroutine(GetToken());
        ReadKeyWord("/Json/KeyWord.json");
    }

    /// <summary>
    /// 获取百度用户令牌
    /// </summary>
    /// <param name="url">获取的url</param>
    /// <returns></returns>
    private IEnumerator<float> GetToken()
    {

        //isGetToken = true;

        WWWForm getTForm = new WWWForm();
        getTForm.AddField("grant_type", grant_Type);
        getTForm.AddField("client_id", client_ID);
        getTForm.AddField("client_secret", client_Secret);

        WWW getTW = new WWW(getTokenAPIPath, getTForm);
        while (!getTW.isDone)
        {
            yield return 0;
        }
        if (getTW.isDone)
        {
            if (getTW.error == null)
            {
                token = JsonMapper.ToObject(getTW.text)["access_token"].ToString();
                Debug.Log("token:" + token);
            }
            else
            {
                Debug.LogError("正在连接语音识别系统，请保持网络畅通");
            }
        }
        //isGetToken = false;
    }
    /// <summary>
    /// 开始录音
    /// </summary>
    /// <param name="time"></param>
    /// <returns>是否有麦克风</returns>
    public void StartMic(int time)
    {

        isRecognize = false;

        if (Microphone.devices.Length == 0)
        {

            Debug.LogError("未发现麦克风设备");
            // Mgrs.Ins.sceneTwoMgr.isMike = true;
            // EventManager.Ins.Emit("KeywordResult", -2);
            // return false;
        }
        Microphone.End(null);
        Debug.Log("StartMic");
        Debug.Log(Microphone.devices + " " + Microphone.devices[0]);

        audioLength = time;
        audioClip = Microphone.Start(null, false, time, rate);

        // return true;
    }




    /// <summary>
    /// 放开录音按钮，结束录音并提交识别
    /// </summary>
    /// <param name="eventData"></param>
    public void StopMic(Action<string> action = null)
    {
        string name = DateTime.Now.Year.ToString()
            + "-" + DateTime.Now.Month.ToString()
            + "-" + DateTime.Now.Day.ToString()
            + "-" + DateTime.Now.Hour.ToString()
            + "-" + DateTime.Now.Minute.ToString()
            + "-" + DateTime.Now.Second.ToString();
        StopRecordAudio(Application.streamingAssetsPath + "/Audio/" + name + ".wav");
        EndMic(action, true);
    }

    /// <summary>
    /// 停止录音,并保存到指定路径
    /// </summary>
    /// <param name="name">存储名字</param>
    public AudioClip StopRecordAudio(string filepath)
    {
        if (!isRecognize)
        {
            int lastPos = Microphone.GetPosition(null);
            if (Microphone.IsRecording(null))
            {
                audioLength = lastPos / rate;//录音时长  
                //用户自由固定录音时长 如果录音时间小于指定长度，由于audioClip不可以缩短时长，重新用数据生成
                if (lastPos > 0)
                {
                    var soundata = new float[audioClip.samples * audioClip.channels];
                    audioClip.GetData(soundata, 0);
                    var newdata = new float[lastPos * audioClip.channels];
                    var dLen = newdata.Length;
                    for (int i = 0; i < dLen; i++)
                    {
                        newdata[i] = soundata[i];
                    }
                    audioClip = AudioClip.Create(audioClip.name, lastPos, audioClip.channels, audioClip.frequency, false);
                    audioClip.SetData(newdata, 0);
                }
                //Debug.Log("EndMic");
                Microphone.End(null);
                //保存录音
                byte[] recordData = WavUtility.FromAudioClip(audioClip, filepath);
                //byte[] recordData = WavUtility.TestFromAudioClip(CurAudioSource.clip);
                Debug.Log("录音文件保存的路径在：" + filepath);
                return audioClip;
                //AssetDatabase.Refresh();
                //return recordData;
            }
            //audioClip.Stop();
        }
        return null;
    }

    /// <summary>
    /// 结束录音并上传
    /// </summary>
    /// <param name="Result">识别出来的结果</param>
    /// <param name="isNeedRecognize">是否需要识别</param>
    /// <param name="isPinyin">是否需要z转成拼音</param>
    public void EndMic(Action<string> action = null, bool isNeedRecognize = true, bool isPinyin = true)
    {
        if (!isRecognize)
        {
            int lastPos = Microphone.GetPosition(null);
            if (Microphone.IsRecording(null))
            {
                audioLength = lastPos / rate;//录音时长  
                //用户自由固定录音时长 如果录音时间小于指定长度，由于audioClip不可以缩短时长，重新用数据生成
                if (lastPos > 0)
                {
                    var soundata = new float[audioClip.samples * audioClip.channels];
                    audioClip.GetData(soundata, 0);
                    var newdata = new float[lastPos * audioClip.channels];
                    var dLen = newdata.Length;
                    for (int i = 0; i < dLen; i++)
                    {
                        newdata[i] = soundata[i];
                    }
                    audioClip = AudioClip.Create(audioClip.name, lastPos, audioClip.channels, audioClip.frequency, false);
                    audioClip.SetData(newdata, 0);
                }
                //Debug.Log("EndMic");
                Microphone.End(null);
            }
            clipByte = GetClipData();
            if (clipByte == null)
            {
                Debug.LogError("clipByte == null");
                return;
            }

            len = clipByte.Length;
            speech = Convert.ToBase64String(clipByte);
            //Debug.Log(len);
            //Debug.Log(audioLength + " " + audioClip.length);
        }

        isRecognize = isNeedRecognize;

        if (isRecognize)
        {
            //StartCoroutine(GetAudioString(baiduAPI));
            JsonWriter jw = new JsonWriter();
            jw.WriteObjectStart();
            jw.WritePropertyName("format");
            jw.Write(format);
            jw.WritePropertyName("rate");
            jw.Write(rate);
            jw.WritePropertyName("dev_pid");
            jw.Write(dev_pid);
            jw.WritePropertyName("channel");
            jw.Write(channel);
            jw.WritePropertyName("token");
            jw.Write(token);
            jw.WritePropertyName("cuid");
            jw.Write(cuid);
            jw.WritePropertyName("len");
            jw.Write(len);
            jw.WritePropertyName("speech");
            jw.Write(speech);
            jw.WriteObjectEnd();


            StartCoroutine(BasePostFromWWW(1, baiduAPI, jw.ToString(), (www) =>
             {
                 JsonData getASWJson = JsonMapper.ToObject(www.text);
                 Debug.Log(getASWJson.ToString());
                 if (getASWJson["err_msg"].ToString() == "success.")
                 {
                     audioToString = getASWJson["result"][0].ToString();
                     if (audioToString.Substring(audioToString.Length - 1) == "，")
                         audioToString = audioToString.Substring(0, audioToString.Length - 1);

                     if (isPinyin == true)
                     {
                         Result = PinYinHelper.ConvertToAllSpell(audioToString);
                     }
                     else
                     {
                         Result = audioToString;
                     }
                     Debug.Log(Result);
                     //  CheckAll(true);
                     //  action() ?.Invoke();
                     if (action == null) return;
                     action(CheckAll(true));
                     //Mgrs.Ins.sceneTwoMgr.CheckWord(pinyin);
                     //处理获得的识别文字
                     //EventManager.GetInstance().DispachEvent(GameEvent.RECORD_RECOGNIZE_COMPLETE, new RecordData(audioToString, audioClip));

                 }
                 else
                 {
                     action(getASWJson["err_msg"].ToString());
                     //    EventManager.Ins.Emit("KeywordResult", -1 );
                     //Debug.LogError(getASWJson["err_msg"].ToString());
                     //    EventManager.Ins.Emit("KeywordResult", -1);
                 }

             }));
        }
    }
    public const int MaxTryCountDownload = 3;
    public const float DownloadTimeout = 15.0f;     // 下载超时时间
    private IEnumerator<float> BasePostFromWWW(int tryCount, string pUrl, string pPostData, Action<WWW> E_OnCallback)
    {
        //logger.LogInfo("BasePostFromWWW Try Count = {0}, PostData = {1}, Url = {2}".F(tryCount, pPostData, pUrl));
        if (tryCount < MaxTryCountDownload)
        {
            //将request文本转为byte数组  
            byte[] bs = System.Text.UTF8Encoding.UTF8.GetBytes(pPostData);
            //向HTTP服务器提交Post数据  
            WWW getData = new WWW(pUrl, bs);
            bool bTimeout = false;
            float waitingTime = 0.0f;
            float progress = 0.0f;

            //yield return getData;
            while ((!getData.isDone) && getData.error == null && !bTimeout)
            {
                if (progress == getData.progress)
                    waitingTime += Time.deltaTime;
                else
                {
                    progress = getData.progress;
                    waitingTime = 0.0f;
                }

                if (waitingTime >= DownloadTimeout)
                {
                    bTimeout = true;
                    break;
                }
                yield return 0;
            }

            if (getData.error != null || bTimeout)
            {
                Debug.LogError(getData.error);
                BasePostFromWWW(tryCount + 1, pUrl, pPostData, E_OnCallback);
            }
            else
            {
                E_OnCallback.InvokeSafely(getData);
                getData.Dispose();
            }
        }
        else
        {
            Debug.LogError("DownloadManager::DownloadFromWWWAsync download failed.");
            E_OnCallback(null);
        }
    }
    /// <summary>
    /// 把录音转换为Byte[]
    /// </summary>
    /// <returns></returns>
    private byte[] GetClipData()
    {
        if (audioClip == null)
        {
            Debug.LogError("录音数据为空");
            return null;
        }
        float[] samples = new float[audioClip.samples];
        audioClip.GetData(samples, 0);
        byte[] outData = new byte[samples.Length * 2];
        int rescaleFactor = 32767; //to convert float to Int16   
        for (int i = 0; i < samples.Length; i++)
        {
            short temshort = (short)(samples[i] * rescaleFactor);

            byte[] temdata = System.BitConverter.GetBytes(temshort);

            outData[i * 2] = temdata[0];
            outData[i * 2 + 1] = temdata[1];
        }
        if (outData == null || outData.Length <= 0)
        {
            Debug.LogError("录音数据为空");
            return null;
        }
        //return SubByte(outData, 0, audioLength * 8000 * 2);
        return outData;
    }

    AllKey AllKW = null;
    //读取所有关键字
    public void ReadKeyWord(string path)
    {
        //文件路径
        string testJsonPath = Application.streamingAssetsPath + path;
        //AllKeyWord.Clear();
        // Debug.LogError(testJsonPath);
        if (File.Exists(testJsonPath))
        {
            string jsonString = File.ReadAllText(testJsonPath);
            //Debug.LogError(jsonString);
            AllKW = JsonUtility.FromJson<AllKey>(jsonString);
            if (AllKW.list.Count == 0)
            {
                Debug.LogError("文件已找到，但读取到的内容为空");
            }
            else
            {
                Debug.LogWarning("数据读取成功");
            }
        }
        else
        {
            Debug.LogError("读取文件失败，未找到 " + testJsonPath + " 路径");
        }

    }

    //任意关键字匹配
    public string CheckAll(bool isMust)
    {
        // bool isSucceed = false;
        //如果需要判断must关键字
        if (isMust == true)
        {
            //遍历所有省份
            for (int i = 0; i < AllKW.list.Count; i++)
            {
                //遍历整个省份下的must关键字
                for (int j = 0; j < AllKW.list[i].must.Count; j++)
                {
                    //如果识别出来的语音中包含某条must关键字
                    if (Result.Contains(AllKW.list[i].must[j]))
                    {
                        int mustIndex = Result.IndexOf(AllKW.list[i].must[j]);
                        Debug.Log("包含must关键字");
                        //如果这个省份有maybe关键字
                        if (AllKW.list[i].maybe.Count > 0)
                        {
                            //继续遍历该省份下所有maybe关键字
                            for (int k = 0; k < AllKW.list[i].maybe.Count; k++)
                            {
                                //如果识别出来的语音中包含某条maybe关键字
                                if (Result.Contains(AllKW.list[i].maybe[k]))
                                {
                                    Debug.Log("包含maybe关键字");
                                    int mybeIndex = Result.IndexOf(AllKW.list[i].maybe[k]);
                                    //判断maybe关键字是不是在must关键字后面
                                    if (mybeIndex > mustIndex)
                                    {
                                        Debug.Log("包含must关键字和maybe关键字");
                                        //输出这个省份的名称
                                        Debug.Log(AllKW.list[i].name);
                                        // isSucceed = true;
                                        // return isSucceed;
                                        return AllKW.list[i].name;
                                    }
                                }
                            }
                        }
                        //如果这个省份没有maybe关键字
                        else
                        {
                            Debug.Log(AllKW.list[i].name);
                            // isSucceed = true;
                            // return isSucceed;
                            return AllKW.list[i].name;
                        }

                    }
                }

            }
        }
        //如果不需要判断must关键字
        else
        {
            //遍历所有省份
            for (int i = 0; i < AllKW.list.Count; i++)
            {
                //Debug.LogError(ak.list[i].maybe[0]);
                //遍历每个省份下的每个maybe关键字
                for (int j = 0; j < AllKW.list[i].maybe.Count; j++)
                {
                    //如果识别出来的语音中包含某条maybe关键字
                    if (Result.Contains(AllKW.list[i].maybe[j]))
                    {
                        Debug.Log("包含");
                        //输出这个省的名字
                        Debug.Log(AllKW.list[i].name);
                        // isSucceed = true;
                        // return isSucceed;
                        return AllKW.list[i].name;
                    }
                }
            }
        }
        // return isSucceed;
        return string.Empty;
    }

    //指定题目关键字匹配
    public bool CheckAssign(int index, bool isMust)
    {
        bool isSucceed = false;
        if (isMust == true)
        {
            //遍历指定的省份must关键字
            for (int i = 0; i < AllKW.list[index].must.Count; i++)
            {
                //如果识别结果中包含某个must关键字
                if (Result.Contains(AllKW.list[index].must[i]))
                {
                    int mustIndex = Result.IndexOf(AllKW.list[i].must[i]);
                    Debug.Log("包含must关键字");
                    //如果这个省份有maybe关键字
                    if (AllKW.list[index].maybe.Count > 0)
                    {
                        //继续遍历该省份下所有maybe关键字
                        for (int j = 0; j < AllKW.list[index].maybe.Count; j++)
                        {
                            //如果识别出来的语音中包含某条maybe关键字
                            if (Result.Contains(AllKW.list[index].maybe[j]))
                            {
                                Debug.Log("包含maybe关键字");
                                int mybeIndex = Result.IndexOf(AllKW.list[i].maybe[j]);
                                if (mybeIndex > mustIndex)
                                {
                                    Debug.Log("包含must关键字和maybe关键字");
                                    //输出这个省份的名称
                                    Debug.Log(AllKW.list[i].name);
                                    isSucceed = true;
                                    return isSucceed;
                                }
                            }
                        }
                    }
                    //如果没有maybe关键字
                    else
                    {
                        Debug.Log(AllKW.list[i].name);
                        isSucceed = true;
                        return isSucceed;
                    }


                }
            }
        }
        else
        {
            //遍历指定省份下的所有maybe关键字
            for (int i = 0; i < AllKW.list[index].maybe.Count; i++)
            {
                //如果识别结果中包含某一个maybe关键字
                if (Result.Contains(AllKW.list[index].maybe[i]))
                {
                    //输出这个省的名字
                    Debug.Log(AllKW.list[i].name);
                    isSucceed = true;
                    return isSucceed;
                }
            }
        }
        return isSucceed;
    }

}

[System.Serializable]
public class KeyWord
{
    public string name;
    public List<string> must = new List<string>();
    public List<string> maybe = new List<string>();
}
[System.Serializable]
public class AllKey
{
    public List<KeyWord> list = new List<KeyWord>();
}