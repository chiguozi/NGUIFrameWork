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

    UIRoot _uiRoot = null;
    public UIRoot uiRoot { get { return _uiRoot; } }
    GameObject rootGo;
    public GameObject UIRootGo { get { return rootGo; } }
    Dictionary<UIConst.UILayer, UILayer> layerMap = new Dictionary<UIConst.UILayer, UILayer>();
    Dictionary<string, UIBasePanel> panelMap = new Dictionary<string, UIBasePanel>();
    Camera uiCamera;

    public static void Init()
    {
        GameObject uiManagerGameObject = new GameObject("UIManager");
        DontDestroyOnLoad(uiManagerGameObject);
        instance = uiManagerGameObject.AddComponent<UIManager>();
        instance.InitUIRoot();
    }

    void InitUIRoot()
    {
        _uiRoot = UIRoot.list[0];
        rootGo = _uiRoot.gameObject;
        var panel = rootGo.AddComponent<UIPanel>();
        panel.depth = -1;
        uiCamera = GameObject.Find("UI Root/UICamera").GetComponent<Camera>();
        UICamera.currentCamera = uiCamera;
        InitLayers();
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

    public UILayer GetLayer(UIConst.UILayer enumlayer)
    {
        UILayer layer = layerMap[enumlayer];
        return layer;
    }


    public  T GetPanel<T>() where T : UIBasePanel
    {
        UIBasePanel panel = null;
        panelMap.TryGetValue(typeof(T).Name, out panel);
        return panel as T;
    }

    public UIBasePanel GetPanel(string name)
    {
        UIBasePanel panel = null;
        panelMap.TryGetValue(name, out panel);
        return panel;
    }


    public UIBasePanel ShowPanel(string name)
    {
        UIBasePanel panel = null;
        panelMap.TryGetValue(name, out panel);
        if (panel == null)
        {
            panel = rootGo.AddComponent(Type.GetType(name)) as UIBasePanel;
            panelMap.Add(name, panel);
        }
        //bg？
        panel.SetPanelToLayerTop();
        panel.isActive = true;
        return panel;
    }

    public T ShowPanel<T>() where T : UIBasePanel
    {
        Type type = typeof(T);
        return ShowPanel(type.Name) as T;
    }

    public void HidePanel(string name)
    {
        UIBasePanel panel = null;
        panelMap.TryGetValue(name, out panel);
        if (panel == null)
            return;
        //bg
        panel.isActive = false;
        if (!panel.isPersistence)
        {
            panelMap.Remove(name);
            GameObject.Destroy(panel);
        }
    }

    //可以使用事件代替这种刷新方式，不够灵活，不能部分刷新
    public void UpdatePanel(string name)
    {
        UIBasePanel panel = null;
        panelMap.TryGetValue(name,out panel);
        if (panel == null)
            return;
        panel.UpdateUI();
    }

}
