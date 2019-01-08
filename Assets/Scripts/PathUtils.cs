using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;



public class PathUtils {

    /// <summary>
    /// 根据一个绝对路径 获得这个资源的assetbundle name
    /// </summary>
    /// <param name="path"></param>
    /// <param name="root">资源文件夹的根目录</param>
    /// <returns></returns>
    public static string GetAssetBundleNameWithPath(string path,string root)
    {
        string str = NormalizePath(path );
        str = ReplaceFirst(str, root + "/", "");
        return str;
    }

    /// <summary>
    /// 获取文件夹的所有文件，包括子文件夹 不包含.meta文件
    /// </summary>
    /// <returns>The files.</returns>
    /// <param name="path">Path.</param>
    public static FileInfo[] GetFiles(string path)
    {
        DirectoryInfo folder = new DirectoryInfo(path);

        DirectoryInfo[] subFolders = folder.GetDirectories();
        List<FileInfo> filesList = new List<FileInfo>();

        foreach (DirectoryInfo subFolder in subFolders)
        {
            filesList.AddRange(GetFiles(subFolder.FullName));
        }

        FileInfo[] files = folder.GetFiles();
        foreach (FileInfo file in files)
        {
            if (file.Extension != ".meta")
            {
                filesList.Add(file);
            }

        }
        return filesList.ToArray();
    }
    /// <summary>
    /// 获取文件夹的所有文件路径，包括子文件夹 不包含.meta文件
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public static string[] GetFilesPath(string path)
    {
        DirectoryInfo folder = new DirectoryInfo(path);
        DirectoryInfo[] subFolders = folder.GetDirectories();
        List<string> filesList = new List<string>();

        foreach (DirectoryInfo subFolder in subFolders)
        {
            filesList.AddRange(GetFilesPath(subFolder.FullName));
        }

        FileInfo[] files = folder.GetFiles();
        foreach (FileInfo file in files)
        {
            if (file.Extension != ".meta")
            {
                filesList.Add(NormalizePath(file.FullName));
            }

        }
        return filesList.ToArray();
    }

    /// <summary>
    /// 创建文件目录前的文件夹，保证创建文件的时候不会出现文件夹不存在的情况
    /// </summary>
    /// <param name="path"></param>
    public static void CreateFolderByFilePath(string path)
    {
        FileInfo fi = new FileInfo(path);
        DirectoryInfo dir = fi.Directory;
        if (!dir.Exists)
        {
            dir.Create();
        }
    }

    /// <summary>
    /// 创建文件夹
    /// </summary>
    /// <param name="path"></param>
    public static void CreateFolder(string path)
    {
        DirectoryInfo dir = new DirectoryInfo(path);
        if (!dir.Exists)
        {
            dir.Create();
        }
    }

    /// <summary>
    /// 规范化路径名称 修正路径中的正反斜杠
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public static string NormalizePath(string path)
    {
        return path.Replace(@"\", "/");
    }

    /// <summary>
    /// //将绝对路径转成工作空间内的相对路径
    /// </summary>
    /// <param name="fullPath"></param>
    /// <returns></returns>
    public static string GetRelativePath(string fullPath,string root)
    {
        string path = NormalizePath(fullPath);
        //path = path.Replace(Application.dataPath,"Assets");
        path = ReplaceFirst(path, root, "Assets");
        return path;
    }
    /// <summary>
    /// 将相对路径转成绝对路径
    /// </summary>
    /// <param name="relativePath"></param>
    /// <returns></returns>
    public static string GetAbsolutelyPath(string relativePath,string root)
    {
        string path = NormalizePath(relativePath);
        //path = Application.dataPath.Replace("Assets","") + path;
        path = ReplaceFirst(root, "Assets", "") + path;
        return path;
    }

    /// <summary>
    /// 替换掉第一个遇到的指定字符串
    /// </summary>
    /// <param name="str"></param>
    /// <param name="oldValue"></param>
    /// <param name="newValue"></param>
    /// <returns></returns>
    public static string ReplaceFirst(string str, string oldValue, string newValue)
    {
        int i = str.IndexOf(oldValue);
        str = str.Remove(i, oldValue.Length);
        str = str.Insert(i, newValue);
        return str;
    }
}
