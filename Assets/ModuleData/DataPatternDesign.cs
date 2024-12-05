using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 图案设计数据
/// </summary>
public class DataPatternDesign {
    /// <summary> 图案设计数据 </summary>
    public DataPatternDesign() => id = Guid.NewGuid().ToString("N");

    /// <summary> 唯一标识符 </summary>
    public string id;
    /// <summary> 纹理 </summary>
    public Texture2D texture;
    /// <summary> 更新纹理 </summary>
    public Action<Texture2D> OnUpdateTexture;
    /// <summary> 图案项目列表 </summary>
    public List<DataPatternDesignItem> items = new List<DataPatternDesignItem>();
}
/// <summary>
/// 图案设计项目
/// </summary>
public class DataPatternDesignItem {
    /// <summary> 图案设计 </summary>
    public readonly DataPatternDesign patternDesign;
    /// <summary> 图案设计项目 </summary>
    public DataPatternDesignItem(DataPatternDesign patternDesign) => this.patternDesign = patternDesign;

    /// <summary> 图案纹理 </summary>
    public Texture2D texture;
    /// <summary> 位置 </summary>
    public Vector2 position = new Vector2(0, 0);
    /// <summary> 规模 </summary>
    public Vector2 scale = new Vector2(0.5f, 0.5f);
    /// <summary> 旋转 </summary>
    public float rotate = 0;
    /// <summary> 颜色 </summary>
    public Color color = new Color(1, 1, 1, 1);
    /// <summary> 镜像X </summary>
    public bool isReverseX = false;
    /// <summary> 镜像Y </summary>
    public bool isReverseY = false;

    #region 可视化数据
    /// <summary> 可视化对象 </summary>
    public ModulePrefab<DataPatternDesignItem> prefabItem;
    #endregion
}