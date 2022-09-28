using SnkFramework.Mvvm.Base;
using SnkFramework.Mvvm.View;
using UnityEngine;

using UnityEngine.UI;

public class UGUILayer : SnkUILayerBase, IUGUILayer
{
    private Canvas _canvas;

    public Canvas mCanvas => this._canvas;

    private CanvasScaler _canvasScaler;
    public CanvasScaler mCanvasScaler => _canvasScaler;


    private Transform _transform;
    protected Transform transform => _transform ??= this._canvas ? this._canvas.transform : null;

    private GameObject _gameObject;
    protected GameObject gameObject => _gameObject ??= this._canvas ? this._canvas.gameObject : null;

    public void SetOwner(Canvas canvas)
    {
        this._canvas = canvas;
        this._canvasScaler = this._canvas.GetComponent<CanvasScaler>() ?? this._canvas.gameObject.AddComponent<CanvasScaler>();
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
        if (child == null || this.transform.Equals(child.parent))
            return;

        child.gameObject.layer = this.gameObject.layer;
        child.SetParent(this.transform, worldPositionStays);
        child.SetAsFirstSibling();
    }
}