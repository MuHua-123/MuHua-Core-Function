using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatternDesignLefts : UnitMouseInput {
    private Vector3 mousePosition;
    private Vector3 originalPosition;

    /// <summary> 视图相机模块 </summary>
    public ModuleCamera CameraView => ModuleCore.CameraView;
    /// <summary> 图案设计 DataPatternDesign 数据处理器 </summary>
    public ModuleHandle<DataPatternDesign> HandlePatternDesign => ModuleCore.HandlePatternDesign;
    /// <summary> 图案设计项目 DataPatternDesignItem 数据处理器 </summary>
    public ModuleHandle<DataPatternDesignItem> HandlePatternDesignItem => ModuleCore.HandlePatternDesignItem;

    public override void MouseDown(DataMouseInput data) {
        if (!HandlePatternDesignItem.IsValid) { return; }
        mousePosition = data.ScreenPosition;
        originalPosition = HandlePatternDesignItem.Current.position;
    }
    public override void MouseDrag(DataMouseInput data) {
        if (!HandlePatternDesignItem.IsValid) { return; }
        Vector3 original = CameraView.ScreenToWorldPosition(mousePosition);
        Vector3 current = data.WorldPosition;
        Vector3 offset = current - original;

        string x = offset.x.ToString("0.000");
        string y = (-offset.y).ToString("0.000");
        HandlePatternDesignItem.Current.position = originalPosition + new Vector3(float.Parse(x), float.Parse(y));
        HandlePatternDesignItem.Change();
    }
}
