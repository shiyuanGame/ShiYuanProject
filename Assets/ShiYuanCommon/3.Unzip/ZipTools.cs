using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ICSharpCode.SharpZipLib.Zip;
using System.IO;
using System;
using System.Text;
using System.Threading.Tasks;
using ICSharpCode.SharpZipLib.Core;
// using ICSharpCode.SharpZipLib.Zip;
using ICSharpCode.SharpZipLib.Zip.Compression.Streams;
using HotUpdateScripts;

public class ZipTools : MonoBehaviour
{
    private static ZipTools _instance;
    public static ZipTools Instance
    {
        get { return _instance; }
    }
    private void Awake()
    {
        _instance = this;
    }
    /// <summary>
    /// 简单创建压缩Zip文件
    /// </summary>
    /// <param name="fileNames">需要压缩的文件集合</param>
    /// <param name="outputFilePath">压缩文件生成的路径</param>
    /// <param name="compressLevel">压缩等级0-9</param>
    public void TestZipFile(string[] fileNames, string outputFilePath, int compressLevel)
    {
        try
        {
            using (ZipOutputStream stream = new ZipOutputStream(File.Create(outputFilePath)))
            {
                stream.SetLevel(compressLevel); //设置压缩等级
                byte[] buffer = new byte[4096];
                foreach (string file in fileNames)
                {
                    var entry = new ZipEntry(Path.GetFileName(file));
                    entry.DateTime = DateTime.Now;
                    stream.PutNextEntry(entry);

                    using (FileStream fs = File.OpenRead(file))
                    {
                        int sourceBytes;
                        do
                        {
                            sourceBytes = fs.Read(buffer, 0, buffer.Length);
                            stream.Write(buffer, 0, sourceBytes);

                        } while (sourceBytes > 0);
                    }
                }
                stream.Finish();
                stream.Close();
                Debug.Log("压缩完成！");
            }
        }
        catch (Exception ex)
        {
            Debug.Log("异常为：" + ex);
        }
    }

