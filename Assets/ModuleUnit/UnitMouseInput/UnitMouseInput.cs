using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 鼠标输入单元
/// </summary>
public abstract class UnitMouseInput {
    /// <summary> 核心模块 </summary>
    protected virtual ModuleCore ModuleCore => ModuleCore.I;

    /// <summary> 按下鼠标 </summary>
    public virtual void MouseDown(DataMouseInput data) { }
    /// <summary> 拖拽鼠标 </summary>
    public virtual void MouseDrag(DataMouseInput data) { }
    /// <summary> 移动鼠标 </summary>
    public virtual void MouseMove(DataMouseInput data) { }
    /// <summary> 释放鼠标 </summary>
    public virtual void MouseRelease(DataMouseInput data) { }
}
