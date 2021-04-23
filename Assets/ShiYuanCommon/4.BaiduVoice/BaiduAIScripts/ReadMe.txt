

>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>使用说明<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<

1.将Plugins文件夹放入Asset路径下。
2.将Scripts文件夹中的脚本复制到自己的工程里。
3.将SpeechRecognition脚本挂载到GameCanvas对象上。
4.在CommonResources脚本中声明SpeechRecognition的对象SR。
5.获取SR对象并在CommonResources的Init方法中调用SR的Init方法。
6.完成上述操作后即可在项目中通过T.CRE.SR来调用相关的方法。


>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>主要代码说明<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<

1.SpeechRecognition为主要脚本。
2.SpeechRecognition中的Init方法为初始化方法，只在整个游戏开启时调用一次。
3.grant_Type，client_ID，client_Secret，baiduAPI，getTokenAPIPath是调用百度语音识别API时的相关配置。
4.GetToken协程为获取Token的方法。
5.StartMic方法为开始录音方法，参数time为Int类型，是录音时长。
6.StopMic方法为停止录音并保存，同时提交给百度语音识别接口进行识别，参数action是识别完成后的回调。
7.StopRecordAudio方法是停止录音并保存到本地的方法，参数filepath为保存的路径。
8.EndMic方法是结束录音并上传进行识别的方法，参数action为识别完成后的回调，isNeedRecognize为是否需要进行识别，isPinyin为识别后是否需要转为拼音（默认为true）。
9.BasePostFromWWW协程为向百度语音识别接口提交内容进行识别的方法，目前采用WWW通讯协议，后续有时间会进行优化。

10.ReadKeyWord方法为读取所有关键字方法，参数path为要读取的文件路径（根据自己的文件存放位置定）。
11.CheckAll是在所有关键字中进行匹配方法，参数isMust是控制本次识别是只识别maybe关键字（false），还是需同时识别must关键字和maybe关键字（true）。返回值为bool。
12.CheckAssign是识别指定的某一套关键字，参数index是指定识别所有关键字list中的哪一组关键字，参数isMust是控制本次识别是只识别maybe关键字（false），
     还是需同时识别must关键字和maybe关键字（true）。返回值为bool。

13.KeyWord是单组关键字的结构体,name是这一组的名字标签，
     List must存放的关键字是必须要有且完全说对的关键字，通常只有一条。
     List mabe用于存放只需说中一条就算对的关键字，一般会有多条。
     可根据需求酌情添加其他字段，但must 和 maybe两个list尽量不要修改。
14.AllKey是用于接收读所有关键字的结构体，所读取到的所有keyWord都被List list接收并存储。


>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>配置关键字<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<

 按照格式配置，如下。
{
  "list":[
    {
      "name": "hebei",
      "must": [ "_shi_jia_zhuang_" ],
      "maybe": [ "_zhang_jia_kou_", "_qin_huang_dao_" ]
    },
    {
      "name": "henan",
      "must": [ "_zheng_zhou_"],
      "maybe": [ "_xin_xiang_","_kai_feng_","_zheng_zhou_","_luo_yang_" ]
    }
  ]
}