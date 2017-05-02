using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UILayer : MonoBehaviour
{
    UIConst.UILayer _layer;
    UIPanel _panel;
    GameObject _gameObject;
    int _maxDepth;
    List<UIBasePanel> _childList = new List<UIBasePanel>();
    GameObject GameObject
    {
        get
        {
            if (_gameObject == null)
                _gameObject = gameObject;
            return gameObject;
        }
    }
    public UIConst.UILayer layer
    {
        get { return _layer; }
        set
        {
            _layer = value;
            if (_panel == null)
                _panel = GameObject.AddComponent<UIPanel>();
            _panel.depth = ( (int)_layer - 1 ) * UIConst.LayerDepth;
            _maxDepth = (int)_layer * UIConst.LayerDepth;
            //需要宏吗？
#if UNITY_EDITOR
            name = UIConst.layerNameMap[_layer];
#endif
        }
    }

    public void AddChild(UIBasePanel panel)
    {
        if (_childList.Contains(panel))
            return;
        panel.parentGo = _gameObject;
        _childList.Add(panel);
    }


    public void RemoveChild(UIBasePanel panel)
    {
        if (!_childList.Contains(panel))
            return;
        panel.parentGo = null;
        _childList.Remove(panel);
    }

    public bool IsOnTop(UIBasePanel panel)
    {
        int index = _childList.Count - 1;
        if (index < 0)
            return false;
        return _childList[index] == panel;
    }

    public void SortDepth()
    {
        int depth = _panel.depth;
        int childLength = _childList.Count;
        List<UIPanel> tmpList = new List<UIPanel>();
        UIBasePanel panel;
        for (int i = 0; i < childLength; i++)
        {
            panel = _childList[i];
            if (!panel.isLoadComplete || !panel.isActive)
                continue;
            if (panel != null)
                panel.uiPanel.depth = ++depth;
            //@todo 使用pool对tmpList回收
            panel.uiGameObject.GetComponentsInChildren(true, tmpList);
            tmpList.Sort(UIPanel.CompareFunc);
            for (int j = 0; j < tmpList.Count; j++)
            {
                var child = tmpList[j];
                if (child == panel.uiPanel)
                    continue;
                child.depth = ++depth;
            }
        }

#if UNITY_EDITOR
        if (depth >= _maxDepth)
            Debug.LogError("layer's depth is limited, cur depth=" + depth + " ,max depth=" + _maxDepth);
#endif
    }



}
