using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using UnityEngine;
using LitJson;
using System;
using System.Text;
using HotUpdateScripts;
/// <summary>
/// 基础接口返回内容   根据服务器定义自己的基础数据结构
/// </summary>
[System.Serializable]
public class IBaseOption
{
    /// <summary>
    /// 200-OK
    /// 201-Created
    /// 401-Unauthorized
    /// 403-Forbidden
    /// 404-Not Found
    /// </summary>
    public int code;
    public object exception;
    /// <summary>
    /// 返回结果提示
    /// </summary>
    public string message;
    /// <summary>
    /// 其他返回值
    /// </summary>
    // public Other other;
    public string path;
    public string time;
}
public class SyWebClient : MonoBehaviour
{

    public async Task<(bool, T)> GetHtml<T>(string url, JsonData data = null, string X_Token = null) where T : IBaseOption
    {
        bool isSendSucceed = true;
        T Data = null;
        var MyWebClient = new ShiYuanWebClient();
        MyWebClient.Credentials = CredentialCache.DefaultCredentials;//获取或设置用于向Internet资源的请求进行身份验证的网络凭据
        MyWebClient.Headers[HttpRequestHeader.ContentType] = "application/json";

        if (!string.IsNullOrEmpty(X_Token))
        {
            MyWebClient.Headers["X-Token"] = X_Token;
        }
        MyWebClient.Encoding = Encoding.UTF8;
        try
        {
            string pageData = null;
            if (data == null)
            {
                pageData = await MyWebClient.UploadStringTaskAsync(url, "{}"); //从指定网站下载数据
            }
            else
            {
                pageData = await MyWebClient.UploadStringTaskAsync(url, data.ToJson()); //从指定网站下载数据
            }
            // Data = LitJson.JsonMapper.ToObject<T>(pageData.ToString());
            Data = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(pageData.ToString());
            if (Data.code != 0)
            {
                isSendSucceed = false;
            }
        }
        catch (Exception e)
        {
            Debug.Log("GetHtml error" + e.Message.ToString());
            isSendSucceed = false;
        }
        return (isSendSucceed, Data);
    }
    public async Task<(bool, T)> GetHtml<T>(string url, string data = null, string X_Token = null) where T : IBaseOption
    {
        bool isSendSucceed = true;
        T Data = null;
        // string pageHtml = "";
        var MyWebClient = new ShiYuanWebClient();
        MyWebClient.Credentials = CredentialCache.DefaultCredentials;//获取或设置用于向Internet资源的请求进行身份验证的网络凭据
        MyWebClient.Headers[HttpRequestHeader.ContentType] = "application/json";

        if (!string.IsNullOrEmpty(X_Token))
        {
            MyWebClient.Headers["X-Token"] = X_Token;
        }
        MyWebClient.Encoding = Encoding.UTF8;
        try
        {
            string pageData = null;
            if (data == null)
            {
                pageData = await MyWebClient.UploadStringTaskAsync(url, "{}"); //从指定网站下载数据
            }
            else
            {
                pageData = await MyWebClient.UploadStringTaskAsync(url, data); //从指定网站下载数据
            }
            // Data = LitJson.JsonMapper.ToObject<T>(pageData.ToString());
            Data = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(pageData.ToString());
            if (Data.code != 0)
            {
                isSendSucceed = false;
            }
        }
        catch (Exception e)
        {
            Debug.Log("GetHtml error" + e.Message.ToString());
            isSendSucceed = false;
        }
        return (isSendSucceed, Data);
    }


    public async Task<(bool, string)> GetHtmlString(string url, JsonData data = null, string X_Token = null)
    {
        bool isSendSucceed = true;
        // T Data = null;
        // string str
        string pageHtml = "";
        var MyWebClient = new ShiYuanWebClient();
        MyWebClient.Credentials = CredentialCache.DefaultCredentials;//获取或设置用于向Internet资源的请求进行身份验证的网络凭据
        MyWebClient.Headers[HttpRequestHeader.ContentType] = "application/json";
        if (!string.IsNullOrEmpty(X_Token))
        {
            MyWebClient.Headers["X-Token"] = X_Token;
        }
        // Log.PrintError("-0--------");
        MyWebClient.Encoding = Encoding.UTF8;
        try
        {
            string pageData = null;
            if (data == null)
            {
                pageData = await MyWebClient.UploadStringTaskAsync(url, "{}"); //从指定网站下载数据 
            }
            else
            {
                pageData = await MyWebClient.UploadStringTaskAsync(url, data.ToJson()); //从指定网站下载数据 

            }
            pageHtml = pageData;
        }
        catch (Exception e)
        {
            Debug.Log("GetHtml error" + e.Message.ToString());
            isSendSucceed = false;
        }
        return (isSendSucceed, pageHtml);
    }
}
