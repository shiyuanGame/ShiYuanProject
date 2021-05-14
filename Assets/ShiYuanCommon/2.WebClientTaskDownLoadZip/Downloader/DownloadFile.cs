using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
namespace HotUpdateScripts
{

    //下载资源文件，支持断点续传
    public class DownloadFile : MonoBehaviour
    {
        private static DownloadFile _instance;
        public static DownloadFile Instance
        {
            get { return _instance; }
        }
        public CancellationTokenSource tokenSource = new CancellationTokenSource();
        private void Awake()
        {
            _instance = this;
        }
        private float UpdateProgress;
        private float timer = 0;
        private void Update()
        {
            /// <summary>
            /// async 异步进行下载 不会手动释放   需要通过 计时器进行dispos 才能拿到下载错误
            /// </summary>
            /// <value></value>
            for (int i = 0; i < loadActions?.Count; i++)
            {
                loadActions[i]?.Invoke();
            }
        }
        public List<Downloader> Loaders = new List<Downloader>();
        public List<Action> loadActions = new List<Action>();
        public async Task<(bool, string)> DownloadZip(string tempServerUrl, string localPath, System.Action<float> Progress1 = null)
        {
            string ResFilePath = System.IO.Path.GetDirectoryName(System.IO.Path.GetFullPath(localPath));
            if (!Directory.Exists(ResFilePath))
            {
                Directory.CreateDirectory(ResFilePath);
            }
            Uri uri = new Uri(tempServerUrl);
            var Loader = new Downloader();
            Loaders.Add(Loader);
            float UpdateProgress = 0;
            float timer = 0;
            Action action = null;
            action = () =>
            {
                if (Loader != null)
                {
                    if (UpdateProgress == Loader.progress)
                    {
                        timer += Time.deltaTime;
                        if (timer >= 30)
                        {
                            timer = 0;
                            Loader.Dispose();
                            loadActions.Remove(action);
                        }
                    }
                    else
                    {
                        UpdateProgress = Loader.progress;
                        timer = 0;
                    }
                }
            };
            loadActions.Add(action);
            Task tempAwait = null;
            tempAwait = Loader.Downloaderr(tempServerUrl, localPath);
            while (Loader != null && Loader.isDone == false)
            {
                if (null != Progress1)
                {
                    Progress1(Loader.progress);
                }
                await Task.Yield();
            }
            var tempResult = Loader.reslut;
            if (Loader != null && Loader.error != null)
            {
                if (loadActions.Contains(action))
                {
                    loadActions.Remove(action);
                }
                Debug.LogError("下载失败！" + tempResult);
                return tempResult;
            }
            else
            {
                Debug.Log("下载完成");
            }
            await tempAwait;
            Loader.Dispose();
            Loader = null;
            return tempResult;
        }
        private void Start()
        {
            // GameManager.Instance.DownLoadDispos = Dispos;
        }
        private void Dispos()
        {
            Loaders?.ForEach((_) =>
            {
                _?.Dispose();
                _ = null;
            });
            Loaders.Clear();
            Loaders = null; ;
        }
        /// <summary>
        /// 下载三次zip 或者文件
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

            //自己申请
            CancellationToken token = tokenSource.Token;
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
}