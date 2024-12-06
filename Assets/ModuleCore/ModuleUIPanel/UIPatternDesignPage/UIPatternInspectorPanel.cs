using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using MuHua;

/// <summary>
/// 图案设计检查器
/// </summary>
public class UIPatternInspectorPanel : ModuleUIPanel {
    public VisualTreeAsset PatternDesignTemplateAsset;
    public VisualTreeAsset PatternDesignItemTemplateAsset;
    private List<UIPatternDesign> designs = new List<UIPatternDesign>();
    private List<UIPatternDesignItem> designItems = new List<UIPatternDesignItem>();

    public override VisualElement Element => ModuleUIPage.Q<VisualElement>("PatternInspector");
    //public Button Button1 => Element.Q<Button>("Button1");
    //public Button Button2 => Element.Q<Button>("Button2");
    public MUScrollViewVertical PatternDesignList => Element.Q<MUScrollViewVertical>("PatternDesignList");
    public MUScrollViewVertical PatternDesignItemList => Element.Q<MUScrollViewVertical>("PatternDesignItemList");

    /// <summary> 图案设计库 </summary>
    public ModuleAssets<DataPatternDesign> AssetsPatternDesign => ModuleCore.AssetsPatternDesign;
    /// <summary> 图案设计 DataPatternDesign 数据处理器 </summary>
    public ModuleHandle<DataPatternDesign> HandlePatternDesign => ModuleCore.HandlePatternDesign;
    /// <summary> 图案设计项目 DataPatternDesignItem 数据处理器 </summary>
    public ModuleHandle<DataPatternDesignItem> HandlePatternDesignItem => ModuleCore.HandlePatternDesignItem;

    protected void Start() {
        AssetsPatternDesign.OnChange += AssetsPatternDesign_OnChange;
        HandlePatternDesign.OnChange += HandlePatternDesign_OnChange;
        HandlePatternDesignItem.OnChange += HandlePatternDesignItem_OnChange;

        //Button1.clicked += Button1_Clicked;
        //Button2.clicked += Button2_Clicked;
        //Button1_Clicked();
    }
    protected void OnDestroy() {
        designs.ForEach(obj => obj.Release());
        designItems.ForEach(obj => obj.Release());
        AssetsPatternDesign.OnChange -= AssetsPatternDesign_OnChange;
        HandlePatternDesign.OnChange -= HandlePatternDesign_OnChange;
        HandlePatternDesignItem.OnChange -= HandlePatternDesignItem_OnChange;
    }

    #region 事件函数
    private void AssetsPatternDesign_OnChange() {
        CreateUIPatternDesign();
    }
    private void HandlePatternDesign_OnChange(DataPatternDesign obj) {
        UIPatternDesign.Select(obj);
        CreateUIPatternDesignItem(obj);
    }
    private void HandlePatternDesignItem_OnChange(DataPatternDesignItem obj) {
        UIPatternDesignItem.Select(obj);
    }
    //private void Button1_Clicked() {
    //    Button1.AddToClassList("inspector-button-s");
    //    Button2.RemoveFromClassList("inspector-button-s");
    //    PatternDesignList.style.display = DisplayStyle.None;
    //    PatternDesignItemList.style.display = DisplayStyle.Flex;
    //}
    //private void Button2_Clicked() {
    //    Button1.RemoveFromClassList("inspector-button-s");
    //    Button2.AddToClassList("inspector-button-s");
    //    PatternDesignList.style.display = DisplayStyle.Flex;
    //    PatternDesignItemList.style.display = DisplayStyle.None;
    //}
    #endregion

    #region 创建模板
    public void CreateUIPatternDesign() {
        PatternDesignList.ClearContainer();
        designs.ForEach(obj => obj.Release());
        designs = new List<UIPatternDesign>();
        AssetsPatternDesign.ForEach(CreateUIPatternDesign);
    }
    private void CreateUIPatternDesign(DataPatternDesign data) {
        VisualElement element = PatternDesignTemplateAsset.Instantiate();
        UIPatternDesign patternDesign = new UIPatternDesign(data, element, this);
        PatternDesignList.AddContainer(patternDesign.element);
        designs.Add(patternDesign);
        if (HandlePatternDesign.Current == data) { patternDesign.SelectState(); }
    }
    public void CreateUIPatternDesignItem(DataPatternDesign obj) {
        PatternDesignItemList.ClearContainer();
        designItems.ForEach(obj => obj.Release());
        designItems = new List<UIPatternDesignItem>();
        if (obj == null) { return; }
        List<DataPatternDesignItem> datas = obj.items;
        datas.ForEach(CreateUIPatternDesignItem);
    }
    private void CreateUIPatternDesignItem(DataPatternDesignItem data) {
        VisualElement element = PatternDesignItemTemplateAsset.Instantiate();
        UIPatternDesignItem designItem = new UIPatternDesignItem(data, element, this);
        PatternDesignItemList.AddContainer(designItem.element);
        designItems.Add(designItem);
        if (HandlePatternDesignItem.Current == data) { designItem.SelectState(); }
    }
    #endregion

