using UnityEngine;
using System.Collections.Generic;
using System;

public class UIBasePanel : UIBaseView
{
    UIAnchor uiAnchor = null;
    UIAnchor.Side uiAnchorSide = UIAnchor.Side.TopLeft;
    public Vector2 uiAnchorOffset = Vector2.zero;
    protected UIConst.UILayer uiLayer = UIConst.UILayer.popup;
    UIPanel _uiPanel;
    public UIPanel uiPanel { get { return _uiPanel; } }
    protected bool _isPersistence = true;
    //关闭界面时是否需要销毁
    public bool isPersistence { get { return _isPersistence; } }

    protected List<UIBaseView> childList = new List<UIBaseView>();

    protected float uiWidth = 0f;
    protected float uiHeight = 0f;

    protected override void LoadComplete(object obj)
    {
        InitUIGameObject(obj);
        _uiPanel = uiGameObject.AddMissingComponent<UIPanel>();

        BoxCollider panelCollider = uiGameObject.AddMissingComponent<BoxCollider>();
        panelCollider.center = uiBounds.center;
        panelCollider.size = uiBounds.size;


        uiAnchor = uiGameObject.AddComponent<UIAnchor>();
        uiAnchor.container = parentGo;
        uiAnchor.enabled = false;

        InitView();
        SortDepth();
        DoShowOrHide();
    }
    //@todo
    // 1. tween
    // 2. panelType

    protected UIAnchor.Side anchorSide
    {
        set
        {
            uiAnchorSide = value;
            Relocate();
        }
        get { return uiAnchor.side; }
    }

    public virtual void Relocate()
    {
        if (uiAnchor == null || uiGameObject == null)
            return;

        uiAnchor.enabled = true;
        uiAnchor.side = uiAnchorSide;
        uiWidth = uiBounds.size.x * uiGameObject.transform.localScale.x;
        uiHeight = uiBounds.size.y * uiGameObject.transform.localScale.y;
        switch (uiAnchorSide)
        {
            case UIAnchor.Side.TopLeft:
                uiAnchor.pixelOffset.x = 0;
                uiAnchor.pixelOffset.y = 0;
                break;
            case UIAnchor.Side.Top:
                uiAnchor.pixelOffset.x = -uiWidth / 2;
                uiAnchor.pixelOffset.y = 0;
                break;
            case UIAnchor.Side.TopRight:
                uiAnchor.pixelOffset.x = -uiWidth;
                uiAnchor.pixelOffset.y = 0;
                break;
            case UIAnchor.Side.Left:
                uiAnchor.pixelOffset.x = 0;
                uiAnchor.pixelOffset.y = uiHeight / 2;
                break;
            case UIAnchor.Side.Center:
                uiAnchor.pixelOffset.x = -uiWidth / 2;
                uiAnchor.pixelOffset.y = uiHeight / 2;
                break;
            case UIAnchor.Side.Right:
                uiAnchor.pixelOffset.x = -uiWidth;
                uiAnchor.pixelOffset.y = uiHeight / 2;
                break;
            case UIAnchor.Side.BottomLeft:
                uiAnchor.pixelOffset.x = 0;
                uiAnchor.pixelOffset.y = uiHeight;
                break;
            case UIAnchor.Side.Bottom:
                uiAnchor.pixelOffset.x = -uiWidth / 2;
                uiAnchor.pixelOffset.y = uiHeight;
                break;
            case UIAnchor.Side.BottomRight:
                uiAnchor.pixelOffset.x = -uiWidth;
                uiAnchor.pixelOffset.y = uiHeight;
                break;
        }
        uiAnchor.pixelOffset.x += uiAnchorOffset.x;
        uiAnchor.pixelOffset.y += uiAnchorOffset.y;
        //强制刷新一次
        uiAnchor.enabled = false;
        uiAnchor.enabled = true;
        localPos = uiGameObject.transform.localPosition;
    }


    public void SortDepth()
    {
        if (!isLoadComplete)
            return;
        UILayer layer = UIManager.Instance.GetLayer(uiLayer);
        layer.SortDepth();
    }

    protected override void OnBeforeHide()
    {
        base.OnBeforeHide();
    }

    protected override void OnBeforeShow()
    {
        base.OnBeforeShow();
    }

    public int GetChildIndex(UIBaseView com)
    {
        return childList.IndexOf(com);
    }
    public T AddChild<T>(params object[] paramlist) where T : UIBaseView
    {
        UIBaseView com = uiGameObject.AddComponent<T>();
        childList.Add(com);
        com.parentGo = uiGameObject;
        com.isActive = true;
        return com as T;
    }

    public T AddChildAt<T>(int index, params object[] paramlist) where T : UIBaseView
    {
        UIBaseView com = uiGameObject.AddComponent<T>();
        childList.Insert(index, com);
        com.parentGo = uiGameObject;
        com.isActive = true;
        return com as T;
    }

    public UIBaseView AddChildByType(Type type, params object[] paramlist)
    {
        UIBaseView com = uiGameObject.AddComponent(type) as UIBaseView;
        childList.Add(com);
        com.parentGo = uiGameObject;
        com.isActive = true;
        return com;
    }

    public UIBaseView AddChildAtByType(Type type, int index, params object[] paramlist)
    {
        UIBaseView com = uiGameObject.AddComponent(type) as UIBaseView;
        childList.Insert(index, com);
        com.parentGo = uiGameObject;
        com.isActive = true;
        return com;
    }

    public void RemoveChild(UIBaseView com)
    {
        if (!childList.Remove(com))
            return;
        com.isActive = false;
        GameObject.DestroyObject(com);
    }

    //@todo 挪到上层  或者通过事件触发
    public void SetPanelToLayerTop()
    {
        UILayer layer = UIManager.Instance.GetLayer(uiLayer);
        if (layer.IsOnTop(this))
            return;

        layer.RemoveChild(this);
        layer.AddChild(this);
        SortDepth();
    }
}

