using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

/// <summary>
/// 输入模块
/// </summary>
public abstract class ModuleInput<T> : MonoBehaviour {
    /// <summary> 必须要初始化 </summary>
    protected abstract void Awake();
    /// <summary> 核心模块 </summary>
    protected virtual ModuleCore ModuleCore => ModuleCore.I;

    /// <summary> 鼠标位置 </summary>
    public abstract Vector2 MousePosition { get; }

    /// <summary> 当前输入单元 </summary>
    public abstract T Current { get; }
    /// <summary> 改变输入单元时触发 </summary>
    public abstract event Action<T> OnChangeInput;
    /// <summary> 改变输入单元 </summary>
    public abstract void ChangeInput(T input);

    /// <summary> 绑定UI </summary>
    public abstract void Binding(VisualElement element);
}
