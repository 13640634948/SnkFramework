using SampleDevelop.Mvvm.Implments.UGUI;
using SampleDevelop.Test;
using SnkFramework.Mvvm.Base;
using SnkFramework.Mvvm.View;
using UnityEngine;

using UnityEngine.UI;

public class UGUILayer : SnkUIlayerBase, IUGUILayer
{
    public UGUILayer(IUILocator locator) : base(locator)
    {
    }
    
    private Canvas _canvas;

    public Canvas mCanvas => this._canvas;

    private CanvasScaler _canvasScaler;
    public CanvasScaler mCanvasScaler => _canvasScaler;


    private Transform _transform;
    public Transform transform => _transform ??= this._canvas ? this._canvas.transform : null;

    private GameObject _gameObject;
    public GameObject gameObject => _gameObject ??= this._canvas ? this._canvas.gameObject : null;

    public void SetOwner(Canvas canvas)
    {
        this._canvas = canvas;
        this._canvasScaler = this._canvas.GetComponent<CanvasScaler>() ?? this._canvas.gameObject.AddComponent<CanvasScaler>();
    }

    /*
    public override int GetSortingLayerID()
        => this.mCanvas.sortingLayerID;
        */

    /*
    protected override void onAdd(ISnkWindow window)
    {
        base.onAdd(window);
        this.AddChild(((IUGUIWindow)window).mUGUIOwner.transform);
    }

    protected override void onRemove(ISnkWindow window)
    {
        base.onRemove(window);
        this.RemoveChild(GetTransform(window));
    }

    protected override void onRemoveAt(ISnkWindow window, int index)
    {
        base.onRemoveAt(window, index);
        this.RemoveChild(GetTransform(window));
    }
    */
    
    protected Transform GetTransform(ISnkWindow window)
    {
        IUGUIWindow uWindow = (IUGUIWindow) window;
        return uWindow.mUGUIOwner.transform;
    }

    public void AddChild(Transform child, bool worldPositionStays = false)
    {
        if (child == null || this.transform.Equals(child.parent))
            return;

        child.gameObject.layer = this.gameObject.layer;
        child.SetParent(this.transform, worldPositionStays);
        child.SetAsFirstSibling();
    }
    public virtual void RemoveChild(Transform child, bool worldPositionStays = false)
    {
        if (child == null || !this.transform.Equals(child.parent))
            return;

        child.SetParent(null, worldPositionStays);
    }

    protected override void PrintWindowPriority()
    {
        //throw new NotImplementedException();
        SortSibling();
    }

    protected void SortSibling()
    {
        windowList.Sort((left, right)=> left.mPriority.CompareTo(right));
        Debug.Log("SortSibling:" + windowList.Count);
        for (int i = 0; i < windowList.Count; i++)
        {
            Debug.Log(GetTransform(windowList[i]).name + ":" + windowList[i].mPriority + ", " + i);
            if(windowList[i].mPriority == i)
                continue;
            transform.SetSiblingIndex(i);
        }
    }

}