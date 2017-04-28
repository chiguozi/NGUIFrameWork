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
    Dictionary<UIConst.UILayer, UILayer> layerMap = new Dictionary<UIConst.UILayer, UILayer>();
    Dictionary<string, UIBasePanel> panelMap = new Dictionary<string, UIBasePanel>();


    public static void Init()
    {
        GameObject uiManagerGameObject = new GameObject("UIManager");
        DontDestroyOnLoad(uiManagerGameObject);
        instance = uiManagerGameObject.AddComponent<UIManager>();
    }
}
