using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    private static UIManager instance = null;
    public static UIManager Instance
    {
        get
        {
            return instance;
        }
    }

    UIRoot uiRoot = null;
    GameObject rootGo;
    Dictionary<UIConst.UILayer, UILayer> layerMap = new Dictionary<UIConst.UILayer, UILayer>();
    Dictionary<string, UIBasePanel> panelMap = new Dictionary<string, UIBasePanel>();
    Camera uiCamera;

    public static void Init()
    {
        GameObject uiManagerGameObject = new GameObject("UIManager");
        DontDestroyOnLoad(uiManagerGameObject);
        instance = uiManagerGameObject.AddComponent<UIManager>();
    }

    void InitUIRoot()
    {
        rootGo = gameObject;
        var panel = rootGo.AddComponent<UIPanel>();
        panel.depth = -1;
        uiCamera = GameObject.Find("UI Root/UICamera").GetComponent<Camera>();
        UICamera.currentCamera = uiCamera;
    }


    void InitLayers()
    {
        foreach (UIConst.UILayer enumLayer in Enum.GetValues(typeof(UIConst.UILayer)))
        {
            if (enumLayer == UIConst.UILayer.root)
                continue;
            UILayer layer = NGUITools.AddChild<UILayer>(rootGo);
            layer.layer = enumLayer;
            layerMap[enumLayer] = layer;
        }
    }
}
