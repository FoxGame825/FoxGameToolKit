using System;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System.IO;
using System.Text;
using tabtoy;
using UnityEngine.Events;
using FoxGame;
using UnityEditor;
using FoxGame.Asset;
using cfg;



//配置文件管理器
public class ConfigManager : MonoSingleton<ConfigManager>
{
    public DataConfig Cfg;

    public void ParseConfigData(byte[] bytes)
    {
        MemoryStream stream = new MemoryStream(bytes);
        stream.Position = 0;
        DataReader reader = new DataReader(stream);
        if (!reader.ReadHeader()) {
            Debug.LogError("配置文件被损坏");
            return;
        }

        this.Cfg = new DataConfig();
        DataConfig.Deserialize(this.Cfg, reader);

        Debug.Log("[配置文件数据解析完成]");
    }


    #region 查询扩展
   
    #endregion

}
