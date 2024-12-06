using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using MuHua;

/// <summary>
/// 素材库单元
/// </summary>
public class UIPatternLibraryPanel : ModuleUIPanel {
    public VisualTreeAsset PatternTemplateAsset;
    public VisualTreeAsset PatternTypeTemplateAsset;
    private List<UIPattern> patterns = new List<UIPattern>();
    private List<UIPatternType> patternTypes = new List<UIPatternType>();

    public override VisualElement Element => ModuleUIPage.Q<VisualElement>("PatternLibrary");
    public MUScrollViewHorizontal TypeList => Element.Q<MUScrollViewHorizontal>();
    public MUScrollViewVertical TextureList => Element.Q<MUScrollViewVertical>();

    /// <summary> 图案设计库 </summary>
    public ModuleAssets<DataPatternDesign> AssetsPatternDesign => ModuleCore.AssetsPatternDesign;
    /// <summary> 图案素材库 </summary>
    public ModuleAssets<DataPatternMaterials> AssetsPatternMaterials => ModuleCore.AssetsPatternMaterials;
    /// <summary> 图案设计 DataPatternDesign 数据处理器 </summary>
    public ModuleHandle<DataPatternDesign> HandlePatternDesign => ModuleCore.HandlePatternDesign;
    /// <summary> 图案设计项目 DataPatternDesignItem 数据处理器 </summary>
    public ModuleHandle<DataPatternDesignItem> HandlePatternDesignItem => ModuleCore.HandlePatternDesignItem;

    protected void Start() {
        CreateUIPatternType();
    }
    protected void OnDestroy() {
        patternTypes.ForEach(obj => obj.Release());
        patterns.ForEach(obj => obj.Release());
    }

    public void UpdatePatternDesign(Texture2D texture) {
        if (!HandlePatternDesign.IsValid) {
            DataPatternDesign patternDesign = new DataPatternDesign();
            AssetsPatternDesign.Add(patternDesign);
            HandlePatternDesign.Change(patternDesign);
        }
        DataPatternDesignItem patternDesignItem = new DataPatternDesignItem(HandlePatternDesign.Current);
        patternDesignItem.texture = texture;
        HandlePatternDesign.Current.items.Add(patternDesignItem);
        HandlePatternDesign.Change();
        HandlePatternDesignItem.Change(patternDesignItem);
    }

    #region 创建模板
    public void CreateUIPatternType() {
        TypeList.ClearContainer();
        patternTypes.ForEach(obj => obj.Release());
        patternTypes = new List<UIPatternType>();
        AssetsPatternMaterials.ForEach(CreateUIPatternType);
        patternTypes[0].Select();
    }
    public void CreateUIPatternType(DataPatternMaterials data) {
        VisualElement element = PatternTypeTemplateAsset.Instantiate();
        UIPatternType patternType = new UIPatternType(data, element, this);
        TypeList.AddContainer(patternType.element);
        patternTypes.Add(patternType);
    }
    public void CreateUIPattern(DataPatternMaterials patternMaterials) {
        TextureList.ClearContainer();
        patterns.ForEach(obj => obj.Release());
        patterns = new List<UIPattern>();
        List<Texture2D> textures = patternMaterials.textures;
        textures.ForEach(CreateUIPattern);
    }
    public void CreateUIPattern(Texture2D texture) {
        VisualElement element = PatternTemplateAsset.Instantiate();
        UIPattern pattern = new UIPattern(texture, element, this);
        TextureList.AddContainer(pattern.element);
        patterns.Add(pattern);
    }
    #endregion

    #region UI项目
    /// <summary> 图案分类列表项目 </summary>
    public class UIPatternType : UIItem<DataPatternMaterials, UIPatternType> {
        public readonly UIPatternLibraryPanel panel;
        public Button Type => element.Q<Button>();
        public UIPatternType(DataPatternMaterials value, VisualElement element, UIPatternLibraryPanel panel) : base(value, element) {
            this.panel = panel;
            Type.text = value.name;
            Type.clicked += Select;
        }
        public override void Select() {
            base.Select();
            panel.CreateUIPattern(value);
        }
        public override void DefaultState() => Type.RemoveFromClassList("pt-button-a");
        public override void SelectState() => Type.AddToClassList("pt-button-a");
    }
    /// <summary> 图案列表项目 </summary>
    public class UIPattern : UIItem<Texture2D, UIPattern> {
        public readonly UIPatternLibraryPanel panel;
        public VisualElement Image => element.Q<VisualElement>("Image");
        public UIPattern(Texture2D value, VisualElement element, UIPatternLibraryPanel panel) : base(value, element) {
            this.panel = panel;
            Background background = Background.FromTexture2D(value);
            StyleBackground style = new StyleBackground(background);
            Image.style.backgroundImage = style;
            Image.RegisterCallback<ClickEvent>(evt => Select());
        }
        public override void Select() {
            base.Select();
            panel.UpdatePatternDesign(value);
        }
    }
    #endregion

}
