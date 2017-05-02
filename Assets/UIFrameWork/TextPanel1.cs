using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextPanel1 : UIBasePanel
{
    // Use this for initialization
    protected override void InitView()
    {
        Debug.LogError("init view1");
    }

    protected override void OnHide()
    {
        Debug.LogError("on hide1");
    }

    protected override void OnShow()
    {
        Debug.LogError("on show1");
    }
}
