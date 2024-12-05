using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 核心模块，实现业务逻辑
/// </summary>
public class ModuleCore : Module<ModuleCore> {

    #region 资产模块
    /// <summary> 图案设计库 </summary>
    public ModuleAssets<DataPatternDesign> AssetsPatternDesign;
    /// <summary> 图案素材库 </summary>
    public ModuleAssets<DataPatternMaterials> AssetsPatternMaterials;
    #endregion

    #region 数据模块
    /// <summary> 图案设计 DataPatternDesign 数据处理器 </summary>
    public ModuleHandle<DataPatternDesign> HandlePatternDesign;
    /// <summary> 图案设计项目 DataPatternDesignItem 数据处理器 </summary>
    public ModuleHandle<DataPatternDesignItem> HandlePatternDesignItem;
    #endregion

    #region 输入模块
    /// <summary> 图案设计 输入模块 </summary>
    public ModuleInput<UnitMouseInput> InputPatternDesign;
    #endregion

    #region 相机模块
    /// <summary> 视图相机模块 </summary>
    public ModuleCamera CameraView;
    #endregion

    #region 页面模块
    /// <summary> 不会被销毁的全局唯一页面模块 (UIDocument) </summary>
    public ModuleUIPage GlobalPage;
    /// <summary> 当前的主要页面模块 (UIDocument) </summary>
    public ModuleUIPage CurrentPage;
    #endregion

    #region 独立模块
    /// <summary> Web请求模块 </summary>
    public ModuleSingle<UnitWebRequest> SingleWebRequest;
    #endregion

    #region 可视模块
    /// <summary> 图案设计 可视化内容生成模块 </summary>
    public ModuleVisual<DataPatternDesign> VisualPatternDesign;
    /// <summary> 图案设计项目 可视化内容生成模块 </summary>
    public ModuleVisual<DataPatternDesignItem> VisualPatternDesignItem;
    #endregion

}
