using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 视图相机
/// </summary>
public class CameraView : ModuleCamera {
    public Camera viewCamera;
    public Transform viewSpace;
    private RenderTexture renderTexture;

    public override Vector3 Position {
        get => viewCamera.transform.localPosition;
        set => viewCamera.transform.localPosition = value;
    }
    public override Vector3 EulerAngles {
        get => viewCamera.transform.localEulerAngles;
        set => viewCamera.transform.localEulerAngles = value;
    }
    public override float VisualField {
        get => viewCamera.orthographicSize;
        set => viewCamera.orthographicSize = value;
    }

    public override RenderTexture RenderTexture => renderTexture;

    protected override void Awake() => ModuleCore.CameraView = this;

    public override void UpdateRenderTexture(int x, int y) {
        renderTexture = new RenderTexture(x, y, 16, RenderTextureFormat.ARGB32);
        viewCamera.targetTexture = renderTexture;
    }
    public override Vector3 ScreenToViewPosition(Vector3 screenPosition) {
        return viewCamera.ScreenToViewportPoint(screenPosition);
    }
    public override Vector3 ScreenToWorldPosition(Vector3 screenPosition) {
        Vector3 position = viewCamera.ScreenToWorldPoint(screenPosition);
        position.z = 0;
        return position;
    }

    public override bool ScreenToWorldObject<T>(Vector3 screenPosition, out T value) {
        throw new System.NotImplementedException();
    }
    public override bool ScreenToWorldObject<T>(Vector3 screenPosition, out T value, LayerMask planeLayerMask) {
        throw new System.NotImplementedException();
    }
    public override bool ScreenToWorldObjectParent<T>(Vector3 screenPosition, out T value) {
        throw new System.NotImplementedException();
    }
    public override bool ScreenToWorldObjectParent<T>(Vector3 screenPosition, out T value, LayerMask planeLayerMask) {
        throw new System.NotImplementedException();
    }
}
