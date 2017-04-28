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
    protected string uiPath = "";
    public string UIName { get { return uiName; } }

    protected bool _isActive = false;
    public virtual bool isActive
    {
        get { return _isActive; }
        set
        {
            //如果UI需要强制刷新active  可以将这个判断去掉
            if (_isActive == value)
                return;
            _isActive = value;
            if (uiGameObject != null)
                uiGameObject.SetActive(_isActive);
        }
    }

    protected LoadingState loadingState = LoadingState.None;
    public bool isLoadComplete { get { return loadingState == LoadingState.Loading; } }

    public GameObject uiGameObject;
    public Transform uiTransform;
    public Bounds uiBounds = new Bounds(Vector3.zero, Vector3.zero);
    protected Vector3 localPos = Vector3.zero;

    protected virtual void Awake()
    {
        uiName = this.GetType().Name; 
    }

    //参数和接口由UI各自提供，基类不提供通用参数处理
    public virtual void OnShow()
    {
        if (loadingState == LoadingState.Loading || loadingState == LoadingState.Finish)
            return;
        loadingState = LoadingState.Loading;

        isActive = true;

        ResourceManager.LoadAsset(uiPath + uiName, LoadComplete);
    }


    protected virtual void LoadComplete(object obj)
    {
    }

}

