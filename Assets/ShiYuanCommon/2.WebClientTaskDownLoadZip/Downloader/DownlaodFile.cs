using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using HotUpdateScripts;
using UnityEngine;
using UnityEngine.Networking;
using UniRx;

//下载资源文件，支持断点续传
public class DownlaodFile : MonoBehaviour
{
    public CancellationTokenSource tokenSource = new CancellationTokenSource();
    public CancellationToken token;
    private static DownlaodFile _instance;
    public static DownlaodFile Instance
    {
        get { return _instance; }
    }
    private void Awake()
    {
        _instance = this;
        token = tokenSource.Token;

    }


    public List<Downloader> _thisLoaders;
    public async Task<(bool, string)> DownloadZip(string tempServerUrl, string localPath, System.Action<float> Progress1 = null)
    {
        if (_thisLoaders == null)
        {
            _thisLoaders = new List<Downloader>();
        }
        float UpdateProgress = 0;
        float timer = 0;
        string ResFilePath = System.IO.Path.GetDirectoryName(System.IO.Path.GetFullPath(localPath));
        if (!Directory.Exists(ResFilePath))
        {
            Directory.CreateDirectory(ResFilePath);
        }
        Uri uri = new Uri(tempServerUrl);
        string filePath = System.IO.Path.GetFileNameWithoutExtension(uri.LocalPath);
        var _thisLoader = new Downloader();
        _thisLoaders.Add(_thisLoader);
        Observable.EveryUpdate()
           .Subscribe(_ =>
           {
               if (_thisLoader != null)
               {
                   if (UpdateProgress == _thisLoader.progress)
                   {
                       timer += Time.deltaTime;
                       if (timer >= 30)
                       {
                           timer = 0;
                           _thisLoader.Dispose();
                       }
                   }
                   else
                   {
                       UpdateProgress = _thisLoader.progress;
                       timer = 0;
                   }
                   if (_thisLoader != null)
                   {
                       // Log.PrintWarning("----" + UpdateProgress);
                   }
               }
           }).AddTo(this);
        Task tempAwait = null;
        tempAwait = _thisLoader.Downloaderr(tempServerUrl, localPath);
        while (_thisLoader != null && _thisLoader.isDone == false)
        {
            if (Progress1 != null)
            {
                Progress1(_thisLoader.progress);
            }
            await Task.Yield();
        }

        Debug.LogError(" _thisLoader.reslut :" + _thisLoader.reslut);
        var tempResult = _thisLoader.reslut;
        if (_thisLoader != null && _thisLoader.error != null)
        {
            Debug.LogError("下载失败！" + tempResult);
            return tempResult;
        }
        else
        {
            Debug.Log("下载完成");
        }
        await tempAwait;
        _thisLoader.Dispose();
        _thisLoader = null;
        return tempResult;
    }
    private void OnDestroy()
    {

    }
    private void Start()
    {
        //软件退出会disopos
        // GameManager.Instance.DownLoadDispos += Dispos;
    }

    private void Dispos()
    {
        _thisLoaders.ForEach(_ =>
        {
            _?.Dispose();
            _ = null;
        });
        _thisLoaders.Clear();
        _thisLoaders = null;
    }
    /// <summary>
    /// 下载三遍 带进度
    /// </summary>
    /// <param name="serverPath"></param>
    /// <param name="localPath"></param>
    /// <param name="downloadprogress"></param>
    /// <returns></returns>
    public async Task<(bool, string)> DownLoadZip(string serverPath, string localPath, Action<float> downloadprogress = null)
    {
        int allCount = 4;
        Task<(bool, string)> tempDownLoad = null;
        (bool, string) _tempDownLoad = (false, "");
        // var token = GameManager.Instance.token;
        for (int i = 1; i < allCount; i++)
        {
            await Task.Run(() =>
            {
                Loom.Ins.QueueOnMainThread(() =>
                {
                    tempDownLoad = DownloadZip(serverPath, localPath, downloadprogress);
                });
            }, token);
            await Task.Delay(2000);
            _tempDownLoad = await tempDownLoad;
            if (_tempDownLoad.Item1)
            {
                break;
            }
        }
        return _tempDownLoad;
    }
}