    /// <summary>
    /// 解压
    /// </summary>
    /// <param name="zipPath">压缩文件路径</param>
    /// <param name="outPath">解压出去路径</param>
    public void DecompressZipFile(string zipPath, string outPath, Action decompressSuccessCallBcak = null)
    {
        if (!File.Exists(zipPath))
        {
            Debug.LogError("没有此文件路径：" + zipPath);
            return;
        }
        try
        {
            using (ZipInputStream zipStream = new ZipInputStream(File.OpenRead(zipPath)))
            {
                //程序运行路径
                string appEXEFilePath = string.Empty;

                Encoding gbk = Encoding.GetEncoding("GB18030");
                ZipConstants.DefaultCodePage = gbk.CodePage;
                ZipEntry theEntry;
                while ((theEntry = zipStream.GetNextEntry()) != null)
                {
                    string entryName = theEntry.Name;
                    //Debug.Log("theEntry.Name：" + entryName);
                    string fileName = Path.GetFileName(entryName);
                    //Debug.Log ("fileName：" + fileName);
                    string filePath = Path.Combine(outPath, entryName);
                    //Debug.Log ("filePath:" + filePath);
                    string directoryName = Path.GetDirectoryName(filePath);
                    // Debug.Log ("directoryName：" + directoryName);
                    string fileExtension = Path.GetExtension(entryName);
                    //Debug.Log("fileExtension：" + fileExtension);
                    // if (!string.IsNullOrEmpty(fileExtension) && fileExtension.Equals(".exe"))
                    // {
                    //     appEXEFilePath = filePath;
                    //     Debug.Log(appEXEFilePath);
                    // }
                    // 创建压缩文件中文件的位置
                    if (directoryName.Length > 0)
                    {
                        Directory.CreateDirectory(directoryName);
                    }
                    if (fileName != String.Empty)
                    {
                        using (FileStream streamWriter = File.Create(filePath))
                        {
                            int size = 2048;
                            byte[] data = new byte[2048];
                            while (true)
                            {
                                size = zipStream.Read(data, 0, data.Length);
                                if (size > 0)
                                {
                                    streamWriter.Write(data, 0, size);
                                }
                                else
                                {
                                    // Debug.Log (theEntry.Name+"解压完成！");
                                    break;
                                }
                            }
                            streamWriter.Close();
                        }
                    }
                }
                zipStream.Close();
                zipStream.Dispose();
                Debug.Log("解压完成！");
                if (File.Exists(zipPath))
                {
                    File.Delete(zipPath);
                }
                if (decompressSuccessCallBcak != null)
                {
                    // decompressSuccessCallBcak(appEXEFilePath);
                    decompressSuccessCallBcak();
                    decompressSuccessCallBcak = null;
                }
            }
        }
        catch (Exception ex)
        {
            Debug.Log("异常为：" + ex);
            DecompressZipFile(zipPath, outPath, decompressSuccessCallBcak);
        }

    }
    public async Task<(bool, string)> DecompressZipFile(string zipPath, string outPath)
    {
        if (!File.Exists(zipPath))
        {
            // Debug.LogError("没有此文件路径：" + zipPath);
            return (false, "没有此文件路径：" + zipPath);
        }
        try
        {
            using (ZipInputStream zipStream = new ZipInputStream(File.OpenRead(zipPath)))
            {
                //程序运行路径
                string appEXEFilePath = string.Empty;

                Encoding gbk = Encoding.GetEncoding("GB18030");
                ZipConstants.DefaultCodePage = gbk.CodePage;
                ZipEntry theEntry;
                while ((theEntry = zipStream.GetNextEntry()) != null)
                {
                    // Log.PrintError(" DecompressZipFile:  " + zipStream.CanTimeout);
                    string entryName = theEntry.Name;
                    //Debug.Log("theEntry.Name：" + entryName);
                    string fileName = Path.GetFileName(entryName);
                    //Debug.Log ("fileName：" + fileName);
                    string filePath = Path.Combine(outPath, entryName);
                    //Debug.Log ("filePath:" + filePath);
                    string directoryName = Path.GetDirectoryName(filePath);
                    // Debug.Log ("directoryName：" + directoryName);
                    string fileExtension = Path.GetExtension(entryName);
                    //Debug.Log("fileExtension：" + fileExtension);
                    // if (!string.IsNullOrEmpty(fileExtension) && fileExtension.Equals(".exe"))
                    // {
                    //     appEXEFilePath = filePath;
                    //     Debug.Log(appEXEFilePath);
                    // }
                    // 创建压缩文件中文件的位置
                    if (directoryName.Length > 0)
                    {
                        Directory.CreateDirectory(directoryName);
                    }
                    if (fileName != String.Empty)
                    {
                        using (FileStream streamWriter = File.Create(filePath))
                        {
                            int size = 2048;
                            byte[] data = new byte[2048];
                            while (true)
                            {
                                size = zipStream.Read(data, 0, data.Length);
                                if (size > 0)
                                {
                                    streamWriter.Write(data, 0, size);
                                }
                                else
                                {
                                    break;
                                }
                                await Task.Yield();
                            }
                            streamWriter.Close();
                        }
                    }
                    await Task.Yield();
                }
                zipStream.Close();
                zipStream.Dispose();
                Debug.Log("解压完成！");
                if (File.Exists(zipPath))
                {
                    File.Delete(zipPath);
                }
                return (true, "成功");
            }
        }
        catch (Exception ex)
        {
            Debug.Log("异常为：" + ex);
            return (false, "异常为：" + ex);
        }
    }
    public bool UnZip(string FileToUpZip, string ZipedFolder, Action decompressSuccessCallBcak = null)
    {
        if (!File.Exists(FileToUpZip))
        {
            Debug.LogError("UnZip Is Not Exists !!");
            return false;
        }

        if (!Directory.Exists(ZipedFolder))
        {
            Directory.CreateDirectory(ZipedFolder);
        }

        ICSharpCode.SharpZipLib.Zip.ZipInputStream s = null;
        ICSharpCode.SharpZipLib.Zip.ZipEntry theEntry = null;

        string fileName;
        try
        {

            Encoding gbk = Encoding.GetEncoding("gbk");      // 防止中文名乱码
                                                             //Debug.Log(gbk);
            ICSharpCode.SharpZipLib.Zip.ZipConstants.DefaultCodePage = Encoding.GetEncoding("UTF-8").CodePage;
            using (FileStream fsteam = File.OpenRead(FileToUpZip))
            {
                s = new ICSharpCode.SharpZipLib.Zip.ZipInputStream(File.OpenRead(FileToUpZip));
                while ((theEntry = s.GetNextEntry()) != null)
                {

                    if (theEntry.Name != "")
                    {
                        fileName = Path.Combine(ZipedFolder, theEntry.Name);
                        ///判断文件路径是否是文件夹

                        if (fileName.EndsWith("/") || fileName.EndsWith("\\"))
                        {
                            Directory.CreateDirectory(fileName);
                            continue;
                        }


                        using (FileStream streamWriter = File.Create(fileName))
                        {
                            int size = 4096;
                            byte[] data = new byte[4096];
                            while (true)
                            {
                                size = s.Read(data, 0, data.Length);

                                if (size > 0)
                                    streamWriter.Write(data, 0, size);
                                else
                                    break;
                            }
                            streamWriter.Close();
                        }
                    }
                }
                fsteam.Close();
            }
            if (decompressSuccessCallBcak != null)
            {
                // decompressSuccessCallBcak(appEXEFilePath);
                decompressSuccessCallBcak();
                decompressSuccessCallBcak = null;
            }
            return true;
        }
        catch (Exception ex)
        {
            Debug.Log("UnZip Exception : " + ex.Message);
            return false;
        }
        finally
        {
            if (theEntry != null)
            {
                theEntry = null;
            }
            if (s != null)
            {
                s.Close();
                s = null;
            }
        }
    }
    public List<ZipFile> AllZipfile = new List<ZipFile>();
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    private async Task<(bool, string)> UnZip(string zipPath, string outPath, Action<float> progress = null, string password = "")
    {
        ZipFile zf = null;
        if (!File.Exists(zipPath))
        {
            return (false, "没有此文件路径：" + zipPath);
        }
        if (!Directory.Exists(outPath))
        {
            Directory.CreateDirectory(outPath);
        }
        long allCount = 0;
        int Currentindex = 0;
        Encoding gbk = Encoding.GetEncoding("gbk");      // 防止中文名乱码
        Debug.LogError(" gbk :" + gbk);
        ICSharpCode.SharpZipLib.Zip.ZipConstants.DefaultCodePage = gbk.CodePage;
        try
        {
            FileStream fs = File.OpenRead(zipPath);
            zf = new ZipFile(fs);
            if (!AllZipfile.Contains(zf))
            {
                AllZipfile.Add(zf);
            }
            if (!String.IsNullOrEmpty(password))
            {
                zf.Password = password;  // AES encrypted entries are handled automatically
            }
            allCount = zf.Count;
            foreach (ZipEntry zipEntry in zf)
            {
                Currentindex += 1;
                if (progress != null)
                {
                    progress(Currentindex / (float)allCount);
                }
                if (!zipEntry.IsFile)
                {
                    continue;// Ignore directories
                }
                String entryFileName = zipEntry.Name;
                // to remove the folder from the entry:- entryFileName = Path.GetFileName(entryFileName);
                // Optionally match entrynames against a selection list here to skip as desired.
                // The unpacked length is available in the zipEntry.Size property.

                byte[] buffer = new byte[4096];     // 4K is optimum
                Stream zipStream = zf.GetInputStream(zipEntry);

                // Manipulate the output filename here as desired.
                String fullZipToPath = Path.Combine(outPath, entryFileName);
                string directoryName = Path.GetDirectoryName(fullZipToPath);
                if (directoryName.Length > 0)
                    Directory.CreateDirectory(directoryName);

                // Unzip file in buffered chunks. This is just as fast as unpacking to a buffer the full size
                // of the file, but does not waste memory.
                // The "using" will close the stream even if an exception occurs.
                using (FileStream streamWriter = File.Create(fullZipToPath))
                {
                    StreamUtils.Copy(zipStream, streamWriter, buffer);
                }
                await Task.Yield();
            }
        }
        catch (System.Exception e)
        {
            return (false, "Un zip error :" + e.Message);
        }
        finally
        {
            if (zf != null)
            {
                zf.IsStreamOwner = true; // Makes close also shut the underlying stream
                zf.Close(); // Ensure we release resources
            }
        }
        if (File.Exists(zipPath))
        {
            File.Delete(zipPath);
        }
        return (true, "解压完成 ");

    }
    public async Task<(bool, string)> UnZip(string localPath, string UnZipPath, Action<float> unZipProgress = null)
    {
        int allCount = 4;
        Task<(bool, string)> _UnZip = null;
        (bool, string) tempUnZip = (false, "");
        var token = DownloadFile.Instance.tokenSource.Token;
        for (int i = 1; i < allCount; i++)
        {
            await Task.Run(() =>
            {
                Loom.Ins.QueueOnMainThread(() =>
                {
                    _UnZip = UnZip(localPath, UnZipPath, unZipProgress, "");
                });
            }, token);
            await Task.Delay(2);
            tempUnZip = await _UnZip;
            if (tempUnZip.Item1)
            {
                Debug.LogError("UnZip 完成");
                break;
            }
        }
        return tempUnZip;
    }
    private void Start()
    {
        // GameManager.Instance.UnZipDispos = Dispos;
    }
    private void Dispos()
    {
        AllZipfile.ForEach(_zf =>
        {
            if (_zf != null)
            {
                _zf.IsStreamOwner = true; // Makes close also shut the underlying stream
                _zf?.Close(); // Ensure we release resources
            }
        });
        AllZipfile.Clear();

    }
}












