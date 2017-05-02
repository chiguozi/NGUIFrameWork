using UnityEngine;

public class UIBaseView : MonoBehaviour
{
    protected enum LoadingState
    {
        None = 0,
        Loading = 1,
        Finish = 2,
        Failed = 3,
    }

    protected string uiName = "";
    protected string uiPath = "prefab/";
    public string UIName { get { return uiName; } }

    protected bool _isActive = false;
    public virtual bool isActive
    {
        get { return _isActive; }
        set
        {
            //如果UI需要强制刷新active  可以将这个判断去掉
            if (_isActive == value)
            {
                DoShowOrHide();
                return;
            }
                
            _isActive = value;
            if (uiGameObject == null)
            {
                loadingState = LoadingState.Loading;
                ResourceManager.LoadAsset(uiPath + uiName, LoadComplete);
                return;
            }

            if (uiGameObject != null)
                uiGameObject.SetActive(_isActive);
            DoShowOrHide();
        }
    }

    protected void DoShowOrHide()
    {
        if (isActive)
        {
            OnBeforeShow();
            OnShow();
        }
        else
        {
            OnBeforeHide();
            OnHide();
        }
    }

    protected virtual void OnBeforeShow()
    { }

    protected virtual void OnBeforeHide()
    { }

    protected LoadingState loadingState = LoadingState.None;
    public bool isLoadComplete { get { return loadingState == LoadingState.Finish; } }

    public GameObject uiGameObject;
    public Transform uiTransform;
    public Bounds uiBounds = new Bounds(Vector3.zero, Vector3.zero);
    protected Vector3 localPos = Vector3.zero;

    GameObject _parentGo;
    public GameObject parentGo
    {
        get
        {
            return _parentGo;
        }
        set { _parentGo = value; }
    }

    protected virtual void Awake()
    {
        uiName = GetType().Name; 
    }

    //参数和接口由UI各自提供，基类不提供通用参数处理
    protected virtual void OnShow()
    {
   
    }


    protected virtual void LoadComplete(object obj)
    {
        if (obj == null)
        {
            Debug.LogError("加载" + uiName + "失败");
            return;
        }
        InitUIGameObject(obj);
        InitView();
        DoShowOrHide();
    }

    protected virtual void Refresh()
    { }

    protected virtual void InitView()
    { }

    protected virtual void OnHide()
    { }

    public void UpdateUI()
    {
        if (isLoadComplete)
            Refresh();
    }

    protected virtual void InitUIGameObject(object obj)
    {
        var go = obj as GameObject;
        uiGameObject = NGUITools.AddChild(parentGo, go);
        uiTransform = uiGameObject.transform;
        uiGameObject.SetActive(_isActive);
        uiBounds = NGUIMath.CalculateRelativeWidgetBounds(uiTransform, true);
        uiTransform.localPosition = localPos;
        loadingState = LoadingState.Finish;
    }


    protected virtual void OnDestroy()
    {
        if (uiGameObject != null)
            GameObject.Destroy(uiGameObject);
    }

    public virtual void SetPosition(float x, float y)
    {
        localPos.x = x;
        localPos.y = y;
        if (isLoadComplete)
            uiTransform.localPosition = localPos;
    }

    #region 工具函数

    protected T GetComponent<T>(string path) where T : MonoBehaviour
    {
        var tChild = uiTransform.FindChild(path);
        var com = tChild.GetComponent<T>();
        return (T)com;
    }

    protected GameObject GetGameObject(string path)
    {
        Transform t = uiTransform.FindChild(path);
        return ( null == t ) ? null : t.gameObject;
    }
    #endregion

}

