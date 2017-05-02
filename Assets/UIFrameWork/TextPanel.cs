using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextPanel : UIBasePanel
{
    // Use this for initialization
    protected override void InitView()
    {
        Debug.LogError("init view");
    }

    protected override void OnHide()
    {
        Debug.LogError("on hide");
    }

    protected override void OnShow()
    {
        Debug.LogError("on show");
    }
}
