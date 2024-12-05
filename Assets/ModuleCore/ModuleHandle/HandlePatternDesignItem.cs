using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 图案设计项目 DataPatternDesignItem
/// </summary>
public class HandlePatternDesignItem : ModuleHandle<DataPatternDesignItem> {
    /// <summary> 视图相机模块 </summary>
    public ModuleCamera CameraView => ModuleCore.CameraView;
    /// <summary> 图案设计项目 可视化内容生成模块 </summary>
    public ModuleVisual<DataPatternDesignItem> VisualPatternDesignItem => ModuleCore.VisualPatternDesignItem;

    public override bool IsValid => value != null;

    protected override void Awake() => ModuleCore.HandlePatternDesignItem = this;

    public override void Change() {
        base.Change();
        if (value == null) { return; }
        VisualPatternDesignItem.UpdateVisual(value);
        StartCoroutine(IGenerateTexture(value.patternDesign));
    }
    public override void Change(DataPatternDesignItem value) {
        base.Change(value);
        if (value == null) { return; }
        VisualPatternDesignItem.UpdateVisual(this.value);
        StartCoroutine(IGenerateTexture(this.value.patternDesign));
    }

    private IEnumerator IGenerateTexture(DataPatternDesign patternDesign) {
        yield return new WaitForEndOfFrame();
        Texture2D texture = RenderTextureToTexture2D(CameraView.RenderTexture);
        patternDesign.texture = texture;
        patternDesign.OnUpdateTexture?.Invoke(texture);
    }
    private Texture2D RenderTextureToTexture2D(RenderTexture renderTexture) {
        int width = renderTexture.width;
        int height = renderTexture.height;
        Texture2D texture2D = new Texture2D(width, height, TextureFormat.ARGB32, false);
        RenderTexture.active = renderTexture;
        texture2D.ReadPixels(new Rect(0, 0, width, height), 0, 0);
        texture2D.Apply();
        return GetTexture(texture2D);
    }
    private Texture2D GetTexture(Texture2D texture2D) {
        Color[] colors = texture2D.GetPixels();
        Texture2D target = new Texture2D(texture2D.width, texture2D.height);
        target.SetPixels(colors);
        target.Apply();
        return target;
    }
}
