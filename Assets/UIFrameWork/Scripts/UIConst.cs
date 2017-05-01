using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIConst
{
    //每一层的深度区间
    public static int LayerDepth = 50;
    public enum UILayer
    {
        root = 0,
        name,
        mainui,
        popup,
        top,
        //effect层如果不需要可以删除，特效直接保留在面板上
        effect,
    }

    public static Dictionary<UILayer, string> layerNameMap = new Dictionary<UILayer, string>()
    {
        { UILayer.name , "NameLayer"},
        { UILayer.mainui , "MainUILayer"},
        { UILayer.popup , "PopupLayer"},
        { UILayer.top , "TopLayer"},
        { UILayer.effect , "EffectLayer"},
    };


    public static int UI_WIDTH = 1280;
    public static int UI_HEIGHT = 720;

}
