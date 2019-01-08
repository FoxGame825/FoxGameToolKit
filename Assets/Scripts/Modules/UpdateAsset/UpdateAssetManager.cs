using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FoxGame;
using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System.IO;
using UnityEngine.Events;

public class UpdateAssetManager:MonoSingleton<UpdateAssetManager> 
{
    private AssetBundleManifest curManifest;
    private AssetBundleManifest onlineManifest;

    public void CheckAsset(UnityAction onComplete =null) {
        MsgDispatcher.GetInstance().Fire(GameEvents.UpdateAsset.ShowUpdateAssetInfo, "检测资源...");
        StartCoroutine(progress(onComplete));
    }


    IEnumerator progress(UnityAction onComplete) {
        //第一次进入游戏 把streamingassets文件夹数据解压缩到指定的下载目录
        if(true || PlayerPrefs.GetString("IsFirstLaunch","true") == "true") {
            yield return StartCoroutine(streamingAssetfolderCopyToDownloadFolder());
        }

        // 加载本地 manifest文件
        if (File.Exists(GameConfigs.LocalManifestPath)) {
            var manifestAB = AssetBundle.LoadFromFile(GameConfigs.LocalManifestPath);
            curManifest = manifestAB.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
            manifestAB.Unload(false);
        } else {
            Debug.Log("本地资源文件丢失:" + GameConfigs.LocalManifestPath);
        }

        //获取资源服务器端manifest
        Debug.Log("获取资源服务器资源manifest :"+ GameConfigs.OnlineManifestPath);
        MsgDispatcher.GetInstance().Fire(GameEvents.UpdateAsset.ShowUpdateAssetInfo, "检测是否更新资源...");

        UnityWebRequest webReq = UnityWebRequest.Get(GameConfigs.OnlineManifestPath);
        yield return webReq.SendWebRequest();

        if (webReq.isNetworkError || webReq.isHttpError) {
            Debug.Log(webReq.error);
        } else {
            if(webReq.responseCode == 200) {
                byte[] result = webReq.downloadHandler.data;
                AssetBundle onlineManifestAB = AssetBundle.LoadFromMemory(result);
                onlineManifest = onlineManifestAB.LoadAsset<AssetBundleManifest>("AssetBundlemanifest");
                onlineManifestAB.Unload(false);
                //更新本地manifest
                writeFile(GameConfigs.LocalManifestPath, webReq.downloadHandler.data);
            }
            yield return StartCoroutine(download());


            if (onComplete != null) {
                onComplete();
            }
        }

    }
   
    // streamingAsset文件夹数据解压缩到下载文件夹
    IEnumerator streamingAssetfolderCopyToDownloadFolder() {
        Debug.Log("初次运行,解压缩包数据到本地下载文件夹!");
        MsgDispatcher.GetInstance().Fire(GameEvents.UpdateAsset.ShowUpdateAssetInfo, "解压缩包数据...");

        string srcmanifestpath = GameConfigs.StreamingAssetManifestPath;

        // way 1
        if (Directory.Exists(GameConfigs.GameResExportPath)) {
            Debug.Log("存在:" + GameConfigs.GameResExportPath);

            //获取该文件夹下所有文件(包含子文件夹)
            var list = PathUtils.GetFilesPath(GameConfigs.GameResExportPath);
            int total = list.Length;
            int count = 0;
            foreach (var iter in list) {
                string srcPath = iter;
                string tarPath = iter.Replace(GameConfigs.GameResExportPath, GameConfigs.LocalABRootPath);

                UnityWebRequest req = UnityWebRequest.Get(srcmanifestpath);
                yield return req.SendWebRequest();

                if (req.isNetworkError || req.isHttpError) {
                    Debug.Log(req.error);
                } else {
                    if (File.Exists(tarPath)) {
                        File.Delete(tarPath);
                    } else {
                        PathUtils.CreateFolderByFilePath(tarPath);
                    }
                    FileStream fs2 = File.Create(tarPath);
                    fs2.Write(req.downloadHandler.data, 0, req.downloadHandler.data.Length);
                    fs2.Flush();
                    fs2.Close();
                    Debug.LogFormat("->解压缩文件{0}到{1}成功", srcPath, tarPath);
                }
                count++;
                MsgDispatcher.GetInstance().Fire(GameEvents.UpdateAsset.ShowUpdateAssetInfo, string.Format("解压缩包数据...({0}/{1})",count,total));

            }

        } else {
            Debug.Log("无需解压缩!");
        }
        


        //// way 2
        //if (File.Exists(srcmanifestpath)) {
        //    Debug.Log("存在:" + srcmanifestpath);

        //    UnityWebRequest req = UnityWebRequest.Get(srcmanifestpath);
        //    yield return req.SendWebRequest();

        //    if (req.isNetworkError) {
        //        Debug.Log(req.error);
        //    } else {
        //        string tarmanifestpath = GameConfigs.LocalManifestPath;

        //        // copy manifest file
        //        if (File.Exists(tarmanifestpath)) {
        //            File.Delete(tarmanifestpath);
        //        } else {
        //            PathUtils.CreateFolderByFilePath(tarmanifestpath);
        //        }
        //        FileStream fs2 = File.Create(tarmanifestpath);
        //        fs2.Write(req.downloadHandler.data, 0, req.downloadHandler.data.Length);
        //        fs2.Flush();
        //        fs2.Close();
        //        Debug.LogFormat("解压缩文件{0}到{1}成功", srcmanifestpath, tarmanifestpath);



        //        var manifestAB = AssetBundle.LoadFromMemory(req.downloadHandler.data);
        //        var manifest = manifestAB.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
        //        manifestAB.Unload(false);



        //        var allABList = manifest.GetAllAssetBundles();


        //        foreach (var iter in allABList) {
        //            string oriPath = GameConfigs.GameResExportPath + "/" + iter;
        //            string tarPath = GameConfigs.DownLoadAssetPath + "/" + iter;

        //            req = UnityWebRequest.Get(oriPath);
        //            yield return req.SendWebRequest();

        //            if (req.isNetworkError) {
        //                Debug.Log("加载文件失败:" + oriPath);
        //            } else {
        //                if (File.Exists(tarPath)) {
        //                    File.Delete(tarPath);
        //                } else {
        //                    PathUtils.CreateFolderByFilePath(tarPath);
        //                }

        //                Debug.LogFormat("解压缩文件{0}到{1}成功", oriPath, tarPath);


        //                FileStream fs = File.Open(tarPath, FileMode.OpenOrCreate);
        //                fs.Write(req.downloadHandler.data, 0, req.downloadHandler.data.Length);
        //                fs.Flush();
        //                fs.Close();
        //            }
        //        }

        //        Debug.Log("解压缩完成!");
        //    }

        //} else {
        //    Debug.Log("不存在:" + GameConfigs.StreamingAssetManifestPath);
        //}
    }