    #region UI项目
    /// <summary> 图案设计 </summary>
    public class UIPatternDesign : UIItem<DataPatternDesign, UIPatternDesign> {
        public readonly UIPatternInspectorPanel panel;
        public VisualElement BG => element.Q<VisualElement>("BG");
        public VisualElement Button => element.Q<VisualElement>("Button");
        public VisualElement Image => element.Q<VisualElement>("Image");
        public Button Delete => element.Q<Button>("Delete");
        public ModuleAssets<DataPatternDesign> AssetsPatternDesign => ModuleCore.I.AssetsPatternDesign;
        public ModuleHandle<DataPatternDesign> HandlePatternDesign => ModuleCore.I.HandlePatternDesign;
        public ModuleVisual<DataPatternDesign> VisualPatternDesign => ModuleCore.I.VisualPatternDesign;
        public ModuleHandle<DataPatternDesignItem> HandlePatternDesignItem => ModuleCore.I.HandlePatternDesignItem;
        public UIPatternDesign(DataPatternDesign value, VisualElement element, UIPatternInspectorPanel panel) : base(value, element) {
            this.panel = panel;
            UpdateTexture(value.texture);
            Delete.clicked += Delete_clicked;
            value.OnUpdateTexture = UpdateTexture;
            element.RegisterCallback<MouseDownEvent>(evt => Select());
            element.RegisterCallback<MouseEnterEvent>(MouseEnter);
            element.RegisterCallback<MouseLeaveEvent>(MouseLeave);
        }
        private void UpdateTexture(Texture2D texture) {
            Background background = Background.FromTexture2D(texture);
            StyleBackground style = new StyleBackground(background);
            Image.style.backgroundImage = style;
        }
        public override void Select() {
            base.Select();
            HandlePatternDesign.Change(value);
            HandlePatternDesignItem.Change(null);
        }
        public override void DefaultState() {
            Button.EnableInClassList("pd-button-a", false);
        }
        public override void SelectState() {
            Button.EnableInClassList("pd-button-a", true);
        }
        private void Delete_clicked() {
            VisualPatternDesign.ReleaseVisual(value);
            if (value == HandlePatternDesign.Current) {
                HandlePatternDesign.Change(null);
            }
            AssetsPatternDesign.Remove(value);
        }
        private void MouseEnter(MouseEnterEvent evt) {
            BG.EnableInClassList("pd-bg-s", true);
            Button.EnableInClassList("pd-button-s", true);
        }
        private void MouseLeave(MouseLeaveEvent evt) {
            BG.EnableInClassList("pd-bg-s", false);
            Button.EnableInClassList("pd-button-s", false);
        }
    }
    /// <summary> 图案设计项目 </summary>
    public class UIPatternDesignItem : UIItem<DataPatternDesignItem, UIPatternDesignItem> {
        public readonly UIPatternInspectorPanel panel;
        public VisualElement BG => element.Q<VisualElement>("BG");
        public VisualElement Button => element.Q<VisualElement>("Button");
        public VisualElement Image => element.Q<VisualElement>("Image");
        public Button Delete => element.Q<Button>("Delete");
        public ModuleHandle<DataPatternDesign> HandlePatternDesign => ModuleCore.I.HandlePatternDesign;
        public ModuleHandle<DataPatternDesignItem> HandlePatternDesignItem => ModuleCore.I.HandlePatternDesignItem;
        public ModuleVisual<DataPatternDesignItem> VisualPatternDesignItem => ModuleCore.I.VisualPatternDesignItem;
        public UIPatternDesignItem(DataPatternDesignItem value, VisualElement element, UIPatternInspectorPanel panel) : base(value, element) {
            this.panel = panel;
            Background background = Background.FromTexture2D(value.texture);
            StyleBackground style = new StyleBackground(background);
            Image.style.backgroundImage = style;
            Delete.clicked += Delete_clicked;

            element.RegisterCallback<MouseDownEvent>(evt => Select());
            element.RegisterCallback<MouseEnterEvent>(MouseEnter);
            element.RegisterCallback<MouseLeaveEvent>(MouseLeave);
        }
        public override void Select() {
            base.Select();
            HandlePatternDesignItem.Change(value);
        }
        public override void DefaultState() {
            Button.EnableInClassList("pd-button-a", false);
        }
        public override void SelectState() {
            Button.EnableInClassList("pd-button-a", true);
        }
        private void Delete_clicked() {
            VisualPatternDesignItem.ReleaseVisual(value);
            if (value == HandlePatternDesignItem.Current) {
                HandlePatternDesignItem.Change(null);
            }
            HandlePatternDesign.Current.items.Remove(value);
            HandlePatternDesign.Change();
        }
        private void MouseEnter(MouseEnterEvent evt) {
            BG.EnableInClassList("pd-bg-s", true);
            Button.EnableInClassList("pd-button-s", true);
        }
        private void MouseLeave(MouseLeaveEvent evt) {
            BG.EnableInClassList("pd-bg-s", false);
            Button.EnableInClassList("pd-button-s", false);
        }
    }
    #endregion

}
