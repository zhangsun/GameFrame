using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class BuildCommon 
{
  

    /// <summary>
    /// 获取文件后缀
    /// </summary>
    /// <param name="filePath"></param>
    /// <returns></returns>
    public static string getFileSuffix(string filePath)
    {
        int index = filePath.LastIndexOf(".");
        if(-1 ==index)
        {
            Debug.LogError("找不到后缀!!! 文件路径为 : " + filePath);
            return null;
          
        }

        return filePath.Substring(index + 1, filePath.Length - index - 1);
    }

    public static string getFileName(string filePath,bool suffix)
    {
        if(!suffix)
        {
            string path = filePath.Replace("\\", "/");
            int index = path.LastIndexOf("/");

            if (-1 == index)
                throw new Exception("can not find /!!!");

            int index2 = path.LastIndexOf(".");
            if (-1 == index2)
                throw new Exception("can not find . !!!");

            return path.Substring(index + 1, index2 - index - 1);
        }
        else
        {
            string path = filePath.Replace("\\", "/");
            int index = path.LastIndexOf("/");
            if (-1 == index)
                throw new Exception("can not find / !!!");

            return path.Substring(index + 1, path.Length - index - 1);

        }
    }


    public static int getAssetLevel(string filePath)
    {
        string[] depencys = AssetDatabase.GetDependencies(new string[] { filePath });

        List<string> deps = new List<string>();

        foreach (var file in depencys)
        {
            //排除关联脚本
            string suffix = BuildCommon.getFileSuffix(file);
            if (suffix == "dll")
                continue;
            deps.Add(file);
        }

        if (deps.Count == 1)
            return 1;

        int maxLevel = 0;
        foreach (var file in deps)
        {
            if (file == filePath)
                continue;
            int level = getAssetLevel(file);
            maxLevel = maxLevel > level + 1 ? maxLevel : level + 1;
        
        }
        return maxLevel;
    }


    /// <summary>
    /// 检查文件夹路径
    /// </summary>
    /// <param name="path"></param>
    public static void CheckFolder(string path)
    {
        if (!Directory.Exists(path))
            Directory.CreateDirectory(path);
    }

    public static string getPath(string filePath)
    {
        string path = filePath.Replace("\\", "/");
        int index = path.LastIndexOf("/");
        if(-1==index)
        {
            throw new Exception("can not find /!!!!");
        }
        return path.Substring(0, index);
    }

    public static string getFolder(string path)
    {
        path = path.Replace("\\", "/");
        int index = path.LastIndexOf("/");
        if (-1 == index)
            throw new Exception("can not find /!!!");
        return path.Substring(index + 1, path.Length - index - 1);
    }

   

    public static void GetFiles(string path,List<string>list ,bool recursion)
    {
        if(recursion)
        {
            string[] dirs = Directory.GetDirectories(path);
            foreach (string dir in dirs)
            {
                if (getFolder(dir) == ".svn")
                    continue;

                GetFiles(dir, list, recursion);
            }
        }

        string[] files = Directory.GetFiles(path);
        foreach (string file in files)
        {
            string suffix = getFileSuffix(file);
            if (suffix == "meta")
                continue;
            string realFile = file.Replace("\\", "/");
            realFile = realFile.Replace(Application.streamingAssetsPath + "/", "");
            list.Add(realFile);
        }
    }
}
