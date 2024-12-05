using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualPatternDesign : ModuleVisual<DataPatternDesign> {
    
    /// <summary> 图案设计项目 可视化内容生成模块 </summary>
    public ModuleVisual<DataPatternDesignItem> VisualPatternDesignItem => ModuleCore.VisualPatternDesignItem;

    protected override void Awake() => ModuleCore.VisualPatternDesign = this;

    public override void UpdateVisual(DataPatternDesign patternDesign) {
        for (int i = 0; i < patternDesign.items.Count; i++) {
            VisualPatternDesignItem.UpdateVisual(patternDesign.items[i]);
        }
    }
    public override void ReleaseVisual(DataPatternDesign patternDesign) {
        if (patternDesign == null) { return; }
        for (int i = 0; i < patternDesign.items.Count; i++) {
            VisualPatternDesignItem.ReleaseVisual(patternDesign.items[i]);
        }
    }
}
