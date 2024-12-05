using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 图案素材库
/// </summary>
public class AssetsPatternMaterial : ModuleAssets<DataPatternMaterials> {
    public List<DataPatternMaterials> materials;

    public override event Action OnChange;
    public override int Count => materials.Count;
    public override List<DataPatternMaterials> Datas => materials;

    protected override void Awake() => ModuleCore.AssetsPatternMaterials = this;

    public override void Add(DataPatternMaterials data) {
        materials.Add(data);
        OnChange?.Invoke();
    }
    public override void Remove(DataPatternMaterials data) {
        materials.Remove(data);
        OnChange?.Invoke();
    }
    public override DataPatternMaterials Find(int index) {
        return materials[index];
    }
    public override void ForEach(Action<DataPatternMaterials> action) {
        materials.ForEach(action);
    }

    #region 保存和加载
    public override void Save() {
        throw new NotImplementedException();
    }
    public override void Load() {
        throw new NotImplementedException();
    }
    #endregion
}
