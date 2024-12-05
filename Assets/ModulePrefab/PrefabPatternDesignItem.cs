using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabPatternDesignItem : ModulePrefab<DataPatternDesignItem> {
    private DataPatternDesignItem designItem;

    public override DataPatternDesignItem Value => designItem;
    public Material material => GetComponent<MeshRenderer>().material;

    public override void UpdateVisual(DataPatternDesignItem designItem) {
        this.designItem = designItem;
        material.mainTexture = designItem.texture;
        material.color = designItem.color;

        float x = designItem.isReverseX ? -1f : 1f;
        float y = designItem.isReverseY ? -1f : 1f;
        material.mainTextureScale = new Vector2(x, y);

        transform.localPosition = designItem.position;
        transform.localScale = designItem.scale;
        transform.localEulerAngles = new Vector3(0, 0, designItem.rotate);
    }
}
