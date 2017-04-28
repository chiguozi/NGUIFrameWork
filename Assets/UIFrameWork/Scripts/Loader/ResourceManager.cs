using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ResourceManager
{

    //@todo  assetbundle支持和异步加载支持
    public static void LoadAsset(string abName, Action<object> callback, string assetName = "")
    {
        LoadAssetInternal(abName, callback, assetName);
    }



    static void LoadAssetInternal(string path, Action<object> callback, string assetName)
    {
        var obj = Resources.Load(path);
        if (null != callback)
            callback(obj);
    }

}
