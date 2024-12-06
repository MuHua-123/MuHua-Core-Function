using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 调色板数据
/// </summary>
public class DataPalette {
    /// <summary> 颜色 </summary>
    public Color color;
    /// <summary> 设置颜色回调 </summary>
    public Action<Color> callback;
}
