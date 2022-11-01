using UnityEngine;


public interface ISnkViewCamera
{
    public Camera ViewCamera { get; }
}


[RequireComponent(typeof(Camera))]
public class SnkViewCamera : MonoBehaviour, ISnkViewCamera
{
    private Camera _viewCamera;
    public Camera ViewCamera => _viewCamera ??= this.GetComponent<Camera>();
    
}