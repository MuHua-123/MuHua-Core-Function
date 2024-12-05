using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualPatternDesignItem : ModuleVisual<DataPatternDesignItem> {
    public Transform viewSpace;
    public Transform prefabPatternDesignItem;

    protected override void Awake() => ModuleCore.VisualPatternDesignItem = this;

    public override void UpdateVisual(DataPatternDesignItem item) {
        Create(ref item.prefabItem, prefabPatternDesignItem, viewSpace);
        item.prefabItem.UpdateVisual(item);
    }
    public override void ReleaseVisual(DataPatternDesignItem item) {
        if (item.prefabItem != null) { Destroy(item.prefabItem.gameObject); }
    }
}
