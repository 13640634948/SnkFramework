using SnkFramework.Mvvm.Base;
using SnkFramework.Mvvm.View;
using UnityEngine;


public class UGUILayer : SnkUILayerBase, IUGUILayer
{
    private Canvas _canvas;

    public Canvas mCanvas => this._canvas;
    //public GameObject mLayerOwner => this._canvas.gameObject;

    protected Transform _transform => this._canvas ? this._canvas.transform : null;
    protected GameObject _gameObject => this._canvas ? this._canvas.gameObject : null;

    public void SetOwner(Canvas canvas)
    {
        this._canvas = canvas;
    }

    public override int GetSortingLayerID()
        => this.mCanvas.sortingLayerID;

    public override void Add(IWindow window)
    {
        base.Add(window);
        this.AddChild(window.mOwner.transform);
    }

    protected void AddChild(Transform child, bool worldPositionStays = false)
    {
        if (child == null || this._transform.Equals(child.parent))
            return;

        child.gameObject.layer = this._gameObject.layer;
        child.SetParent(this._transform, worldPositionStays);
        child.SetAsFirstSibling();
    }
}