    IEnumerator download() {

        var downloadFileList = getDownloadFileName();
        int totalCount = downloadFileList.Count;
        int count = 0;
        if (totalCount <= 0) {
            Debug.Log("没有需要更新的资源");
        } else {
            foreach (var iter in downloadFileList) {
                string path = GameConfigs.ResServerUrl + "/" + GameConfigs.CurPlatformName + "/" + iter;

                UnityWebRequest req = UnityWebRequest.Get(path);
                yield return req.SendWebRequest();

                if (req.isNetworkError) {
                    Debug.Log(req.error);
                    yield return null;
                } else {
                    if (req.responseCode == 200) {
                        byte[] result = req.downloadHandler.data;

                        //save file
                        string downloadPath = GameConfigs.LocalABRootPath + "/" + iter;
                        writeFile(downloadPath, result);
                        Debug.LogFormat("写入:{0} 成功 -> {1} | len =[{2}]", path, downloadPath, result.Length);

                        AssetBundle onlineManifestAB = AssetBundle.LoadFromMemory(result);
                        onlineManifest = onlineManifestAB.LoadAsset<AssetBundleManifest>("AssetBundlemanifest");
                        onlineManifestAB.Unload(false);
                    }
                }
                count++;
                MsgDispatcher.GetInstance().Fire(GameEvents.UpdateAsset.ShowUpdateAssetInfo, string.Format("下载资源...({0}/{1})", count, totalCount));
                yield return new WaitForEndOfFrame();
            }
        }
    }


    //获取需要下载的文件列表
    private List<string> getDownloadFileName() {
        if(curManifest == null) {
            if(onlineManifest == null) {
                return new List<string>();
            } else {
                return new List<string>(onlineManifest.GetAllAssetBundles());
            }
        }

        List<string> tempList = new List<string>();
        var curHashCode = curManifest.GetHashCode();
        var onlineHashCode = onlineManifest.GetHashCode();

        if (curHashCode != onlineHashCode) {
            // 比对筛选
            var curABList = curManifest.GetAllAssetBundles();
            var onlineABList = onlineManifest.GetAllAssetBundles();
            Dictionary<string, Hash128> curABHashDic = new Dictionary<string, Hash128>();
            foreach(var iter in curABList) {
                curABHashDic.Add(iter, curManifest.GetAssetBundleHash(iter));
            }

            foreach(var iter in onlineABList) {
                if (curABHashDic.ContainsKey(iter)) { //本地有该文件 但与服务器不同
                    Hash128 onlineHash = onlineManifest.GetAssetBundleHash(iter);
                    if(onlineHash != curABHashDic[iter]) {
                        tempList.Add(iter);
                    }
                } else {
                    tempList.Add(iter);
                }
            }
        }

        return tempList;
    }

    private void writeFile(string path,byte[] data) {
        FileInfo fi = new FileInfo(path);
        DirectoryInfo dir = fi.Directory;
        if (!dir.Exists) {
            dir.Create();
        }

        FileStream fs = fi.Create();
        fs.Write(data, 0, data.Length);
        fs.Flush();
        fs.Close();
    }

}

